extends Control

func _ready() -> void:
	$CenterContainer/MainUIContainer/PanelContainer/MarginContainer/VBoxContainer/VBoxContainer/PlayButton.pressed.connect(_on_play_button_pressed)
	$CenterContainer/MainUIContainer/PanelContainer/MarginContainer/VBoxContainer/VBoxContainer/ExitButton.pressed.connect(_on_exit_button_pressed)

#optional: how should look Option B — Connect via editor in this case?
func _on_play_button_pressed():
	get_tree().change_scene_to_file("res://screens/game/main.tscn")


func _on_exit_button_pressed():
	get_tree().quit()
