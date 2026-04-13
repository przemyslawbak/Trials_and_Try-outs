@tool
extends EditorScript

func _run():
	var scene = load("res://Scenes/world.tscn")
	var world = scene.instantiate()
	var tileset = world.get_node("Ground-lvl0").tile_set
	var source = tileset.get_source(2)
	
	# Fix texture origins for all tiles
	for x in range(11):
		for y in range(4, 20):
			if source.has_tile(Vector2i(x, y)):
				var data = source.get_tile_data(Vector2i(x, y), 0)
				data.texture_origin = Vector2i(0, 0)

	# Replace bordered grass (y=2,3) with seamless grass (y=4,5)
	for layer_name in ["Ground-lvl0", "Ground-lvl1", "Decoration-lvl-0"]:
		var layer = world.get_node_or_null(layer_name)
		if layer:
			for cell in layer.get_used_cells():
				var atlas = layer.get_cell_atlas_coords(cell)
				if layer.get_cell_source_id(cell) == 2:
					var new_atlas = atlas
					if atlas.y == 2:
						new_atlas.y = 4
					elif atlas.y == 3:
						new_atlas.y = 5
					if new_atlas != atlas:
						layer.set_cell(cell, 2, new_atlas)

	var packed = PackedScene.new()
	packed.pack(world)
	ResourceSaver.save(packed, "res://Scenes/world.tscn")
	print("Fix applied successfully!")
