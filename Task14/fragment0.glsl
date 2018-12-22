varying float lightness;
uniform vec3 color;
void main(void) {
	gl_FragColor = vec4(color * lightness, 1.0);
}