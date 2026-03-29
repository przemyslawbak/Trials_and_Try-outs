extends Node2D

const ARROW_COLOR := Color(1.0, 0.85, 0.0, 0.85)
const SHAFT_WIDTH := 1.0
const HOP_HEIGHT  := 6.0
const DOT_RADIUS  := 7.0

var _path: Array[Vector2] = []


func set_path(positions: Array[Vector2]) -> void:
	_path = positions
	queue_redraw()


func clear_path() -> void:
	_path = []
	queue_redraw()


func _draw() -> void:
	if _path.size() < 2:
		return

	for i in range(_path.size() - 1):
		var a := _path[i]
		var b := _path[i + 1]
		_draw_shaft(a, b)

	# Draw isometric ellipse dot at the destination
	_draw_iso_dot(_path[-1])


func _draw_shaft(a: Vector2, b: Vector2) -> void:
	var mid := (a + b) * 0.5 + Vector2(0, -HOP_HEIGHT)
	var pts := _bezier_points(a, mid, b, 12)
	for i in range(pts.size() - 1):
		draw_line(pts[i], pts[i + 1], ARROW_COLOR, SHAFT_WIDTH, true)


func _draw_iso_dot(center: Vector2) -> void:
	# Isometric ellipse: full width, half height to match tile proportions
	var points := PackedVector2Array()
	var steps := 32
	for i in range(steps):
		var angle := TAU * i / steps
		var x := center.x + cos(angle) * DOT_RADIUS
		var y := center.y + sin(angle) * (DOT_RADIUS * 0.5)
		points.append(Vector2(x, y))
	draw_colored_polygon(points, ARROW_COLOR)

	# Bright inner highlight for depth
	var inner := PackedVector2Array()
	for i in range(steps):
		var angle := TAU * i / steps
		var x := center.x + cos(angle) * (DOT_RADIUS * 0.45)
		var y := center.y + sin(angle) * (DOT_RADIUS * 0.45 * 0.5)
		inner.append(Vector2(x, y))
	draw_colored_polygon(inner, Color(1.0, 1.0, 0.6, 0.95))


func _bezier_points(p0: Vector2, p1: Vector2, p2: Vector2, steps: int) -> Array[Vector2]:
	var pts: Array[Vector2] = []
	for i in range(steps + 1):
		var t := float(i) / float(steps)
		var q := (1.0 - t) * (1.0 - t) * p0 \
			   + 2.0 * (1.0 - t) * t * p1 \
			   + t * t * p2
		pts.append(q)
	return pts
