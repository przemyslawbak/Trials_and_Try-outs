extends Node2D

const CLICK_RADIUS := 12.0

const PlayerScene = preload("res://Scenes/player.tscn")
const EnemyScene = preload("res://Scenes/enemy.tscn")

enum Weather { SUNNY, RAINING, CLOUDY, WINDY }
enum Turn { PLAYER, ENEMY }

var current_weather: Weather
var current_turn: Turn

var player: CharacterBody2D
var enemy: CharacterBody2D

@onready var next_turn_button: Button		  = $UI/MarginContainer/NextTurnButton
@onready var arrow_overlay:	Node2D		  = $ArrowOverlay
@onready var tile_highlight:   Node2D		  = $TileHighlight

@onready var weather_label: Label = $UI/RightMarginContainer/VBoxContainer/WeatherLabel
@onready var turn_label: RichTextLabel = $UI/RightMarginContainer/VBoxContainer/TurnLabel

var _selected_direction: String  = ""
var _selected_move_tiles: int	= 0
var _selected_tiles:	 int	 = 0
var _selected_facing:	String  = ""
var _reachable_positions: Array[Vector2] = []

var _awaiting_facing: bool	= false
var _destination:	 Vector2 = Vector2.INF

var path: WorldPath
var movement: WorldMovement
var facing: WorldFacing

func _ready() -> void:
	path = WorldPath.new(self)
	movement = WorldMovement.new(self)
	facing = WorldFacing.new(self)

	next_turn_button.pressed.connect(_on_next_turn_button_pressed)
	
	player = get_node_or_null("Player")
	player = _deploy_character(PlayerScene, "Player", player)
	
	enemy = get_node_or_null("Enemy")
	enemy = _deploy_character(EnemyScene, "Enemy", enemy)
	
	_initialize_game_state()
	
	if player:
		_refresh_reachable()

func _initialize_game_state() -> void:
	# Select random weather
	var weather_values = Weather.values()
	current_weather = weather_values[randi() % weather_values.size()]
	weather_label.text = "Weather: " + Weather.keys()[current_weather].capitalize()
	
	# Set turn to Player
	current_turn = Turn.PLAYER
	if player and player.shield:
		turn_label.text = "[right]Turn: [color=#%s]Player[/color][/right]" % player.shield.color.to_html(false)
	else:
		turn_label.text = "[right]Turn: Player[/right]"

func _deploy_character(character_scene: PackedScene, char_name: String, existing_ref: CharacterBody2D) -> CharacterBody2D:
	var ground = get_node_or_null("Ground-lvl0") as SpawnPoints
	if not ground or ground._black_cells.is_empty():
		return existing_ref
		
	var spawn_points = ground._black_cells.duplicate()
	spawn_points.shuffle()
	
	var existing_chars: Array[Node] = []
	for child in get_children():
		if child is CharacterBody2D and child != existing_ref:
			existing_chars.append(child)
			
	var chosen_cell: Vector2i
	var cell_found := false
	
	for cell in spawn_points:
		var world_pos = ground.map_to_local(cell)
		var occupied = false
		for char_node in existing_chars:
			if char_node.position.distance_to(world_pos) < 5.0:
				occupied = true
				break
				
		if not occupied:
			chosen_cell = cell
			cell_found = true
			break
			
	if cell_found:
		var char_inst = existing_ref
		if not char_inst:
			char_inst = character_scene.instantiate()
			char_inst.name = char_name
			add_child(char_inst)
		char_inst.position = ground.map_to_local(chosen_cell)
		return char_inst
		
	return existing_ref


func _refresh_reachable() -> void:
	if not player:
		return
	player.reset_available_tiles()
	_reachable_positions = player.get_reachable_positions()
	_selected_direction  = ""
	_selected_move_tiles = 0
	_selected_tiles	  = 0
	player.set_move_tiles_label_value(player.available_tiles)
	_selected_facing	 = ""
	_awaiting_facing	 = false
	_destination		 = Vector2.INF
	if arrow_overlay:
		arrow_overlay.clear_path()
		arrow_overlay.hide_facing_cursor()
		arrow_overlay.set_confirmed_facing_visual(false)
	
	if tile_highlight:
		tile_highlight.set_highlights(_reachable_positions)


func _clear_selection() -> void:
	_selected_direction = ""
	_selected_move_tiles = 0
	_selected_tiles	  = 0
	if player:
		player.set_move_tiles_label_value(player.available_tiles)
	_selected_facing	= ""
	_awaiting_facing	= false
	_destination		= Vector2.INF
	arrow_overlay.clear_path()
	arrow_overlay.hide_facing_cursor()
	arrow_overlay.set_confirmed_facing_visual(false)
	
	if tile_highlight:
		if current_turn == Turn.PLAYER:
			tile_highlight.set_highlights(_reachable_positions)
		else:
			tile_highlight.clear_highlights()





func _input(event: InputEvent) -> void:
	if not player or player.is_moving():
		return
	if current_turn != Turn.PLAYER:
		return

	# ── escape key: clear all selection ──────────────────────────────────────
	if event is InputEventKey and event.pressed and not event.echo:
		if event.keycode == KEY_ESCAPE:
			_clear_selection()
		return

	# ── mouse motion: update orbiting facing cursor ───────────────────────────
	if event is InputEventMouseMotion:
		if _awaiting_facing:
			var dir := facing.nearest_facing_dir(get_global_mouse_position(), _destination)
			# Only move the facing arrow if AP budget allows this facing change
			if dir != "" and facing.ap_after_facing(dir) >= 0:
				arrow_overlay.update_facing_cursor(dir)
		return

	if not (event is InputEventMouseButton):
		return
	if not event.pressed:
		return

	# ── right click: clear all selection ─────────────────────────────────────
	if event.button_index == MOUSE_BUTTON_RIGHT:
		_clear_selection()
		return

	# ── left click ────────────────────────────────────────────────────────────
	if event.button_index == MOUSE_BUTTON_LEFT:

		if _awaiting_facing:
			var dir := facing.nearest_facing_dir(get_global_mouse_position(), _destination)
			# Only confirm facing if AP budget allows it
			if dir != "" and facing.ap_after_facing(dir) >= 0:
				_selected_facing = dir
			var confirmed := _selected_facing if _selected_facing != "" else _selected_direction
			var turn_cost: int = player.facing_cost(_selected_direction, confirmed)
			_selected_tiles = _selected_move_tiles + turn_cost
			if player.has_method("set_move_tiles_label_ok"):
				player.set_move_tiles_label_ok()
			_awaiting_facing = false
			# Show the confirmed facing (falls back to movement direction if facing
			# was never affordable / never changed)
			arrow_overlay.keep_facing_cursor(confirmed)
			arrow_overlay.set_confirmed_facing_visual(true)
			
			if tile_highlight:
				tile_highlight.clear_highlights()
			return

		# Selection lock: once movement + facing are chosen, ignore extra left-clicks.
		# Selection is only cleared by right-click / Escape (or after turn resolution).
		if _selected_direction != "" and _selected_tiles > 0 and not _awaiting_facing:
			return

		var picked := path.pick_tile(get_global_mouse_position())
		if picked == Vector2.INF:
			arrow_overlay.clear_path()
			arrow_overlay.hide_facing_cursor()
			return

		var data: Dictionary = player.get_move_data_for(picked)
		if data.is_empty():
			arrow_overlay.clear_path()
			arrow_overlay.hide_facing_cursor()
			return

		_selected_direction = data["direction"]
		_selected_move_tiles = data["tiles"]
		_selected_tiles	  = _selected_move_tiles
		player.set_move_tiles_label_value(player.available_tiles - _selected_tiles)
		_selected_facing	= ""
		_destination		= picked
		_awaiting_facing	= true

		var path_positions = path.build_path_positions(_selected_direction, _selected_tiles)
		arrow_overlay.set_confirmed_facing_visual(false)
		arrow_overlay.set_path(path_positions)
		arrow_overlay.show_facing_cursor(_destination, _selected_direction)


func _on_next_turn_button_pressed() -> void:
	if current_turn == Turn.PLAYER:
		if not player or player.is_moving():
			return

		next_turn_button.disabled = true

		if _selected_move_tiles > 0 and _selected_direction != "":
			_awaiting_facing = false
			arrow_overlay.clear_path()
			arrow_overlay.hide_facing_cursor()
			arrow_overlay.set_confirmed_facing_visual(false)
			
			if tile_highlight:
				tile_highlight.clear_highlights()

			player.move_in_direction(_selected_direction, _selected_move_tiles)
			await get_tree().create_timer(0.1).timeout
			while player.is_moving():
				await get_tree().create_timer(0.05).timeout

			# Apply confirmed facing at the very end of movement
			if _selected_facing != "" and _selected_facing != _selected_direction:
				player.set_facing(_selected_facing)

		current_turn = Turn.ENEMY
		if enemy and enemy.shield:
			turn_label.text = "[right]Turn: [color=#%s]Enemy[/color][/right]" % enemy.shield.color.to_html(false)
		else:
			turn_label.text = "[right]Turn: Enemy[/right]"
		_clear_selection()
		next_turn_button.disabled = false
	else:
		# Currently it's ENEMY turn
		current_turn = Turn.PLAYER
		if player and player.shield:
			turn_label.text = "[right]Turn: [color=#%s]Player[/color][/right]" % player.shield.color.to_html(false)
		else:
			turn_label.text = "[right]Turn: Player[/right]"
		
		# Roll new random weather for the new turn
		var weather_values = Weather.values()
		current_weather = weather_values[randi() % weather_values.size()]
		weather_label.text = "Weather: " + Weather.keys()[current_weather].capitalize()
		
		_refresh_reachable()
