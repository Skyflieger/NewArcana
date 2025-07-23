shader_type canvas_item;

uniform float alpha : hint_range(0, 1) = 0.0;

void fragment() {
    vec4 texture_color = texture(TEXTURE, UV);
    COLOR = vec4(texture_color.rgb, texture_color.a * alpha);
}
