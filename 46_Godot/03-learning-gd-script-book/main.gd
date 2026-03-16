extends Node2D

#Called when the node enters the scene
func _ready(): 
	
	var number_of_lives = 3
	var fireball_damage = 2
	number_of_lives -= number_of_lives
	prints("The player has", number_of_lives, "out of", 10, "lives left")
