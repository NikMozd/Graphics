varying vec3 n;
varying vec3 v;
varying vec2 tex;

uniform vec3 l0_dir;
varying vec3 l0_dir_;
varying vec3 l0_r_;

varying vec3 l1;
varying vec3 r1;

uniform vec3 l2_pos;
uniform vec3 l2_dir;
varying vec3 l2_dir_;
varying vec3 l2_l;

uniform vec3 eyePos;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;
uniform mat4 normal;

void main(void) {
	vec3 p = vec3(model * gl_Vertex);
	l1 = normalize(vec3(gl_LightSource[0].position) - p);
	n = normalize(vec3(normal * vec4(gl_Normal, 1.0)));
	v = normalize(eyePos - p);
	r1 = reflect(l1 * (-1.0), n);
	tex = vec2(gl_MultiTexCoord0);
	l0_dir_ = normalize(l0_dir);
	l0_r_ = reflect(l0_dir_ * (-1.0), n);
	l2_l = normalize(l2_pos - p);
	l2_dir_ = normalize(l2_dir);
	gl_Position = projection * view * model * gl_Vertex;
}