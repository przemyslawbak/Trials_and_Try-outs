class_name SpawnPoints
extends TileMapLayer

const WATER_ATLAS_ROW := 10
var _black_cells: Array[Vector2i] = []

func _ready() -> void:
	var world := get_parent()
	var ground_lvl1 = world.get_node_or_null("Ground-lvl1") as TileMapLayer if world else null
	var dec_lvl0 = world.get_node_or_null("Decoration-lvl-0") as TileMapLayer if world else null
	
	var used := get_used_cells()
	var valid_cells: Array[Vector2i] = []
	
	for cell in used:
		var source_id := get_cell_source_id(cell)
		if source_id == -1:
			continue
			
		var atlas_coords := get_cell_atlas_coords(cell)
		if atlas_coords.y == WATER_ATLAS_ROW:
			continue
			
		var is_blocked = false
		if ground_lvl1 and ground_lvl1.get_cell_source_id(cell) != -1:
			is_blocked = true
		if dec_lvl0 and dec_lvl0.get_cell_source_id(cell) != -1:
			is_blocked = true
			
		if is_blocked:
			continue
			
		var is_edge = false
		for dx in range(-1, 2):
			for dy in range(-1, 2):
				if dx == 0 and dy == 0:
					continue
				if get_cell_source_id(cell + Vector2i(dx, dy)) == -1:
					is_edge = true
					break
			if is_edge:
				break
				
		if is_edge:
			continue
			
		valid_cells.append(cell)
		
	if valid_cells.size() > 0:
		valid_cells.shuffle()
		var limit = mini(3, valid_cells.size())
		_black_cells = valid_cells.slice(0, limit)
		
		notify_runtime_tile_data_update()

func _use_tile_data_runtime_update(coords: Vector2i) -> bool:
	return coords in _black_cells

func _tile_data_runtime_update(coords: Vector2i, tile_data: TileData) -> void:
	if coords in _black_cells:
		tile_data.modulate = Color.BLACK
