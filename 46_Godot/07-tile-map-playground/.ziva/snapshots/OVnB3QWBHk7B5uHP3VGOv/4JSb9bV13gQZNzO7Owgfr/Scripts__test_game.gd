extends SceneTree

func _init() -> void:
	var world_scene = load("res://Scenes/world.tscn")
	if not world_scene:
		print("Failed to load world.tscn")
		quit(1)
		return
	
	var world = world_scene.instantiate()
	if not world:
		print("Failed to instantiate world")
		quit(1)
		return
		
	var player = world.get_node("Player")
	if not player:
		print("Failed to get player")
		quit(1)
		return
		
	print("SUCCESS")
	quit(0)
