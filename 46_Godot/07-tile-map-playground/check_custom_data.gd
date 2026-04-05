extends Node

func test_custom_data():
	var scene = load("res://Scenes/world.tscn").instantiate()
	var ground = scene.get_node("Ground-lvl0")
	var cell = ground.get_used_cells()[0]
	var td = ground.get_cell_tile_data(cell)
	var f = FileAccess.open("res://output.txt", FileAccess.WRITE)
	if td:
		f.store_line("Physics shapes: " + str(td.get_collision_polygons_count(0)))
	scene.queue_free()
