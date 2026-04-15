# Isometric Map Generator – Godot 4 Sample Project

## Overview
Procedurally generates a square horizontal map from five isometric tile
spritesheets using a **wave-based noise approach**.  Terrain types form
smooth, organic bands (not random scatter) and obey hard constraints:

| Biome  | Target % | Constraint |
|--------|----------|------------|
| Grass  | 30 %     | –          |
| Bushes | 30 %     | –          |
| Forest | 30 %     | –          |
| River  | 9 %      | Ends must be connected (no isolated cells) |
| Lake   | 1 %      | No two lake cells may be 4-directionally adjacent |

---

## Project structure

```
godot_map_project/
├── project.godot
├── icon.svg
├── assets/
│   ├── base-grass-world.png    (1 tile,  32×32)
│   ├── bushes-grass-world.png  (9 tiles, 288×32)
│   ├── forest-grass-world.png  (47 tiles, 1504×32)
│   ├── river-grass-world.png   (20 tiles, 640×32)
│   └── lakes-grass-world.png   (3 tiles,  96×32)
├── scenes/
│   └── main.tscn
└── scripts/
    ├── tile_definitions.gd   ← all constants & ratios
    ├── map_generator.gd      ← pure generation logic (no nodes)
    ├── tile_renderer.gd      ← Node2D that draws tiles via _draw()
    ├── minimap_node.gd       ← colour-coded minimap overlay
    └── main.gd               ← scene controller, input, UI
```

---

## How to open

1. Copy the entire `godot_map_project/` folder to your machine.
2. Open **Godot 4.3+** and choose **Import** → select `project.godot`.
3. Press **F5** (or the Play button) – the map generates immediately.

---

## Generation algorithm

### Step 1 – Noise field
`FastNoiseLite` (Perlin, FBM, 4 octaves, frequency 0.06) generates a
smooth scalar field.  The low frequency ensures wide, wave-like bands
rather than noisy scatter.

### Step 2 – Threshold assignment
All cells are sorted by noise value.  The sorted list is divided into
contiguous ranges whose lengths match the target percentages:

```
lowest noise → LAKE → RIVER → GRASS → BUSHES → FOREST → highest noise
```

This guarantees exact ratios and naturally clusters each biome into
smooth geographic zones.

### Step 3 – River connectivity
Union-Find identifies disconnected river components.  Pairs of
neighbouring components are bridged by a Manhattan-path carved through
the tile grid, ensuring every river cell has at least one river
neighbour.

### Step 4 – Lake isolation
A single top-left–to–bottom-right pass converts any lake cell that is
4-directionally adjacent to another lake cell into grass.

### Step 5 – Variant selection
Each cell picks a random sprite index within its biome's spritesheet
column count, giving natural visual variety.

---

## Tuning

Edit **`scripts/tile_definitions.gd`**:

```gdscript
const TARGET_RATIOS = {
    TileType.GRASS:  0.30,
    TileType.BUSHES: 0.30,
    TileType.FOREST: 0.30,
    TileType.RIVER:  0.09,
    TileType.LAKE:   0.01,
}
```

Edit **`scripts/map_generator.gd`** → `_build_noise()`:

```gdscript
noise.frequency       = 0.06   # lower = wider biome bands
noise.fractal_octaves = 4      # more = rougher edges
```

---

## Controls (in-game)

| Input | Action |
|-------|--------|
| Left-drag | Pan map |
| Arrow keys | Pan map |
| Scroll wheel | Zoom |
| `+` / `-` keys | Zoom |
| **Generate New Map** button | New map (new random seed) |
| Seed field = 0 | Random seed each time |
| Seed field ≠ 0 | Deterministic / reproducible map |

---

## Extending

- **TileMap integration**: replace `TileRenderer._draw()` with calls to
  `TileMap.set_cell()` using `AtlasSource` per biome.
- **Larger maps**: switch `TileRenderer` to `MultiMeshInstance2D` for
  GPU-instanced rendering.
- **More biomes**: add entries to `TileType`, `TILE_COUNTS`,
  `TILE_TEXTURES`, and `TARGET_RATIOS` in `tile_definitions.gd`, then
  insert the new type into the `dist_order` array in `map_generator.gd`.
- **Saving maps**: serialise `_current_map["tiles"]` as a JSON file –
  the data is plain `Array[Array[int]]`.
