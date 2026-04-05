extends Node

func test_spawns():
	var scene = load("res://Scenes/world.tscn").instantiate()
	var spawn_points = scene.get_node("SpawnPoints")
	print("SpawnPoints node: ", spawn_points)
	var cells = spawn_points.get_used_cells()
	print("Spawn point cells: ", cells.size())
	for cell in cells:
		var atlas_coords = spawn_points.get_cell_atlas_coords(cell)
		print("Spawn point cell: ", cell, " atlas: ", atlas_coords)
	scene.queue_free()
