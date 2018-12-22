varying vec3 l;
varying vec3 n;
varying vec3 v;
varying vec3 r;
uniform vec3 color;
void main(void) {
	float lightness = 0.2 + max(dot(n, l), 0.0) + pow(max(dot(v, r), 0.0), 5);
	gl_FragColor = vec4(color * lightness, 1.0);
}