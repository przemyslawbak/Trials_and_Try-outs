## map_generator.gd
## Attach to the World (Node2D) root node.
##
## Scene tree:
##   World (Node2D)   ← this script
##     └─ Ground-lvl0    (TileMapLayer)
##        └─ Camera2D
##     └─ Ground-bottom  (TileMapLayer)
##     └─ Ground-front   (TileMapLayer)
##
## tileset_builder.gd must be in the same folder (or anywhere on res://).
## No manual TileSet setup in the editor is required — the builder creates
## everything at runtime.

class_name MapGenerator
extends RefCounted

# ---------------------------------------------------------------------------
# MAP DIMENSIONS
# ---------------------------------------------------------------------------
const MAP_WIDTH  : int = 32
const MAP_HEIGHT : int = 32

# ---------------------------------------------------------------------------
# SOURCE IDS  (must match TileSetBuilder._add_source calls)
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
# TILE COUNTS
# ---------------------------------------------------------------------------
const COUNT_GREEN_1          : int = 1
const COUNT_TURF             : int = 1
const COUNT_FOREST           : int = 5
const COUNT_MEADOW           : int = 5
const COUNT_MOUNTAINS        : int = 5
const COUNT_MTN_PLAIN        : int = 14
const COUNT_TURF_PLAIN       : int = 17
const COUNT_GRASS_PLAIN      : int = 18
const COUNT_GRASS_FLOWERS    : int = 19
const COUNT_FOREST_SPRUCE    : int = 19
const COUNT_FOREST_MIXED     : int = 16
const COUNT_FOREST_DECIDUOUS : int = 18

# ---------------------------------------------------------------------------
# BIOME THRESHOLDS
# ---------------------------------------------------------------------------
enum Biome { MOUNTAINS, MEADOW, FOREST, TURF, WATER }

# ---------------------------------------------------------------------------
# STATE
# ---------------------------------------------------------------------------
var layer0 : TileMapLayer
var layer1 : TileMapLayer
var layer2 : TileMapLayer

var _biome_map : Array = []

var _elevation_noise := FastNoiseLite.new()
var _moisture_noise := FastNoiseLite.new()

# ===========================================================================
# PUBLIC API
# ===========================================================================
static func generate_map(base_node: Node2D) -> void:
	var gen := new()
	gen._initialize(base_node)
	gen._assign_tilesets()
	gen._clear_all_layers()
	gen._build_biome_map()
	gen._populate_layer0()
	gen._populate_layer1()
	gen._populate_layer2()

# ===========================================================================
# PRIVATE – SETUP
# ===========================================================================
func _initialize(base_node: Node2D) -> void:
	var parent = base_node.get_parent()
	layer0 = parent.get_node("Ground-lvl0") as TileMapLayer
	layer1 = parent.get_node("Ground-bottom") as TileMapLayer
	layer2 = parent.get_node("Ground-front") as TileMapLayer
	
	randomize()
	_elevation_noise.seed = randi()
	_elevation_noise.noise_type = FastNoiseLite.TYPE_SIMPLEX
	_elevation_noise.frequency = 0.05
	
	_moisture_noise.seed = randi()
	_moisture_noise.noise_type = FastNoiseLite.TYPE_SIMPLEX
	_moisture_noise.frequency = 0.05

func _assign_tilesets() -> void:
	var ts_base  : TileSet = TileSetBuilder.build_base_tileset()
	var ts_front : TileSet = TileSetBuilder.build_front_tileset()

	layer0.tile_set = ts_base
	layer1.tile_set = ts_base
	layer2.tile_set = ts_front

func _clear_all_layers() -> void:
	layer0.clear()
	layer1.clear()
	layer2.clear()
	_biome_map.clear()

func _build_biome_map() -> void:
	_biome_map.resize(MAP_WIDTH)
	for x in MAP_WIDTH:
		_biome_map[x] = []
		_biome_map[x].resize(MAP_HEIGHT)
		for y in MAP_HEIGHT:
			_biome_map[x][y] = _determine_biome(x, y)

func _determine_biome(x: int, y: int) -> Biome:
	var elevation = _elevation_noise.get_noise_2d(x, y)
	var moisture = _moisture_noise.get_noise_2d(x, y)
	
	if elevation > 0.4:
		return Biome.MOUNTAINS
	elif elevation < -0.2:
		return Biome.TURF
	else:
		if moisture > 0.1:
			return Biome.FOREST
		else:
			return Biome.MEADOW

# ===========================================================================
# LAYER 0  – solid green base
# ===========================================================================
func _populate_layer0() -> void:
	for x in MAP_WIDTH:
		for y in MAP_HEIGHT:
			layer0.set_cell(Vector2i(x, y), SRC_GREEN_1,
				_random_atlas_coords(COUNT_GREEN_1))

# ===========================================================================
# LAYER 1  – biome ground tiles
# ===========================================================================
func _populate_layer1() -> void:
	for x in MAP_WIDTH:
		for y in MAP_HEIGHT:
			var biome : Biome = _biome_map[x][y]
			var src   : int
			var count : int
			match biome:
				Biome.MOUNTAINS: src = SRC_MOUNTAINS; count = COUNT_MOUNTAINS
				Biome.MEADOW:    src = SRC_MEADOW;    count = COUNT_MEADOW
				Biome.FOREST:    src = SRC_FOREST;    count = COUNT_FOREST
				Biome.TURF:      src = SRC_TURF;      count = COUNT_TURF
				_:               src = SRC_TURF;      count = COUNT_TURF
			
			layer1.set_cell(Vector2i(x, y), src, _random_atlas_coords(count))

# ===========================================================================
# LAYER 2  – front detail tiles
# ===========================================================================
func _populate_layer2() -> void:
	for x in MAP_WIDTH:
		for y in MAP_HEIGHT:
			_place_front_tile(x, y, _biome_map[x][y])

func _place_front_tile(base_x: int, base_y: int, biome: Biome) -> void:
	var src   : int = -1
	var count : int = 0
	var roll  : float = randf()
	
	match biome:
		Biome.MOUNTAINS:
			if roll < 0.6:
				src = SRC_MTN_PLAIN;   count = COUNT_MTN_PLAIN
		Biome.TURF:
			if roll < 0.3:
				src = SRC_TURF_PLAIN;  count = COUNT_TURF_PLAIN
		Biome.MEADOW:
			if roll < 0.8:
				if randf() < 0.5:
					src = SRC_GRASS_PLAIN;   count = COUNT_GRASS_PLAIN
				else:
					src = SRC_GRASS_FLOWERS; count = COUNT_GRASS_FLOWERS
		Biome.FOREST:
			if roll < 0.33:
				src = SRC_FOREST_SPRUCE;    count = COUNT_FOREST_SPRUCE
			elif roll < 0.66:
				src = SRC_FOREST_MIXED;     count = COUNT_FOREST_MIXED
			else:
				src = SRC_FOREST_DECIDUOUS; count = COUNT_FOREST_DECIDUOUS

	if src != -1:
		layer2.set_cell(Vector2i(base_x, base_y), src, _random_atlas_coords(count))

# ===========================================================================
# HELPER
# ===========================================================================
func _random_atlas_coords(tile_count: int) -> Vector2i:
	return Vector2i(randi() % tile_count, 0)
