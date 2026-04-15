# tile_renderer.gd
# Attach to a Node2D.  Call set_map_data() to display a generated map.
# Renders each 32x32 tile by sampling the correct column from its spritesheet.

extends Node2D

const Defs = preload("res://scripts/tile_definitions.gd")

const TILE_W: int = 32
const TILE_H: int = 32

var _textures: Dictionary = {}
var _map_data: Dictionary = {}

func _ready() -> void:
	_load_textures()

func _load_textures() -> void:
	for t in Defs.TILE_TEXTURES:
		var tex = load(Defs.TILE_TEXTURES[t])
		if tex == null:
			push_error("TileRenderer: cannot load %s" % Defs.TILE_TEXTURES[t])
		else:
			_textures[t] = tex

func set_map_data(data: Dictionary) -> void:
	_map_data = data
	queue_redraw()

func _draw() -> void:
	if _map_data.is_empty():
		return

	var tiles:    Array = _map_data["tiles"]
	var variants: Array = _map_data["variants"]
	var w: int          = _map_data["width"]
	var h: int          = _map_data["height"]

	for y in h:
		for x in w:
			var t: int  = tiles[y][x]
			var v: int  = variants[y][x]
			if not _textures.has(t):
				continue
			var src := Rect2(v * TILE_W, 0, TILE_W, TILE_H)
			var dst := Rect2(x * TILE_W,  y * TILE_H, TILE_W, TILE_H)
			draw_texture_rect_region(_textures[t], dst, src)

## Colour used by the minimap for each tile type
static func type_color(tile_type: int) -> Color:
	match tile_type:
		Defs.TileType.GRASS:  return Color(0.35, 0.65, 0.25)
		Defs.TileType.BUSHES: return Color(0.20, 0.50, 0.15)
		Defs.TileType.FOREST: return Color(0.10, 0.35, 0.08)
		Defs.TileType.RIVER:  return Color(0.15, 0.45, 0.80)
		Defs.TileType.LAKE:   return Color(0.05, 0.30, 0.70)
		_:                    return Color.MAGENTA
