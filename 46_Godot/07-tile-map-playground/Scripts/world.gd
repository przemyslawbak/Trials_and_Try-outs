extends Node2D

const MAX_TILES    := 3
const CLICK_RADIUS := 12.0

@onready var player:           CharacterBody2D = $Player
@onready var next_turn_button: Button          = $UI/MarginContainer/NextTurnButton
@onready var arrow_overlay:    Node2D          = $ArrowOverlay

var _selected_direction: String  = ""
var _selected_tiles:     int     = 0
var _selected_facing:    String  = ""
var _reachable_positions: Array[Vector2] = []

var _awaiting_facing: bool    = false
var _destination:     Vector2 = Vector2.INF


func _ready() -> void:
	next_turn_button.pressed.connect(_on_next_turn_button_pressed)
	_refresh_reachable()


func _refresh_reachable() -> void:
	_reachable_positions = player.get_reachable_positions(MAX_TILES)
	_selected_direction  = ""
	_selected_tiles      = 0
	_selected_facing     = ""
	_awaiting_facing     = false
	_destination         = Vector2.INF
	if arrow_overlay:
		arrow_overlay.clear_path()
		arrow_overlay.hide_facing_cursor()


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

	# ── mouse motion: update orbiting facing cursor ───────────────────────────
	if event is InputEventMouseMotion:
		if _awaiting_facing:
			var dir := _nearest_facing_dir(get_global_mouse_position(), _destination)
			if dir != "":
				arrow_overlay.update_facing_cursor(dir)
		return

	if not (event is InputEventMouseButton):
		return
	if not event.pressed:
		return

	# ── left click ────────────────────────────────────────────────────────────
	if event.button_index == MOUSE_BUTTON_LEFT:

		if _awaiting_facing:
			# Confirm facing, keep arrow + frozen arrowhead visible
			var dir := _nearest_facing_dir(get_global_mouse_position(), _destination)
			if dir != "":
				_selected_facing = dir
			_awaiting_facing = false
			arrow_overlay.keep_facing_cursor(_selected_facing)
			return

		if event.double_click:
			_on_next_turn_button_pressed()
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
		_selected_tiles     = data["tiles"]
		_selected_facing    = ""
		_destination        = picked
		_awaiting_facing    = true

		var path := _build_path_positions(_selected_direction, _selected_tiles)
		arrow_overlay.set_path(path)
		arrow_overlay.show_facing_cursor(_destination, _selected_direction)


func _on_next_turn_button_pressed() -> void:
	if player.is_moving():
		return
	if _selected_tiles == 0 or _selected_direction == "":
		return

	_awaiting_facing = false
	arrow_overlay.clear_path()
	arrow_overlay.hide_facing_cursor()

	player.move_in_direction(_selected_direction, _selected_tiles)
	await get_tree().create_timer(0.1).timeout
	while player.is_moving():
		await get_tree().create_timer(0.05).timeout

	# Apply confirmed facing at the very end of movement
	if _selected_facing != "" and _selected_facing != _selected_direction:
		player.set_facing(_selected_facing)

	_refresh_reachable()
