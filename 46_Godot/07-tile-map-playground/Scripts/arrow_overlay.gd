extends Node2D

const ARROW_COLOR      := Color(1.0, 0.85, 0.0, 0.85)
const SHAFT_WIDTH      := 4.0
const HOP_HEIGHT       := 6.0
const DOT_RADIUS       := 7.0
const FACING_HEAD_SIZE := 8.0
const FACING_ORBIT     := 16.0

# ISO scale matches tile ratio 32x16 → x:y = 2:1
const ISO_X := 1.0
const ISO_Y := 0.5

var _path: Array[Vector2]     = []
var _destination: Vector2     = Vector2.INF
var _facing_dir: String       = ""
var _show_facing_cursor: bool = false


func set_path(positions: Array[Vector2]) -> void:
	_path = positions
	queue_redraw()


func clear_path() -> void:
	_path = []
	queue_redraw()


func show_facing_cursor(destination: Vector2, default_dir: String) -> void:
	_destination        = destination
	_facing_dir         = default_dir
	_show_facing_cursor = true
	queue_redraw()


func update_facing_cursor(direction: String) -> void:
	if _facing_dir == direction:
		return
	_facing_dir = direction
	queue_redraw()


func hide_facing_cursor() -> void:
	_show_facing_cursor = false
	queue_redraw()


func keep_facing_cursor(direction: String) -> void:
	# Keep cursor visible but frozen on confirmed direction
	_facing_dir         = direction
	_show_facing_cursor = true
	queue_redraw()


func _draw() -> void:
	if _path.size() >= 2:
		for i in range(_path.size() - 1):
			_draw_shaft(_path[i], _path[i + 1])
		_draw_iso_dot(_path[-1])

	if _show_facing_cursor and _destination != Vector2.INF and _facing_dir != "":
		_draw_facing_arrowhead(_destination, _facing_dir)


# ── shaft ─────────────────────────────────────────────────────────────────────

func _draw_shaft(a: Vector2, b: Vector2) -> void:
	var mid := (a + b) * 0.5 + Vector2(0, -HOP_HEIGHT)
	var pts := _bezier_points(a, mid, b, 12)
	for i in range(pts.size() - 1):
		draw_line(pts[i], pts[i + 1], ARROW_COLOR, SHAFT_WIDTH, true)


# ── destination dot ───────────────────────────────────────────────────────────

func _draw_iso_dot(center: Vector2) -> void:
	var steps := 32
	var outer := PackedVector2Array()
	var inner := PackedVector2Array()
	for i in range(steps):
		var angle := TAU * i / steps
		outer.append(Vector2(
			center.x + cos(angle) * DOT_RADIUS,
			center.y + sin(angle) * DOT_RADIUS * ISO_Y))
		inner.append(Vector2(
			center.x + cos(angle) * DOT_RADIUS * 0.45,
			center.y + sin(angle) * DOT_RADIUS * 0.45 * ISO_Y))
	draw_colored_polygon(outer, ARROW_COLOR)
	draw_colored_polygon(inner, Color(1.0, 1.0, 0.6, 0.95))


# ── facing arrowhead ──────────────────────────────────────────────────────────

func _iso_offset(direction: String) -> Vector2:
	# Each iso direction maps to a diagonal in screen space,
	# flattened by the tile ratio so the arrowhead sits exactly
	# on the isometric grid axes (NE = right+up, SE = right+down, etc.)
	match direction:
		"NE": return Vector2( ISO_X, -ISO_Y).normalized() * FACING_ORBIT
		"SE": return Vector2( ISO_X,  ISO_Y).normalized() * FACING_ORBIT
		"SW": return Vector2(-ISO_X,  ISO_Y).normalized() * FACING_ORBIT
		"NW": return Vector2(-ISO_X, -ISO_Y).normalized() * FACING_ORBIT
	return Vector2.ZERO


func _draw_facing_arrowhead(center: Vector2, direction: String) -> void:
	var offset  := _iso_offset(direction)
	var tip     := center + offset
	var dir_vec := offset.normalized()

	# Perp in isometric space: rotate dir_vec 90° then flatten Y
	var perp_raw := Vector2(-dir_vec.y, dir_vec.x)
	var perp     := Vector2(perp_raw.x * ISO_X, perp_raw.y * ISO_Y).normalized()

	var base  := tip - dir_vec * FACING_HEAD_SIZE
	var left  := base + perp * 2
	var right := base - perp * 1

	draw_colored_polygon(
		PackedVector2Array([tip, left, right]),
		Color(1.0, 1.0, 0.0, 1.0)
	)


# ── bezier helper ─────────────────────────────────────────────────────────────

func _bezier_points(p0: Vector2, p1: Vector2, p2: Vector2, steps: int) -> Array[Vector2]:
	var pts: Array[Vector2] = []
	for i in range(steps + 1):
		var t := float(i) / float(steps)
		pts.append((1.0-t)*(1.0-t)*p0 + 2.0*(1.0-t)*t*p1 + t*t*p2)
	return pts
