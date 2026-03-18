extends Node2D
const MAX_HEALTH: int = 10
@export var damage: float = 0.0
@export var player_name: String = "Erika"
@export var health: int = 10:
	get:
		return health
	set(new_value):
		health = clamp(new_value, 0, MAX_HEALTH)
		update_health_label()

@onready var _health_label: Label = $HealthLabel

func add_health_points(difference: int):
	health += difference

func update_health_label():
	if not is_instance_valid(_health_label):
		return
	_health_label.text = str(health) + "/" + str(MAX_HEALTH)

func _ready():
	update_health_label()
