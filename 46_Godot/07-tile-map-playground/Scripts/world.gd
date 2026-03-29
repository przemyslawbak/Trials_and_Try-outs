extends Node2D

const DIRECTIONS := ["NE", "SE", "SW", "NW"]
const MIN_TILES := 0
const MAX_TILES := 3

@onready var player: CharacterBody2D = $Player
@onready var next_turn_button: Button = $UI/MarginContainer/NextTurnButton

var selected_direction: String = "SE"
var selected_tiles: int = 1

# Guard flag — prevents toggled handlers from firing during programmatic updates
var _updating := false


func _ready() -> void:
	next_turn_button.pressed.connect(_on_next_turn_button_pressed)
	next_turn_button.text = "Next Turn"
	_build_controls()


func _build_controls() -> void:
	var margin: MarginContainer = $UI/MarginContainer
	var vbox := VBoxContainer.new()
	vbox.name = "Controls"
	margin.add_child(vbox)
	margin.remove_child(next_turn_button)

	# Direction buttons
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

	# Tile count buttons
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
