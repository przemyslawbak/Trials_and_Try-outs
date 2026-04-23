extends Node

## Mist Transition — dark smoke swirls outward from the centre, dissolving image "2" to reveal image "1".
##
## Scene structure:
##   ImageBurn        ← attach this script here
##   ├── 1            ← TextureRect  (bottom — revealed as smoke clears)
##   └── 2            ← TextureRect  (top    — consumed by smoke expanding from centre)
##
## The shader is applied to node "2" only.
## Animated layered noise creates volumetric-feeling smoke that swirls and
## expands outward from the image centre, with wispy tendrils trailing at
## the annular boundary before dissolving completely.

# ── Exports ──────────────────────────────────────────────────────────────────

@export_group("Timing")
@export var duration:   float = 2.5   ## Seconds for the full transition
@export var auto_play:  bool  = true  ## Start on _ready()

@export_group("Smoke")
## Speed of the smoke layers swirling and expanding outward.
@export_range(0.01, 0.4)  var drift_speed:    float = 0.16
## How turbulent / billowy the smoke edge is. Higher = more chaotic tendrils.
@export_range(0.5, 4.0)   var turbulence:     float = 1.8
## Softness of the smoke boundary. Higher = thicker, hazier transition band.
@export_range(0.02, 0.5)  var smoke_softness: float = 0.10
## Opacity of the smoke colour at its densest (0=invisible, 1=fully opaque).
@export_range(0.0, 1.0)   var smoke_opacity:  float = 0.82
## Primary smoke / mist colour.
@export var smoke_color: Color = Color(0.04, 0.04, 0.06, 1.0)  ## Near-black smoke
## Secondary wisp colour — thinner edges of the smoke tendrils.
@export var wisp_color:  Color = Color(0.12, 0.12, 0.16, 1.0)  ## Dark grey wisps

@export_group("Reveal")
## How the bottom image "1" appears — 0 = instant, 1 = fades in gently.
@export_range(0.0, 1.0) var reveal_softness: float = 0.6
## Noise seed (0 = random every run).
@export var noise_seed: int = 0

# ── Internals ────────────────────────────────────────────────────────────────

var _img2:    TextureRect
var _mat:     ShaderMaterial
var _elapsed: float = 0.0
var _active:  bool  = false

signal transition_finished

# ── Shader ───────────────────────────────────────────────────────────────────
#
#  Five layered noise samples at different scales and drift speeds create
#  a convincing volumetric smoke feel without a 3-D renderer.
#
#  The "smoke front" rises from the bottom: as progress → 1, the threshold
#  climbs from UV.y = 1.0 (bottom) to UV.y = 0.0 (top).
#  Noise warps the front into irregular billowing tendrils.
#  The image dims and desaturates as the smoke thickens over it,
#  then pixels become transparent once fully consumed.
#
const _SHADER := """
shader_type canvas_item;
render_mode blend_mix;

uniform float progress       : hint_range(0.0, 1.05) = 0.0;
uniform float time_val                               = 0.0;
uniform float drift_speed    : hint_range(0.01, 0.4) = 0.16;
uniform float turbulence     : hint_range(0.5, 4.0)  = 1.8;
uniform float smoke_softness : hint_range(0.02, 0.5) = 0.10;
uniform float smoke_opacity  : hint_range(0.0, 1.0)  = 0.82;
uniform float reveal_soft    : hint_range(0.0, 1.0)  = 0.6;

uniform vec4 smoke_color : source_color = vec4(0.04, 0.04, 0.06, 1.0);
uniform vec4 wisp_color  : source_color = vec4(0.12, 0.12, 0.16, 1.0);

uniform sampler2D noise_a;   // 128 px  — macro smoke bodies
uniform sampler2D noise_b;   // 256 px  — fine wisp detail
uniform sampler2D noise_c;   // 64  px  — slow rolling base

// ── animated multi-layer smoke field ─────────────────────────────────────
float smoke_density(vec2 uv, float t) {
	vec2  c     = uv - vec2(0.5);
	float angle = atan(c.y, c.x);
	float spin1 =  t * drift_speed * 0.30;
	float spin2 = -t * drift_speed * 0.22;
	float spin3 =  t * drift_speed * 0.17;

	float push  = t * drift_speed * 0.55;

	vec2 off1 = vec2(cos(angle + spin1), sin(angle + spin1)) * push * 0.60;
	vec2 off2 = vec2(cos(angle + spin2), sin(angle + spin2)) * push * 0.80;
	vec2 off3 = vec2(cos(angle + spin3), sin(angle + spin3)) * push * 0.40;

	float sway = sin(t * 0.18) * 0.010;

	float v  = texture(noise_a, uv * 1.0  + off1                      ).r * 0.38;
	      v += texture(noise_b, uv * 2.1  + off2 + vec2(0.31,  0.71)  ).r * 0.24;
	      v += texture(noise_c, uv * 0.55 + off3                       ).r * 0.20;
	      v += texture(noise_b, uv * 3.8  + off1 + vec2(0.67,  0.13)  ).r * 0.12;
	      v += texture(noise_a, uv * 1.5  + off2 + vec2(sway,  -sway) ).r * 0.06;

	return v;
}

void fragment() {
	vec4 tex = texture(TEXTURE, UV);
	if (tex.a < 0.01) { discard; }

	float smoke = smoke_density(UV, time_val);

	// ── domain warp: warp the UV before sampling smoke a second time ──
	// This folds the noise back on itself, producing deeply irregular,
	// non-circular tendrils rather than a smooth ring.
	float warp_coarse = (texture(noise_a,
		UV * 1.8 + vec2(time_val * drift_speed * 0.28, time_val * drift_speed * 0.19)
	).r - 0.5) * 2.0;
	float warp_fine = (texture(noise_b,
		UV * 3.7 + vec2(-time_val * drift_speed * 0.41, time_val * drift_speed * 0.33)
	).r - 0.5) * 2.0;
	float warp_micro = (texture(noise_c,
		UV * 7.2 + vec2(time_val * drift_speed * 0.55, -time_val * drift_speed * 0.47)
	).r - 0.5) * 2.0;

	// Combine three warp scales — coarse shape, fine tears, micro fraying.
	float warp = warp_coarse * turbulence * smoke_softness * 1.20
	           + warp_fine   * turbulence * smoke_softness * 0.55
	           + warp_micro  * turbulence * smoke_softness * 0.25;

	// Radial distance from centre: 0 at centre, 1.0 at corners.
	float dist_centre  = length(UV - vec2(0.5)) * 1.4142;
	float front_radius = progress * (1.0 + smoke_softness);
	float dist = dist_centre - front_radius + warp;  // <0 consumed, >0 clear

	// band: 1.0 = fully consumed, 0.0 = untouched.
	float band = clamp(-dist / smoke_softness, 0.0, 1.0);

	// Extra high-frequency fraying right at the boundary.
	float angle_uv    = atan(UV.y - 0.5, UV.x - 0.5);
	float edge_detail = texture(noise_b,
		UV * 6.5 + vec2(cos(angle_uv + time_val * drift_speed * 1.3),
		                sin(angle_uv + time_val * drift_speed * 1.1)) * 0.06
	).r;
	// Sharpen so tendrils are spiky, not blobby.
	edge_detail = pow(edge_detail, 0.7);
	float tendril = edge_detail * (1.0 - abs(band - 0.5) * 2.0);

	if (band >= 1.0) {
		discard;

	} else if (band > 0.0) {
		float smoke_mix = smoothstep(0.0, 1.0, band);

		// Desaturate as smoke covers the image.
		float lum  = dot(tex.rgb, vec3(0.299, 0.587, 0.114));
		vec3 desat = mix(vec3(lum), tex.rgb, 1.0 - smoke_mix * 0.8);

		// Darken toward smoke colour.
		vec3 smoked = mix(desat, smoke_color.rgb, smoke_mix * smoke_opacity);

		// Wisp highlights at the churning edge.
		smoked = mix(smoked, wisp_color.rgb, tendril * (1.0 - smoke_mix) * 0.6);

		// Alpha: fade out; wisp tails keep a little opacity at the very edge.
		float alpha = (1.0 - smoothstep(0.3, 1.0, band)) * tex.a;
		alpha = max(alpha, tendril * (1.0 - band) * 0.35);

		COLOR = vec4(smoked, alpha);

	} else {
		// Untouched — faint smoke bloom bleeds inward toward the front.
		float bloom  = clamp(1.0 - dist / (smoke_softness * 0.5), 0.0, 1.0);
		bloom        = pow(bloom, 2.2) * smoke_opacity * 0.25;
		vec3 bloomed = mix(tex.rgb, smoke_color.rgb, bloom);
		COLOR        = vec4(bloomed, tex.a);
	}
}
"""

# ── Lifecycle ────────────────────────────────────────────────────────────────

func _ready() -> void:
	_img2 = $"2"

	var shader := Shader.new()
	shader.code = _SHADER

	_mat = ShaderMaterial.new()
	_mat.shader = shader

	var seed_val := noise_seed if noise_seed != 0 else randi()
	_mat.set_shader_parameter("noise_a",       _make_noise(128, seed_val,        0.030, 4))
	_mat.set_shader_parameter("noise_b",       _make_noise(256, seed_val + 1337, 0.050, 5))
	_mat.set_shader_parameter("noise_c",       _make_noise(64,  seed_val + 42,   0.018, 3))

	_sync_params()
	_mat.set_shader_parameter("progress",  0.0)
	_mat.set_shader_parameter("time_val",  0.0)

	_img2.material = _mat

	if auto_play:
		start()


func _process(delta: float) -> void:
	if not _active:
		return

	_elapsed += delta

	var p : float = clamp(_elapsed / duration, 0.0, 1.0)

	_mat.set_shader_parameter("progress",  p)
	_mat.set_shader_parameter("time_val",  _elapsed)

	if p >= 1.0:
		_active = false
		_img2.visible = false
		transition_finished.emit()

# ── Public API ───────────────────────────────────────────────────────────────

func start() -> void:
	## Play the smoke transition.
	_elapsed = 0.0
	_active  = true
	_img2.visible = true
	_sync_params()
	_mat.set_shader_parameter("progress", 0.0)
	_mat.set_shader_parameter("time_val", 0.0)


func reset() -> void:
	## Restore image "2" to fully visible.
	_active  = false
	_elapsed = 0.0
	_img2.visible = true
	_mat.set_shader_parameter("progress", 0.0)
	_mat.set_shader_parameter("time_val", 0.0)

# ── Helpers ──────────────────────────────────────────────────────────────────

func _sync_params() -> void:
	_mat.set_shader_parameter("drift_speed",    drift_speed)
	_mat.set_shader_parameter("turbulence",     turbulence)
	_mat.set_shader_parameter("smoke_softness", smoke_softness)
	_mat.set_shader_parameter("smoke_opacity",  smoke_opacity)
	_mat.set_shader_parameter("reveal_soft",    reveal_softness)
	_mat.set_shader_parameter("smoke_color",    smoke_color)
	_mat.set_shader_parameter("wisp_color",     wisp_color)


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
