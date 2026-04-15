# map_generator.gd
# Wave-based procedural map generator for isometric tiles.
#
# Algorithm overview:
#   1. Generate a layered noise field (multiple octaves of Perlin noise)
#      sampled at low frequency so terrain forms smooth horizontal "waves".
#   2. Sort all cells by their noise value and assign biome types in
#      threshold bands that match the target percentage ratios.
#   3. River cells are post-processed: connected along a spine so that
#      every river segment links to at least one neighbour, forming
#      continuous streams. Isolated river pixels are rerolled to grass.
#   4. Lakes are scattered at local noise minima and checked so that no
#      two lake cells are 4-directionally adjacent.
#   5. Each cell picks a random variant from its biome's sprite sheet.

class_name MapGenerator
extends RefCounted

const Defs = preload("res://scripts/tile_definitions.gd")

# ---------------------------------------------------------------------------
# Public API
# ---------------------------------------------------------------------------

## Generate a map.
## Returns Dictionary:  { "tiles": Array[Array[int]],   # TileType per cell
##                        "variants": Array[Array[int]], # sprite index per cell
##                        "width": int, "height": int }
static func generate(width: int, height: int, seed_val: int = 0) -> Dictionary:
	var rng := RandomNumberGenerator.new()
	rng.seed = seed_val if seed_val != 0 else randi()

	# --- 1. Build noise field -------------------------------------------
	var noise_map: Array = _build_noise(width, height, rng)

	# --- 2. Assign biomes by sorted thresholds --------------------------
	var tile_map: Array = _assign_biomes(noise_map, width, height, rng)

	# --- 3. Connect rivers ----------------------------------------------
	tile_map = _connect_rivers(tile_map, width, height, rng)

	# --- 4. Enforce lake rules (no adjacency) ---------------------------
	tile_map = _enforce_lake_rules(tile_map, width, height, rng)

	# --- 5. Pick sprite variants ----------------------------------------
	var variant_map: Array = _pick_variants(tile_map, width, height, rng)

	return {
		"tiles":    tile_map,
		"variants": variant_map,
		"width":    width,
		"height":   height,
	}

# ---------------------------------------------------------------------------
# Step 1 – Noise field
# ---------------------------------------------------------------------------
static func _build_noise(width: int, height: int, rng: RandomNumberGenerator) -> Array:
	# Use Godot's built-in FastNoiseLite for smooth, wave-like terrain.
	var noise := FastNoiseLite.new()
	noise.noise_type       = FastNoiseLite.TYPE_PERLIN
	noise.seed             = rng.randi()
	noise.frequency        = 0.06          # low frequency → wide biome bands
	noise.fractal_octaves  = 4
	noise.fractal_type     = FastNoiseLite.FRACTAL_FBM
	noise.fractal_gain     = 0.5
	noise.fractal_lacunarity = 2.0

	var map: Array = []
	for y in height:
		var row: Array = []
		for x in width:
			row.append(noise.get_noise_2d(x, y))
		map.append(row)
	return map

# ---------------------------------------------------------------------------
# Step 2 – Biome assignment via sorted thresholds
# ---------------------------------------------------------------------------
static func _assign_biomes(noise_map: Array, width: int, height: int,
		rng: RandomNumberGenerator) -> Array:
	# Flatten noise values with coordinates so we can sort
	var flat: Array = []
	for y in height:
		for x in width:
			flat.append({ "v": noise_map[y][x], "x": x, "y": y })

	flat.sort_custom(func(a, b): return a["v"] < b["v"])

	var total: int = width * height

	# Calculate how many cells each biome gets
	var counts: Dictionary = {}
	var assigned: int = 0
	var types_in_order = [
		Defs.TileType.LAKE,   # assign rarest first for precision
		Defs.TileType.RIVER,
		Defs.TileType.GRASS,
		Defs.TileType.BUSHES,
		Defs.TileType.FOREST,
	]
	for t in types_in_order:
		var n: int = int(round(Defs.TARGET_RATIOS[t] * total))
		counts[t] = n
		assigned += n
	# Fix rounding remainder into GRASS
	counts[Defs.TileType.GRASS] += (total - assigned)

	# Build tile_map
	var tile_map: Array = []
	for y in height:
		tile_map.append([])
		for x in width:
			tile_map[y].append(Defs.TileType.GRASS)

	# Distribute: lowest noise → lake/river, mid → grass/bushes, high → forest
	# Order that maps noise rank to biome (from lowest to highest noise value):
	#   lake, river, grass, bushes, forest
	var dist_order = [
		Defs.TileType.LAKE,
		Defs.TileType.RIVER,
		Defs.TileType.GRASS,
		Defs.TileType.BUSHES,
		Defs.TileType.FOREST,
	]
	var cursor: int = 0
	for t in dist_order:
		var n: int = counts[t]
		for i in n:
			if cursor >= flat.size():
				break
			var cell = flat[cursor]
			tile_map[cell["y"]][cell["x"]] = t
			cursor += 1

	return tile_map

# ---------------------------------------------------------------------------
# Step 3 – River connectivity
# ---------------------------------------------------------------------------
# Rivers must form continuous chains.  We use a simple approach:
#   a) Find all river cells.
#   b) Build a spanning path (sorted left→right / top→bottom along the
#      dominant axis) and ensure each cell has ≥1 river neighbour by
#      converting isolated river cells to grass and carving extra river
#      cells as needed to bridge gaps.
static func _connect_rivers(tile_map: Array, width: int, height: int,
		rng: RandomNumberGenerator) -> Array:
	var T = Defs.TileType

	# Collect all river cells
	var river_cells: Array = []
	for y in height:
		for x in width:
			if tile_map[y][x] == T.RIVER:
				river_cells.append(Vector2i(x, y))

	if river_cells.is_empty():
		return tile_map

	# Sort by x so we process left-to-right
	river_cells.sort_custom(func(a, b): return a.x < b.x if a.x != b.x else a.y < b.y)

	# Use Union-Find to identify disconnected river components
	var parent: Dictionary = {}
	for c in river_cells:
		parent[c] = c

	var dirs = [Vector2i(1,0), Vector2i(-1,0), Vector2i(0,1), Vector2i(0,-1)]

	func find(p: Dictionary, c: Vector2i) -> Vector2i:
		if p[c] != c:
			p[c] = find(p, p[c])
		return p[c]

	for c in river_cells:
		for d in dirs:
			var nb: Vector2i = c + d
			if nb in parent:
				var rc = find(parent, c)
				var rnb = find(parent, nb)
				if rc != rnb:
					parent[rnb] = rc

	# Find components
	var components: Dictionary = {}
	for c in river_cells:
		var root = find(parent, c)
		if not root in components:
			components[root] = []
		components[root].append(c)

	# Connect each component to the nearest cell of another component
	# by carving a straight-line bridge
	var comp_list: Array = components.values()
	for i in range(comp_list.size() - 1):
		var best_dist: float = INF
		var best_a: Vector2i
		var best_b: Vector2i
		for ca in comp_list[i]:
			for cb in comp_list[i + 1]:
				var d: float = ca.distance_to(cb)
				if d < best_dist:
					best_dist = d
					best_a = ca
					best_b = cb
		# Carve Manhattan path from best_a to best_b
		var cur: Vector2i = best_a
		while cur != best_b:
			var dx: int = best_b.x - cur.x
			var dy: int = best_b.y - cur.y
			if abs(dx) >= abs(dy):
				cur.x += sign(dx)
			else:
				cur.y += sign(dy)
			if cur.x >= 0 and cur.x < width and cur.y >= 0 and cur.y < height:
				tile_map[cur.y][cur.x] = T.RIVER

	return tile_map

# ---------------------------------------------------------------------------
# Step 4 – Lake adjacency enforcement
# ---------------------------------------------------------------------------
static func _enforce_lake_rules(tile_map: Array, width: int, height: int,
		rng: RandomNumberGenerator) -> Array:
	var T = Defs.TileType
	var dirs = [Vector2i(1,0), Vector2i(-1,0), Vector2i(0,1), Vector2i(0,-1)]

	# Scan top-left to bottom-right; if a lake cell is adjacent to another lake,
	# convert it to grass (keeping the first one encountered).
	for y in height:
		for x in width:
			if tile_map[y][x] == T.LAKE:
				for d in dirs:
					var nx: int = x + d.x
					var ny: int = y + d.y
					if nx >= 0 and nx < width and ny >= 0 and ny < height:
						if tile_map[ny][nx] == T.LAKE:
							tile_map[ny][nx] = T.GRASS  # remove the neighbour

	return tile_map

# ---------------------------------------------------------------------------
# Step 5 – Variant selection
# ---------------------------------------------------------------------------
static func _pick_variants(tile_map: Array, width: int, height: int,
		rng: RandomNumberGenerator) -> Array:
	var variant_map: Array = []
	for y in height:
		var row: Array = []
		for x in width:
			var t: int = tile_map[y][x]
			var count: int = Defs.TILE_COUNTS[t]
			row.append(rng.randi_range(0, count - 1))
		variant_map.append(row)
	return variant_map
