#version 330 core

varying vec3 n;
varying vec3 v;

varying vec2 tex;
uniform sampler2D obj_texture;

uniform float obj_diff;
uniform float obj_spec;
uniform float obj_refl;

uniform float ambient;
varying vec3 l0_dir_;
varying vec3 l0_r_;
uniform float l0_diff;
uniform float l0_spec;

varying vec3 l1;
varying vec3 r1;
uniform float l1_diff;
uniform float l1_spec;

varying vec3 l2_l;
uniform float l2_phi;
varying vec3 l2_dir_;
uniform float l2_diff;
uniform float l2_spec;

void main(void) {
	vec3 n_ = normalize(n);
	vec3 v_ = normalize(v);

	vec3 l1_ = normalize(l1);
	vec3 r1_ = normalize(r1);

	float lightness0 = 
		max(dot(n_, l0_dir_), 0.0) * l0_diff * obj_diff + 
		pow(max(dot(v_, l0_r_), 0.0), obj_refl) * l0_spec * obj_spec;

	float lightness1 = 
		max(dot(n_, l1_), 0.0) * l1_diff * obj_diff + 
		pow(max(dot(v_, r1_), 0.0), obj_refl) * l1_spec * obj_spec;

	vec3 l2_l_ = normalize(l2_l);
	vec3 l2_r_ = reflect(l2_l_, n_);
	float f_sp = dot(l2_l_ * (-1), l2_dir_);
	if (f_sp < cos(l2_phi))
		f_sp = 0;
	float lightness2 = f_sp *
		max(dot(n_, l2_l), 0.0) * l2_diff * obj_diff + 
		pow(max(dot(v_, l2_r_), 0.0), obj_refl) * l2_spec * obj_spec;

	float lightness = ambient + lightness0 + lightness1 + lightness2;

	vec3 tex_clr = vec3(texture(obj_texture, tex) * lightness);
	gl_FragColor = vec4(tex_clr, 1.0);
}