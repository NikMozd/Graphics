#version 330 core
varying vec3 l;
varying vec3 n;
varying vec3 v;
varying vec3 r;
varying vec2 tex;
uniform sampler2D our_texture;
void main(void) {
	float lightness = 0.2 + max(dot(n, l), 0.0) + pow(max(dot(v, r), 0.0), 5);
	vec3 tex_clr = vec3(texture(our_texture, tex) * lightness);
	gl_FragColor = vec4(tex_clr, 1.0);
}