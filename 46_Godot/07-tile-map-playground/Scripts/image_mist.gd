extends Node

## Slow Pan + Cross-Dissolve — cinematic narration effect.
##
## Scene structure:
##   ImageBurn        ← attach this script here
##   ├── 1            ← TextureRect  (second image — revealed by dissolve)
##   └── 2            ← TextureRect  (first image — pans Left→Right, then dissolves out)
##
## Timeline:
##   Phase 1 — HOLD    : image "2" sits still for a moment (optional beat)
##   Phase 2 — PAN     : image "2" slowly pans Left→Right (Ken Burns style)
##   Phase 3 — DISSOLVE: image "2" cross-fades into image "1" underneath
##
## Both nodes should use STRETCH_MODE_KEEP_ASPECT_COVERED (or similar) so
## there is texture to pan into.  The pan works by shifting the MATERIAL
## OFFSET of node "2"; no viewport or camera needed.

# ── Exports ──────────────────────────────────────────────────────────────────

@export_group("Timing")
## Seconds to hold still before the pan begins.
@export var hold_duration:     float = 0.6
## Seconds for the pan movement.
@export var pan_duration:      float = 4.5
## Seconds for the cross-dissolve after the pan.
@export var dissolve_duration: float = 1.2
## Start automatically on _ready().
@export var auto_play:         bool  = true

@export_group("Pan")
## Total UV distance panned (0.08 = 8% of image width — subtle and cinematic).
## Increase for a more dramatic push.
@export_range(0.02, 0.40) var pan_amount: float = 0.10
## Easing of the pan motion.
## 0 = linear, 1 = ease-in-out (cinematic), 2 = ease-in only, 3 = ease-out only.
@export_range(0, 3) var pan_easing: int = 1
## Gentle zoom-in during the pan (1.0 = no zoom, 1.05 = 5% zoom-in).
@export_range(1.0, 1.20) var zoom_amount: float = 1.04

@export_group("Dissolve")
## Dissolve style.
## 0 = simple alpha fade, 1 = luminance-dip (dips through black mid-dissolve).
@export_range(0, 1) var dissolve_style: int = 0
## Dip-to-black depth when dissolve_style = 1  (0 = no dip, 1 = full black dip).
@export_range(0.0, 1.0) var dip_strength: float = 0.55

# ── Internals ────────────────────────────────────────────────────────────────

var _img1:    TextureRect   # bottom — "1"
var _img2:    TextureRect   # top    — "2"
var _mat2:    ShaderMaterial

var _elapsed: float = 0.0
var _active:  bool  = false

# Phase durations baked at start for convenience.
var _t_pan_start:      float
var _t_dissolve_start: float
var _t_total:          float

signal effect_finished

# ── Shader ───────────────────────────────────────────────────────────────────
# Applied to node "2" only.
# Handles both the UV pan/zoom AND the cross-dissolve alpha in one pass.
const _SHADER := """
shader_type canvas_item;
render_mode blend_mix;

uniform vec2  uv_offset    = vec2(0.0, 0.0);   // animated pan offset
uniform float uv_scale     = 1.0;               // animated zoom  (>1 = zoom in)
uniform float dissolve_t   : hint_range(0.0, 1.0) = 0.0;  // 0=opaque 1=transparent
uniform int   dissolve_style                   = 0;
uniform float dip_strength : hint_range(0.0, 1.0) = 0.55;

void fragment() {
	// ── pan + zoom ─────────────────────────────────────────────────────
	// Scale UV around image centre, then shift by offset.
	vec2 uv = (UV - vec2(0.5)) * uv_scale + vec2(0.5) + uv_offset;
	uv = clamp(uv, vec2(0.0), vec2(1.0));

	vec4 col = texture(TEXTURE, uv);
	if (col.a < 0.01) { discard; }

	// ── dissolve ───────────────────────────────────────────────────────
	if (dissolve_style == 0) {
		// Simple alpha fade.
		col.a *= 1.0 - dissolve_t;

	} else {
		// Luminance dip: brightness collapses to black at mid-dissolve,
		// then alpha fades — mimics a photochemical cross-dissolve.
		float dip = sin(dissolve_t * PI);            // peaks at t=0.5
		col.rgb   = mix(col.rgb, vec3(0.0), dip * dip_strength);
		col.a    *= 1.0 - dissolve_t;
	}

	COLOR = col;
}
"""

# ── Lifecycle ────────────────────────────────────────────────────────────────

func _ready() -> void:
	_img1 = $"1"
	_img2 = $"2"

	# Node "1" starts invisible — it will show through as "2" dissolves.
	_img1.visible = true
	_img1.modulate.a = 1.0

	var shader := Shader.new()
	shader.code = _SHADER

	_mat2 = ShaderMaterial.new()
	_mat2.shader = shader
	_mat2.set_shader_parameter("uv_offset",      Vector2.ZERO)
	_mat2.set_shader_parameter("uv_scale",       1.0)
	_mat2.set_shader_parameter("dissolve_t",     0.0)
	_mat2.set_shader_parameter("dissolve_style", dissolve_style)
	_mat2.set_shader_parameter("dip_strength",   dip_strength)
	_img2.material = _mat2

	_bake_timeline()

	if auto_play:
		start()


func _process(delta: float) -> void:
	if not _active:
		return

	_elapsed += delta
	var t: float = clamp(_elapsed, 0.0, _t_total)

	# ── Phase 1: Hold ──────────────────────────────────────────────────
	if t < _t_pan_start:
		# Slight breathing zoom even during hold — adds life.
		var hold_t : float = t / max(hold_duration, 0.001)
		_mat2.set_shader_parameter("uv_scale",  lerp(1.0, zoom_amount, hold_t * 0.3))
		_mat2.set_shader_parameter("uv_offset", Vector2.ZERO)
		_mat2.set_shader_parameter("dissolve_t", 0.0)

	# ── Phase 2: Pan ───────────────────────────────────────────────────
	elif t < _t_dissolve_start:
		var pan_t := (t - _t_pan_start) / pan_duration
		var eased := _ease(pan_t, pan_easing)

		# Left→Right: UV offset goes from 0 → +pan_amount
		# (shifting the sample window right = image appears to pan right)
		var offset := Vector2(eased * pan_amount, 0.0)
		var scale  : float = lerp(1.0, zoom_amount, eased)

		_mat2.set_shader_parameter("uv_offset", offset)
		_mat2.set_shader_parameter("uv_scale",  scale)
		_mat2.set_shader_parameter("dissolve_t", 0.0)

	# ── Phase 3: Cross-Dissolve ────────────────────────────────────────
	else:
		var dis_t := (t - _t_dissolve_start) / dissolve_duration
		dis_t = clamp(dis_t, 0.0, 1.0)

		# Hold the final pan position during dissolve.
		_mat2.set_shader_parameter("uv_offset", Vector2(pan_amount, 0.0))
		_mat2.set_shader_parameter("uv_scale",  zoom_amount)
		_mat2.set_shader_parameter("dissolve_t", _ease(dis_t, 1))  # ease-in-out

	if _elapsed >= _t_total:
		_active = false
		_img2.visible = false
		effect_finished.emit()

# ── Public API ───────────────────────────────────────────────────────────────

func start() -> void:
	## Play the full pan + dissolve sequence.
	_elapsed = 0.0
	_active  = true
	_img2.visible = true
	_img2.modulate.a = 1.0
	_bake_timeline()
	_mat2.set_shader_parameter("uv_offset",  Vector2.ZERO)
	_mat2.set_shader_parameter("uv_scale",   1.0)
	_mat2.set_shader_parameter("dissolve_t", 0.0)
	_mat2.set_shader_parameter("dissolve_style", dissolve_style)
	_mat2.set_shader_parameter("dip_strength",   dip_strength)


func reset() -> void:
	## Return both images to their starting state.
	_active  = false
	_elapsed = 0.0
	_img2.visible    = true
	_img2.modulate.a = 1.0
	_mat2.set_shader_parameter("uv_offset",  Vector2.ZERO)
	_mat2.set_shader_parameter("uv_scale",   1.0)
	_mat2.set_shader_parameter("dissolve_t", 0.0)


func skip_to_dissolve() -> void:
	## Jump straight to the cross-dissolve phase (useful for skipping narration).
	_elapsed = _t_dissolve_start

# ── Helpers ──────────────────────────────────────────────────────────────────

func _bake_timeline() -> void:
	_t_pan_start      = hold_duration
	_t_dissolve_start = hold_duration + pan_duration
	_t_total          = hold_duration + pan_duration + dissolve_duration


func _ease(t: float, mode: int) -> float:
	t = clamp(t, 0.0, 1.0)
	match mode:
		0: return t                                      # linear
		1: return t * t * (3.0 - 2.0 * t)               # smooth-step (ease in-out)
		2: return t * t                                  # ease in (slow start)
		3: return 1.0 - (1.0 - t) * (1.0 - t)          # ease out (slow end)
		_: return t
