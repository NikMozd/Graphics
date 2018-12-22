varying vec3 l;
varying vec3 n;
varying vec3 v;
uniform vec3 color;
void main(void) {
	vec4 clr = vec4(color, 1.0);
	vec3 n2 = normalize(n);
	vec3 l2 = normalize(l);
	vec3 v2 = normalize(v);
	float light = max(dot(n2, l2), 0.0);
	vec3 h = normalize(l2 + v2);
	float deg = 6;
	light = light + pow(max(dot(n2, h), 0), deg);
	vec4 res = clr * light;
	gl_FragColor = res;
}