extends Node2D

const DIRECTIONS := ["NE", "SE", "SW", "NW"]
const MIN_TILES := 0
const MAX_TILES := 3

# Click must land within this pixel radius of a valid tile centre to count
const CLICK_RADIUS := 12.0

@onready var player: CharacterBody2D = $Player
@onready var next_turn_button: Button = $UI/MarginContainer/NextTurnButton

var selected_direction: String = "SE"
var selected_tiles: int = 1

var _updating := false
var _reachable_positions: Array[Vector2] = []


func _ready() -> void:
	next_turn_button.pressed.connect(_on_next_turn_button_pressed)
	next_turn_button.text = "Next Turn"
	_build_controls()
	_refresh_reachable()


func _refresh_reachable() -> void:
	_reachable_positions = player.get_reachable_positions(MAX_TILES)


# Returns the reachable position closest to the click, or Vector2.INF if none
# is within CLICK_RADIUS
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
		return  # click was not on a valid tile — ignore

	var data: Dictionary = player.get_move_data_for(picked)
	if data.is_empty():
		return

	_set_direction(data["direction"])
	_set_tiles(data["tiles"])


# ── UI ────────────────────────────────────────────────────────────────────────

func _build_controls() -> void:
	var margin: MarginContainer = $UI/MarginContainer
	var vbox := VBoxContainer.new()
	vbox.name = "Controls"
	margin.add_child(vbox)
	margin.remove_child(next_turn_button)

	var dir_label := Label.new()
	dir_label.text = "Direction:"
	vbox.add_child(dir_label)

	var dir_box := HBoxContainer.new()
	dir_box.name = "DirectionBox"
	vbox.add_child(dir_box)

	for dir in DIRECTIONS:
		var btn := Button.new()
		btn.text = dir
		btn.toggle_mode = true
		btn.button_pressed = (dir == selected_direction)
		btn.name = "Dir_" + dir
		dir_box.add_child(btn)
		btn.toggled.connect(_on_direction_toggled.bind(dir))

	var tile_label := Label.new()
	tile_label.text = "Tiles (0–3):"
	vbox.add_child(tile_label)

	var tile_box := HBoxContainer.new()
	tile_box.name = "TileBox"
	vbox.add_child(tile_box)

	for t in range(MIN_TILES, MAX_TILES + 1):
		var btn := Button.new()
		btn.text = str(t)
		btn.toggle_mode = true
		btn.button_pressed = (t == selected_tiles)
		btn.name = "Tile_" + str(t)
		tile_box.add_child(btn)
		btn.toggled.connect(_on_tile_toggled.bind(t))

	vbox.add_child(next_turn_button)


func _set_direction(dir: String) -> void:
	if selected_direction == dir:
		return
	selected_direction = dir
	_updating = true
	for child in $UI/MarginContainer/Controls/DirectionBox.get_children():
		child.button_pressed = (child.name == "Dir_" + dir)
	_updating = false


func _set_tiles(count: int) -> void:
	if selected_tiles == count:
		return
	selected_tiles = count
	_updating = true
	for child in $UI/MarginContainer/Controls/TileBox.get_children():
		child.button_pressed = (child.name == "Tile_" + str(count))
	_updating = false


func _on_direction_toggled(pressed: bool, dir: String) -> void:
	if _updating:
		return
	if not pressed:
		_updating = true
		$UI/MarginContainer/Controls/DirectionBox.get_node("Dir_" + dir).button_pressed = true
		_updating = false
		return
	selected_direction = dir
	_updating = true
	for child in $UI/MarginContainer/Controls/DirectionBox.get_children():
		if child.name != "Dir_" + dir:
			child.button_pressed = false
	_updating = false


func _on_tile_toggled(pressed: bool, count: int) -> void:
	if _updating:
		return
	if not pressed:
		_updating = true
		$UI/MarginContainer/Controls/TileBox.get_node("Tile_" + str(count)).button_pressed = true
		_updating = false
		return
	selected_tiles = count
	_updating = true
	for child in $UI/MarginContainer/Controls/TileBox.get_children():
		if child.name != "Tile_" + str(count):
			child.button_pressed = false
	_updating = false


func _on_next_turn_button_pressed() -> void:
	if player.is_moving():
		return
	if selected_tiles == 0:
		return
	player.move_in_direction(selected_direction, selected_tiles)
	# Refresh reachable positions after movement finishes
	await get_tree().create_timer(0.1).timeout
	while player.is_moving():
		await get_tree().create_timer(0.05).timeout
	_refresh_reachable()
