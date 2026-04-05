extends SceneTree

func _init():
	print("Running game...")
	var scn = load("res://Scenes/world.tscn").instantiate()
	root.add_child(scn)
	
	await root.get_tree().create_timer(1.0).timeout
	quit()
