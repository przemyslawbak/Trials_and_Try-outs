extends Node2D

const ARROW_COLOR_YELLOW := Color(1.0, 0.85, 0.0, 0.85)
const ARROW_COLOR_GREEN  := Color(0.2, 0.95, 0.35, 0.9)
const ARROW_COLOR_RED	:= Color(1.0, 0.2, 0.2, 0.85)
const SHAFT_WIDTH	  := 4.0
const HOP_HEIGHT       := 6.0
const DOT_RADIUS       := 7.0
const FACING_HEAD_SIZE := 8.0
const FACING_ORBIT     := 16.0

# ISO scale matches tile ratio 32x16 → x:y = 2:1
const ISO_X := 1.0
const ISO_Y := 0.5

var _path: Array[Vector2]	 = []
var _destination: Vector2	 = Vector2.INF
var _facing_dir: String	   = ""
var _show_facing_cursor: bool = false
var _confirmed_facing: bool   = false


func set_confirmed_facing_visual(is_confirmed: bool) -> void:
	_confirmed_facing = is_confirmed
	queue_redraw()


func _main_color() -> Color:
	return ARROW_COLOR_GREEN if _confirmed_facing else ARROW_COLOR_YELLOW


func _inner_dot_color() -> Color:
	return Color(0.75, 1.0, 0.8, 0.95) if _confirmed_facing else Color(1.0, 1.0, 0.6, 0.95)


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
			_draw_shaft(to_local(_path[i]), to_local(_path[i + 1]))
		_draw_iso_dot(to_local(_path[-1]))

	if _show_facing_cursor and _destination != Vector2.INF and _facing_dir != "":
		_draw_facing_arrowhead(to_local(_destination), _facing_dir)


# ── shaft ─────────────────────────────────────────────────────────────────────

func _draw_shaft(a: Vector2, b: Vector2) -> void:
	var mid := (a + b) * 0.5 + Vector2(0, -HOP_HEIGHT)
	var pts := _bezier_points(a, mid, b, 12)
	for i in range(pts.size() - 1):
		draw_line(pts[i], pts[i + 1], _main_color(), SHAFT_WIDTH, true)


# ── destination dot ───────────────────────────────────────────────────────────

var _iso_dot_outer: PackedVector2Array = []
var _iso_dot_inner: PackedVector2Array = []

var _arrowhead_polygons: Dictionary = {}

func _ready() -> void:
	var steps := 32
	for i in range(steps):
		var angle := TAU * i / steps
		_iso_dot_outer.append(Vector2(
			cos(angle) * DOT_RADIUS,
			sin(angle) * DOT_RADIUS * ISO_Y))
		_iso_dot_inner.append(Vector2(
			cos(angle) * DOT_RADIUS * 0.45,
			sin(angle) * DOT_RADIUS * 0.45 * ISO_Y))

	for direction in ["NE", "SE", "SW", "NW"]:
		var cart_dir := Vector2.ZERO
		match direction:
			"NE": cart_dir = Vector2( 1, -1).normalized()
			"SE": cart_dir = Vector2( 1,  1).normalized()
			"SW": cart_dir = Vector2(-1,  1).normalized()
			"NW": cart_dir = Vector2(-1, -1).normalized()

		var cart_tip  := cart_dir * (FACING_ORBIT * 1.25)
		var cart_base := cart_tip - cart_dir * (FACING_HEAD_SIZE * 1.25)
		var cart_perp  := Vector2(-cart_dir.y, cart_dir.x)
		var cart_left  := cart_base + cart_perp * 7.0
		var cart_right := cart_base - cart_perp * 7.0

		var tip   := Vector2(cart_tip.x, cart_tip.y * ISO_Y)
		var left  := Vector2(cart_left.x, cart_left.y * ISO_Y)
		var right := Vector2(cart_right.x, cart_right.y * ISO_Y)
		
		_arrowhead_polygons[direction] = PackedVector2Array([tip, left, right])

func _draw_iso_dot(center: Vector2) -> void:
	draw_set_transform(center, 0.0, Vector2.ONE)
	draw_colored_polygon(_iso_dot_outer, _main_color())
	draw_colored_polygon(_iso_dot_inner, _inner_dot_color())
	draw_set_transform(Vector2.ZERO, 0.0, Vector2.ONE)


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


func _facing_color() -> Color:
	return ARROW_COLOR_GREEN if _confirmed_facing else ARROW_COLOR_RED

func _draw_facing_arrowhead(center: Vector2, direction: String) -> void:
	if _arrowhead_polygons.has(direction):
		draw_set_transform(center, 0.0, Vector2.ONE)
		draw_colored_polygon(_arrowhead_polygons[direction], _facing_color())
		draw_set_transform(Vector2.ZERO, 0.0, Vector2.ONE)


# ── bezier helper ─────────────────────────────────────────────────────────────

func _bezier_points(p0: Vector2, p1: Vector2, p2: Vector2, steps: int) -> Array[Vector2]:
	var pts: Array[Vector2] = []
	for i in range(steps + 1):
		var t := float(i) / float(steps)
		pts.append((1.0-t)*(1.0-t)*p0 + 2.0*(1.0-t)*t*p1 + t*t*p2)
	return pts

# trailing line to keep editor caret restore safely in range
