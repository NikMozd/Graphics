#include <gl/glew.h>
#include <gl/freeglut.h> 
#include <gl/SOIL.h>
#include <iostream> 
#include <vector>
#include <string>
#include <fstream>

//! Переменные с индентификаторами ID

//! ID шейдерной программы 
GLuint Program[6];
int num = 0;
int cnt = 4;

//! ID юниформ переменной цвета 
GLuint  color[6];
float my_color[3] = { 0.0, 0.8, 0.8 };

//! ID Vertex Buffer Object 
GLuint VBO, NBO, IBO, TBO;

GLuint eye[6];
float eye_pos[4] = { 5, 5, 5, 1 };

GLuint light;
double light_dist = 5;
double ang_hor = 0, ang_vert = 30;
float light_pos[4] = { 3, 3, 3, 1 };

GLuint texture;

void Init()
{
	glClearColor(0, 0, 0, 0);
	glEnable(GL_DEPTH_TEST);

	glMatrixMode(GL_PROJECTION);
	glLoadIdentity();
	gluPerspective(60.0, 1, 0.1, 10);

	glMatrixMode(GL_MODELVIEW);
	glLoadIdentity();
	gluLookAt(eye_pos[0], eye_pos[1], eye_pos[2], 0, 0, 0, 0, 0, 1);

	int width, height;
	unsigned char* image = SOIL_load_image("texture2.jpg", &width, &height, 0, SOIL_LOAD_RGB);
	glGenTextures(1, &texture);
	glBindTexture(GL_TEXTURE_2D, texture);
	glTexImage2D(GL_TEXTURE_2D, 0, GL_RGB, width, height, 0, GL_RGB, GL_UNSIGNED_BYTE, image);
	glGenerateMipmap(GL_TEXTURE_2D);
	SOIL_free_image_data(image);
	glBindTexture(GL_TEXTURE_2D, 0);
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
	std::string vFiles[] = {
		"vertex0.glsl",
		"vertex1.glsl",
		"vertex2.glsl",
		"vertex3.glsl"
	};

	std::string fFiles[] = {
		"fragment0.glsl",
		"fragment1.glsl",
		"fragment2.glsl",
		"fragment3.glsl"
	};

	for (int i = 0; i < cnt; ++i)
	{
		//! Исходный код шейдеров 
		std::string vs = readFile(vFiles[i]);
		const char * vsSource = vs.c_str();
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
void initBuffers()
{
	GLfloat vertices[] = {
		0, 0, 0,	
		0, 0, 1,	
		0, 1, 0,	
		0, 1, 1,	
		1, 0, 0,	 
		1, 0, 1,	
		1, 1, 0,	 
		1, 1, 1	 
	};

	GLfloat normals[] = {
		-1, -1, -1,
		-1, -1,  1,
		-1,  1, -1,
		-1,  1,  1,
		 1, -1, -1,
		 1, -1,  1,
		 1,  1, -1,
		 1,  1,  1
	};

	GLfloat textures[] = {
		1.0, 0.0, //0
		0.0, 0.0, //1
		1.0, 1.0, //2
		0.0, 1.0, //3
		0.0, 1.0, //4
		1.0, 1.0, //5
		0.0, 0.0, //6
		1.0, 0.0  //7
	};

	GLubyte indices[] = {
		0, 4, 6, 2,
		1, 5, 7, 3,
		4, 6, 7, 5,
		0, 2, 3, 1,
		0, 4, 5, 1,
		2, 6, 7, 3
	};

	glGenBuffers(1, &VBO);
	glBindBuffer(GL_ARRAY_BUFFER, VBO);
	glBufferData(GL_ARRAY_BUFFER, sizeof(vertices) * sizeof(GLfloat), vertices, GL_STATIC_DRAW);
	glBindBuffer(GL_ARRAY_BUFFER, 0);

	glGenBuffers(1, &NBO);
	glBindBuffer(GL_ARRAY_BUFFER, NBO);
	glBufferData(GL_ARRAY_BUFFER, sizeof(normals) * sizeof(GLfloat), normals, GL_STATIC_DRAW);
	glBindBuffer(GL_ARRAY_BUFFER, 0);

	glGenBuffers(1, &TBO);
	glBindBuffer(GL_ARRAY_BUFFER, TBO);
	glBufferData(GL_ARRAY_BUFFER, sizeof(textures) * sizeof(GLfloat), textures, GL_STATIC_DRAW);

	glGenBuffers(1, &IBO);
	glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, IBO);
	glBufferData(GL_ELEMENT_ARRAY_BUFFER, sizeof(indices) * sizeof(GLubyte), indices, GL_STATIC_DRAW);
	glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, 0);

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
void freeBuffers()
{
	glBindBuffer(GL_ARRAY_BUFFER, 0);
	glDeleteBuffers(1, &VBO);
	glDeleteBuffers(1, &NBO);
	glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, 0);
	glDeleteBuffers(1, &IBO);
}

void resizeWindow(int width, int height)
{
	glViewport(0, 0, width, height);
}

//! Отрисовка 
void render()
{
	glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
	glLightfv(GL_LIGHT0, GL_POSITION, light_pos);

	glUseProgram(Program[num]);

	glUniform3fv(eye[num], 1, eye_pos);
	glUniform3fv(color[num], 1, my_color);

	glEnableClientState(GL_VERTEX_ARRAY);
	glBindBuffer(GL_ARRAY_BUFFER, VBO);
	glVertexPointer(3, GL_FLOAT, 0, NULL);

	glEnableClientState(GL_NORMAL_ARRAY);
	glBindBuffer(GL_ARRAY_BUFFER, NBO);
	glNormalPointer(GL_FLOAT, 0, NULL);

	glEnableClientState(GL_TEXTURE_COORD_ARRAY);
	glBindBuffer(GL_ARRAY_BUFFER, TBO);
	glTexCoordPointer(2, GL_FLOAT, 0, NULL);

	glBindTexture(GL_TEXTURE_2D, texture);
	glUniform1i(glGetUniformLocation(Program[num], "our_texture"), 0);
	glEnable(GL_TEXTURE_2D);

	glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, IBO);

	// !!!!!!!!!!
	glDrawElements(GL_QUADS, 24 * sizeof(GLubyte), GL_UNSIGNED_BYTE, NULL);

	glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, 0);
	glDisableClientState(GL_VERTEX_ARRAY);
	glDisableClientState(GL_NORMAL_ARRAY);
	glDisableClientState(GL_TEXTURE_COORD_ARRAY);
	glDisable(GL_TEXTURE_2D);

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
	initBuffers();
	initShader();
	glutReshapeFunc(resizeWindow);
	glutDisplayFunc(render);
	glutKeyboardFunc(Keyboard);
	glutMainLoop();

	//! Освобождение ресурсов  
	freeShader();
	freeBuffers();
}
