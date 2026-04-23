extends Node

## Attach this script to the ImageBurn node (parent of both TextureRect children).
##
## Scene structure:
##   ImageBurn        ← attach this script here
##   ├── 1            ← TextureRect  (top image — burns away)
##   └── 2            ← TextureRect  (bottom image — revealed beneath)
##
## The shader is applied directly to node "1" only.
## Pixels that burn away are discarded → the effect is strictly limited
## to that node's own texture rect, nothing bleeds outside.

# ── Exports ──────────────────────────────────────────────────────────────────

@export_group("Timing")
@export var burn_duration: float = 4.0   ## Seconds for a full burn
@export var auto_play: bool      = true  ## Begin on _ready()

@export_group("Burn Shape")
## How radial vs chaotic the fire front spreads.
## 0 = pure noise chaos, 1 = strict radial wave from centre.
@export_range(0.0, 1.0) var radial_blend: float = 0.35
## Noise seed (0 = random each run).
@export var noise_seed: int = 0

@export_group("Ember Edge")
## Total width of the glowing edge band (UV units, ~0.03–0.10).
@export_range(0.002, 0.15) var edge_width: float  = 0.018
## White-hot core colour.
@export var color_core:  Color = Color(1.00, 0.98, 0.80, 1.0)
## Mid ember colour (orange).
@export var color_ember: Color = Color(1.00, 0.40, 0.02, 1.0)
## Outer char/glow colour (deep red-black).
@export var color_char:  Color = Color(0.18, 0.02, 0.00, 1.0)

@export_group("Char & Darken")
## Width of the dark-charring region just behind the fire front.
@export_range(0.0, 0.3)  var char_width:    float = 0.025
## Maximum darkness of the charred region (1 = pitch black).
@export_range(0.0, 1.0)  var char_strength: float = 0.72

@export_group("Heat Distortion")
## UV distortion magnitude near the fire front.  0 = disabled.
@export_range(0.0, 0.03) var distortion_strength: float = 0.012
## Speed of the shimmer ripple.
@export_range(0.0, 8.0)  var distortion_speed:    float = 3.5

# ── Internals ────────────────────────────────────────────────────────────────

var _top_image:     TextureRect
var _mat:           ShaderMaterial
var _burn_progress: float = 0.0
var _burning:       bool  = false
var _elapsed:       float = 0.0

signal burn_finished

# ── Shader ────────────────────────────────────────────────────────────────────
#
#  Applied exclusively to node "1".
#  Burned pixels are discarded → effect is strictly bounded by the TextureRect.
#  Two noise layers give an organic, multi-scale fire front.
#  A separate animated noise layer drives heat distortion on surviving pixels.
#
const _SHADER := """
shader_type canvas_item;
render_mode blend_mix;

// ── uniforms ────────────────────────────────────────────────────────────
uniform float  burn_progress    : hint_range(0.0, 1.1) = 0.0;
uniform float  edge_width       : hint_range(0.002, 0.15) = 0.018;
uniform float  char_width       : hint_range(0.0, 0.3)   = 0.08;
uniform float  char_strength    : hint_range(0.0, 1.0)   = 0.72;
uniform float  radial_blend     : hint_range(0.0, 1.0)   = 0.35;
uniform float  distortion_str   : hint_range(0.0, 0.03)  = 0.012;
uniform float  distortion_speed : hint_range(0.0, 8.0)   = 3.5;
uniform float  time_val                                   = 0.0;

uniform vec4   color_core  : source_color = vec4(1.00, 0.98, 0.80, 1.0);
uniform vec4   color_ember : source_color = vec4(1.00, 0.40, 0.02, 1.0);
uniform vec4   color_char  : source_color = vec4(0.18, 0.02, 0.00, 1.0);

uniform sampler2D noise_tex;   // coarse burn-shape noise  (128 px)
uniform sampler2D noise_fine;  // fine detail + distortion (256 px)

// ── multi-octave noise from a pre-baked texture ─────────────────────────
float fbm(sampler2D tex, vec2 uv, float t) {
	float v  = texture(tex, uv                              ).r * 0.50;
	      v += texture(tex, uv * 2.10 + vec2(0.31,  0.71)  ).r * 0.25;
	      v += texture(tex, uv * 4.30 + vec2(t * 0.07, 0.0)).r * 0.15;
	      v += texture(tex, uv * 8.70 + vec2(0.0, t * 0.04)).r * 0.10;
	// Sharpen contrast: push values toward 0/1 so the threshold cuts a
	// crisp, thin line instead of a wide blurry gradient.
	v = clamp((v - 0.5) * 2.2 + 0.5, 0.0, 1.0);
	return v;
}

void fragment() {
	// ── 1. heat distortion ─────────────────────────────────────────────
	// Independently animated noise drives UV warp near the fire front.
	float dn1 = texture(noise_fine,
		UV * 3.0 + vec2( time_val * distortion_speed * 0.031,
		                 time_val * distortion_speed * 0.017)).r;
	float dn2 = texture(noise_fine,
		UV * 2.2 + vec2(-time_val * distortion_speed * 0.019,
		                 time_val * distortion_speed * 0.027)).r;

	// Distortion peaks right at the burn front and fades away from it.
	float burn_local = fbm(noise_tex, UV, time_val * 0.4);
	float proximity  = 1.0 - abs(burn_local - burn_progress) / max(edge_width * 2.5, 0.001);
	proximity = pow(clamp(proximity, 0.0, 1.0), 1.5);

	vec2 warped_uv = UV + vec2(
		(dn1 - 0.5) * 2.0,
		(dn2 - 0.5) * 2.0
	) * distortion_str * proximity;

	// Hard-clamp to [0,1]: keeps distortion strictly inside the node rect.
	warped_uv = clamp(warped_uv, vec2(0.0), vec2(1.0));

	vec4 tex = texture(TEXTURE, warped_uv);

	// Discard fully transparent source pixels (non-rect sprites, etc.)
	// so the fire never appears on empty areas of the texture.
	if (tex.a < 0.01) { discard; }

	// ── 2. burn mask ───────────────────────────────────────────────────
	// Coarse noise drives the macro shape; fine noise adds jagged detail.
	float coarse    = fbm(noise_tex,  UV, time_val * 0.4);
	float fine      = fbm(noise_fine, UV, time_val * 0.6);
	float noise_val = coarse * 0.70 + fine * 0.30;

	// Optional radial component: fire starts in the centre and spreads out.
	float radial    = length(UV - vec2(0.5)) * 1.4142;
	float burn_mask = mix(noise_val, radial, radial_blend);

	float threshold    = burn_progress;
	float burned_edge  = threshold - edge_width;
	float charred_edge = burned_edge - char_width;

	// ── 3. classify and shade ──────────────────────────────────────────
	if (burn_mask < burned_edge) {
		// Fully burned → discard (transparent → node "2" shows through).
		discard;

	} else if (burn_mask < threshold) {
		// ── Glowing ember band ──────────────────────────────────────────
		// t = 0 at innermost (just-burned) edge, 1 at outermost (unburned) edge.
		float t = (burn_mask - burned_edge) / edge_width;

		// Three-stop colour ramp: char_color → ember → core (white-hot).
		vec4 glow;
		if (t < 0.45) {
			float u = t / 0.45;
			glow = mix(color_char, color_ember, smoothstep(0.0, 1.0, u));
		} else {
			float u = (t - 0.45) / 0.55;
			glow = mix(color_ember, color_core, smoothstep(0.0, 1.0, u));
		}

		// Flicker the glow with animated noise for a live-ember look.
		float flicker = texture(noise_fine,
			UV * 5.0 + vec2(time_val * 1.3, -time_val * 0.9)).r;
		glow.rgb *= 0.75 + 0.25 * flicker;

		// Blend ember over the still-surviving source texture.
		float ember_mix = smoothstep(0.0, 1.0, 1.0 - t * 0.6);
		COLOR.rgb = mix(tex.rgb, glow.rgb, ember_mix);
		COLOR.a   = tex.a;   // preserve source alpha for non-rect sprites

	} else if (burn_mask < threshold + char_width && char_strength > 0.0) {
		// ── Charring band (dark scorch just ahead of the fire front) ───
		float t = (burn_mask - threshold) / char_width; // 0=just ahead 1=undamaged
		float darkness = char_strength * (1.0 - smoothstep(0.0, 1.0, t));

		// Darken and tint toward char colour.
		vec3 charred = mix(
			tex.rgb * (1.0 - darkness),
			color_char.rgb,
			darkness * 0.6
		);
		// Subtle red-orange glow bleed into the char zone near the fire.
		float near_fire = 1.0 - smoothstep(0.0, 0.35, t);
		charred = mix(charred, color_char.rgb * 1.4, near_fire * 0.25);

		COLOR = vec4(charred, tex.a);

	} else {
		// ── Undamaged — render with heat distortion only ────────────────
		COLOR = tex;
	}
}
"""

# ── Lifecycle ────────────────────────────────────────────────────────────────

func _ready() -> void:
	_top_image = $"2"
	
	get_viewport().size_changed.connect(_on_viewport_size_changed)
	_on_viewport_size_changed()

	var shader := Shader.new()
	shader.code = _SHADER

	_mat = ShaderMaterial.new()
	_mat.shader = shader

	var seed_val := noise_seed if noise_seed != 0 else randi()
	_mat.set_shader_parameter("noise_tex",        _make_noise(128, seed_val,       0.035, 5))
	_mat.set_shader_parameter("noise_fine",       _make_noise(256, seed_val + 999, 0.055, 6))
	_mat.set_shader_parameter("burn_progress",    0.0)
	_mat.set_shader_parameter("edge_width",       edge_width)
	_mat.set_shader_parameter("char_width",       char_width)
	_mat.set_shader_parameter("char_strength",    char_strength)
	_mat.set_shader_parameter("radial_blend",     radial_blend)
	_mat.set_shader_parameter("distortion_str",   distortion_strength)
	_mat.set_shader_parameter("distortion_speed", distortion_speed)
	_mat.set_shader_parameter("color_core",       color_core)
	_mat.set_shader_parameter("color_ember",      color_ember)
	_mat.set_shader_parameter("color_char",       color_char)
	_mat.set_shader_parameter("time_val",         0.0)

	_top_image.material = _mat

	if auto_play:
		start_burn()


func _process(delta: float) -> void:
	if not _burning:
		return

	_elapsed       += delta
	_burn_progress  = clamp(_elapsed / burn_duration, 0.0, 1.0)

	# time_val runs independently so distortion/flicker continue at their own pace.
	_mat.set_shader_parameter("time_val",      _elapsed)
	_mat.set_shader_parameter("burn_progress", _burn_progress)

	if _burn_progress >= 1.0:
		_burning = false
		_top_image.visible = false
		burn_finished.emit()

func _on_viewport_size_changed() -> void:
	var viewport_size := get_viewport().get_visible_rect().size
	if is_instance_valid(_top_image):
		_top_image.expand_mode = TextureRect.EXPAND_IGNORE_SIZE
		_top_image.size = viewport_size
	if has_node("1"):
		$"1".expand_mode = TextureRect.EXPAND_IGNORE_SIZE
		$"1".size = viewport_size

# ── Public API ───────────────────────────────────────────────────────────────

func start_burn() -> void:
	## Trigger (or restart) the burn animation.
	_elapsed       = 0.0
	_burn_progress = 0.0
	_burning       = true
	_top_image.visible = true
	_mat.set_shader_parameter("burn_progress", 0.0)
	_mat.set_shader_parameter("time_val",      0.0)


func reset() -> void:
	## Restore the top image to fully visible, no animation.
	_burning       = false
	_elapsed       = 0.0
	_burn_progress = 0.0
	_top_image.visible = true
	_mat.set_shader_parameter("burn_progress", 0.0)
	_mat.set_shader_parameter("time_val",      0.0)

# ── Noise baking ─────────────────────────────────────────────────────────────

func _make_noise(size: int, seed_val: int, freq: float, octaves: int) -> ImageTexture:
	## Bake a Perlin noise field into an RF ImageTexture.
	## Two separate textures (coarse + fine) give the shader richer detail
	## without expensive per-pixel computation.
	var img := Image.create(size, size, false, Image.FORMAT_RF)

	var fn := FastNoiseLite.new()
	fn.noise_type      = FastNoiseLite.TYPE_PERLIN
	fn.frequency       = freq
	fn.fractal_octaves = octaves
	fn.seed            = seed_val

	for y in size:
		for x in size:
			var v: float = fn.get_noise_2d(x, y)
			v = (v + 1.0) * 0.5          # remap [-1, 1] → [0, 1]
			img.set_pixel(x, y, Color(v, v, v, 1.0))

	return ImageTexture.create_from_image(img)
