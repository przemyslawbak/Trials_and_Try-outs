class_name CharacterFacing
extends RefCounted

const DIR_ANIMATION := {
	"NE": "up-right",
	"SE": "down-right",
	"SW": "down-left",
	"NW": "up-left",
}

const DIR_RING := ["NE", "SE", "SW", "NW"]

var character

func _init(base) -> void:
	character = base

func play_idle(dir: String = "") -> void:
	if dir != "":
		character.facing_direction = dir
	var anim_base: String = DIR_ANIMATION.get(character.facing_direction, "down-right")
	if character.sprite:
		character.sprite.play(anim_base + "-idle")

func play_walk(dir: String = "") -> void:
	if dir != "":
		character.facing_direction = dir
	var anim_base: String = DIR_ANIMATION.get(character.facing_direction, "down-right")
	if character.sprite:
		character.sprite.play(anim_base + "-walk")

func facing_cost(from_dir: String, to_dir: String) -> int:
	if from_dir == to_dir:
		return 0
	var a := DIR_RING.find(from_dir)
	var b := DIR_RING.find(to_dir)
	if a == -1 or b == -1:
		return 0
	var size  := DIR_RING.size()
	var diff  := absi(b - a)
	return mini(diff, size - diff)

func set_facing(direction: String) -> void:
	if not DIR_ANIMATION.has(direction):
		return
	character.facing_direction = direction
	play_idle()
