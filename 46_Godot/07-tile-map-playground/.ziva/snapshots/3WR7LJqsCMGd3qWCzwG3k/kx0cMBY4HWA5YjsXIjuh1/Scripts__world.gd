extends Node2D

const CLICK_RADIUS := 12.0

@onready var player:           CharacterBody2D = $Player
@onready var next_turn_button: Button          = $UI/MarginContainer/NextTurnButton
@onready var arrow_overlay:    Node2D          = $ArrowOverlay

var _selected_direction: String  = ""
var _selected_move_tiles: int	= 0
var _selected_tiles:	 int	 = 0
var _selected_facing:	String  = ""
var _reachable_positions: Array[Vector2] = []

var _awaiting_facing: bool    = false
var _destination:     Vector2 = Vector2.INF


func _ready() -> void:
	next_turn_button.pressed.connect(_on_next_turn_button_pressed)
	_refresh_reachable()


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


# AP remaining after the planned move is spent.
func _ap_after_move() -> int:
	return player.available_tiles - _selected_move_tiles


# AP remaining after both the planned move AND a facing change to `to_dir`.
func _ap_after_facing(to_dir: String) -> int:
	# Facing rotates from the movement direction (where player ends up facing),
	# or from current facing if no move is planned.
	var from_dir: String = _selected_direction if _selected_direction != "" else player.facing_direction
	var cost:     int    = player.facing_cost(from_dir, to_dir)
	return _ap_after_move() - cost


func _pick_tile(mouse_pos: Vector2) -> Vector2:
	var best      := Vector2.INF
	var best_dist := CLICK_RADIUS
	for pos in _reachable_positions:
		var d := mouse_pos.distance_to(pos)
		if d < best_dist:
			best_dist = d
			best      = pos
	return best


func _build_path_positions(direction: String, tiles: int) -> Array[Vector2]:
	var step: Vector2       = player.ISO_DIRECTIONS.get(direction, Vector2.ZERO)
	var pts: Array[Vector2] = [player.global_position]
	for t in range(1, tiles + 1):
		pts.append(player.global_position + step * t)
	return pts


func _nearest_facing_dir(mouse_pos: Vector2, center: Vector2) -> String:
	var best_dir  := ""
	var best_dist := INF
	for dir in player.ISO_DIRECTIONS:
		var neighbour: Vector2 = center + player.ISO_DIRECTIONS[dir]
		var d := mouse_pos.distance_to(neighbour)
		if d < best_dist:
			best_dist = d
			best_dir  = dir
	return best_dir


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
			var dir := _nearest_facing_dir(get_global_mouse_position(), _destination)
			# Only move the facing arrow if AP budget allows this facing change
			if dir != "" and _ap_after_facing(dir) >= 0:
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
			var dir := _nearest_facing_dir(get_global_mouse_position(), _destination)
			# Only confirm facing if AP budget allows it
			if dir != "" and _ap_after_facing(dir) >= 0:
				_selected_facing = dir
			var confirmed := _selected_facing if _selected_facing != "" else _selected_direction
			var turn_cost: int = player.facing_cost(_selected_direction, confirmed)
			_selected_tiles = _selected_move_tiles + turn_cost
			player.set_move_tiles_label_value(player.available_tiles - _selected_tiles)
			_awaiting_facing = false
			# Show the confirmed facing (falls back to movement direction if facing
			# was never affordable / never changed)
			arrow_overlay.keep_facing_cursor(confirmed)
			return

		# Selection lock: once movement + facing are chosen, ignore extra left-clicks.
		# Selection is only cleared by right-click / Escape (or after turn resolution).
		if _selected_direction != "" and _selected_tiles > 0 and not _awaiting_facing:
			return

		var picked := _pick_tile(get_global_mouse_position())
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

		var path := _build_path_positions(_selected_direction, _selected_tiles)
		arrow_overlay.set_path(path)
		arrow_overlay.show_facing_cursor(_destination, _selected_direction)


func _on_next_turn_button_pressed() -> void:
	if player.is_moving():
		return
	if _selected_move_tiles == 0 or _selected_direction == "":
		return

	_awaiting_facing = false
	arrow_overlay.clear_path()
	arrow_overlay.hide_facing_cursor()

	player.move_in_direction(_selected_direction, _selected_move_tiles)
	await get_tree().create_timer(0.1).timeout
	while player.is_moving():
		await get_tree().create_timer(0.05).timeout

	# Apply confirmed facing at the very end of movement
	if _selected_facing != "" and _selected_facing != _selected_direction:
		player.set_facing(_selected_facing)

	_refresh_reachable()
