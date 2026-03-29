extends CharacterBody2D

const TILE_WIDTH := 32
const TILE_HEIGHT := 16
const MOVE_SPEED := 120.0

const ISO_DIRECTIONS := {
	"NE": Vector2(TILE_WIDTH / 2.0,  -TILE_HEIGHT / 2.0),
	"SE": Vector2(TILE_WIDTH / 2.0,   TILE_HEIGHT / 2.0),
	"SW": Vector2(-TILE_WIDTH / 2.0,  TILE_HEIGHT / 2.0),
	"NW": Vector2(-TILE_WIDTH / 2.0, -TILE_HEIGHT / 2.0),
}

const DIR_ANIMATION := {
	"NE": "up-right",
	"SE": "down-right",
	"SW": "down-left",
	"NW": "up-left",
}

var _is_moving := false

@onready var sprite: AnimatedSprite2D = $AnimatedSprite2D

func _ready() -> void:
	var anim_base: String = DIR_ANIMATION.get( "NE", "down-right")
	sprite.play(anim_base + "-idle")

func get_reachable_positions(max_tiles: int = 3) -> Array[Vector2]:
	var result: Array[Vector2] = []
	for dir in ISO_DIRECTIONS:
		var step: Vector2 = ISO_DIRECTIONS[dir]
		for t in range(1, max_tiles + 1):
			result.append(global_position + step * t)
	return result


func get_move_data_for(target: Vector2) -> Dictionary:
	for dir in ISO_DIRECTIONS:
		var step: Vector2 = ISO_DIRECTIONS[dir]
		for t in range(1, 4):
			if (global_position + step * t).distance_to(target) < 2.0:
				return {"direction": dir, "tiles": t}
	return {}


func move_in_direction(direction: String, tiles: int) -> void:
	if _is_moving or tiles <= 0:
		return

	var dir_vector: Vector2 = ISO_DIRECTIONS.get(direction, Vector2.ZERO)
	if dir_vector == Vector2.ZERO:
		push_error("Invalid direction: " + direction)
		return

	var target_position: Vector2 = global_position + dir_vector * tiles
	var distance: float = global_position.distance_to(target_position)
	var duration: float = distance / MOVE_SPEED

	var anim_base: String = DIR_ANIMATION.get(direction, "down-right")
	sprite.play(anim_base + "-walk")
	_is_moving = true

	var tween := create_tween()
	tween.tween_property(self, "global_position", target_position, duration) \
		.set_trans(Tween.TRANS_SINE).set_ease(Tween.EASE_IN_OUT)
	tween.tween_callback(_on_move_finished.bind(anim_base))


func _on_move_finished(anim_base: String) -> void:
	_is_moving = false
	sprite.play(anim_base + "-idle")


func is_moving() -> bool:
	return _is_moving
