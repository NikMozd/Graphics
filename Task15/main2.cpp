#include <gl/glew.h>
#include <gl/freeglut.h> 
#include <iostream> 
#include <vector>
#include <string>
#include <fstream>

//! Переменные с индентификаторами ID

//! ID шейдерной программы 
GLuint Program[6]; 
int num = 0;
int cnt = 6;

//! ID юниформ переменной цвета 
GLuint  color[6]; 
float my_color[3] = { 0.0, 0.8, 0.8 };

//! ID Vertex Buffer Object 
GLuint VBO, VAO, IBO;
GLuint * inds;

GLuint eye[6];
float eye_pos[4] = { 5, 5, 5, 1 };

GLuint light;
double light_dist = 5;
double ang_hor = 0, ang_vert = 30;
float light_pos[4] = { 3, 3, 3, 1 };

void Init()
{
	glClearColor(0, 0, 0, 0);
	
	glMatrixMode(GL_PROJECTION);
	glLoadIdentity();
	gluPerspective(60.0, 1, 0.1, 10);

	glMatrixMode(GL_MODELVIEW);
	glLoadIdentity();
	gluLookAt(eye_pos[0], eye_pos[1], eye_pos[2], 0, 0, 0, 0, 0, 1);
}

//! Функция печати лога шейдера 
void shaderLog(unsigned int shader)
{
	int   infologLen = 0;
	int   charsWritten = 0;
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

//! Проверка ошибок OpenGL, если есть то вывод в консоль тип ошибки 
void checkOpenGLerror()
{
	GLenum errCode;
	if ((errCode = glGetError()) != GL_NO_ERROR)
		std::cout << "OpenGl error! - " << gluErrorString(errCode);
}

std::string readFile(std::string path)
{
	std::string res = "";
	std::ifstream file(path);
	std::string line;
	getline(file, res, '\0');
	while (getline(file, line))
	{
		res += "\n " + line;
	}
	return res;
}

//! Инициализация шейдеров 
void initShader()
{
	std::string vs = readFile("vertex.glsl");
	const char * vsSource = vs.c_str();

	std::string fFiles[6] = {
		"fragment_lambert.glsl",
		"fragment_blinn.glsl",
		"fragment_minnaert.glsl",
		"fragment_toon.glsl",
		"fragment_gooch.glsl",
		"fragment_rim.glsl"
	};

	for (int i = 0; i < cnt; ++i)
	{
		//! Исходный код шейдеров 
		std::string fs = readFile(fFiles[i]);
		const char * fsSource = fs.c_str();

		//! Переменные для хранения идентификаторов шейдеров  
		GLuint vShader, fShader;

		//! Создаем вершинный шейдер  
		vShader = glCreateShader(GL_VERTEX_SHADER);
		//! Передаем исходный код  
		glShaderSource(vShader, 1, &vsSource, NULL);
		//! Компилируем шейдер  
		glCompileShader(vShader);

		std::cout << "vertex shader \n";
		shaderLog(vShader);

		//! Создаем фрагментный шейдер  
		fShader = glCreateShader(GL_FRAGMENT_SHADER);
		//! Передаем исходный код  
		glShaderSource(fShader, 1, &fsSource, NULL);
		//! Компилируем шейдер
		glCompileShader(fShader);

		std::cout << "fragment shader \n";
		shaderLog(fShader);

		//! Создаем программу и прикрепляем шейдеры к ней
		Program[i] = glCreateProgram();
		glAttachShader(Program[i], vShader);
		glAttachShader(Program[i], fShader);

		//! Линкуем шейдерную программу
		glLinkProgram(Program[i]);

		//! Проверяем статус сборки
		int link_ok;
		glGetProgramiv(Program[i], GL_LINK_STATUS, &link_ok);
		if (!link_ok)
		{
			std::cout << "error attach shaders \n";
			return;
		}

		//! Вытягиваем ID юниформ
		const char* eye_name = "eyePos";
		eye[i] = glGetUniformLocation(Program[i], eye_name);
		if (eye[i] == -1)
		{
			std::cout << "could not bind uniform " << eye_name << std::endl;
		}

		const char* color_name = "color";
		color[i] = glGetUniformLocation(Program[i], color_name);
		if (color[i] == -1)
		{
			std::cout << "could not bind uniform " << color_name << std::endl;
		}

		checkOpenGLerror();
	}
}

//! Инициализация VBO 
void initVBO()
{
	glGenBuffers(1, &VBO);
	glGenBuffers(1, &IBO);

	glGenVertexArrays(1, &VAO);
	glBindVertexArray(VAO);
	
	GLfloat verts[] = {
		0, 0, 0,	-1, -1, -1,
		0, 0, 1,	-1, -1,  1,
		0, 1, 0,	-1,  1, -1,
		0, 1, 1,	-1,  1,  1,
		1, 0, 0,	 1, -1, -1,
		1, 0, 1,	 1, -1,  1,
		1, 1, 0,	 1,  1, -1,
		1, 1, 1,	 1,  1,  1
	};

	int sz = 24;
	inds = new GLuint[sz] {
		0, 4, 6, 2,
		1, 5, 7, 3,
		4, 6, 7, 5,
		0, 2, 3, 1,
		0, 4, 5, 1,
		2, 6, 7, 3
	};

	//! Передаем вершины в буфер 
	glBindBuffer(GL_ARRAY_BUFFER, VBO);
	glBufferData(GL_ARRAY_BUFFER, sizeof(verts), verts, GL_STATIC_DRAW);
	glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, IBO);
	glBufferData(GL_ELEMENT_ARRAY_BUFFER, sizeof(inds[0]) * sz, inds, GL_STATIC_DRAW);

	glVertexAttribPointer(0, 3, GL_FLOAT, GL_FALSE, 6 * sizeof(GLfloat), 0);
	glEnableVertexAttribArray(0);
	glVertexAttribPointer(1, 3, GL_FLOAT, GL_FALSE, 6 * sizeof(GLfloat), (GLvoid*)(3 * sizeof(GLfloat)));
	glEnableVertexAttribArray(1);

	checkOpenGLerror();
}

//! Освобождение шейдеров 
void freeShader()
{
	//! Передавая ноль, мы отключаем шейдрную программу  
	glUseProgram(0);
	//! Удаляем шейдерную программу
	for (int i = 0; i < cnt; ++i)
		glDeleteProgram(Program[i]);
}

//! Освобождение буфера 
void freeVBO()
{
	glBindBuffer(GL_ARRAY_BUFFER, 0);
	glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, 0);
	
	glDeleteBuffers(1, &VBO);
	glDeleteBuffers(1, &VAO);
	glDeleteBuffers(1, &IBO);

	glDisableVertexAttribArray(0);
	glDisableVertexAttribArray(1);
	glDisableVertexAttribArray(2);
}

void resizeWindow(int width, int height)
{
	glViewport(0, 0, width, height);
}

//! Отрисовка 
void render()
{
	glClear(GL_COLOR_BUFFER_BIT);
	glLightfv(GL_LIGHT0, GL_POSITION, light_pos);

	glUseProgram(Program[num]);

	glUniform3fv(eye[num], 1, eye_pos);
	glUniform3fv(color[num], 1, my_color);

	//! Передаем юниформ в шейдер
	 
	//glUniform4fv(eye, 1, eye_pos);

	glBegin(GL_QUADS);
	glNormal3f(-1, -1, -1); glVertex3f(0, 0, 0);
	glNormal3f(1, -1, -1); glVertex3f(1, 0, 0);
	glNormal3f(1, 1, -1); glVertex3f(1, 1, 0);
	glNormal3f(-1, 1, -1); glVertex3f(0, 1, 0);

	glNormal3f(-1, -1, -1); glVertex3f(0, 0, 0);
	glNormal3f(-1, -1, 1); glVertex3f(0, 0, 1);
	glNormal3f(1, -1, 1); glVertex3f(1, 0, 1);
	glNormal3f(1, -1, -1); glVertex3f(1, 0, 0);

	glNormal3f(-1, -1, -1); glVertex3f(0, 0, 0);
	glNormal3f(-1, -1, 1); glVertex3f(0, 0, 1);
	glNormal3f(-1, 1, 1); glVertex3f(0, 1, 1);
	glNormal3f(-1, 1, -1); glVertex3f(0, 1, 0);

	glNormal3f(-1, -1, 1); glVertex3f(0, 0, 1);
	glNormal3f(1, -1, 1); glVertex3f(1, 0, 1);
	glNormal3f(1, 1, 1); glVertex3f(1, 1, 1);
	glNormal3f(-1, 1, 1); glVertex3f(0, 1, 1);

	glNormal3f(-1, 1, -1); glVertex3f(0, 1, 0);
	glNormal3f(-1, 1, 1); glVertex3f(0, 1, 1);
	glNormal3f(1, 1, 1); glVertex3f(1, 1, 1);
	glNormal3f(1, 1, -1); glVertex3f(1, 1, 0);

	glNormal3f(1, -1, -1); glVertex3f(1, 0, 0);
	glNormal3f(1, -1, 1); glVertex3f(1, 0, 1);
	glNormal3f(1, 1, 1); glVertex3f(1, 1, 1);
	glNormal3f(1, 1, -1); glVertex3f(1, 1, 0);
	glEnd();

	//glBindVertexArray(VAO);
	//glDrawElements(GL_QUADS, 24, GL_UNSIGNED_SHORT, 0);
	//glBindVertexArray(0);

	//! Отключаем шейдерную программу  
	glUseProgram(0);
	checkOpenGLerror();

	glFlush();
	glutSwapBuffers();
}

void Keyboard(unsigned char key, int x, int y)
{
	switch (key)
	{
	case 'w':
		ang_vert += 5;
		break;
	case 's':
		ang_vert -= 5;
		break;
	case 'a':
		ang_hor -= 5;
		break;
	case 'd':
		ang_hor += 5;
		break;
	case 'q':
		light_dist--;
		break;
	case 'z':
		light_dist++;
		break;
	case '0':
		num = 0;
		break;
	case '1':
		num = 1;
		break;
	case '2':
		num = 2;
		break;
	case '3':
		num = 3;
		break;
	case '4':
		num = 4;
		break;
	case '5':
		num = 5;
		break;
	}

	double ang_vert_r = ang_vert / 180 * 3.1416;
	double ang_hor_r = ang_hor / 180 * 3.1416;
	light_pos[0] = light_dist * std::sin(ang_vert_r) * std::cos(ang_hor_r);
	light_pos[1] = light_dist * std::sin(ang_vert_r) * std::sin(ang_hor_r);
	light_pos[2] = light_dist * std::cos(ang_vert_r);
	std::cout << light_pos[0] << " " << light_pos[1] << " " << light_pos[2] << "\n";

	glutPostRedisplay();
}

int main(int argc, char **argv)
{
	glutInit(&argc, argv);
	glutInitDisplayMode(GLUT_RGBA | GLUT_ALPHA | GLUT_DOUBLE);
	glutInitWindowSize(600, 600);
	glutCreateWindow("Simple shaders");

	//! Обязательно перед инициализацией шейдеров  
	GLenum glew_status = glewInit();
	if (GLEW_OK != glew_status)
	{
		//! GLEW не проинициализировалась   
		std::cout << "Error: " << glewGetErrorString(glew_status) << "\n";
		return 1;
	}

	//! Проверяем доступность OpenGL 2.0  
	if (!GLEW_VERSION_2_0)
	{
		//! OpenGl 2.0 оказалась не доступна
		std::cout << "No support for OpenGL 2.0 found\n";
		return 1;
	}

	//! Инициализация  
	Init();
	initVBO();
	initShader();
	glutReshapeFunc(resizeWindow);
	glutDisplayFunc(render);
	glutKeyboardFunc(Keyboard);
	glutMainLoop();

	//! Освобождение ресурсов  
	freeShader();
	freeVBO();
}
