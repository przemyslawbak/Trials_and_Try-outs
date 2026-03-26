extends Sprite2D


# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	$GreetingsLabel.hide()


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	pass
	
# Called when the player performs an input action that hasn't already been handled.
func _unhandled_input(event):
	# Check if the event was a mouse button press.
	if event is InputEventMouseButton:
		# Show the label whenever a mouse button is pressed.
		$GreetingsLabel.show()

		# Check which mouse button was pressed.
		if event.button_index == MOUSE_BUTTON_LEFT:
			# If the left mouse button was clicked, set the label's text to "Hi Folks!"
			$GreetingsLabel.text = "Hi Folks!"
		elif event.button_index == MOUSE_BUTTON_RIGHT:
			# If the right mouse button was clicked, set the label's text to "Godot is Great!"
			$GreetingsLabel.text = "Godot is Great!"
	
	# Check if the event was a key press.
	elif event is InputEventKey:
		# Only show the label and set the text if the Enter key was pressed.
		if event.keycode == KEY_ENTER:
			$GreetingsLabel.show()
			$GreetingsLabel.text = "Hello World!"
