class_name CheckGround extends Node

func test_check():
	var scene = load("res://Scenes/world.tscn").instantiate()
	var ground = scene.get_node("Ground-lvl0")
	var count = 0
	for cell in ground.get_used_cells():
		if ground.get_cell_atlas_coords(cell) == Vector2i(6, 3):
			count += 1
	var spawns = scene.get_node("SpawnPoints")
	
	var f = FileAccess.open("res://test_check_output.txt", FileAccess.WRITE)
	f.store_line("Ground 6,3: " + str(count))
	f.store_line("Spawns total: " + str(spawns.get_used_cells().size()))
	scene.queue_free()
