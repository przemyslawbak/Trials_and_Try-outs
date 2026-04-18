extends SceneTree

func _init():
	var tex = load("res://MapSheets/green_1.png")
	if tex:
		print("TEXTURE_SIZE:", tex.get_size())
	else:
		print("TEXTURE_NOT_FOUND")
	quit()
