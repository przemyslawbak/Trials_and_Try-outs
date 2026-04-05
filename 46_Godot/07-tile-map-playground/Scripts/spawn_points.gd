class_name SpawnPoints
extends TileMapLayer

const WATER_ATLAS_ROW := 10
var _black_cells: Array[Vector2i] = []

func _ready() -> void:
	var world := get_parent()
	if not world:
		return
		
	var ground_layer := world.get_node_or_null("Ground-lvl0") as TileMapLayer
	if not ground_layer:
		return
		
	var used := ground_layer.get_used_cells()
	var valid_cells: Array[Vector2i] = []
	
	for cell in used:
		var source_id := ground_layer.get_cell_source_id(cell)
		if source_id == -1:
			continue
			
		var atlas_coords := ground_layer.get_cell_atlas_coords(cell)
		if atlas_coords.y == WATER_ATLAS_ROW:
			continue
			
		valid_cells.append(cell)
		
	if valid_cells.size() > 0:
		valid_cells.shuffle()
		var limit = mini(3, valid_cells.size())
		_black_cells = valid_cells.slice(0, limit)
		
		for cell in _black_cells:
			var source_id = ground_layer.get_cell_source_id(cell)
			var atlas_coords = ground_layer.get_cell_atlas_coords(cell)
			var alt_tile = ground_layer.get_cell_alternative_tile(cell)
			set_cell(cell, source_id, atlas_coords, alt_tile)
			
		notify_runtime_tile_data_update()

func _use_tile_data_runtime_update(coords: Vector2i) -> bool:
	return coords in _black_cells

func _tile_data_runtime_update(coords: Vector2i, tile_data: TileData) -> void:
	if coords in _black_cells:
		tile_data.modulate = Color.BLACK
