extends Node2D

@export var camera_distance: float = 200
@onready var _player: CharacterBody2D = get_parent()
func _process(_delta):
	var move_direction: Vector2 = _player.velocity.normalized()
	position = move_direction * camera_distance
