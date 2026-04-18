class_name SpawnPoints
extends TileMapLayer

const WATER_ATLAS_ROW := 10
var _black_cells: Array[Vector2i] = []

func _ready() -> void:
	update_spawn_points()

func update_spawn_points() -> void:
	_black_cells.clear()
	var world := get_parent()
	var ground_lvl1 = world.get_node_or_null("Ground-lvl1") as TileMapLayer if world else null
	var dec_lvl0 = world.get_node_or_null("Decoration-lvl-0") as TileMapLayer if world else null
	
	var used := get_used_cells()
	var valid_cells: Array[Vector2i] = []
	
	for cell in used:
		var source_id := get_cell_source_id(cell)
		if source_id == -1:
			continue
			
		# River and Lake types are water
		if source_id == TileDefinitions.TileType.RIVER or source_id == TileDefinitions.TileType.LAKE:
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
		
	if valid_cells.size() >= 3:
		var selected_cells: Array[Vector2i] = []
		var min_distance := 10.0
		
		# Try multiple times to find 3 spots that are far enough apart.
		# If the map is too small to fit them, gradually reduce the required distance to ensure 3 are always found.
		while selected_cells.size() < 3 and min_distance >= 0.0:
			for attempt in range(50):
				valid_cells.shuffle()
				selected_cells.clear()
				
				for cell in valid_cells:
					var valid_distance = true
					for s_cell in selected_cells:
						if cell.distance_to(s_cell) < min_distance:
							valid_distance = false
							break
					if valid_distance:
						selected_cells.append(cell)
					if selected_cells.size() == 3:
						break
						
				if selected_cells.size() == 3:
					break
					
			if selected_cells.size() < 3:
				min_distance -= 1.0
				
		_black_cells = selected_cells
		
	notify_runtime_tile_data_update()

func _use_tile_data_runtime_update(coords: Vector2i) -> bool:
	return coords in _black_cells

func _tile_data_runtime_update(coords: Vector2i, tile_data: TileData) -> void:
	if coords in _black_cells:
		tile_data.modulate = Color.BLACK
