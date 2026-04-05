class_name WorldMovement
extends RefCounted

var world

func _init(base) -> void:
	world = base

func ap_after_move() -> int:
	return world.player.available_tiles - world._selected_move_tiles
