#version 330 core
varying vec3 l1;
varying vec3 n;
varying vec2 tex;
uniform sampler2D obj_texture;
void main(void) {
	vec4 clr = texture(obj_texture, tex);
	vec3 n2 = normalize(n);
	vec3 l2 = normalize(l1);
	float diff = 0.2 + max(dot(n2, l2), 0.0);
	if (diff < 0.4)
		clr = clr * 0.3;
	else if (diff > 0.7)
		clr = clr * 1.3;
	gl_FragColor = clr;
}