varying vec3 l;
varying vec3 n;
uniform vec3 color;
void main(void) {
	vec4 clr = vec4(color, 1.0);
	vec3 n2 = normalize(n);
	vec3 l2 = normalize(l);
	float diff = 0.2 + max(dot(n2, l2), 0.0);
	if (diff < 0.4)
		clr = clr * 0.3;
	else if (diff > 0.7)
		clr = clr * 1.3;
	gl_FragColor = clr;
}