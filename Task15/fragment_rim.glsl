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
	vec3 h2 = normalize(l2 + l2);
	vec4 clr = vec4(color, 1.0);

    vec4  specColor = vec4( 0.8, 0.0, 0.8, 1.0 );
    float specPower = 2.0;
    float rimPower  = 1.0;
    float bias      = 0.8;

    vec4  diff = clr * max(dot(n2, l2), 0.0);
    vec4  spec = specColor * pow(max(dot(n2, h2), 0.0), specPower);
    float rim  = pow(1.0 + bias - max(dot(n2, v2), 0.0), rimPower);
    gl_FragColor = diff + rim * vec4 ( 0.5, 0.0, 0.2, 1.0 ) + spec * specColor;
}