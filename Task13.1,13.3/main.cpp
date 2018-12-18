#include "GL/glew.h"
#include "GL/freeglut.h"
#include "GL/SOIL.h"
#include <iostream>
#include <cmath>

GLuint Program;

GLint Attrib_vertex;
GLint Attrib_vertex1;

GLint Unif_color;

GLint Unif_affine; 

GLuint texture;

GLuint VBO;

float affineMatr[9] = { 1.0, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0, 1.0 };

struct vertex
{
	GLfloat x;
	GLfloat y;
	GLfloat z;
	GLfloat tx;
	GLfloat ty;
};

void shaderLog(unsigned int shader)
{
	int infologLen = 0;
	int charsWritten = 0;
	char *infoLog;
	glGetShaderiv(shader, GL_INFO_LOG_LENGTH, &infologLen);
	if (infologLen > 1)
	{
		infoLog = new char[infologLen];
		if (infoLog == NULL)
		{
			std::cout << "ERROR: Could not allocate InfoLog buffer\n";
			exit(1);
		}
		glGetShaderInfoLog(shader, infologLen, &charsWritten, infoLog);
		std::cout << "InfoLog: " << infoLog << "\n\n\n";
		delete[] infoLog;
	}
}

void initGL()
{
	glClearColor(0, 0, 0, 0);
}

void checkOpenGLerror()
{
	GLenum errCode;
	if ((errCode = glGetError()) != GL_NO_ERROR)
		std::cout << "OpenGl error! - " << errCode;//gluErrorString(errCode);
}

void initShader()
{

	const char* vsSource =
		"#version 330 core\n"
		"attribute vec3 coord;\n"
		"attribute vec2 texCoord;\n"
		"uniform mat3 affine;\n"
		"out vec2 TexCoord;\n"
		"void main() {\n"
		"vec3 tmp = affine * coord;\n"
		"gl_Position = vec4(tmp, (0.1 * tmp.z + 1.0) * 1.0);\n"
		"TexCoord = texCoord;\n"
		"}\n";
	//const char* fsSource =
	//	"uniform vec4 color;\n"
	//	"void main() {\n"
	//	" gl_FragColor = color;\n"
	//	"}\n";
	const char* fsSource =
		"#version 330 core\n"
		"in vec2 TexCoord;\n"
		"uniform sampler2D ourTexture;\n"
		"out vec4 color;\n"
		"void main() {\n"
		"color = texture(ourTexture, TexCoord);\n"
		"}\n";


	GLuint vShader, fShader;
	vShader = glCreateShader(GL_VERTEX_SHADER);
	glShaderSource(vShader, 1, &vsSource, NULL);
	glCompileShader(vShader);
	std::cout << "vertex shader \n";
	shaderLog(vShader);
	fShader = glCreateShader(GL_FRAGMENT_SHADER);
	glShaderSource(fShader, 1, &fsSource, NULL);
	glCompileShader(fShader);
	std::cout << "fragment shader \n";
	shaderLog(fShader);
	Program = glCreateProgram();
	glAttachShader(Program, vShader);
	glAttachShader(Program, fShader);
	glLinkProgram(Program);

	int link_ok;
	glGetProgramiv(Program, GL_LINK_STATUS, &link_ok);
	if (!link_ok)
	{
		std::cout << "error attach shaders \n";
		return;
	}

	const char* attr_name0 = "coord";
	Attrib_vertex = glGetAttribLocation(Program, attr_name0);
	if (Attrib_vertex == -1)
	{
		std::cout << "could not bind attrib " << attr_name0 << std::endl;
		return;
	}

	const char* attr_name1 = "texCoord";
	Attrib_vertex1 = glGetAttribLocation(Program, attr_name1);
	if (Attrib_vertex1 == -1)
	{
		std::cout << "could not bind uniform " << attr_name1 << std::endl;
		return;
	}

	//const char* unif_name0 = "color";
	//Unif_color = glGetUniformLocation(Program, unif_name0);
	//if (Unif_color == -1)
	//{
	//	std::cout << "could not bind uniform " << unif_name0 << std::endl;
	//	return;
	//}

	const char* unif_name1 = "affine";
	Unif_affine = glGetUniformLocation(Program, unif_name1);
	if (Unif_affine == -1)
	{
		std::cout << "could not bind uniform " << unif_name1 << std::endl;
		return;
	}
	checkOpenGLerror();
}

void initVBO()
{
	glEnable(GL_DEPTH_TEST);
	
	glGenBuffers(1, &VBO);
	glBindBuffer(GL_ARRAY_BUFFER, VBO);

	//glTranslatef(0.5, 0, 0);

	//vertex triangle[] = {
		/*{ -0.5, -0.5, -0.5 },
		{ -0.5, 0.5, -0.5 },
		{ 0.5, -0.5, -0.5 },
		{ 0.5, -0.5, -0.5 },
		{ 0.5, 0.5, -0.5 },
		{ 0.5, 0.5, 0.5 },
		{ 0.5, 0.5, 0.5 },
		{ 0.5, -0.5, 0.5 },
		{ -0.5, 0.5, 0.5 },
		{ -0.5, -0.5, -0.5 },
		{ -0.5, 0.5, 0.5 },
		{ -0.5, -0.5, 0.5 },

		{ -0.5, 0.5, -0.5 },
		{ 0.5, -0.5, -0.5 },
		{ 0.5, 0.5, -0.5 },
		{ -0.5, 0.5, 0.5 },
		{ 0.5, -0.5, 0.5 },
		{ -0.5, -0.5, 0.5 },
		{ 0.5, 0.5, 0.5 },
		{ -0.5, 0.5, -0.5 },
		{ -0.5, 0.5, 0.5 },
		{ 0.5, -0.5, 0.5 },
		{ -0.5, -0.5, -0.5 },
		{ 0.5, -0.5, -0.5 },
		
		{ 0.5, 0.5, 0.5 },
		{ 0.5, -0.5, -0.5 },
		{ 0.5, -0.5, 0.5 },
		{ -0.5, 0.5, 0.5 },
		{ -0.5, -0.5, -0.5 },
		{ -0.5, 0.5, -0.5 },
		{ 0.5, 0.5, 0.5 },
		{ -0.5, 0.5, -0.5 },
		{ 0.5, 0.5, -0.5 },
		{ 0.5, -0.5, 0.5 },
		{ -0.5, -0.5, -0.5 },
		{ -0.5, -0.5, 0.5 }*/

	GLfloat triangle[] = {
		 -0.5, -0.5, -0.5, 
		 -0.5, 0.5, -0.5,  
		 0.5, 0.5, -0.5,   
		 0.5, -0.5, -0.5,  

		 0.5, 0.5, -0.5, 
		 0.5, 0.5, 0.5,  
		 0.5, -0.5, 0.5, 
		 0.5, -0.5, -0.5,

		 -0.5, 0.5, 0.5, 
		 -0.5, -0.5, 0.5,
		 0.5, -0.5, 0.5, 
		 0.5, 0.5, 0.5,  

		 -0.5, 0.5, -0.5, 
		 -0.5, -0.5, -0.5,
		 -0.5, -0.5, 0.5, 
		 -0.5, 0.5, 0.5,  

		 -0.5, 0.5, 0.5, 
		 -0.5, 0.5, -0.5,
		 0.5, 0.5, -0.5, 
		 0.5, 0.5, 0.5,  

		 -0.5, -0.5, 0.5, 
		 0.5, -0.5, 0.5,  
		 0.5, -0.5, -0.5, 
		 -0.5, -0.5, -0.5,

		 0.0, 0.0,
		 0.0, 1.0,
		 1.0, 1.0,
		 1.0, 0.0,

		 1.0, 0.0,
		 1.0, 1.0,
		 0.0, 1.0,
		 0.0, 0.0,

		 0.0, 1.0,
		 0.0, 0.0,
		 1.0, 0.0,
		 1.0, 1.0,

		 1.0, 0.0,
		 0.0, 0.0,
		 0.0, 1.0,
		 1.0, 1.0,

		 0.0, 1.0,
		 0.0, 0.0,
		 1.0, 0.0,
		 1.0, 1.0,

		 0.0, 1.0,
		 1.0, 1.0,
		 1.0, 0.0,
		 0.0, 0.0
	};

	glBufferData(GL_ARRAY_BUFFER, sizeof(triangle), triangle, GL_STATIC_DRAW);

	
	checkOpenGLerror();
}

void freeShader()
{
	glUseProgram(0);
	glDeleteProgram(Program);
}

void freeVBO()
{
	glBindBuffer(GL_ARRAY_BUFFER, 0);
	glDeleteBuffers(1, &VBO);
}

void initTexture() 
{
	int width, height;
	unsigned char* image = SOIL_load_image("texture.jpg", &width, &height, 0, SOIL_LOAD_RGB);
	glGenTextures(1, &texture);
	glBindTexture(GL_TEXTURE_2D, texture);
	glTexImage2D(GL_TEXTURE_2D, 0, GL_RGB, width, height, 0, GL_RGB, GL_UNSIGNED_BYTE, image);
	glGenerateMipmap(GL_TEXTURE_2D);
	SOIL_free_image_data(image);
	glBindTexture(GL_TEXTURE_2D, 0);
}

void resizeWindow(int width, int height)
{
	glViewport(0, 0, width, height);
}

void render()
{
	//glClear(GL_COLOR_BUFFER_BIT);

	glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);

	//glUseProgram(Program);
	//static float red[4] = { 1.0f, 0.0f, 0.0f, 1.0f };
	//static float green[4] = { 0.0f, 1.0f, 0.0f, 1.0f };
	//static float blue[4] = { 0.0f, 0.0f, 1.0f, 1.0f };
	//glUniform4fv(Unif_color, 1, red);
	//glUniformMatrix3fv(Unif_affine, 1, GL_FALSE, affineMatr);
	//glEnableVertexAttribArray(Attrib_vertex);
	//glBindBuffer(GL_ARRAY_BUFFER, VBO);
	//glVertexAttribPointer(Attrib_vertex, 3, GL_FLOAT, GL_FALSE, 0, 0);
	//glBindBuffer(GL_ARRAY_BUFFER, 0);
	//glDrawArrays(GL_TRIANGLES, 0, 12);
	//glUniform4fv(Unif_color, 1, green);
	//glDrawArrays(GL_TRIANGLES, 12, 12);
	//glUniform4fv(Unif_color, 1, blue);
	//glDrawArrays(GL_TRIANGLES, 24, 12);
	//glDisableVertexAttribArray(Attrib_vertex);
	//glUseProgram(0);
	//checkOpenGLerror();
	//glutSwapBuffers();

	GLintptr tmp = 72 * sizeof(float);

	glUseProgram(Program);
	glUniformMatrix3fv(Unif_affine, 1, GL_FALSE, affineMatr);
	glEnableVertexAttribArray(Attrib_vertex);
	glEnableVertexAttribArray(Attrib_vertex1);
	glBindBuffer(GL_ARRAY_BUFFER, VBO);
	glVertexAttribPointer(Attrib_vertex, 3, GL_FLOAT, GL_FALSE, 0, 0);
	glVertexAttribPointer(Attrib_vertex1, 2, GL_FLOAT, GL_FALSE, 0, (GLvoid*)tmp);
	glBindBuffer(GL_ARRAY_BUFFER, 0);
	glBindTexture(GL_TEXTURE_2D, texture);
	glEnable(GL_TEXTURE_2D);
	glDrawArrays(GL_QUADS, 0, 24);
	glDisable(GL_TEXTURE_2D);
	glDisableVertexAttribArray(Attrib_vertex);
	glDisableVertexAttribArray(Attrib_vertex1);
	glUseProgram(0);
	checkOpenGLerror();
	glutSwapBuffers();
}

void MatrProduct(float a[])
{
	float tmp[9];
	for (int i = 0; i < 9; ++i) tmp[i] = 0;
	
	for (int i = 0; i < 9; i += 3)
	for (int j = 0; j < 3; ++j)
	for (int k = 0; k < 3; ++k)
		tmp[i + j] += a[i + k] * affineMatr[3 * k + j];

	for (int i = 0; i < 9; ++i) affineMatr[i] = tmp[i];
}

void Keyboard(unsigned char key, int x, int y)
{
	float a[9] = { 1.0, 0.0, 0.0, 0.0, 1.0, 0.0, 0.0, 0.0, 1.0 };
	switch (key)
	{
	case 'w':
		a[4] = std::cos(0.08727);
		a[5] = std::sin(0.08727);
		a[7] = -a[5];
		a[8] = a[4];
		break;
	case 's':
		a[4] = std::cos(0.08727);
		a[5] = -std::sin(0.08727);
		a[7] = -a[5];
		a[8] = a[4];
		break;
	case 'a':
		a[0] = std::cos(0.08727);
		a[2] = -std::sin(0.08727);
		a[6] = -a[2];
		a[8] = a[0];
		break;
	case 'd':
		a[0] = std::cos(0.08727);
		a[2] = std::sin(0.08727);
		a[6] = -a[2];
		a[8] = a[0];
		break;
	case 'q':
		a[0] = std::cos(0.08727);
		a[1] = std::sin(0.08727);
		a[3] = -a[1];
		a[4] = a[0];
		break;
	case 'e':
		a[0] = std::cos(0.08727);
		a[1] = -std::sin(0.08727);
		a[3] = -a[1];
		a[4] = a[0];
		break;
	case '+':
		a[0] = 1.1;
		a[4] = 1.1;
		a[8] = 1.1;
		break;
	case '-':
		a[0] = 1.0 / 1.1;
		a[4] = 1.0 / 1.1;
		a[8] = 1.0 / 1.1;
		break;
	}
	MatrProduct(a);
	glutPostRedisplay();
}

int main(int argc, char **argv)
{
	glutInit(&argc, argv);
	glutInitDisplayMode(GLUT_RGBA | GLUT_ALPHA | GLUT_DOUBLE);
	glutInitWindowSize(600, 600);
	glutCreateWindow("Simple shaders");
	GLenum glew_status = glewInit();
	if (GLEW_OK != glew_status)
	{
			std::cout << "Error: " << glewGetErrorString(glew_status) << "\n";
		return 1;
	}
	if (!GLEW_VERSION_2_0)
	{
			std::cout << "No support for OpenGL 2.0 found\n";
		return 1;
	}
	initGL();
	initVBO();
	initShader();
	initTexture();
	glutReshapeFunc(resizeWindow);
	glutDisplayFunc(render);
	glutKeyboardFunc(Keyboard);
	glutMainLoop();
	freeShader();
	freeVBO();
}