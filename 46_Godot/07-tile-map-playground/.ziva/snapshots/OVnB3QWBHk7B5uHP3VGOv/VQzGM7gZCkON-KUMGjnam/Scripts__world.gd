extends Node2D

const CLICK_RADIUS := 12.0

@onready var player:		   CharacterBody2D = $Player
@onready var next_turn_button: Button		  = $UI/MarginContainer/NextTurnButton
@onready var arrow_overlay:	Node2D		  = $ArrowOverlay
@onready var tile_highlight:   Node2D		  = $TileHighlight

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

	_spawn_characters()
	
	next_turn_button.pressed.connect(_on_next_turn_button_pressed)
	_refresh_reachable()


func _spawn_characters() -> void:
	var spawn_points: Array[Vector2] = []
	for layer_name in ["Ground-lvl0", "Ground-lvl1", "Decoration-lvl-0"]:
		var layer = get_node_or_null(layer_name) as TileMapLayer
		if not layer:
			continue
		var cells = layer.get_used_cells()
		for cell in cells:
			if layer.get_cell_atlas_coords(cell) == Vector2i(6, 3):
				spawn_points.append(layer.map_to_local(cell))
				layer.erase_cell(cell)
	
	if spawn_points.is_empty():
		return
		
	# Spawn player at the first point
	if player:
		player.position = spawn_points[0]
	
	# Spawn enemies at the remaining points
	var enemy_scene = load("res://Scenes/enemy.tscn")
	for i in range(1, spawn_points.size()):
		if enemy_scene:
			var enemy = enemy_scene.instantiate()
			enemy.position = spawn_points[i]
			add_child(enemy)

func _refresh_reachable() -> void:
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
	player.set_move_tiles_label_value(player.available_tiles)
	_selected_facing	= ""
	_awaiting_facing	= false
	_destination		= Vector2.INF
	arrow_overlay.clear_path()
	arrow_overlay.hide_facing_cursor()
	arrow_overlay.set_confirmed_facing_visual(false)
	
	if tile_highlight:
		tile_highlight.set_highlights(_reachable_positions)





func _input(event: InputEvent) -> void:
	if player.is_moving():
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
	if player.is_moving():
		return
	if _selected_move_tiles == 0 or _selected_direction == "":
		return

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

	_refresh_reachable()
