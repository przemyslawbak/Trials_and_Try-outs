extends Node

## Attach this script to the ImageBurn node.
##
## Scene structure:
##   ImageBurn        ← attach this script here
##   ├── 1            ← TextureRect  (bottom image — revealed beneath)
##   └── 2            ← TextureRect  (top image — fades into darkness)
##
## Node "2" darkens from pure black outward, as though a shadow or void is
## consuming it from within.  As its pixels become fully black they become
## transparent, gradually unveiling node "1" underneath.

# ── Exports ──────────────────────────────────────────────────────────────────

@export_group("Timing")
@export var fade_duration: float = 3.5  ## Seconds for a full fade
@export var auto_play:     bool  = true ## Begin on _ready()

@export_group("Fade Shape")
## How the darkness spreads.
## 0 = pure noise (organic blotches), 1 = clean radial from centre.
@export_range(0.0, 1.0) var radial_blend: float = 0.25
## Direction bias for the darkness wave (degrees, 0 = no bias / uniform).
## e.g. 270 = sweeps top-down, 90 = bottom-up, 0 = pure radial/noise.
@export_range(0.0, 360.0) var sweep_angle: float = 0.0
## Strength of the directional sweep vs radial/noise (0 = off).
@export_range(0.0, 1.0)   var sweep_strength: float = 0.0
## Noise seed (0 = random each run).
@export var noise_seed: int = 0

@export_group("Darkness Edge")
## Soft gradient width at the darkness boundary (UV units).
## Larger = a wider, smoother fade-in of the black; smaller = sharper edge.
@export_range(0.005, 0.25) var edge_softness: float = 0.06
## Colour the darkness fades TO (default: pure black).
@export var dark_color: Color = Color(0.0, 0.0, 0.0, 1.0)

@export_group("Vignette")
## Add a subtle brightness dip around the edges of the whole image
## as the fade progresses (0 = off, 1 = strong vignette).
@export_range(0.0, 1.0) var vignette_strength: float = 0.35

# ── Internals ────────────────────────────────────────────────────────────────

var _top_image:    TextureRect   # node "2"
var _mat:          ShaderMaterial
var _progress:     float = 0.0
var _active:       bool  = false
var _elapsed:      float = 0.0

signal fade_finished

# ── Shader ───────────────────────────────────────────────────────────────────
#
#  Applied to node "2" only.
#  Each pixel darkens toward dark_color then fades its alpha to 0,
#  revealing node "1" beneath — strictly bounded to node "2"'s own rect.
#
const _SHADER := """
shader_type canvas_item;
render_mode blend_mix;

uniform float progress       : hint_range(0.0, 1.1) = 0.0;
uniform float edge_softness  : hint_range(0.005, 0.25) = 0.06;
uniform float radial_blend   : hint_range(0.0, 1.0)    = 0.25;
uniform float sweep_angle_rad                          = 0.0;
uniform float sweep_strength : hint_range(0.0, 1.0)    = 0.0;
uniform float vignette_str   : hint_range(0.0, 1.0)    = 0.35;
uniform vec4  dark_color     : source_color = vec4(0.0, 0.0, 0.0, 1.0);

uniform sampler2D noise_coarse;   // 128 px Perlin
uniform sampler2D noise_fine;     // 256 px Perlin

// ── helpers ──────────────────────────────────────────────────────────────

float fbm(sampler2D tex, vec2 uv) {
	float v  = texture(tex, uv                          ).r * 0.50;
	      v += texture(tex, uv * 2.1 + vec2(0.31, 0.71)).r * 0.25;
	      v += texture(tex, uv * 4.5 + vec2(0.67, 0.13)).r * 0.15;
	      v += texture(tex, uv * 9.1 + vec2(0.44, 0.88)).r * 0.10;
	// Sharpen: push values toward 0/1 for a crisper, non-blobby edge.
	v = clamp((v - 0.5) * 2.0 + 0.5, 0.0, 1.0);
	return v;
}

void fragment() {
	vec4 tex = texture(TEXTURE, UV);
	if (tex.a < 0.01) { discard; }

	// ── darkness mask ──────────────────────────────────────────────────
	float coarse    = fbm(noise_coarse, UV);
	float fine      = fbm(noise_fine,   UV * 1.6 + vec2(0.2, 0.5));
	float noise_val = coarse * 0.65 + fine * 0.35;

	// Radial: 0 at centre → 1 at corners.
	float radial = length(UV - vec2(0.5)) * 1.4142;

	// Optional directional sweep (e.g. top-down).
	vec2 dir = vec2(cos(sweep_angle_rad), sin(sweep_angle_rad));
	float sweep = dot(UV - vec2(0.5), dir) + 0.5;  // 0..1 across the image

	float mask = noise_val;
	mask = mix(mask, radial, radial_blend);
	mask = mix(mask, sweep,  sweep_strength);

	// ── vignette ───────────────────────────────────────────────────────
	// Independent of the fade — darkens edges gently as progress grows.
	float vig_dist = length(UV - vec2(0.5)) * 1.4142;  // 0 centre, 1 corner
	float vig = pow(vig_dist, 2.5) * vignette_str * progress;

	// ── classify pixel ─────────────────────────────────────────────────
	// The darkness threshold sweeps from 0 → 1 as progress increases.
	float threshold = progress;

	if (mask < threshold - edge_softness) {
		// Fully consumed by darkness → transparent (reveals node "1").
		discard;

	} else if (mask < threshold) {
		// Softening edge: image fades smoothly into dark_color then vanishes.
		float t = (mask - (threshold - edge_softness)) / edge_softness;
		// t = 1 at the untouched side, 0 at the fully-dark side.

		// Darken the pixel toward dark_color as t → 0.
		vec3 darkened = mix(dark_color.rgb, tex.rgb, t);
		// Then fade alpha to 0 in the innermost part of the edge.
		float alpha   = smoothstep(0.0, 0.55, t) * tex.a;

		COLOR = vec4(darkened, alpha);

	} else {
		// Untouched region — apply vignette only.
		vec3 vignetted = mix(tex.rgb, dark_color.rgb, vig);
		COLOR = vec4(vignetted, tex.a);
	}
}
"""

# ── Lifecycle ────────────────────────────────────────────────────────────────

func _ready() -> void:
	_top_image = $"2"   # node "2" is the TOP image that fades out

	var shader := Shader.new()
	shader.code = _SHADER

	_mat = ShaderMaterial.new()
	_mat.shader = shader

	var seed_val := noise_seed if noise_seed != 0 else randi()
	_mat.set_shader_parameter("noise_coarse",    _make_noise(128, seed_val,       0.035, 5))
	_mat.set_shader_parameter("noise_fine",      _make_noise(256, seed_val + 999, 0.055, 6))

	_sync_params()
	_mat.set_shader_parameter("progress", 0.0)

	_top_image.material = _mat

	if auto_play:
		start_fade()


func _process(delta: float) -> void:
	if not _active:
		return

	_elapsed   += delta
	_progress   = clamp(_elapsed / fade_duration, 0.0, 1.0)
	_mat.set_shader_parameter("progress", _progress)

	if _progress >= 1.0:
		_active = false
		_top_image.visible = false
		fade_finished.emit()

# ── Public API ───────────────────────────────────────────────────────────────

func start_fade() -> void:
	## Trigger (or restart) the fade-to-darkness animation.
	_elapsed  = 0.0
	_progress = 0.0
	_active   = true
	_top_image.visible = true
	_sync_params()
	_mat.set_shader_parameter("progress", 0.0)


func reset() -> void:
	## Restore node "2" to fully visible.
	_active   = false
	_elapsed  = 0.0
	_progress = 0.0
	_top_image.visible = true
	_mat.set_shader_parameter("progress", 0.0)

# ── Helpers ──────────────────────────────────────────────────────────────────

func _sync_params() -> void:
	## Push all export values to the shader uniforms.
	_mat.set_shader_parameter("edge_softness",   edge_softness)
	_mat.set_shader_parameter("radial_blend",    radial_blend)
	_mat.set_shader_parameter("sweep_angle_rad", deg_to_rad(sweep_angle))
	_mat.set_shader_parameter("sweep_strength",  sweep_strength)
	_mat.set_shader_parameter("vignette_str",    vignette_strength)
	_mat.set_shader_parameter("dark_color",      dark_color)


func _make_noise(size: int, seed_val: int, freq: float, octaves: int) -> ImageTexture:
	var img := Image.create(size, size, false, Image.FORMAT_RF)
	var fn   := FastNoiseLite.new()
	fn.noise_type      = FastNoiseLite.TYPE_PERLIN
	fn.frequency       = freq
	fn.fractal_octaves = octaves
	fn.seed            = seed_val
	for y in size:
		for x in size:
			var v: float = fn.get_noise_2d(x, y)
			v = (v + 1.0) * 0.5
			img.set_pixel(x, y, Color(v, v, v, 1.0))
	return ImageTexture.create_from_image(img)
