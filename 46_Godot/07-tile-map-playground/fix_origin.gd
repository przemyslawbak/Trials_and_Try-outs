@tool
extends EditorScript

func _run():
	var ts = load("res://Scenes/world.tscn").instantiate().get_node("Ground-lvl0").tile_set
	var source = ts.get_source(2)
	for i in range(11):
		for j in range(19):
			if source.has_tile(Vector2i(i, j)):
				var data = source.get_tile_data(Vector2i(i, j), 0)
				data.texture_origin = Vector2i(0, 0)
	ResourceSaver.save(ts, "res://modified_tileset.tres")
