# map_generator.gd
# Wave-based procedural map generator for isometric tiles.
#
# Algorithm:
#   1. FastNoiseLite Perlin FBM at low frequency → smooth biome "waves"
#   2. Sort all cells by noise value, slice into exact-ratio bands:
#      LAKE → RIVER → GRASS → BUSHES → FOREST  (lowest → highest noise)
#   3. River connectivity: Union-Find detects isolated river components,
#      Manhattan bridges connect them so river ends always link up.
#   4. Lake isolation: scan and convert adjacent lake pairs to grass.
#   5. Random variant selection per cell from each spritesheet.

class_name MapGenerator
extends RefCounted

const Defs = preload("res://scripts/tile_definitions.gd")

# ---------------------------------------------------------------------------
# Union-Find helpers (must be at class level – GDScript forbids nested funcs)
# ---------------------------------------------------------------------------

static func _uf_find(parent: Dictionary, key: Vector2i) -> Vector2i:
	if parent[key] != key:
		parent[key] = _uf_find(parent, parent[key])
	return parent[key]

static func _uf_union(parent: Dictionary, a: Vector2i, b: Vector2i) -> void:
	var ra: Vector2i = _uf_find(parent, a)
	var rb: Vector2i = _uf_find(parent, b)
	if ra != rb:
		parent[rb] = ra

# ---------------------------------------------------------------------------
# Public API
# ---------------------------------------------------------------------------

static func generate(width: int, height: int, seed_val: int = 0) -> Dictionary:
	var rng := RandomNumberGenerator.new()
	rng.seed = seed_val if seed_val != 0 else randi()

	var noise_map: Array = _build_noise(width, height, rng)
	var tile_map:  Array = _assign_biomes(noise_map, width, height)
	tile_map = _connect_rivers(tile_map, width, height)
	tile_map = _enforce_lake_rules(tile_map, width, height)
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
	var noise := FastNoiseLite.new()
	noise.noise_type         = FastNoiseLite.TYPE_PERLIN
	noise.seed               = rng.randi()
	noise.frequency          = 0.06
	noise.fractal_type       = FastNoiseLite.FRACTAL_FBM
	noise.fractal_octaves    = 4
	noise.fractal_gain       = 0.5
	noise.fractal_lacunarity = 2.0

	var map: Array = []
	for y in height:
		var row: Array = []
		for x in width:
			row.append(noise.get_noise_2d(float(x), float(y)))
		map.append(row)
	return map

# ---------------------------------------------------------------------------
# Step 2 – Biome assignment via sorted thresholds
# ---------------------------------------------------------------------------
static func _assign_biomes(noise_map: Array, width: int, height: int) -> Array:
	var flat: Array = []
	for y in height:
		for x in width:
			flat.append({ "v": noise_map[y][x], "x": x, "y": y })

	flat.sort_custom(func(a, b): return a["v"] < b["v"])

	var total: int = width * height
	var dist_order: Array = [
		Defs.TileType.LAKE,
		Defs.TileType.RIVER,
		Defs.TileType.GRASS,
		Defs.TileType.BUSHES,
		Defs.TileType.FOREST,
	]

	var counts: Dictionary = {}
	var assigned: int = 0
	for t in dist_order:
		var n: int = int(round(Defs.TARGET_RATIOS[t] * total))
		counts[t] = n
		assigned += n
	counts[Defs.TileType.GRASS] += (total - assigned)  # absorb rounding

	# Empty tile map
	var tile_map: Array = []
	for y in height:
		var row: Array = []
		for _x in width:
			row.append(Defs.TileType.GRASS)
		tile_map.append(row)

	var cursor: int = 0
	for t in dist_order:
		for _i in counts[t]:
			if cursor >= flat.size():
				break
			var cell = flat[cursor]
			tile_map[cell["y"]][cell["x"]] = t
			cursor += 1

	return tile_map

# ---------------------------------------------------------------------------
# Step 3 – River connectivity
# ---------------------------------------------------------------------------
static func _connect_rivers(tile_map: Array, width: int, height: int) -> Array:
	var T = Defs.TileType
	var dirs: Array[Vector2i] = [Vector2i(1,0), Vector2i(-1,0), Vector2i(0,1), Vector2i(0,-1)]

	var river_cells: Array = []
	for y in height:
		for x in width:
			if tile_map[y][x] == T.RIVER:
				river_cells.append(Vector2i(x, y))

	if river_cells.size() < 2:
		return tile_map

	var parent: Dictionary = {}
	for c: Vector2i in river_cells:
		parent[c] = c

	for c: Vector2i in river_cells:
		for d: Vector2i in dirs:
			var nb: Vector2i = c + d
			if nb in parent:
				_uf_union(parent, c, nb)

	var components: Dictionary = {}
	for c: Vector2i in river_cells:
		var root: Vector2i = _uf_find(parent, c)
		if not root in components:
			components[root] = []
		components[root].append(c)

	var comp_list: Array = components.values()
	if comp_list.size() < 2:
		return tile_map

	for i in range(comp_list.size() - 1):
		var best_sq: int  = 999999999
		var best_a: Vector2i = comp_list[i][0]
		var best_b: Vector2i = comp_list[i + 1][0]

		for ca: Vector2i in comp_list[i]:
			for cb: Vector2i in comp_list[i + 1]:
				var sq: int = ca.distance_squared_to(cb)
				if sq < best_sq:
					best_sq = sq
					best_a  = ca
					best_b  = cb

		# Carve Manhattan bridge
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
# Step 4 – Lake isolation (no two lakes adjacent)
# ---------------------------------------------------------------------------
static func _enforce_lake_rules(tile_map: Array, width: int, height: int) -> Array:
	var T = Defs.TileType
	var dirs: Array[Vector2i] = [Vector2i(1,0), Vector2i(-1,0), Vector2i(0,1), Vector2i(0,-1)]

	for y in height:
		for x in width:
			if tile_map[y][x] == T.LAKE:
				for d: Vector2i in dirs:
					var nx: int = x + d.x
					var ny: int = y + d.y
					if nx >= 0 and nx < width and ny >= 0 and ny < height:
						if tile_map[ny][nx] == T.LAKE:
							tile_map[ny][nx] = T.GRASS

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
			row.append(rng.randi_range(0, Defs.TILE_COUNTS[t] - 1))
		variant_map.append(row)
	return variant_map
