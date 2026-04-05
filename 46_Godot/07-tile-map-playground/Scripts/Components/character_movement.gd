class_name CharacterMovement
extends RefCounted

var character
var is_moving := false

func _init(base) -> void:
	character = base

func get_reachable_positions() -> Array[Vector2]:
	var result: Array[Vector2] = []
	for dir in character.ISO_DIRECTIONS:
		var step: Vector2 = character.ISO_DIRECTIONS[dir]
		for t in range(1, character.available_tiles + 1):
			var target: Vector2 = character.global_position + (step * float(t))
			if is_position_blocked(target):
				break
			result.append(target)
	return result

func get_move_data_for(target: Vector2) -> Dictionary:
	for dir in character.ISO_DIRECTIONS:
		var step: Vector2 = character.ISO_DIRECTIONS[dir]
		for t in range(1, character.available_tiles + 1):
			var candidate: Vector2 = character.global_position + (step * float(t))
			if is_position_blocked(candidate):
				break
			if candidate.distance_to(target) < 2.0:
				return {"direction": dir, "tiles": t}
	return {}

func is_position_blocked(world_position: Vector2) -> bool:
	for layer in character._blocked_layers:
		if not is_instance_valid(layer):
			continue
		var map_coords: Vector2i = layer.local_to_map(layer.to_local(world_position))
		if layer.get_cell_source_id(map_coords) != -1:
			return true

	if is_instance_valid(character._ground_lvl0):
		var ground_coords: Vector2i = character._ground_lvl0.local_to_map(character._ground_lvl0.to_local(world_position))
		if character._ground_lvl0.get_cell_source_id(ground_coords) != -1:
			var atlas_coords: Vector2i = character._ground_lvl0.get_cell_atlas_coords(ground_coords)
			if atlas_coords.y == character.WATER_ATLAS_ROW:
				return true

	return false

func move_in_direction(direction: String, tiles: int) -> void:
	if is_moving or tiles <= 0:
		return

	var dir_vector: Vector2 = character.ISO_DIRECTIONS.get(direction, Vector2.ZERO)
	if dir_vector == Vector2.ZERO:
		push_error("Invalid direction: " + direction)
		return

	var target_position: Vector2 = character.global_position + dir_vector * tiles
	var distance: float = character.global_position.distance_to(target_position)
	var duration: float = distance / character.MOVE_SPEED

	character.facing_direction = direction
	character.facing.play_walk(direction)
	is_moving = true

	var tween: Tween = character.create_tween()
	tween.tween_property(character, "global_position", target_position, duration) \
		.set_trans(Tween.TRANS_SINE).set_ease(Tween.EASE_IN_OUT)
	tween.tween_callback(on_move_finished.bind(direction))

func on_move_finished(direction: String) -> void:
	is_moving = false
	character.facing.play_idle(direction)
