# tile_definitions.gd
# Defines all tile types, their source images, and atlas coordinates
# Each sprite sheet uses 32x32 tiles arranged horizontally

class_name TileDefinitions
extends RefCounted

enum TileType {
	GRASS,
	BUSHES,
	FOREST,
	RIVER,
	LAKE
}

# How many variants each type has (tiles per sheet)
const TILE_COUNTS = {
	TileType.GRASS:  1,
	TileType.BUSHES: 9,
	TileType.FOREST: 47,
	TileType.RIVER:  20,
	TileType.LAKE:   3,
}

# Source image paths (relative to res://)
const TILE_TEXTURES = {
	TileType.GRASS:  "res://assets/base-grass-world.png",
	TileType.BUSHES: "res://assets/bushes-grass-world.png",
	TileType.FOREST: "res://assets/forest-grass-world.png",
	TileType.RIVER:  "res://assets/river-grass-world.png",
	TileType.LAKE:   "res://assets/lakes-grass-world.png",
}

# Target distribution ratios (must sum to 1.0)
const TARGET_RATIOS = {
	TileType.GRASS:  0.30,
	TileType.BUSHES: 0.30,
	TileType.FOREST: 0.30,
	TileType.RIVER:  0.09,
	TileType.LAKE:   0.01,
}

# Display names for UI
const TYPE_NAMES = {
	TileType.GRASS:  "Grass",
	TileType.BUSHES: "Bushes",
	TileType.FOREST: "Forest",
	TileType.RIVER:  "River",
	TileType.LAKE:   "Lake",
}

# Wave generation parameters per type
# frequency: how wide each biome band is (higher = wider bands)
# These control the Perlin noise sampling
const WAVE_PARAMS = {
	TileType.GRASS:  { "frequency": 0.08, "octaves": 3 },
	TileType.BUSHES: { "frequency": 0.07, "octaves": 2 },
	TileType.FOREST: { "frequency": 0.06, "octaves": 3 },
	TileType.RIVER:  { "frequency": 0.04, "octaves": 1 },
	TileType.LAKE:   { "frequency": 0.10, "octaves": 2 },
}
