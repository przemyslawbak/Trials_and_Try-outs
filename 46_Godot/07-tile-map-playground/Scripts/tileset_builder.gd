## tileset_builder.gd
## Autoload or attach to World and call build() before map_generator runs.
##
## Usage (in World._ready, BEFORE generate_map()):
##
##   var ts_base   = TileSetBuilder.build_base_tileset()
##   var ts_front  = TileSetBuilder.build_front_tileset()
##   $"Ground-lvl0".tile_set   = ts_base
##   $"Ground-bottom".tile_set = ts_base
##   $"Ground-front".tile_set  = ts_front
##
## All PNG paths are relative to "res://".  Adjust if your folder layout differs.

class_name TileSetBuilder


# ---------------------------------------------------------------------------
# BASE TILESET  (layers 0 & 1)   tile_size = 64×32  isometric diamond-down
# ---------------------------------------------------------------------------
static func build_base_tileset() -> TileSet:
	var ts := TileSet.new()
	ts.tile_shape       = TileSet.TILE_SHAPE_ISOMETRIC
	ts.tile_layout      = TileSet.TILE_LAYOUT_DIAMOND_DOWN
	ts.tile_offset_axis = TileSet.TILE_OFFSET_AXIS_HORIZONTAL
	ts.tile_size        = Vector2i(64, 32)

	# Source 0 – green_1  (1 tile)
	_add_source(ts, 0,
		"res://MapSheets/Base/green_1.png",
		Vector2i(64, 64), 1)

	# Source 1 – turf  (1 tile)
	_add_source(ts, 1,
		"res://MapSheets/Ground/ground_turf_sheet.png",
		Vector2i(64, 64), 1)

	# Source 2 – forest  (5 tiles)
	_add_source(ts, 2,
		"res://MapSheets/Ground/ground_forest_sheet.png",
		Vector2i(64, 64), 5)

	# Source 3 – meadow  (5 tiles)
	_add_source(ts, 3,
		"res://MapSheets/Ground/ground_meadow_sheet.png",
		Vector2i(64, 64), 5)

	# Source 4 – mountains  (5 tiles)
	_add_source(ts, 4,
		"res://MapSheets/Ground/ground_mountains_sheet.png",
		Vector2i(64, 64), 5)

	return ts


# ---------------------------------------------------------------------------
# FRONT TILESET  (layer 2 / Ground-front)   tile_size = 32×16  isometric
# Quarter tiles are 32×42 px art in a 32×32 atlas cell.
# texture_origin is set to Vector2i(0, -26) so the extra 10 px of art
# (42 - 32 = 10, but with the isometric half-height of 16 the overlap is
# 42 - 16 = 26 px) hangs upward over the tile above, giving correct depth.
# ---------------------------------------------------------------------------
static func build_front_tileset() -> TileSet:
	var ts := TileSet.new()
	ts.tile_shape       = TileSet.TILE_SHAPE_ISOMETRIC
	ts.tile_layout      = TileSet.TILE_LAYOUT_DIAMOND_DOWN
	ts.tile_offset_axis = TileSet.TILE_OFFSET_AXIS_HORIZONTAL
	ts.tile_size        = Vector2i(64, 32)

	# Source 5 – mountains plain  (14 tiles)
	_add_source(ts, 5,
		"res://MapSheets/Quarters/mountains_plain_sheet.png",
		Vector2i(32, 32), 14,
		Vector2i.ZERO)

	# Source 6 – turf plain  (17 tiles)
	_add_source(ts, 6,
		"res://MapSheets/Quarters/turf_plain_sheet.png",
		Vector2i(32, 32), 17,
		Vector2i.ZERO)

	# Source 7 – grass plain  (18 tiles)
	_add_source(ts, 7,
		"res://MapSheets/Quarters/grass_plain_sheet.png",
		Vector2i(32, 32), 18,
		Vector2i.ZERO)

	# Source 8 – grass flowers  (19 tiles)
	_add_source(ts, 8,
		"res://MapSheets/Quarters/grass_flowers_sheet.png",
		Vector2i(32, 32), 19,
		Vector2i.ZERO)

	# Source 9 – forest spruce  (19 tiles)
	_add_source(ts, 9,
		"res://MapSheets/Quarters/forest_spruce_sheet.png",
		Vector2i(32, 32), 19,
		Vector2i.ZERO)

	# Source 10 – forest mixed  (16 tiles)
	_add_source(ts, 10,
		"res://MapSheets/Quarters/forest_mixed_sheet.png",
		Vector2i(32, 32), 16,
		Vector2i.ZERO)

	# Source 11 – forest deciduous  (18 tiles)
	_add_source(ts, 11,
		"res://MapSheets/Quarters/forest_dedicious_sheet.png",
		Vector2i(32, 32), 18,
		Vector2i.ZERO)

	return ts


# ---------------------------------------------------------------------------
# INTERNAL HELPER
# Adds a TileSetAtlasSource to `ts` at `source_id`.
# Tiles are arranged in a single horizontal row (col 0..tile_count-1, row 0).
# `region_size` is the size of one cell in the atlas texture.
# `origin`      is the texture_origin offset applied to every tile
#               (use Vector2i.ZERO for base tiles, Vector2i(0,-26) for quarters).
# ---------------------------------------------------------------------------
static func _add_source(
		ts          : TileSet,
		source_id   : int,
		texture_path: String,
		region_size : Vector2i,
		tile_count  : int,
		origin      : Vector2i = Vector2i.ZERO
) -> void:
	var tex : Texture2D = load(texture_path)
	if tex == null:
		push_error("TileSetBuilder: could not load texture: %s" % texture_path)
		return

	var src := TileSetAtlasSource.new()
	src.texture              = tex
	src.texture_region_size  = region_size
	src.use_texture_padding  = false

	for col in tile_count:
		var atlas_coord := Vector2i(col, 0)
		src.create_tile(atlas_coord)

		if origin != Vector2i.ZERO:
			# Access the TileData to set the texture origin per tile
			var tile_data : TileData = src.get_tile_data(atlas_coord, 0)
			if tile_data:
				tile_data.texture_origin = origin

	ts.add_source(src, source_id)
