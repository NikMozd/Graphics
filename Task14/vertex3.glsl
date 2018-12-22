varying vec3 l;
varying vec3 n;
varying vec3 v;
varying vec3 r;
varying vec2 tex;
uniform vec3 eyePos;
void main(void) {
	vec3 p = vec3(gl_ModelViewMatrix * gl_Vertex);
	l = normalize(vec3(gl_LightSource[0].position) - p);
	n = normalize(gl_NormalMatrix * gl_Normal);
	v = normalize(eyePos - p);
	vec3 i = l * (-1.0);
	r = normalize(i - n * 2 * dot(n, i));
	tex = vec2(gl_MultiTexCoord0);
	gl_Position = gl_ModelViewProjectionMatrix * gl_Vertex;
}