#include <gl/glew.h>
#include <gl/freeglut.h>
#include <gl/SOIL.h>
#include <cmath>
#include <assimp/Importer.hpp>
#include <assimp/scene.h>
#include <assimp/postprocess.h>
#include <vector>
#include <iostream>

static int w = 0, h = 0;
float ambient[4] = { 0.3, 0.3, 0.3, 1 };

GLuint Program1, Program2;
GLint Unif_color1, Unif_param1, Unif_param2, Unif_param3, Unif_param4;

void initShader()
{
	const char * fsSource1 =
		"uniform vec4 color;\n"
		"void main() {\n"
		"    gl_FragColor = color;\n"
		"}\n";
	GLuint fShader1 = glCreateShader(GL_FRAGMENT_SHADER);
	glShaderSource(fShader1, 1, &fsSource1, NULL);
	glCompileShader(fShader1);
	Program1 = glCreateProgram();
	glAttachShader(Program1, fShader1);
	glLinkProgram(Program1);
	const char * unif_name1 = "color";
	Unif_color1 = glGetUniformLocation(Program1, unif_name1);

	const char * fsSource2 =
		"uniform vec4 color1;\n"
		"uniform vec4 color2;\n"
		"uniform float width1;\n"
		"uniform float width2;\n"
		"void main() {\n"
		"    if (mod(gl_FragCoord.x,width1+width2)<width1)\n"
		"        gl_FragColor = color1;\n"
		"    else\n"
		"        gl_FragColor = color2;\n"
		"}\n";
	GLuint fShader2 = glCreateShader(GL_FRAGMENT_SHADER);
	glShaderSource(fShader2, 1, &fsSource2, NULL);
	glCompileShader(fShader2);
	Program2 = glCreateProgram();
	glAttachShader(Program2, fShader2);
	glLinkProgram(Program2);
	const char * unif_name21 = "color1";
	Unif_param1 = glGetUniformLocation(Program2, unif_name21);
	const char * unif_name22 = "color2";
	Unif_param2 = glGetUniformLocation(Program2, unif_name22);
	const char * unif_name23 = "width1";
	Unif_param3 = glGetUniformLocation(Program2, unif_name23);
	const char * unif_name24 = "width2";
	Unif_param4 = glGetUniformLocation(Program2, unif_name24);
}

void Init(void)
{
	glEnable(GL_DEPTH_TEST);
	glEnable(GL_LIGHTING);
	glEnable(GL_COLOR_MATERIAL);
	glClearColor(0.5, 0, 0.5, 1);
	glLightModelfv(GL_LIGHT_MODEL_AMBIENT, ambient);
}

void Update(void)
{
	glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
	glLoadIdentity();

	gluLookAt(1, 0, 5, 1, 0, 0, 0, 1, 0);

	glUseProgram(Program1);
	static float red[4] = { 1.0f, 0.0f, 0.0f, 1.0f };
	glUniform4fv(Unif_color1, 1, red); 
	glBegin(GL_QUADS);  
		glVertex2f(-0.5f, -0.5f);  
		glVertex2f(-0.5f, 0.5f);  
		glVertex2f(0.5f, 0.5f);  
		glVertex2f(0.5f, -0.5f);  
	glEnd();
	glFlush();
	glUseProgram(0);

	glUseProgram(Program2);
	static float blue[4] = { 0.0f, 0.0f, 1.0f, 1.0f };
	static float green[4] = { 0.7f, 1.0f, 0.7f, 1.0f };
	glUniform4fv(Unif_param1, 1, blue);
	glUniform4fv(Unif_param2, 1, green);
	glUniform1f(Unif_param3, 2);
	glUniform1f(Unif_param4, 10);
	glBegin(GL_TRIANGLES);
		glVertex2f(3.4f, -0.3f);
		glVertex2f(3.0f, 1.1f);
		glVertex2f(1.5f, 0.0f);
	glEnd();
	glFlush();
	glUseProgram(0);

	glutSwapBuffers();
}

void Reshape(int width, int height)
{
	w = width;
	h = height;
	glViewport(0, 0, w, h);
	glMatrixMode(GL_PROJECTION);
	glLoadIdentity();
	gluPerspective(60.0f, w / h, 1.0f, 1000.0f);
	glMatrixMode(GL_MODELVIEW);
}

void SpecialKeys(int key, int x, int y)
{

}

void Keyboard(unsigned char key, int x, int y)
{

}

void freeShader()
{
	glUseProgram(0);
	glDeleteProgram(Program1);
	glDeleteProgram(Program2);
}

int main(int argc, char * argv[])
{
	glutInit(&argc, argv);
	glutInitWindowPosition(100, 100);
	glutInitWindowSize(800, 600);
	glutInitDisplayMode(GLUT_RGBA | GLUT_DOUBLE);
	glutCreateWindow("OpenGL");

	GLenum glew_status = glewInit();
	if (GLEW_OK != glew_status) 
	{   
		return 1;  
	}

	//glutIdleFunc(Update);
	initShader();
	glutDisplayFunc(Update);
	glutReshapeFunc(Reshape);
	glutSpecialFunc(SpecialKeys);
	glutKeyboardFunc(Keyboard);
	Init();
	glutMainLoop();
	freeShader();
	return 0;
}