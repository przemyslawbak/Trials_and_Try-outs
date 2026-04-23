extends Control

var is_overlay := false

func _ready() -> void:
	process_mode = Node.PROCESS_MODE_ALWAYS
	
	$VBoxContainer/StartButton.pressed.connect(_on_start_pressed)
	$VBoxContainer/BurningButton.pressed.connect(_on_burning_pressed)
	$VBoxContainer/FadingButton.pressed.connect(_on_fading_pressed)
	$VBoxContainer/ExitButton.pressed.connect(_on_exit_pressed)
	
	if is_overlay:
		$VBoxContainer/StartButton.text = "CONTINUE GAME"

func _on_start_pressed() -> void:
	if is_overlay:
		get_tree().paused = false
		queue_free()
	else:
		get_tree().change_scene_to_file("res://Scenes/world.tscn")

func _on_burning_pressed() -> void:
	if is_overlay:
		get_tree().paused = false
		queue_free()
	get_tree().change_scene_to_file("res://Scenes/image_burning.tscn")

func _on_fading_pressed() -> void:
	if is_overlay:
		get_tree().paused = false
		queue_free()
	get_tree().change_scene_to_file("res://Scenes/image_fade.tscn")

func _on_exit_pressed() -> void:
	get_tree().quit()
