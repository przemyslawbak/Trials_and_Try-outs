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
	
	var xxx = Enemy.new()
	
	var i: int = 1
	#i = "dupa" <= does not work
	
	print(add(1, 1))

func add(a: int, b: int):
	return a + b
	
class Enemy:
	var damage = 5
	var health = 10
	func take_damage(amount):
		health -= amount
		if health <= 0:
			die()
	func die():
		print("Aaargh I died!")
