extends SceneTree
func _init():
	var scene = load("res://Scenes/world.tscn").instantiate()
	for layer_name in ["Ground-lvl0", "Ground-lvl1", "Decoration-lvl-0", "SpawnPoints"]:
		var layer = scene.get_node_or_null(layer_name) as TileMapLayer
		if layer:
			var cells = layer.get_used_cells()
			for cell in cells:
				var atlas_coords = layer.get_cell_atlas_coords(cell)
				if atlas_coords == Vector2i(6, 3):
					print("Spawn point on ", layer_name, " at ", cell)
	quit()