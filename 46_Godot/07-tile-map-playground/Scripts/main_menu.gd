extends Control

func _ready() -> void:
	$VBoxContainer/StartButton.pressed.connect(_on_start_pressed)
	$VBoxContainer/ExitButton.pressed.connect(_on_exit_pressed)

func _on_start_pressed() -> void:
	get_tree().change_scene_to_file("res://Scenes/world.tscn")

func _on_exit_pressed() -> void:
	get_tree().quit()
