# minimap_node.gd
# Colour-coded minimap overlay.  Attach to the Minimap Node2D inside UI.

extends Node2D

const CELL_SIZE: int = 3
const BORDER:    int = 1

func _draw() -> void:
	# Reach up to the Main node (in group "main") to get the current map
	var nodes: Array = get_tree().get_nodes_in_group("main")
	if nodes.is_empty():
		return
	var main_node = nodes[0]
	if not main_node.has_method("get_current_map"):
		return

	var map_data: Dictionary = main_node.get_current_map()
	if map_data.is_empty():
		return

	var tiles: Array = map_data["tiles"]
	var w: int       = map_data["width"]
	var h: int       = map_data["height"]

	# Dark background border
	draw_rect(
		Rect2(-BORDER, -BORDER, w * CELL_SIZE + BORDER * 2, h * CELL_SIZE + BORDER * 2),
		Color(0.0, 0.0, 0.0, 0.65)
	)

	for y in h:
		for x in w:
			var col: Color = _tile_color(tiles[y][x])
			draw_rect(Rect2(x * CELL_SIZE, y * CELL_SIZE, CELL_SIZE, CELL_SIZE), col)

func _tile_color(tile_type: int) -> Color:
	# Inline colours so we don't need a static cross-script call
	match tile_type:
		0: return Color(0.35, 0.65, 0.25)   # GRASS
		1: return Color(0.20, 0.50, 0.15)   # BUSHES
		2: return Color(0.10, 0.35, 0.08)   # FOREST
		3: return Color(0.15, 0.45, 0.80)   # RIVER
		4: return Color(0.05, 0.30, 0.70)   # LAKE
		_: return Color.MAGENTA
