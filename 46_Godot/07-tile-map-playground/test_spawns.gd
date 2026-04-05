extends Node

func test_spawns():
	var scene = load("res://Scenes/world.tscn").instantiate()
	var spawn_points = []
	for layer_name in ["Ground-lvl0", "Ground-lvl1", "Decoration-lvl-0"]:
		var layer = scene.get_node_or_null(layer_name) as TileMapLayer
		if layer:
			var cells = layer.get_used_cells()
			for cell in cells:
				var atlas_coords = layer.get_cell_atlas_coords(cell)
				if atlas_coords == Vector2i(6, 3):
					spawn_points.append({"layer": layer_name, "cell": [cell.x, cell.y]})
	
	var file = FileAccess.open("res://spawns.json", FileAccess.WRITE)
	file.store_string(JSON.stringify(spawn_points))
	file.close()
