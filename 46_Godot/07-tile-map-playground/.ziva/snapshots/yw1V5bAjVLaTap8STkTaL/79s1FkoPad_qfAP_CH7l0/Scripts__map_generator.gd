# map_generator.gd
class_name MapGenerator
extends RefCounted

static func setup_tileset(layer: TileMapLayer) -> void:
	var ts = layer.tile_set
	if not ts:
		ts = TileSet.new()
		ts.tile_shape = TileSet.TILE_SHAPE_ISOMETRIC
		ts.tile_size = Vector2i(32, 16)
		layer.tile_set = ts
	
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
			var tile_data = atlas_source.get_tile_data(coords)
			if tile_data:
				tile_data.texture_origin = Vector2i(0, 8)
			
		ts.add_source(atlas_source, source_id)

static func generate_map(layer: TileMapLayer) -> void:
	setup_tileset(layer)
	
	var rect = layer.get_used_rect()
	if rect.size == Vector2i.ZERO:
		# Use a default size if no tiles exist (e.g. 30x30)
		rect = Rect2i(0, 0, 30, 30)
		
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
			var best_type = TileDefinitions.TileType.GRASS
			var max_score = -100.0
			
			for type in noises:
				var n_val = noises[type].get_noise_2d(x, y)
				# Weight by TARGET_RATIOS to approximate distribution
				var score = n_val + (TileDefinitions.TARGET_RATIOS[type] * 2.0 - 0.5)
				if score > max_score:
					max_score = score
					best_type = type
			
			var count = TileDefinitions.TILE_COUNTS[best_type]
			var variant = randi() % count
			layer.set_cell(Vector2i(x, y), best_type, Vector2i(variant, 0))
