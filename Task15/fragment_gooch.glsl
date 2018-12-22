varying vec3 l;
varying vec3 n;
varying vec3 r;
varying vec3 v;
uniform vec3 color;
void main(void) {
	vec3 n2 = normalize(n);
	vec3 l2 = normalize(l);
	vec3 r2 = normalize(r);
	vec3 v2 = normalize(v);
	
	vec3 clr_warm = vec3(0.6, 0.0, 0.0);
	vec3 clr_cold = vec3(0.0, 0.0, 0.6);
	float diff_warm = 0.4;
	float diff_cold = 0.4;

	vec3 k_cold = min(clr_cold + diff_cold * color, 1.0);
	vec3 k_warm = min(clr_warm + diff_warm * color, 1.0);
	vec3 k_fin  = mix(k_cold, k_warm, dot(n2, l2));
	float spec  = pow(max(dot(r2, v2), 0.0), 32.0);
	vec4 res = vec4(min(k_fin + spec, 1.0), 1.0);
	gl_FragColor = res;
}