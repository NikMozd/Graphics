varying float lightness;
varying vec2 tex;
uniform vec3 eyePos;
void main(void) {
	vec3 p = vec3(gl_ModelViewMatrix * gl_Vertex);
	vec3 l = normalize(vec3(gl_LightSource[0].position) - p);
	vec3 n = normalize(gl_NormalMatrix * gl_Normal);
	vec3 v = normalize(eyePos - p);
	vec3 i = l * (-1.0);
	vec3 r = normalize(i - n * 2 * dot(n, i));
	lightness = 0.2 + max(dot(n, l), 0.0) + pow(max(dot(v, r), 0.0), 5);
	tex = vec2(gl_MultiTexCoord0);
	gl_Position = gl_ModelViewProjectionMatrix * gl_Vertex;
}