shader_type canvas_item;

uniform float speed = 3.0;
uniform float frequency = 5.0;
uniform float amplitude = 0.2;
uniform vec4 flag_color : source_color = vec4(1.0, 1.0, 1.0, 1.0);

void fragment() {
	vec2 uv = UV;
	
	// Left side of the flag should be pinned (less movement), right side waves more
	float pin = uv.x; 
	
	float wave = sin(uv.x * frequency - TIME * speed) * amplitude * pin;
	
	// We want the flag to occupy the center 60% of the rect height
	// So we scale uv.y
	float normalized_y = (uv.y - 0.5) / 0.6 + 0.5 + wave;
	
	if (normalized_y < 0.0 || normalized_y > 1.0) {
		COLOR = vec4(0.0);
	} else {
		// Guidon (swallowtail) cutout on the right
		float cutout_depth = 0.4;
		float cutout_start = 1.0 - cutout_depth;
		
		bool is_cutout = false;
		if (uv.x > cutout_start) {
			float dist_from_tip = uv.x - cutout_start;
			float v_width = dist_from_tip * (0.5 / cutout_depth);
			if (abs(normalized_y - 0.5) < v_width) {
				is_cutout = true;
			}
		}
		
		if (is_cutout) {
			COLOR = vec4(0.0);
		} else {
			COLOR = flag_color;
			// Add shading based on the derivative of the wave
			float shade = cos(uv.x * frequency - TIME * speed) * 0.3 * pin + 0.7;
			COLOR.rgb *= shade;
		}
	}
}
