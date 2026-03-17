extends Node2D
const MAX_HEALTH: int = 10
var health: int = 10
@onready var _health_label: Label = $HealthLabel

func add_health_points(difference: int):
	health += difference
	health = clamp(health, 0, MAX_HEALTH)
	update_health_label()

func update_health_label():
	_health_label.text = str(health) + "/" + str(MAX_HEALTH)

func _ready():
	add_health_points(-2)
