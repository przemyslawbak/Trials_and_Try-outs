# tile_renderer.gd
# Loads all tile spritesheets and renders the map using a Node2D with
# draw_texture_rect calls inside _draw().  For larger maps consider
# switching to a TileMap or MultiMeshInstance2D.

class_name TileRenderer
extends Node2D

const Defs = preload("res://scripts/tile_definitions.gd")

const TILE_W: int = 32
const TILE_H: int = 32

# Loaded textures keyed by TileType
var _textures: Dictionary = {}

# Current map data (set via set_map_data)
var _map_data: Dictionary = {}

# Optional: pan offset (pixels)
var _offset: Vector2 = Vector2.ZERO

# ---------------------------------------------------------------------------

func _ready() -> void:
	_load_textures()

func _load_textures() -> void:
	for t in Defs.TILE_TEXTURES:
		var path: String = Defs.TILE_TEXTURES[t]
		var tex = load(path)
		if tex == null:
			push_error("TileRenderer: could not load texture %s" % path)
		else:
			_textures[t] = tex

## Call this to display a newly generated map.
func set_map_data(data: Dictionary) -> void:
	_map_data = data
	queue_redraw()

## Scroll the view
func set_offset(offset: Vector2) -> void:
	_offset = offset
	queue_redraw()

# ---------------------------------------------------------------------------
func _draw() -> void:
	if _map_data.is_empty():
		return

	var tiles: Array    = _map_data["tiles"]
	var variants: Array = _map_data["variants"]
	var w: int          = _map_data["width"]
	var h: int          = _map_data["height"]

	for y in h:
		for x in w:
			var tile_type: int = tiles[y][x]
			var variant: int   = variants[y][x]

			if not tile_type in _textures:
				continue

			var tex = _textures[tile_type]
			# Source rect: each variant occupies one 32×32 column in the sheet
			var src_rect := Rect2(variant * TILE_W, 0, TILE_W, TILE_H)
			# Destination rect
			var dst_rect := Rect2(
				_offset.x + x * TILE_W,
				_offset.y + y * TILE_H,
				TILE_W, TILE_H
			)
			draw_texture_rect_region(tex, dst_rect, src_rect)

# ---------------------------------------------------------------------------
# Convenience: return the pixel colour of a tile type for minimap use
static func type_color(tile_type: int) -> Color:
	match tile_type:
		Defs.TileType.GRASS:  return Color(0.35, 0.65, 0.25)
		Defs.TileType.BUSHES: return Color(0.20, 0.50, 0.15)
		Defs.TileType.FOREST: return Color(0.10, 0.35, 0.08)
		Defs.TileType.RIVER:  return Color(0.15, 0.45, 0.80)
		Defs.TileType.LAKE:   return Color(0.05, 0.30, 0.70)
		_:                    return Color.MAGENTA
