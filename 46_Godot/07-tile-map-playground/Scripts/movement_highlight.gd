class_name MovementHighlight
extends Node2D

const HIGHLIGHT_COLOR := Color(1.0, 1.0, 0.0, 0.3) # Semi-transparent yellow

# ISO dimensions
const TILE_WIDTH := 32.0
const TILE_HEIGHT := 16.0

var _reachable_positions: Array[Vector2] = []

func set_reachable_positions(positions: Array[Vector2]) -> void:
	_reachable_positions = positions
	queue_redraw()

func clear() -> void:
	_reachable_positions.clear()
	queue_redraw()

func _draw() -> void:
	for pos in _reachable_positions:
		_draw_iso_diamond(pos)

func _draw_iso_diamond(center: Vector2) -> void:
	var pts := PackedVector2Array([
		center + Vector2(0, -TILE_HEIGHT * 0.5), # Top
		center + Vector2(TILE_WIDTH * 0.5, 0),    # Right
		center + Vector2(0, TILE_HEIGHT * 0.5),  # Bottom
		center + Vector2(-TILE_WIDTH * 0.5, 0)    # Left
	])
	draw_colored_polygon(pts, HIGHLIGHT_COLOR)
