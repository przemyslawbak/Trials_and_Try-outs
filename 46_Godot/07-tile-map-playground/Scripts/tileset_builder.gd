## tileset_builder.gd
## Static helper — builds TileSets entirely at runtime from sprite sheets.
## No manual editor setup required.
##
## Two TileSets are produced:
##   build_base_tileset()  → used by Ground-lvl0 and Ground-bottom
##   build_front_tileset() → used by Ground-front
##
## Base tiles:   64×64 px, isometric square grid (TileSet renders as 64×32)
## Front tiles:  32×48 px, isometric square grid (quarter-cell detail)

class_name TileSetBuilder
extends RefCounted

# ---------------------------------------------------------------------------
# PATHS
# ---------------------------------------------------------------------------
const PATH_GREEN_1          := "res://MapSheets/Base/green_1.png"

const PATH_TURF             := "res://MapSheets/Ground/ground_turf_sheet.png"
const PATH_FOREST           := "res://MapSheets/Ground/ground_forest_sheet.png"
const PATH_MEADOW           := "res://MapSheets/Ground/ground_meadow_sheet.png"
const PATH_MOUNTAINS        := "res://MapSheets/Ground/ground_mountains_sheet.png"

const PATH_MTN_PLAIN        := "res://MapSheets/Quarters/mountains_plain_sheet.png"
const PATH_TURF_PLAIN       := "res://MapSheets/Quarters/turf_plain_sheet.png"
const PATH_GRASS_PLAIN      := "res://MapSheets/Quarters/grass_plain_sheet.png"
const PATH_GRASS_FLOWERS    := "res://MapSheets/Quarters/grass_flowers_sheet.png"
const PATH_FOREST_SPRUCE    := "res://MapSheets/Quarters/forest_spruce_sheet.png"
const PATH_FOREST_MIXED     := "res://MapSheets/Quarters/forest_mixed_sheet.png"
const PATH_FOREST_DECIDUOUS := "res://MapSheets/Quarters/forest_dedicious_sheet.png"

# ---------------------------------------------------------------------------
# SOURCE IDs  (shared constants – keep in sync with MapGenerator)
# ---------------------------------------------------------------------------
const SRC_GREEN_1           : int = 0
const SRC_TURF              : int = 1
const SRC_FOREST            : int = 2
const SRC_MEADOW            : int = 3
const SRC_MOUNTAINS         : int = 4

const SRC_MTN_PLAIN         : int = 5
const SRC_TURF_PLAIN        : int = 6
const SRC_GRASS_PLAIN       : int = 7
const SRC_GRASS_FLOWERS     : int = 8
const SRC_FOREST_SPRUCE     : int = 9
const SRC_FOREST_MIXED      : int = 10
const SRC_FOREST_DECIDUOUS  : int = 11

# ---------------------------------------------------------------------------
# BASE TILESET  (64×64 tiles on a 64×32 isometric grid)
# ---------------------------------------------------------------------------
static func build_base_tileset() -> TileSet:
	var ts := TileSet.new()
	ts.tile_shape = TileSet.TILE_SHAPE_ISOMETRIC
	ts.tile_layout = TileSet.TILE_LAYOUT_DIAMOND_DOWN
	ts.tile_offset_axis = TileSet.TILE_OFFSET_AXIS_HORIZONTAL
	ts.tile_size = Vector2i(64, 32)

	_add_source(ts, SRC_GREEN_1,   PATH_GREEN_1,   Vector2i(64, 64), 1)
	_add_source(ts, SRC_TURF,      PATH_TURF,      Vector2i(64, 64), 1)
	_add_source(ts, SRC_FOREST,    PATH_FOREST,    Vector2i(64, 64), 5)
	_add_source(ts, SRC_MEADOW,    PATH_MEADOW,    Vector2i(64, 64), 5)
	_add_source(ts, SRC_MOUNTAINS, PATH_MOUNTAINS, Vector2i(64, 64), 5)

	return ts

# ---------------------------------------------------------------------------
# FRONT TILESET  (32×48 tiles on a 32×16 isometric grid for quarter cells)
# ---------------------------------------------------------------------------
static func build_front_tileset() -> TileSet:
	var ts := TileSet.new()
	ts.tile_shape = TileSet.TILE_SHAPE_ISOMETRIC
	ts.tile_layout = TileSet.TILE_LAYOUT_DIAMOND_DOWN
	ts.tile_offset_axis = TileSet.TILE_OFFSET_AXIS_HORIZONTAL
	# Quarter tiles are half the base width → 32×16 logical cell
	ts.tile_size = Vector2i(32, 16)

	_add_source(ts, SRC_MTN_PLAIN,         PATH_MTN_PLAIN,        Vector2i(32, 42), 14)
	_add_source(ts, SRC_TURF_PLAIN,        PATH_TURF_PLAIN,       Vector2i(32, 42), 17)
	_add_source(ts, SRC_GRASS_PLAIN,       PATH_GRASS_PLAIN,      Vector2i(32, 42), 18)
	_add_source(ts, SRC_GRASS_FLOWERS,     PATH_GRASS_FLOWERS,    Vector2i(32, 42), 19)
	_add_source(ts, SRC_FOREST_SPRUCE,     PATH_FOREST_SPRUCE,    Vector2i(32, 42), 19)
	_add_source(ts, SRC_FOREST_MIXED,      PATH_FOREST_MIXED,     Vector2i(32, 42), 16)
	_add_source(ts, SRC_FOREST_DECIDUOUS,  PATH_FOREST_DECIDUOUS, Vector2i(32, 42), 18)

	return ts

# ---------------------------------------------------------------------------
# INTERNAL
# ---------------------------------------------------------------------------

## Registers one TileSetAtlasSource for a horizontal strip of equal-sized tiles.
## source_id  – integer ID used in set_cell() calls
## path       – res:// path to the PNG
## tile_size  – pixel size of a single tile in the strip
## tile_count – number of tiles in the strip
static func _add_source(
		ts         : TileSet,
		source_id  : int,
		path       : String,
		tile_size  : Vector2i,
		tile_count : int) -> void:

	var tex : Texture2D = load(path)
	if tex == null:
		push_error("TileSetBuilder: cannot load texture: %s" % path)
		return

	var src := TileSetAtlasSource.new()
	src.texture = tex
	src.texture_region_size = tile_size

	# Register every tile in the horizontal strip
	for i in tile_count:
		var atlas_coord := Vector2i(i, 0)
		src.create_tile(atlas_coord)

	ts.add_source(src, source_id)
