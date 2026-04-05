class_name WorldFacing
extends RefCounted

var world

func _init(base) -> void:
	world = base

func nearest_facing_dir(mouse_pos: Vector2, center: Vector2) -> String:
	var best_dir  := ""
	var best_dist := INF
	for dir in world.player.ISO_DIRECTIONS:
		var neighbour: Vector2 = center + world.player.ISO_DIRECTIONS[dir]
		var d := mouse_pos.distance_to(neighbour)
		if d < best_dist:
			best_dist = d
			best_dir  = dir
	return best_dir

func ap_after_facing(to_dir: String) -> int:
	var from_dir: String = world._selected_direction if world._selected_direction != "" else world.player.facing_direction
	var cost:     int    = world.player.facing_cost(from_dir, to_dir)
	return world.movement.ap_after_move() - cost
