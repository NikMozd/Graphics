varying vec3 l;
varying vec3 n;
varying vec3 v;
uniform vec3 color;
void main(void) {
	vec4 clr = vec4(color, 1.0);
	vec3 n2 = normalize(n);
	vec3 l2 = normalize(l);
	vec3 v2 = normalize(v);
	const float k = 0.8;
	float d1 = pow(max(dot(n2, l2), 0), 1.0 + k);
	float d2 = pow(1.0 - dot(n2, v2), 1.0 - k);
	vec4 res = clr * d1 * d2;
	gl_FragColor = res;
}