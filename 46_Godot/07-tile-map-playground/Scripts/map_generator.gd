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
##
## Call from World._ready():
##   MapGenerator.generate_map(self)

class_name MapGenerator
extends RefCounted

# ---------------------------------------------------------------------------
# MAP DIMENSIONS
# ---------------------------------------------------------------------------
const MAP_WIDTH  : int = 64
const MAP_HEIGHT : int = 32

# ---------------------------------------------------------------------------
# SOURCE IDS — base tileset (Ground-lvl0 & Ground-bottom)
# ---------------------------------------------------------------------------
const SRC_GREEN_1   : int = 0
const SRC_TURF      : int = 1
const SRC_FOREST    : int = 2
const SRC_MEADOW    : int = 3
const SRC_MOUNTAINS : int = 4

# ---------------------------------------------------------------------------
# SOURCE IDS — front tileset (Ground-front)
# ---------------------------------------------------------------------------
const SRC_MTN_PLAIN        : int = 5
const SRC_TURF_PLAIN       : int = 6
const SRC_GRASS_PLAIN      : int = 7
const SRC_GRASS_FLOWERS    : int = 8
const SRC_FOREST_SPRUCE    : int = 9
const SRC_FOREST_MIXED     : int = 10
const SRC_FOREST_DECIDUOUS : int = 11

# ---------------------------------------------------------------------------
# TILE COUNTS per atlas strip
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
# BIOME
# ---------------------------------------------------------------------------
enum Biome { MOUNTAINS, MEADOW, FOREST, TURF }

# ---------------------------------------------------------------------------
# WAVE NOISE — cumulative thresholds that produce the target distribution:
#   Mountains 20%  →  0.00 – 0.20
#   Meadow    40%  →  0.20 – 0.60
#   Forest    30%  →  0.60 – 0.90
#   Turf      10%  →  0.90 – 1.00
# FastNoiseLite is remapped from [-1,1] → [0,1] before comparison.
# ---------------------------------------------------------------------------
const THRESHOLD_MOUNTAINS : float = 0.20
const THRESHOLD_MEADOW    : float = 0.60   # cumulative: Meadow ends here
const THRESHOLD_FOREST    : float = 0.90   # cumulative: Forest ends here
# Turf fills the remainder (> 0.90)

# ---------------------------------------------------------------------------
# STATE
# ---------------------------------------------------------------------------
var layer0 : TileMapLayer   # Ground-lvl0   – green base
var layer1 : TileMapLayer   # Ground-bottom – biome ground
var layer2 : TileMapLayer   # Ground-front  – quarter details

var _biome_map : Array = []   # [x][y] → Biome

var _wave_noise := FastNoiseLite.new()

# ===========================================================================
# PUBLIC API
# ===========================================================================
static func generate_map(world_node: Node2D) -> void:
	var gen := MapGenerator.new()
	gen._initialize(world_node)
	gen._assign_tilesets()
	gen._clear_all_layers()
	gen._build_biome_map()
	gen._populate_layer0()
	gen._populate_layer1()
	gen._populate_layer2()

# ===========================================================================
# PRIVATE – SETUP
# ===========================================================================
func _initialize(world_node: Node2D) -> void:
	# world_node IS the World Node2D — children are direct siblings in the tree.
	layer0 = world_node.get_node("Ground-lvl0")   as TileMapLayer
	layer1 = world_node.get_node("Ground-bottom")  as TileMapLayer
	layer2 = world_node.get_node("Ground-front")   as TileMapLayer

	assert(layer0 != null, "MapGenerator: 'Ground-lvl0' TileMapLayer not found")
	assert(layer1 != null, "MapGenerator: 'Ground-bottom' TileMapLayer not found")
	assert(layer2 != null, "MapGenerator: 'Ground-front' TileMapLayer not found")

	randomize()

	# Single noise used as wave field; smoothed frequency gives organic blobs
	# with wave-like bands across the map.
	_wave_noise.seed        = randi()
	_wave_noise.noise_type  = FastNoiseLite.TYPE_SIMPLEX
	_wave_noise.frequency   = 0.04
	# FractalOctaves add wave detail without breaking the banded look
	_wave_noise.fractal_type    = FastNoiseLite.FRACTAL_FBM
	_wave_noise.fractal_octaves = 4

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

# ===========================================================================
# BIOME MAP
# ===========================================================================
func _build_biome_map() -> void:
	_biome_map.resize(MAP_WIDTH)
	for x in MAP_WIDTH:
		_biome_map[x] = []
		_biome_map[x].resize(MAP_HEIGHT)
		for y in MAP_HEIGHT:
			_biome_map[x][y] = _determine_biome(x, y)

func _determine_biome(x: int, y: int) -> Biome:
	# FastNoiseLite returns [-1, 1]; remap to [0, 1] for threshold comparison.
	var n : float = (_wave_noise.get_noise_2d(float(x), float(y)) + 1.0) * 0.5

	if n < THRESHOLD_MOUNTAINS:
		return Biome.MOUNTAINS
	elif n < THRESHOLD_MEADOW:
		return Biome.MEADOW
	elif n < THRESHOLD_FOREST:
		return Biome.FOREST
	else:
		return Biome.TURF

# ===========================================================================
# LAYER 0 – solid green base (every tile is green_1)
# ===========================================================================
func _populate_layer0() -> void:
	for x in MAP_WIDTH:
		for y in MAP_HEIGHT:
			# green_1 has only 1 tile, always atlas coord (0,0)
			layer0.set_cell(Vector2i(x, y), SRC_GREEN_1, Vector2i(0, 0))

# ===========================================================================
# LAYER 1 – biome ground tiles
# One random tile from the biome's sheet is chosen PER TILE CELL.
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

			layer1.set_cell(Vector2i(x, y), src, _rand_atlas(count))

# ===========================================================================
# LAYER 2 – front quarter-detail tiles
#
# Each base tile (64×32) is divided into 4 isometric quarter-diamonds:
#   N quarter  →  offset ( 0, -1) in the front layer's 32×16 grid
#   S quarter  →  offset ( 0,  0)
#   E quarter  →  offset ( 1, -1)
#   W quarter  →  offset (-1,  0)  — adjusted for isometric diamond layout
#
# The front TileSet uses tile_size 16×8 so that four 32×16 atlas tiles map
# exactly onto one 64×32 base tile. We convert the base cell's local-space
# center into the front layer's map coordinates and then shift by the four
# quarter offsets relative to that centre.
# ===========================================================================

# Quarter offsets in the front layer's map coordinate space (32×16 grid).
# In an isometric diamond-down layout the four sub-cells of a 64×32 tile are:
#   N = top diamond    E = right diamond
#   W = left diamond   S = bottom diamond
const QUARTER_OFFSETS : Array = [
	Vector2i( 0, -1),   # N  (top)
	Vector2i( 1,  0),   # E  (right)
	Vector2i(-1,  0),   # W  (left)
	Vector2i( 0,  1),   # S  (bottom) — unused if you only want 4 top quadrants;
						#              keep all 4 for full coverage
]

func _populate_layer2() -> void:
	for x in MAP_WIDTH:
		for y in MAP_HEIGHT:
			_place_quarter_tiles(x, y, _biome_map[x][y])

func _place_quarter_tiles(base_x: int, base_y: int, biome: Biome) -> void:
	# Convert base-layer cell → world position → front-layer cell.
	# Both layers share the same canvas transform, so local_to_map / map_to_local
	# from their respective TileMapLayers handles the different tile sizes.
	var world_pos : Vector2 = layer1.map_to_local(Vector2i(base_x, base_y))
	var front_center : Vector2i = layer2.local_to_map(world_pos)

	# Choose the source(s) for this biome.
	# For biomes with multiple sheets (meadow, forest) we pre-roll the split
	# once per base tile so all 4 quarters look coherent.
	var _roll : float = randf()

	for q_offset in QUARTER_OFFSETS:
		var cell : Vector2i = front_center + q_offset

		var src   : int = -1
		var count : int = 0

		match biome:
			Biome.MOUNTAINS:
				# 100% mountains_plain
				src   = SRC_MTN_PLAIN
				count = COUNT_MTN_PLAIN

			Biome.TURF:
				# 100% turf_plain
				src   = SRC_TURF_PLAIN
				count = COUNT_TURF_PLAIN

			Biome.MEADOW:
				# 50% grass_plain / 50% grass_flowers  (re-roll per quarter for variety)
				if randf() < 0.5:
					src = SRC_GRASS_PLAIN;   count = COUNT_GRASS_PLAIN
				else:
					src = SRC_GRASS_FLOWERS; count = COUNT_GRASS_FLOWERS

			Biome.FOREST:
				# 33% spruce / 33% mixed / 34% deciduous  (re-roll per quarter)
				var fr : float = randf()
				if fr < 0.33:
					src = SRC_FOREST_SPRUCE;    count = COUNT_FOREST_SPRUCE
				elif fr < 0.66:
					src = SRC_FOREST_MIXED;     count = COUNT_FOREST_MIXED
				else:
					src = SRC_FOREST_DECIDUOUS; count = COUNT_FOREST_DECIDUOUS

		if src != -1:
			layer2.set_cell(cell, src, _rand_atlas(count))

# ===========================================================================
# HELPER
# ===========================================================================
func _rand_atlas(tile_count: int) -> Vector2i:
	return Vector2i(randi() % tile_count, 0)
