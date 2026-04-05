class_name CharacterBase
extends CharacterBody2D

const TILE_WIDTH  := 32
const TILE_HEIGHT := 16
const MOVE_SPEED  := 30.0
const WATER_ATLAS_ROW := 10

const ISO_DIRECTIONS := {
	"NE": Vector2(TILE_WIDTH / 2.0,  -TILE_HEIGHT / 2.0),
	"SE": Vector2(TILE_WIDTH / 2.0,   TILE_HEIGHT / 2.0),
	"SW": Vector2(-TILE_WIDTH / 2.0,  TILE_HEIGHT / 2.0),
	"NW": Vector2(-TILE_WIDTH / 2.0, -TILE_HEIGHT / 2.0),
}

var available_tiles: int = 3
var facing_direction: String = "NE"

@onready var sprite: AnimatedSprite2D = $AnimatedSprite2D

var movement: CharacterMovement
var facing: CharacterFacing

var _blocked_layers: Array[TileMapLayer] = []
var _ground_lvl0: TileMapLayer

func _init() -> void:
	movement = CharacterMovement.new(self)
	facing = CharacterFacing.new(self)


func _ready() -> void:
	facing.play_idle()
	
	var world := get_parent()
	if world:
		_ground_lvl0 = world.get_node_or_null("Ground-lvl0") as TileMapLayer
		var ground_lvl1 := world.get_node_or_null("Ground-lvl1") as TileMapLayer
		if ground_lvl1:
			_blocked_layers.append(ground_lvl1)
		var decoration := world.get_node_or_null("Decoration-lvl-0") as TileMapLayer
		if decoration:
			_blocked_layers.append(decoration)

func get_reachable_positions() -> Array[Vector2]:
	return movement.get_reachable_positions()

func get_move_data_for(target: Vector2) -> Dictionary:
	return movement.get_move_data_for(target)

func move_in_direction(direction: String, tiles: int) -> void:
	movement.move_in_direction(direction, tiles)

func is_moving() -> bool:
	return movement.is_moving

func set_facing(direction: String) -> void:
	facing.set_facing(direction)

func facing_cost(from_dir: String, to_dir: String) -> int:
	return facing.facing_cost(from_dir, to_dir)
