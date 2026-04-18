# tile_definitions.gd
# Defines all tile types, their source images, and atlas coordinates
# Each sprite sheet uses 32x32 tiles arranged horizontally

class_name TileLayers
extends RefCounted

enum TileType {
	BASE_green_1,
}

# How many variants each type has (tiles per sheet)
const TILE_COUNTS = {
	TileType.BASE_green_1:  1,
}

# Source image paths (relative to res://)
const TILE_TEXTURES = {
	TileType.BASE_green_1:  "res://MapSheets/green_1.png",
}

# Target distribution ratios (must sum to 1.0)
const TARGET_RATIOS = {
	TileType.BASE_green_1:  1.00,
}

# Display names for UI
const TYPE_NAMES = {
	TileType.BASE_green_1:  "Green_1",
}

# Wave generation parameters per type
# frequency: how wide each biome band is (higher = wider bands)
# These control the Perlin noise sampling
const WAVE_PARAMS = {
	TileType.BASE_green_1:  { "frequency": 0.08, "octaves": 3 },
}
