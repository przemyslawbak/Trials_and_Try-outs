extends Node2D

#Called when the node enters the scene
func _ready(): 
	
	var inventory = [
		"item1",
		"item2",
		"item3",
		"item4",
	]
	
	var dict = {
		"a": 1,
		"b": "bbb",
		"c": 1.5,
	}
	
	for item in inventory:
		print(item)
