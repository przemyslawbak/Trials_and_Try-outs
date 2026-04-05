extends Node

func test_spawns():
	var scene = load("res://Scenes/world.tscn").instantiate()
	var ground = scene.get_node("Ground-lvl0")
	var spawns = scene.get_node("SpawnPoints")
	
	var ground_cells = ground.get_used_cells()
	var count_6_3 = 0
	for cell in ground_cells:
		if ground.get_cell_atlas_coords(cell) == Vector2i(6, 3):
			count_6_3 += 1
			print("Ground has 6,3 at ", cell)
	
	print("Ground 6,3 count: ", count_6_3)
	
	var spawn_cells = spawns.get_used_cells()
	print("Spawn points count: ", spawn_cells.size())
	for cell in spawn_cells:
		if spawns.get_cell_atlas_coords(cell) == Vector2i(6, 3):
			print("SpawnPoints has 6,3 at ", cell)
