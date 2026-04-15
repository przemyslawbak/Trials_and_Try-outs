# main.gd
# Main scene controller.
# – Generates a map on startup (and on button press)
# – Handles camera pan with mouse drag / arrow keys
# – Shows a minimap overlay
# – Displays biome percentage stats

extends Node2D

const MapGenerator = preload("res://scripts/map_generator.gd")
const TileRenderer = preload("res://scripts/tile_renderer.gd")
const Defs         = preload("res://scripts/tile_definitions.gd")

# ---- Editable map settings (change in the Inspector or via the UI) --------
@export var map_width:  int = 40
@export var map_height: int = 25
@export var map_seed:   int = 0   # 0 = random each run

# ---- Node references (wired in the scene) --------------------------------
@onready var renderer:       TileRenderer = $TileRenderer
@onready var minimap:        Node2D       = $UI/Minimap
@onready var stats_label:    Label        = $UI/StatsLabel
@onready var seed_label:     Label        = $UI/SeedLabel
@onready var regen_button:   Button       = $UI/RegenButton
@onready var width_spin:     SpinBox      = $UI/WidthSpin
@onready var height_spin:    SpinBox      = $UI/HeightSpin
@onready var seed_spin:      SpinBox      = $UI/SeedSpin
@onready var camera:         Camera2D     = $Camera2D

# ---- State ---------------------------------------------------------------
var _current_map: Dictionary = {}
var _drag_start:  Vector2    = Vector2.ZERO
var _dragging:    bool       = false
var _used_seed:   int        = 0

# ---------------------------------------------------------------------------
func _ready() -> void:
	width_spin.value  = map_width
	height_spin.value = map_height
	seed_spin.value   = map_seed
	regen_button.pressed.connect(_on_regen_pressed)
	_generate()

# ---------------------------------------------------------------------------
func _generate() -> void:
	map_width  = int(width_spin.value)
	map_height = int(height_spin.value)
	map_seed   = int(seed_spin.value)

	_used_seed = map_seed if map_seed != 0 else randi()
	_current_map = MapGenerator.generate(map_width, map_height, _used_seed)

	renderer.set_map_data(_current_map)

	seed_label.text = "Seed: %d" % _used_seed
	_update_stats()
	_redraw_minimap()

	# Center camera on map
	var map_px_w: float = map_width  * 32.0
	var map_px_h: float = map_height * 32.0
	camera.position = Vector2(map_px_w * 0.5, map_px_h * 0.5)

func _on_regen_pressed() -> void:
	_generate()

# ---------------------------------------------------------------------------
# Stats
# ---------------------------------------------------------------------------
func _update_stats() -> void:
	if _current_map.is_empty():
		return

	var tiles: Array = _current_map["tiles"]
	var total: int   = map_width * map_height
	var counts: Dictionary = {}
	for t in Defs.TileType.values():
		counts[t] = 0

	for row in tiles:
		for cell in row:
			counts[cell] += 1

	var text: String = "Biome distribution:\n"
	for t in [Defs.TileType.GRASS, Defs.TileType.BUSHES,
			  Defs.TileType.FOREST, Defs.TileType.RIVER, Defs.TileType.LAKE]:
		var pct: float = counts[t] / float(total) * 100.0
		var target: float = Defs.TARGET_RATIOS[t] * 100.0
		text += "  %s: %.1f%%  (target %.0f%%)\n" % [
			Defs.TYPE_NAMES[t], pct, target
		]
	stats_label.text = text

# ---------------------------------------------------------------------------
# Minimap
# ---------------------------------------------------------------------------
func _redraw_minimap() -> void:
	# Handled in MinimapNode._draw()
	minimap.queue_redraw()

# ---------------------------------------------------------------------------
# Input: drag to pan
# ---------------------------------------------------------------------------
func _unhandled_input(event: InputEvent) -> void:
	if event is InputEventMouseButton:
		if event.button_index == MOUSE_BUTTON_LEFT:
			_dragging = event.pressed
			if _dragging:
				_drag_start = event.position

	elif event is InputEventMouseMotion and _dragging:
		camera.position -= event.relative / camera.zoom

	elif event is InputEventKey and event.pressed:
		var step: float = 32.0 / camera.zoom.x
		match event.keycode:
			KEY_LEFT:  camera.position.x -= step
			KEY_RIGHT: camera.position.x += step
			KEY_UP:    camera.position.y -= step
			KEY_DOWN:  camera.position.y += step
			KEY_EQUAL, KEY_KP_ADD:
				camera.zoom = (camera.zoom * 1.2).clamp(Vector2(0.3, 0.3), Vector2(4.0, 4.0))
			KEY_MINUS, KEY_KP_SUBTRACT:
				camera.zoom = (camera.zoom * 0.8).clamp(Vector2(0.3, 0.3), Vector2(4.0, 4.0))

	elif event is InputEventMouseButton:
		if event.button_index == MOUSE_BUTTON_WHEEL_UP:
			camera.zoom = (camera.zoom * 1.1).clamp(Vector2(0.3, 0.3), Vector2(4.0, 4.0))
		elif event.button_index == MOUSE_BUTTON_WHEEL_DOWN:
			camera.zoom = (camera.zoom * 0.9).clamp(Vector2(0.3, 0.3), Vector2(4.0, 4.0))

# Expose map reference to the minimap node
func get_current_map() -> Dictionary:
	return _current_map
