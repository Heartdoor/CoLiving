shader_type canvas_item;

uniform float uv_scale = 1.0;

// Define textures for different colors
uniform sampler2D floor_texture_red : filter_nearest, repeat_enable;
uniform sampler2D floor_texture_green : filter_nearest, repeat_enable;
uniform sampler2D floor_texture_blue : filter_nearest, repeat_enable;
uniform sampler2D floor_texture_yellow : filter_nearest, repeat_enable;


// Define the specific colors you want to match (hex codes converted to RGB)
const vec3 color_red = vec3(1.0, 0.0, 0.0); // Hex #FF0000
const vec3 color_green = vec3(0.0, 1.0, 0.0); // Hex #00FF00
const vec3 color_blue = vec3(0.0, 0.0, 1.0); // Hex #0000FF
const vec3 color_yellow = vec3(1.0, 1.0, 0.0); // Hex #0000FF
varying vec2 pos;

void vertex() {
    vec4 v = MODEL_MATRIX * vec4(VERTEX, 1.0, 1.0);
    pos = v.xy;
}

void fragment() {
    // Sample the texture at the given position
    vec4 sampled_color = texture(TEXTURE, UV);
    vec3 color = sampled_color.rgb;

    // Define a threshold for color matching
    float threshold = 0.1;

    // Determine the closest texture based on the sampled color
    if (distance(color, color_red)<threshold) {
        COLOR = texture(floor_texture_red, pos * uv_scale);
    } else if (distance(color, color_green)<threshold) {
        COLOR = texture(floor_texture_green, pos * uv_scale);
    } else if (distance(color, color_blue)<threshold) {
        COLOR = texture(floor_texture_blue, pos * uv_scale);
    }
else if (distance(color, color_yellow)<threshold){
        COLOR = texture(floor_texture_yellow, pos * uv_scale);
    }

}