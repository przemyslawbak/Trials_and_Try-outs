# minimap_node.gd
# Draws a colour-coded minimap of the current map.
# Attach this to the Minimap Node2D in the UI layer.

extends Node2D

const TileRenderer = preload("res://scripts/tile_renderer.gd")

const CELL_SIZE: int = 3   # pixels per cell on the minimap
const BORDER:    int = 1

func _draw() -> void:
	var main_node = get_tree().get_first_node_in_group("main")
	if main_node == null:
		return

	var map_data: Dictionary = main_node.get_current_map()
	if map_data.is_empty():
		return

	var tiles: Array = map_data["tiles"]
	var w: int       = map_data["width"]
	var h: int       = map_data["height"]

	# Background
	draw_rect(Rect2(-BORDER, -BORDER,
		w * CELL_SIZE + BORDER * 2,
		h * CELL_SIZE + BORDER * 2),
		Color(0, 0, 0, 0.6))

	for y in h:
		for x in w:
			var col: Color = TileRenderer.type_color(tiles[y][x])
			draw_rect(Rect2(x * CELL_SIZE, y * CELL_SIZE, CELL_SIZE, CELL_SIZE), col)
