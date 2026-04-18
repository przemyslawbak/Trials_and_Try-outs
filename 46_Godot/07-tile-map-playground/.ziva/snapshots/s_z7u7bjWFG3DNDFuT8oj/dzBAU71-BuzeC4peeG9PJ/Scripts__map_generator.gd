# map_generator.gd
class_name MapGenerator
extends RefCounted

static func setup_tileset(layer: TileMapLayer) -> void:
	var ts = layer.tile_set
	if not ts:
		ts = TileSet.new()
		layer.tile_set = ts
	
	ts.tile_shape = TileSet.TILE_SHAPE_SQUARE
	ts.tile_size = Vector2i(32, 32)
	
	for type in TileDefinitions.TILE_TEXTURES:
		var source_id = type
		# Remove existing source if it's there
		if ts.has_source(source_id):
			ts.remove_source(source_id)
			
		var atlas_source = TileSetAtlasSource.new()
		atlas_source.texture = load(TileDefinitions.TILE_TEXTURES[type])
		atlas_source.texture_region_size = Vector2i(32, 32)
		
		var count = TileDefinitions.TILE_COUNTS[type]
		for i in range(count):
			var coords = Vector2i(i, 0)
			atlas_source.create_tile(coords)
			var tile_data = atlas_source.get_tile_data(coords, 0)
			if tile_data:
				tile_data.texture_origin = Vector2i.ZERO
			
		ts.add_source(atlas_source, source_id)

static func generate_map(layer: TileMapLayer) -> void:
	setup_tileset(layer)
	
	var rect = Rect2i(0, 0, 30, 30)
		
	layer.clear()
	
	var noises = {}
	var seeds = {}
	for type in TileDefinitions.WAVE_PARAMS:
		var n = FastNoiseLite.new()
		n.seed = randi()
		n.frequency = TileDefinitions.WAVE_PARAMS[type]["frequency"]
		n.fractal_octaves = TileDefinitions.WAVE_PARAMS[type]["octaves"]
		noises[type] = n
		
	for x in range(rect.position.x, rect.end.x):
		for y in range(rect.position.y, rect.end.y):
			var scores = []
			for type in noises:
				var n_val = noises[type].get_noise_2d(x, y)
				# Weight by TARGET_RATIOS to approximate distribution
				var score = n_val + (TileDefinitions.TARGET_RATIOS[type] * 2.0 - 0.5)
				scores.append({"type": type, "score": score})
			
			scores.sort_custom(func(a, b): return b.score < a.score)
			
			var best_type = scores[0].type
			
			# Lake constraint: No adjacent lakes
			if best_type == TileDefinitions.TileType.LAKE:
				var has_lake_neighbor = false
				# Check all 8 neighbors (some might not be placed yet, which is fine for one-pass)
				for dx in [-1, 0, 1]:
					for dy in [-1, 0, 1]:
						if dx == 0 and dy == 0: continue
						if layer.get_cell_source_id(Vector2i(x + dx, y + dy)) == TileDefinitions.TileType.LAKE:
							has_lake_neighbor = true
							break
					if has_lake_neighbor: break
				
				if has_lake_neighbor:
					# Use the second best type instead
					best_type = scores[1].type
			
			var count = TileDefinitions.TILE_COUNTS[best_type]
			var variant = randi() % count
			layer.set_cell(Vector2i(x, y), best_type, Vector2i(variant, 0))
