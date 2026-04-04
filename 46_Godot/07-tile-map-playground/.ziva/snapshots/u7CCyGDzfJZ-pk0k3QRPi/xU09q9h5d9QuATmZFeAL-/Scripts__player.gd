extends CharacterBody2D

const TILE_WIDTH  := 32
const TILE_HEIGHT := 16
const MOVE_SPEED  := 30.0

const MAX_AVAILABLE_TILES := 3
const MAX_AVAILABLE_SWORDS := 999

const ISO_DIRECTIONS := {
	"NE": Vector2(TILE_WIDTH / 2.0,  -TILE_HEIGHT / 2.0),
	"SE": Vector2(TILE_WIDTH / 2.0,   TILE_HEIGHT / 2.0),
	"SW": Vector2(-TILE_WIDTH / 2.0,  TILE_HEIGHT / 2.0),
	"NW": Vector2(-TILE_WIDTH / 2.0, -TILE_HEIGHT / 2.0),
}

const DIR_ANIMATION := {
	"NE": "up-right",
	"SE": "down-right",
	"SW": "down-left",
	"NW": "up-left",
}

# Clockwise ring used to measure facing rotation cost
const DIR_RING := ["NE", "SE", "SW", "NW"]

var available_tiles: int = MAX_AVAILABLE_TILES
var available_swords: int = MAX_AVAILABLE_SWORDS:
	set(value):
		available_swords = value
		if sword_label:
			sword_label.text = str(available_swords)

var _is_moving := false
var facing_direction: String = "NE"

@onready var sprite: AnimatedSprite2D = $AnimatedSprite2D
@onready var move_tiles_label: Label = $MoveTilesLabel
@onready var move_tiles_icon: Sprite2D = $MoveTilesIcon
@onready var sword_label: Label = $SwordLabel

var _blocked_layers: Array[TileMapLayer] = []


func _ready() -> void:
	var anim_base: String = DIR_ANIMATION.get(facing_direction, "up-right")
	sprite.play(anim_base + "-idle")
	if move_tiles_label:
		move_tiles_label.add_theme_color_override("font_color", Color.WHITE)
		move_tiles_label.text = "%d/%d" % [available_tiles, MAX_AVAILABLE_TILES]
	if sword_label and move_tiles_label:
		sword_label.add_theme_font_size_override("font_size", move_tiles_label.get_theme_font_size("font_size"))
		sword_label.scale = move_tiles_label.scale
		sword_label.text = str(available_swords)
	if move_tiles_icon:
		move_tiles_icon.visible = true

	# The player cannot enter these layers.
	var world := get_parent()
	if world:
		var ground_lvl1 := world.get_node_or_null("Ground-lvl1") as TileMapLayer
		if ground_lvl1:
			_blocked_layers.append(ground_lvl1)
		var decoration := world.get_node_or_null("Decoration-lvl-0") as TileMapLayer
		if decoration:
			_blocked_layers.append(decoration)


# Returns the AP cost to rotate from `from_dir` to `to_dir`.
# Cost = minimum steps around the 4-direction ring (1 step per adjacent face).
func facing_cost(from_dir: String, to_dir: String) -> int:
	if from_dir == to_dir:
		return 0
	var a := DIR_RING.find(from_dir)
	var b := DIR_RING.find(to_dir)
	if a == -1 or b == -1:
		return 0
	var size  := DIR_RING.size()          # 4
	var diff  := absi(b - a)
	return mini(diff, size - diff)        # min of clockwise / counter-clockwise


func set_move_tiles_label_value(value: int) -> void:
	if move_tiles_label:
		move_tiles_label.visible = true
		move_tiles_label.text = "%d/%d" % [clampi(value, 0, MAX_AVAILABLE_TILES), MAX_AVAILABLE_TILES]
	if move_tiles_icon:
		move_tiles_icon.visible = true


func set_move_tiles_label_ok() -> void:
	if move_tiles_label:
		move_tiles_label.visible = false
	if move_tiles_icon:
		move_tiles_icon.visible = false


func reset_available_tiles() -> void:
	available_tiles = MAX_AVAILABLE_TILES
	set_move_tiles_label_value(available_tiles)


# Only positions reachable within the current available_tiles budget are returned.
func get_reachable_positions() -> Array[Vector2]:
	var result: Array[Vector2] = []
	for dir in ISO_DIRECTIONS:
		var step: Vector2 = ISO_DIRECTIONS[dir]
		for t in range(1, available_tiles + 1):
			var target := global_position + step * t
			if _is_position_blocked(target):
				break
			result.append(target)
	return result


# Returns {direction, tiles} only if the tile costs <= available_tiles.
func get_move_data_for(target: Vector2) -> Dictionary:
	for dir in ISO_DIRECTIONS:
		var step: Vector2 = ISO_DIRECTIONS[dir]
		for t in range(1, available_tiles + 1):
			var candidate := global_position + step * t
			if _is_position_blocked(candidate):
				break
			if candidate.distance_to(target) < 2.0:
				return {"direction": dir, "tiles": t}
	return {}


func _is_position_blocked(world_position: Vector2) -> bool:
	for layer in _blocked_layers:
		if not is_instance_valid(layer):
			continue
		var map_coords: Vector2i = layer.local_to_map(layer.to_local(world_position))
		if layer.get_cell_source_id(map_coords) != -1:
			return true
	return false


func move_in_direction(direction: String, tiles: int) -> void:
	if _is_moving or tiles <= 0:
		return

	var dir_vector: Vector2 = ISO_DIRECTIONS.get(direction, Vector2.ZERO)
	if dir_vector == Vector2.ZERO:
		push_error("Invalid direction: " + direction)
		return

	var target_position: Vector2 = global_position + dir_vector * tiles
	var distance: float           = global_position.distance_to(target_position)
	var duration: float           = distance / MOVE_SPEED

	facing_direction = direction
	var anim_base: String = DIR_ANIMATION.get(direction, "down-right")
	sprite.play(anim_base + "-walk")
	_is_moving = true

	var tween := create_tween()
	tween.tween_property(self, "global_position", target_position, duration) \
		.set_trans(Tween.TRANS_SINE).set_ease(Tween.EASE_IN_OUT)
	tween.tween_callback(_on_move_finished.bind(anim_base))


func _on_move_finished(anim_base: String) -> void:
	_is_moving = false
	sprite.play(anim_base + "-idle")


func set_facing(direction: String) -> void:
	if not ISO_DIRECTIONS.has(direction):
		return
	facing_direction = direction
	var anim_base: String = DIR_ANIMATION.get(direction, "down-right")
	sprite.play(anim_base + "-idle")


func is_moving() -> bool:
	return _is_moving
