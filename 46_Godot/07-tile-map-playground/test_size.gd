class_name TestSize
extends Node

func test_texture_size():
	var tex = load("res://MapSheets/green_1.png")
	if tex:
		push_error("TEXTURE_SIZE: " + str(tex.get_size()))
	else:
		push_error("TEXTURE_NOT_FOUND")
