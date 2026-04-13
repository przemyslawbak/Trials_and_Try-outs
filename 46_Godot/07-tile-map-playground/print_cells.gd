@tool
extends EditorScript

func _run():
	var world = load("res://Scenes/world.tscn").instantiate()
	var ground = world.get_node("Ground-lvl1")
	var cells = ground.get_used_cells()
	for c in cells:
		print("Cell ", c, " -> ", ground.get_cell_atlas_coords(c))
