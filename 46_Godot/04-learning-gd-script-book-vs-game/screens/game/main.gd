extends Node2D
@onready var _game_over_menu: CenterContainer = $CanvasLayer/GameOverMenu
@onready var _enemy_spawner: Node2D = $EnemySpawner
@onready var _health_potion_spawner: Node2D = $HealthPotionSpawner
func _on_player_died() -> void:
	_game_over_menu.show()
	_enemy_spawner.stop()
	_health_potion_spawner.stop()
