extends Node2D

@export var tilemap: TileMapLayer
@export var player: CharacterBody2D  # drag your Player node here

func _unhandled_input(event: InputEvent) -> void:
	if event is InputEventMouseButton and event.pressed and event.button_index == MOUSE_BUTTON_LEFT:
		var world_pos = get_global_mouse_position()
		var clicked_tile = tilemap.local_to_map(tilemap.to_local(world_pos))
		player._on_tile_clicked(clicked_tile)
