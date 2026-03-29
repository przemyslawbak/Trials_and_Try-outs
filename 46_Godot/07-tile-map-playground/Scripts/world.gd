extends Node2D

const MAX_TILES := 3
const CLICK_RADIUS := 12.0

@onready var player: CharacterBody2D = $Player
@onready var next_turn_button: Button = $UI/MarginContainer/NextTurnButton
@onready var arrow_overlay: Node2D = $ArrowOverlay

var _selected_direction: String = ""
var _selected_tiles: int = 0
var _reachable_positions: Array[Vector2] = []


func _ready() -> void:
	next_turn_button.pressed.connect(_on_next_turn_button_pressed)
	_refresh_reachable()


func _refresh_reachable() -> void:
	_reachable_positions = player.get_reachable_positions(MAX_TILES)
	_selected_direction = ""
	_selected_tiles = 0
	arrow_overlay.clear_path()


func _pick_tile(mouse_pos: Vector2) -> Vector2:
	var best := Vector2.INF
	var best_dist := CLICK_RADIUS
	for pos in _reachable_positions:
		var d := mouse_pos.distance_to(pos)
		if d < best_dist:
			best_dist = d
			best = pos
	return best


func _build_path_positions(direction: String, tiles: int) -> Array[Vector2]:
	var step: Vector2 = player.ISO_DIRECTIONS.get(direction, Vector2.ZERO)
	var pts: Array[Vector2] = [player.global_position]
	for t in range(1, tiles + 1):
		pts.append(player.global_position + step * t)
	return pts


func _input(event: InputEvent) -> void:
	if player.is_moving():
		return
	if not (event is InputEventMouseButton):
		return
	if event.button_index != MOUSE_BUTTON_LEFT or not event.pressed:
		return

	if event.double_click:
		_on_next_turn_button_pressed()
		return

	var picked := _pick_tile(get_global_mouse_position())
	if picked == Vector2.INF:
		arrow_overlay.clear_path()
		return

	var data: Dictionary = player.get_move_data_for(picked)
	if data.is_empty():
		arrow_overlay.clear_path()
		return

	_selected_direction = data["direction"]
	_selected_tiles     = data["tiles"]

	var path := _build_path_positions(_selected_direction, _selected_tiles)
	arrow_overlay.set_path(path)


func _on_next_turn_button_pressed() -> void:
	if player.is_moving():
		return
	if _selected_tiles == 0 or _selected_direction == "":
		return
	arrow_overlay.clear_path()
	player.move_in_direction(_selected_direction, _selected_tiles)
	await get_tree().create_timer(0.1).timeout
	while player.is_moving():
		await get_tree().create_timer(0.05).timeout
	_refresh_reachable()
