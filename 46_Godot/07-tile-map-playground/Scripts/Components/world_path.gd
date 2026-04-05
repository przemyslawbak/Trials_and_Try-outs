class_name WorldPath
extends RefCounted

var world

func _init(base) -> void:
	world = base

func pick_tile(mouse_pos: Vector2) -> Vector2:
	var best: Vector2 = Vector2.INF
	var best_dist: float = world.CLICK_RADIUS
	for pos in world._reachable_positions:
		var d: float = mouse_pos.distance_to(pos)
		if d < best_dist:
			best_dist = d
			best      = pos
	return best

func build_path_positions(direction: String, tiles: int) -> Array[Vector2]:
	var step: Vector2	   = world.player.ISO_DIRECTIONS.get(direction, Vector2.ZERO)
	var pts: Array[Vector2] = [world.player.global_position]
	for t in range(1, tiles + 1):
		pts.append(world.player.global_position + (step * float(t)))
	return pts
