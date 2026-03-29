extends Node2D

const MAX_TILES := 3
const CLICK_RADIUS := 12.0

@onready var player: CharacterBody2D = $Player
@onready var next_turn_button: Button = $UI/MarginContainer/NextTurnButton

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


func _pick_tile(mouse_pos: Vector2) -> Vector2:
	var best := Vector2.INF
	var best_dist := CLICK_RADIUS
	for pos in _reachable_positions:
		var d := mouse_pos.distance_to(pos)
		if d < best_dist:
			best_dist = d
			best = pos
	return best


func _input(event: InputEvent) -> void:
	if player.is_moving():
		return
	if not (event is InputEventMouseButton):
		return
	if event.button_index != MOUSE_BUTTON_LEFT or not event.pressed:
		return

	var picked := _pick_tile(get_global_mouse_position())
	if picked == Vector2.INF:
		return

	var data: Dictionary = player.get_move_data_for(picked)
	if data.is_empty():
		return

	_selected_direction = data["direction"]
	_selected_tiles = data["tiles"]


func _on_next_turn_button_pressed() -> void:
	if player.is_moving():
		return
	if _selected_tiles == 0 or _selected_direction == "":
		return
	player.move_in_direction(_selected_direction, _selected_tiles)
	await get_tree().create_timer(0.1).timeout
	while player.is_moving():
		await get_tree().create_timer(0.05).timeout
	_refresh_reachable()
