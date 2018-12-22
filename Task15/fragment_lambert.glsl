varying vec3 l;
varying vec3 n;
uniform vec3 color;
void main(void) {
	vec4 clr = vec4(color, 1.0);
	vec3 n2 = normalize(n);
	vec3 l2 = normalize(l);
	vec4 diff = clr * max(dot(n2, l2), 0.0);
	gl_FragColor = diff;
}