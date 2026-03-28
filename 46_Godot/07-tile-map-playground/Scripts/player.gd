extends CharacterBody2D

const MAX_MOVE_TILES = 3

var target_tile: Vector2i = Vector2i.ZERO
var current_tile: Vector2i = Vector2i.ZERO
var has_pending_move: bool = false
var is_moving: bool = false
var last_dir: Vector2i = Vector2i(1, 0)

@onready var tilemap: TileMapLayer = get_parent().get_node("Ground-lvl0")

func _ready() -> void:
	current_tile = tilemap.local_to_map(tilemap.to_local(global_position))

func on_tile_clicked(clicked_tile: Vector2i) -> void:
	if is_moving:
		return
	var dist = abs(clicked_tile.x - current_tile.x) + abs(clicked_tile.y - current_tile.y)
	if dist > 0 and dist <= MAX_MOVE_TILES:
		target_tile = clicked_tile
		has_pending_move = true

func on_next_turn_pressed() -> void:
	if is_moving or not has_pending_move:
		return
	has_pending_move = false
	_move_to_tile(target_tile)

func _move_to_tile(goal: Vector2i) -> void:
	is_moving = true
	var path = _build_path(current_tile, goal)
	if path.is_empty():
		is_moving = false
		return
	_walk_path(path, 0)

func _walk_path(path: Array, index: int) -> void:
	if index >= path.size():
		is_moving = false
		current_tile = tilemap.local_to_map(tilemap.to_local(global_position))
		_play_idle()
		return

	var next_tile: Vector2i = path[index]
	var dir: Vector2i = next_tile - current_tile

	# Update current_tile tracking as we step
	current_tile = next_tile
	last_dir = dir

	_play_walk_animation(dir)

	var next_world_pos: Vector2 = tilemap.to_global(tilemap.map_to_local(next_tile))

	var tween = create_tween()
	tween.tween_property(self, "global_position", next_world_pos, 0.25) \
		 .set_ease(Tween.EASE_IN_OUT) \
		 .set_trans(Tween.TRANS_SINE)
	tween.finished.connect(func(): _walk_path(path, index + 1))

func _build_path(from: Vector2i, to: Vector2i) -> Array:
	var path: Array = []
	var cur: Vector2i = from
	var safety = 0
	while cur != to and safety < 20:
		safety += 1
		var dx = sign(to.x - cur.x)
		var dy = sign(to.y - cur.y)
		if dx != 0 and dy != 0:
			# Prefer axis that has more distance to cover
			if abs(to.x - cur.x) >= abs(to.y - cur.y):
				cur += Vector2i(dx, 0)
			else:
				cur += Vector2i(0, dy)
		elif dx != 0:
			cur += Vector2i(dx, 0)
		else:
			cur += Vector2i(0, dy)
		path.append(cur)
	return path

func _play_walk_animation(dir: Vector2i) -> void:
	var anim = _dir_to_walk_anim(dir)
	if $AnimatedSprite2D.animation != anim or not $AnimatedSprite2D.is_playing():
		$AnimatedSprite2D.play(anim)

func _play_idle() -> void:
	var anim = _dir_to_idle_anim(last_dir)
	$AnimatedSprite2D.play(anim)

func _dir_to_walk_anim(dir: Vector2i) -> String:
	if dir.x > 0 and dir.y == 0:
		return "down-right-walk"
	elif dir.x < 0 and dir.y == 0:
		return "up-left-walk"
	elif dir.x == 0 and dir.y > 0:
		return "down-left-walk"
	elif dir.x == 0 and dir.y < 0:
		return "up-right-walk"
	elif dir.x > 0 and dir.y > 0:
		return "down-right-walk"
	elif dir.x > 0 and dir.y < 0:
		return "up-right-walk"
	elif dir.x < 0 and dir.y < 0:
		return "up-left-walk"
	else:
		return "down-left-walk"

func _dir_to_idle_anim(dir: Vector2i) -> String:
	return _dir_to_walk_anim(dir).replace("walk", "idle")
