class_name Player
extends CharacterBase

const MAX_AVAILABLE_TILES := 3
const MAX_AVAILABLE_SWORDS := 999

var available_swords: int = MAX_AVAILABLE_SWORDS:
	set(value):
		available_swords = value
		if sword_label:
			sword_label.text = str(available_swords)

@onready var move_tiles_label: Label = $MoveTilesLabel
@onready var move_tiles_icon: Sprite2D = $MoveTilesIcon
@onready var sword_label: Label = $SwordLabel

func _ready() -> void:
	super._ready()
	
	if move_tiles_label:
		move_tiles_label.add_theme_color_override("font_color", Color.WHITE)
		move_tiles_label.text = "%d/%d" % [available_tiles, MAX_AVAILABLE_TILES]
	if sword_label and move_tiles_label:
		sword_label.add_theme_font_size_override("font_size", move_tiles_label.get_theme_font_size("font_size"))
		sword_label.scale = move_tiles_label.scale
		sword_label.text = str(available_swords)
	if move_tiles_icon:
		move_tiles_icon.visible = true

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
