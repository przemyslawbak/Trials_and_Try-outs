# map_generator.gd
class_name MapGenerator
extends RefCounted

static func setup_tileset(layer: TileMapLayer) -> void:
	var ts = layer.tile_set
	if not ts:
		ts = TileSet.new()
		layer.tile_set = ts
	
	ts.tile_shape = TileSet.TILE_SHAPE_ISOMETRIC
	ts.tile_layout = TileSet.TILE_LAYOUT_DIAMOND_DOWN
	ts.tile_size = Vector2i(64, 32)
	
	for type in TileDefinitions.TILE_TEXTURES:
		var source_id = type
		# Remove existing source if it's there
		if ts.has_source(source_id):
			ts.remove_source(source_id)
			
		var atlas_source = TileSetAtlasSource.new()
		atlas_source.texture = load(TileDefinitions.TILE_TEXTURES[type])
		atlas_source.texture_region_size = Vector2i(64, 32)
		
		var count = TileDefinitions.TILE_COUNTS[type]
		for i in range(count):
			var coords = Vector2i(i, 0)
			atlas_source.create_tile(coords)
			var tile_data = atlas_source.get_tile_data(coords, 0)
			if tile_data:
				tile_data.texture_origin = Vector2i(0, 0)
			
		ts.add_source(atlas_source, source_id)

static func generate_map(layer: TileMapLayer) -> void:
	setup_tileset(layer)
	
	var rect = Rect2i(0, 0, 30, 30)
		
	layer.clear()
	
	var fill_type = TileDefinitions.TileType.BASE_green_1
	
	for x in range(rect.position.x, rect.end.x):
		for y in range(rect.position.y, rect.end.y):
			layer.set_cell(Vector2i(x, y), fill_type, Vector2i(0, 0))
