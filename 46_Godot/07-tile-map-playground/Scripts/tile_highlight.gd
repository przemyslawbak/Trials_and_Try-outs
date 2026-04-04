extends Node2D

const ISO_WIDTH = 32.0
const ISO_HEIGHT = 16.0

var _highlighted_positions: Array[Vector2] = []

func set_highlights(positions: Array[Vector2]) -> void:
	_highlighted_positions = positions
	queue_redraw()

func clear_highlights() -> void:
	_highlighted_positions.clear()
	queue_redraw()

func _draw() -> void:
	var color = Color(1.0, 1.0, 0.0, 0.3)
	for pos in _highlighted_positions:
		var pts = PackedVector2Array([
			pos + Vector2(0, -ISO_HEIGHT / 2),
			pos + Vector2(ISO_WIDTH / 2, 0),
			pos + Vector2(0, ISO_HEIGHT / 2),
			pos + Vector2(-ISO_WIDTH / 2, 0)
		])
		draw_colored_polygon(pts, color)
