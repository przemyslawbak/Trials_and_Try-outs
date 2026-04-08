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

func update_command_stars() -> void:
	if not command_stars:
		return
		
	for child in command_stars.get_children():
		child.queue_free()
		
	var star_tex = preload("res://Assets/Icons/star.png")
	for i in range(command):
		var tex_rect = TextureRect.new()
		tex_rect.texture = star_tex
		tex_rect.expand_mode = TextureRect.EXPAND_IGNORE_SIZE
		tex_rect.stretch_mode = TextureRect.STRETCH_KEEP_ASPECT_CENTERED
		tex_rect.custom_minimum_size = Vector2(10, 10)
		command_stars.add_child(tex_rect)

@export var knights: int = 0
@export var footmen: int = 0
@export var horsemen: int = 0
@export var archers: int = 0
@export var pikemen: int = 0
@export var command: int = 0:
	set(value):
		command = value
		if is_node_ready():
			update_command_stars()

@onready var sprite: AnimatedSprite2D = $AnimatedSprite2D
@onready var move_tiles_label: Label = $MoveTilesLabel
@onready var move_tiles_icon: Sprite2D = $MoveTilesIcon
@onready var name_label: Label = $NameLabel
@onready var command_stars: HBoxContainer = $CommandStars
@onready var shield: Polygon2D = $Shield

var movement: CharacterMovement
var facing: CharacterFacing

const MAX_AVAILABLE_TILES := 3

var _blocked_layers: Array[TileMapLayer] = []
var _ground_lvl0: TileMapLayer

var _top_glow: Sprite2D

func _init() -> void:
	movement = CharacterMovement.new(self)
	facing = CharacterFacing.new(self)

func _get_random_name() -> String:
	if not FileAccess.file_exists("res://Assets/Text/names.txt"):
		return name
	var file = FileAccess.open("res://Assets/Text/names.txt", FileAccess.READ)
	if file:
		var content = file.get_as_text().strip_edges()
		if content:
			var names = content.split("\n", false)
			if names.size() > 0:
				return names[randi() % names.size()].strip_edges()
	return name

func set_selected(selected: bool) -> void:
	if _top_glow:
		_top_glow.visible = selected

func _ready() -> void:
	# Create bottom glow
	var bottom_tex = GradientTexture2D.new()
	bottom_tex.width = 40
	bottom_tex.height = 20
	bottom_tex.fill = GradientTexture2D.FILL_RADIAL
	bottom_tex.fill_from = Vector2(0.5, 0.5)
	bottom_tex.fill_to = Vector2(0.5, 0)
	var bottom_grad = Gradient.new()
	bottom_grad.colors = [Color(1.0, 1.0, 0.0, 0.6), Color(1.0, 1.0, 0.0, 0.0)]
	bottom_tex.gradient = bottom_grad

	# Create top glow for NameLabel, CommandStars and Shield
	_top_glow = Sprite2D.new()
	var top_tex = GradientTexture2D.new()
	top_tex.width = 60
	top_tex.height = 40
	top_tex.fill = GradientTexture2D.FILL_RADIAL
	top_tex.fill_from = Vector2(0.5, 0.5)
	top_tex.fill_to = Vector2(0.5, 0)
	var top_grad = Gradient.new()
	top_grad.colors = [Color(1.0, 1.0, 0.0, 0.6), Color(1.0, 1.0, 0.0, 0.0)]
	top_tex.gradient = top_grad
	_top_glow.texture = top_tex
	# Under NameLabel (z_index=2) and Shield (z_index=3), so z_index=1 is good
	_top_glow.z_index = 1
	# Positioned roughly at the height of NameLabel, Stars and Shield (-41)
	_top_glow.position = Vector2(0, -41)
	_top_glow.visible = false
	add_child(_top_glow)

	footmen = randi_range(600, 999)
	horsemen = randi_range(20, 99)
	archers = randi_range(100, 199)
	pikemen = randi_range(100, 199)
	knights = randi_range(0, 19)
	command = randi_range(1, 3)
	
	facing.play_idle()
	
	if move_tiles_label:
		move_tiles_label.add_theme_color_override("font_color", Color.WHITE)
		move_tiles_label.text = "%d/%d" % [available_tiles, MAX_AVAILABLE_TILES]
	if name_label and move_tiles_label:
		name_label.add_theme_font_size_override("font_size", move_tiles_label.get_theme_font_size("font_size"))
		name_label.scale = move_tiles_label.scale
		name_label.text = _get_random_name()
	if move_tiles_icon:
		move_tiles_icon.visible = true
		
	update_command_stars()
	
	var world := get_parent()
	if world:
		_ground_lvl0 = world.get_node_or_null("Ground-lvl0") as TileMapLayer
		var ground_lvl1 := world.get_node_or_null("Ground-lvl1") as TileMapLayer
		if ground_lvl1:
			_blocked_layers.append(ground_lvl1)
		var decoration := world.get_node_or_null("Decoration-lvl-0") as TileMapLayer
		if decoration:
			_blocked_layers.append(decoration)

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
