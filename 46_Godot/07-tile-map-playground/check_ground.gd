@tool
extends EditorScript

func _run():
	var scene = load("res://Scenes/world.tscn").instantiate()
	var ground = scene.get_node("Ground-lvl0")
	var count = 0
	for cell in ground.get_used_cells():
		if ground.get_cell_atlas_coords(cell) == Vector2i(6, 3):
			count += 1
	print("Ground 6,3: ", count)
	
	var spawns = scene.get_node("SpawnPoints")
	print("Spawns total: ", spawns.get_used_cells().size())
	for cell in spawns.get_used_cells():
		print("Spawn at: ", cell, " atlas: ", spawns.get_cell_atlas_coords(cell))
