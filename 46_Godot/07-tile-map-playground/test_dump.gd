class_name DumpTest extends Node

func test_dump():
	var scene = load("res://Scenes/world.tscn").instantiate()
	var spawns = scene.get_node("SpawnPoints")
	var f = FileAccess.open("res://dump.txt", FileAccess.WRITE)
	f.store_line("Spawns: " + str(spawns.get_used_cells().size()))
	for cell in spawns.get_used_cells():
		f.store_line("Cell: " + str(cell))
	f.close()
