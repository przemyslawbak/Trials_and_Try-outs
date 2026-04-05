extends Node

func test_game() -> void:
	var world_scene = load("res://Scenes/world.tscn")
	if not world_scene:
		push_error("Failed to load world.tscn")
		return
	
	var world = world_scene.instantiate()
	if not world:
		push_error("Failed to instantiate world")
		return
		
	var player = world.get_node("Player")
	if not player:
		push_error("Failed to get player")
		return
		
	print("Game initialized successfully")
