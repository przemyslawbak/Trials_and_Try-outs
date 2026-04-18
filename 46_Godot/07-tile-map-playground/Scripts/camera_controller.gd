extends Camera2D

@export var zoom_speed: float = 0.1
@export var min_zoom: float = 0.5
@export var max_zoom: float = 3.0

var _is_panning: bool = false

func _unhandled_input(event: InputEvent) -> void:
	if event is InputEventMouseButton:
		if event.button_index == MOUSE_BUTTON_RIGHT:
			if event.is_pressed():
				_is_panning = true
			else:
				_is_panning = false
		
		elif event.button_index == MOUSE_BUTTON_WHEEL_UP and event.is_pressed():
			zoom += Vector2(zoom_speed, zoom_speed)
			zoom = zoom.clamp(Vector2(min_zoom, min_zoom), Vector2(max_zoom, max_zoom))
			
		elif event.button_index == MOUSE_BUTTON_WHEEL_DOWN and event.is_pressed():
			zoom -= Vector2(zoom_speed, zoom_speed)
			zoom = zoom.clamp(Vector2(min_zoom, min_zoom), Vector2(max_zoom, max_zoom))

	elif event is InputEventMouseMotion and _is_panning:
		position -= event.relative / zoom
