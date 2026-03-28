extends CharacterBody2D

const speed = 80

var current_direction

enum {DOWN_LEFT, DOWN_RIGHT, UP_LEFT, UP_RIGHT}

var KEY_UP = false
var KEY_DOWN = false
var KEY_LEFT = false
var KEY_RIGHT = false

func _process(delta: float) -> void:
	pass

func get_input():
	if Input.is_action_pressed("move_up"):
		KEY_UP = true
	else: KEY_UP = false
	if Input.is_action_pressed("move_down"):
		KEY_DOWN = true
	else: KEY_DOWN = false
	if Input.is_action_pressed("move_left"):
		KEY_LEFT = true
	else: KEY_LEFT = false
	if Input.is_action_pressed("move_right"):
		KEY_RIGHT = true
	else: KEY_RIGHT = false

func cartesian_to_isometric(cartesian):
	return Vector2(cartesian.x - cartesian.y, (cartesian.x + cartesian.y)/2)
