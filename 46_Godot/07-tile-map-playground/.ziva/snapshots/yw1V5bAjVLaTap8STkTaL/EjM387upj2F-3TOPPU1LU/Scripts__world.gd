extends Node2D

const CLICK_RADIUS := 12.0

const PlayerScene = preload("res://Scenes/player.tscn")
const EnemyScene = preload("res://Scenes/enemy.tscn")
const AllyScene = preload("res://Scenes/ally.tscn")

enum Weather { SUNNY, RAINING, CLOUDY, WINDY }
enum Turn { PLAYER, ALLY, ENEMY }

var current_weather: Weather
var current_turn: Turn

var player: CharacterBody2D
var enemy: CharacterBody2D
var ally: CharacterBody2D

@onready var next_turn_button: Button		  = $UI/RightMarginContainer/TurnContainer/NextTurnButton
@onready var menu_button: TextureButton = $UI/LeftMarginContainer/MenuButton
@onready var arrow_overlay:	Node2D		  = $ArrowOverlay
@onready var tile_highlight:   Node2D		  = $TileHighlight

@onready var weather_icon: TextureRect = $UI/RightMarginContainer/TurnContainer/WeatherContainer/WeatherIcon
@onready var turn_label: RichTextLabel = $UI/RightMarginContainer/TurnContainer/TurnLabel
@onready var turn_no_label: Label = $UI/RightMarginContainer/TurnContainer/TurnNoLabel

@onready var stats_container: VBoxContainer = $UI/RightMarginContainer/TurnContainer/StatsContainer
@onready var stats_name_label: RichTextLabel = $UI/RightMarginContainer/TurnContainer/StatsContainer/NameLabel
@onready var footmen_label: RichTextLabel = $UI/RightMarginContainer/TurnContainer/StatsContainer/FootmenLabel
@onready var horsemen_label: RichTextLabel = $UI/RightMarginContainer/TurnContainer/StatsContainer/HorsemenLabel
@onready var archers_label: RichTextLabel = $UI/RightMarginContainer/TurnContainer/StatsContainer/ArchersLabel
@onready var pikemen_label: RichTextLabel = $UI/RightMarginContainer/TurnContainer/StatsContainer/PikemenLabel
@onready var knights_label: RichTextLabel = $UI/RightMarginContainer/TurnContainer/StatsContainer/KnightsLabel

const WEATHER_ICONS = {
	Weather.SUNNY: preload("res://Assets/Icons/Weather/sunny.png"),
	Weather.RAINING: preload("res://Assets/Icons/Weather/rainy.png"),
	Weather.CLOUDY: preload("res://Assets/Icons/Weather/cloudy.png"),
	Weather.WINDY: preload("res://Assets/Icons/Weather/windy.png")
}

var turn_number: int = 1

var _selected_direction: String  = ""
var _selected_move_tiles: int	= 0
var _selected_tiles:	 int	 = 0
var _selected_facing:	String  = ""
var _reachable_positions: Array[Vector2] = []

var _awaiting_facing: bool	= false
var _destination:	 Vector2 = Vector2.INF

var selected_character: CharacterBody2D
var flag_rect: ColorRect
var _name_edit: LineEdit

var path: WorldPath
var movement: WorldMovement
var facing: WorldFacing

func _ready() -> void:
	path = WorldPath.new(self)
	movement = WorldMovement.new(self)
	facing = WorldFacing.new(self)

	flag_rect = ColorRect.new()
	flag_rect.custom_minimum_size = Vector2(0, 40)
	var mat = ShaderMaterial.new()
	mat.shader = preload("res://flag.gdshader")
	flag_rect.material = mat
	stats_container.add_child(flag_rect)
	stats_container.move_child(flag_rect, stats_name_label.get_index())

	_name_edit = LineEdit.new()
	_name_edit.visible = false
	stats_container.add_child(_name_edit)
	_name_edit.text_submitted.connect(_on_name_edit_submitted)
	_name_edit.focus_exited.connect(_on_name_edit_focus_exited)
	_name_edit.gui_input.connect(_on_name_edit_gui_input)
	
	stats_name_label.gui_input.connect(_on_stats_name_label_gui_input)
	stats_name_label.mouse_filter = Control.MOUSE_FILTER_STOP

	next_turn_button.pressed.connect(_on_next_turn_button_pressed)
	menu_button.pressed.connect(_on_menu_button_pressed)
	
	player = get_node_or_null("Player")
	player = _deploy_character(PlayerScene, "Player", player)
	
	enemy = get_node_or_null("Enemy")
	enemy = _deploy_character(EnemyScene, "Enemy", enemy)
	
	ally = get_node_or_null("Ally")
	ally = _deploy_character(AllyScene, "Ally", ally)
	
	_initialize_game_state()
	
	if player:
		_refresh_reachable()

func _roll_weather() -> void:
	var rand_val := randf()
	if rand_val < 0.30:
		current_weather = Weather.SUNNY
	elif rand_val < 0.50: # 0.30 + 0.20
		current_weather = Weather.RAINING
	elif rand_val < 0.65: # 0.50 + 0.15
		current_weather = Weather.WINDY
	else: # remaining 0.35
		current_weather = Weather.CLOUDY
		
	if weather_icon:
		weather_icon.texture = WEATHER_ICONS[current_weather]

func _initialize_game_state() -> void:
	_roll_weather()
	
	# Set turn to Player
	current_turn = Turn.PLAYER
	turn_number = 1
	if turn_no_label:
		turn_no_label.text = "Turn No: " + str(turn_number)
	if player and player.shield:
		turn_label.text = "Turn: [color=#%s]Player[/color]" % player.shield.color.to_html(false)
	else:
		turn_label.text = "Turn: Player"
		
	if player:
		_update_stats_ui(player)

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
		var world_pos = ground.position + ground.map_to_local(cell)
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
		char_inst.position = ground.position + ground.map_to_local(chosen_cell)
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
		if selected_character == player:
			tile_highlight.set_highlights(_reachable_positions)
		else:
			tile_highlight.clear_highlights()


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
		if current_turn == Turn.PLAYER and selected_character == player:
			tile_highlight.set_highlights(_reachable_positions)
		else:
			tile_highlight.clear_highlights()





func _on_stats_name_label_gui_input(event: InputEvent) -> void:
	if event is InputEventMouseButton and event.button_index == MOUSE_BUTTON_LEFT and event.pressed:
		if selected_character == player:
			var current_name = selected_character.name
			if selected_character.has_node("NameLabel"):
				current_name = selected_character.get_node("NameLabel").text
			_name_edit.text = current_name
			_name_edit.visible = true
			stats_name_label.visible = false
			stats_container.move_child(_name_edit, stats_name_label.get_index())
			_name_edit.grab_focus()

func _on_name_edit_submitted(new_text: String) -> void:
	_finish_name_edit(new_text)

func _on_name_edit_focus_exited() -> void:
	_finish_name_edit(_name_edit.text)

func _on_name_edit_gui_input(event: InputEvent) -> void:
	if event is InputEventKey and event.pressed and event.keycode == KEY_ESCAPE:
		_cancel_name_edit()

func _cancel_name_edit() -> void:
	if not _name_edit.visible:
		return
	_name_edit.visible = false
	stats_name_label.visible = true
	if _name_edit.has_focus():
		_name_edit.release_focus()

func _finish_name_edit(new_text: String) -> void:
	if not _name_edit.visible:
		return
	_name_edit.visible = false
	stats_name_label.visible = true
	var final_name = new_text.strip_edges()
	if final_name != "" and selected_character == player:
		if player.has_node("NameLabel"):
			player.get_node("NameLabel").text = final_name
		else:
			player.name = final_name
	if selected_character:
		_update_stats_ui(selected_character)

func _get_character_at_position(pos: Vector2) -> CharacterBody2D:
	var chars = [player, enemy, ally]
	for c in chars:
		if c and c.is_inside_tree():
			# Character sprites are offset by -14px Y usually, so adjust the target click slightly if needed
			# The simplest approach is just a distance check:
			if pos.distance_to(c.position + Vector2(0, -22)) < CLICK_RADIUS * 1.5:
				return c
	return null

func _update_stats_ui(character: CharacterBody2D) -> void:
	if not character:
		return
	
	if _name_edit and _name_edit.visible:
		_name_edit.visible = false
		stats_name_label.visible = true
	
	if selected_character and selected_character != character and selected_character.has_method("set_selected"):
		selected_character.set_selected(false)
		
	selected_character = character
	if selected_character.has_method("set_selected"):
		selected_character.set_selected(true)
		
	if flag_rect and selected_character.has_node("Shield"):
		var shield = selected_character.get_node("Shield") as Polygon2D
		if shield:
			flag_rect.material.set_shader_parameter("flag_color", shield.color)
			
	if tile_highlight:
		if selected_character == player and current_turn == Turn.PLAYER and _selected_direction == "":
			tile_highlight.set_highlights(_reachable_positions)
		else:
			tile_highlight.clear_highlights()
	
	if stats_name_label:
		var char_name = character.name
		if character.has_node("NameLabel"):
			char_name = character.get_node("NameLabel").text
			
		var stars_str = ""
		if "command" in character:
			stars_str = "[img=10]res://Assets/Icons/star.png[/img]".repeat(character.command)
				
		if selected_character == player:
			stats_name_label.text = "[img=10]res://Assets/Icons/UI/edit-icon.png[/img] Name: " + char_name + " " + stars_str
			stats_name_label.mouse_default_cursor_shape = Control.CURSOR_IBEAM
			stats_name_label.tooltip_text = "Click to rename"
		else:
			stats_name_label.text = "Name: " + char_name + " " + stars_str
			stats_name_label.mouse_default_cursor_shape = Control.CURSOR_ARROW
			stats_name_label.tooltip_text = ""
			
	if footmen_label and "footmen" in character:
		footmen_label.text = "[img=16]res://Assets/Icons/sword-pngrepo-com.png[/img] " + str(character.footmen)
	if horsemen_label and "horsemen" in character:
		horsemen_label.text = "[img=16]res://Assets/Icons/horse-head.png[/img] " + str(character.horsemen)
	if archers_label and "archers" in character:
		archers_label.text = "[img=16]res://Assets/Icons/bow.png[/img] " + str(character.archers)
	if pikemen_label and "pikemen" in character:
		pikemen_label.text = "[img=16]res://Assets/Icons/spear.png[/img] " + str(character.pikemen)
	if knights_label and "knights" in character:
		knights_label.text = "[img=16]res://Assets/Icons/knight-helmet.png[/img] " + str(character.knights)

func _input(event: InputEvent) -> void:
	if event is InputEventMouseButton and event.pressed and event.button_index == MOUSE_BUTTON_LEFT:
		var click_pos = get_global_mouse_position()
		var clicked_char = _get_character_at_position(click_pos)
		if clicked_char:
			_update_stats_ui(clicked_char)

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


func _on_menu_button_pressed() -> void:
	var main_menu = preload("res://Scenes/main_menu.tscn").instantiate()
	main_menu.is_overlay = true
	$UI.add_child(main_menu)
	get_tree().paused = true

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

		current_turn = Turn.ALLY
		if ally and ally.shield:
			turn_label.text = "Turn: [color=#%s]Ally[/color]" % ally.shield.color.to_html(false)
		else:
			turn_label.text = "Turn: Ally"
		_clear_selection()
		next_turn_button.disabled = false
		
	elif current_turn == Turn.ALLY:
		current_turn = Turn.ENEMY
		if enemy and enemy.shield:
			turn_label.text = "Turn: [color=#%s]Enemy[/color]" % enemy.shield.color.to_html(false)
		else:
			turn_label.text = "Turn: Enemy"
		_clear_selection()
		
	elif current_turn == Turn.ENEMY:
		current_turn = Turn.PLAYER
		turn_number += 1
		if turn_no_label:
			turn_no_label.text = "Turn No: " + str(turn_number)
		if player and player.shield:
			turn_label.text = "Turn: [color=#%s]Player[/color]" % player.shield.color.to_html(false)
		else:
			turn_label.text = "Turn: Player"
		
		# Roll new random weather for the new turn
		_roll_weather()
		
		_refresh_reachable()
