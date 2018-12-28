#include <gl/glew.h>
#include <gl/freeglut.h> 
#include <gl/SOIL.h>
#include <iostream> 
#include <vector>
#include <string>
#include <fstream>
#include "objects.h"
#include <glm/glm.hpp>
#include <glm/gtc/matrix_transform.hpp>
#include <glm/gtc/type_ptr.hpp>

const int cnt = 11;
const int tex_cnt = 8;

//! Переменные с индентификаторами ID

//! ID шейдерной программы 
GLuint Program;

//! ID Vertex Buffer Object 
GLuint VBO[cnt], NBO[cnt], IBO[cnt], TBO[cnt];
GLuint texture[cnt];

GLuint eye;
float eye_pos[3] = { 10, 10, 3 };
double eye_dist = 10;
double eye_ang_hor = 0, eye_ang_vert = 0;

//GLuint light;
//double light_dist = 10;
//double light_ang_hor = 45, light_ang_vert = 45;

const float ambient = 0.2;
struct 
{
	float direction[3] = { 1, 0, 1 };
	float diffuse = 0;
	float specular = 0;
} l0;
struct
{
	float position[3] = { 5, 5, 5 };
	float diffuse = 0;
	float specular = 0;
} l1;
struct
{
	float position[3] = { 0, 0, 0 };
	float direction[3] = { 0, -3, -5 };
	float phi = 3.14159265359 / 3;
	float diffuse = 0;
	float specular = 0;
} l2;

auto objs = objects();

glm::mat4 view, projection;

void make_texture1(unsigned char img[256][256][3])
{
	for (int x = 0; x < 256; ++x)
		for (int y = 0; y < 256; ++y)
		{
			if (std::sqrt(std::pow(x - 64, 2) + std::pow(y - 64, 2)) < 64)
			{
				img[x][y][0] = 255;
				img[x][y][1] = 255;
				img[x][y][2] = 128;
			}
			else if (std::sqrt(std::pow(x - 64, 2) + std::pow(y - 192, 2)) < 64)
			{
				img[x][y][0] = 255;
				img[x][y][1] = 128;
				img[x][y][2] = 255;
			}
			else if (std::sqrt(std::pow(x - 192, 2) + std::pow(y - 64, 2)) < 64)
			{
				img[x][y][0] = 128;
				img[x][y][1] = 255;
				img[x][y][2] = 128;
			}
			else if (std::sqrt(std::pow(x - 192, 2) + std::pow(y - 192, 2)) < 64)
			{
				img[x][y][0] = 255;
				img[x][y][1] = 192;
				img[x][y][2] = 128;
			}
			else
			{
				img[x][y][0] = 0;
				img[x][y][1] = 0;
				img[x][y][2] = 128;
			}
		}
}

void make_texture2(unsigned char img[256][256][3])
{
	for (int x = 0; x < 256; ++x)
		for (int y = 0; y < 256; ++y)
		{
			if ((x + y) % 64 < 32)
			{
				img[x][y][0] = 255;
				img[x][y][1] = img[x][y][2] = 0;
			}
			else
			{
				img[x][y][0] = img[x][y][1] = img[x][y][2] = 255;
			}
		}
}

void make_texture3(unsigned char img[256][256][3])
{
	for (int x = 0; x < 256; ++x)
		for (int y = 0; y < 256; ++y)
		{
			
			if ((x < 96 || x > 160) && (y < 96 || y > 160) && ((x + y) % 32 < 4 || std::abs(x - y) % 32 < 4))
			{
				img[x][y][0] = 46;
				img[x][y][1] = 80;
				img[x][y][2] = 24;
			}
			else
			{
				img[x][y][0] = 255;
				img[x][y][1] = 136;
				img[x][y][2] = 0;
			}
		}
}

void Init()
{
	glClearColor(0, 0, 0, 0);
	glEnable(GL_DEPTH_TEST);

	projection = glm::perspective(120.0, 1.0, 0.1, 50.0);
	view = glm::lookAt(glm::vec3(eye_pos[0], eye_pos[1], eye_pos[2]), glm::vec3(0, 0, 0), glm::vec3(0, 0, 1));

	std::vector<std::string> textures = {
		"texture_wall.jpg",
		"texture_wall.jpg",
		"texture_floor.jpg",
		//"texture_tree.jpg",
		//"texture_tree.jpg",
		//"texture_tree.jpg",
		"texture_deer.jpg",
		"texture_floor.jpg",
		"texture_floor.jpg",
		"texture_floor.jpg",
		"texture_tree.jpg",
	};
	glGenTextures(cnt, &texture[0]);

	for (int i = 0; i < tex_cnt; ++i)
	{
		int width, height;
		unsigned char* image = SOIL_load_image(textures[i].c_str(), &width, &height, 0, SOIL_LOAD_RGB);
		glBindTexture(GL_TEXTURE_2D, texture[i]);
		glTexImage2D(GL_TEXTURE_2D, 0, GL_RGB, width, height, 0, GL_RGB, GL_UNSIGNED_BYTE, image);
		glGenerateMipmap(GL_TEXTURE_2D);
		SOIL_free_image_data(image);
		glBindTexture(GL_TEXTURE_2D, 0);
	}

	unsigned char img[256][256][3];
	make_texture1(img);
	glBindTexture(GL_TEXTURE_2D, texture[tex_cnt]);
	glTexImage2D(GL_TEXTURE_2D, 0, GL_RGB, 256, 256, 0, GL_RGB, GL_UNSIGNED_BYTE, img);
	glGenerateMipmap(GL_TEXTURE_2D);
	glBindTexture(GL_TEXTURE_2D, 0);

	make_texture2(img);
	glBindTexture(GL_TEXTURE_2D, texture[tex_cnt + 1]);
	glTexImage2D(GL_TEXTURE_2D, 0, GL_RGB, 256, 256, 0, GL_RGB, GL_UNSIGNED_BYTE, img);
	glGenerateMipmap(GL_TEXTURE_2D);
	glBindTexture(GL_TEXTURE_2D, 0);

	make_texture3(img);
	glBindTexture(GL_TEXTURE_2D, texture[tex_cnt + 2]);
	glTexImage2D(GL_TEXTURE_2D, 0, GL_RGB, 256, 256, 0, GL_RGB, GL_UNSIGNED_BYTE, img);
	glGenerateMipmap(GL_TEXTURE_2D);
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
	std::string vFile =
		"vertex.glsl";

	std::string fFile =
		"fragment_phong.glsl";

	//! Исходный код шейдеров 
	std::string vs = readFile(vFile);
	const char * vsSource = vs.c_str();
	std::string fs = readFile(fFile);
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
	Program = glCreateProgram();
	glAttachShader(Program, vShader);
	glAttachShader(Program, fShader);

	//! Линкуем шейдерную программу
	glLinkProgram(Program);

	//! Проверяем статус сборки
	int link_ok;
	glGetProgramiv(Program, GL_LINK_STATUS, &link_ok);
	if (!link_ok)
	{
		std::cout << "error attach shaders \n";
		return;
	}

	//! Вытягиваем ID юниформ
	const char* eye_name = "eyePos";
	eye = glGetUniformLocation(Program, eye_name);
	if (eye == -1)
	{
		std::cout << "could not bind uniform " << eye_name << std::endl;
	}

	checkOpenGLerror();
}

//! Инициализация VBO 
void initBuffers()
{
	glGenBuffers(cnt, &VBO[0]);
	glGenBuffers(cnt, &NBO[0]);
	glGenBuffers(cnt, &TBO[0]);
	glGenBuffers(cnt, &IBO[0]);

	if (objs.size() != cnt)
		std::cout << "objs.size() = " << objs.size() << ", cnt = " << cnt << "\n";

	for (int i = 0; i < cnt; ++i)
	{
		glBindBuffer(GL_ARRAY_BUFFER, VBO[i]);
		glBufferData(GL_ARRAY_BUFFER, objs[i].vertices.size() * sizeof(GLfloat), &(objs[i].vertices[0]), GL_STATIC_DRAW);
		glBindBuffer(GL_ARRAY_BUFFER, 0);

		glBindBuffer(GL_ARRAY_BUFFER, NBO[i]);
		glBufferData(GL_ARRAY_BUFFER, objs[i].normals.size() * sizeof(GLfloat), &(objs[i].normals[0]), GL_STATIC_DRAW);
		glBindBuffer(GL_ARRAY_BUFFER, 0);

		glBindBuffer(GL_ARRAY_BUFFER, TBO[i]);
		glBufferData(GL_ARRAY_BUFFER, objs[i].textures.size() * sizeof(GLfloat), &(objs[i].textures[0]), GL_STATIC_DRAW);
		glBindBuffer(GL_ARRAY_BUFFER, 0);

		glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, IBO[i]);
		glBufferData(GL_ELEMENT_ARRAY_BUFFER, objs[i].indices.size() * sizeof(GLuint), &(objs[i].indices[0]), GL_STATIC_DRAW);
		glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, 0);
	}

	checkOpenGLerror();
}

//! Освобождение шейдеров 
void freeShader()
{
	//! Передавая ноль, мы отключаем шейдрную программу  
	glUseProgram(0);
	//! Удаляем шейдерную программу
	glDeleteProgram(Program);
}

//! Освобождение буфера 
void freeBuffers()
{
	for (int i = 0; i < cnt; ++i)
	{
		glBindBuffer(GL_ARRAY_BUFFER, 0);
		glDeleteBuffers(1, &VBO[i]);
		glDeleteBuffers(1, &NBO[i]);
		glDeleteBuffers(1, &TBO[i]);
		glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, 0);
		glDeleteBuffers(1, &IBO[i]);
	}
}

void resizeWindow(int width, int height)
{
	glViewport(0, 0, width, height);
	projection = glm::perspective(120.0, (double)width / height, 0.1, 50.0);
}

void mat4_to_pointer(glm::mat4 m, float * m_new)
{
	const float *pSource = (const float*)glm::value_ptr(m);
	for (int i = 0; i < 16; ++i)
		m_new[i] = pSource[i];
}

//! Отрисовка 
void render()
{
	glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
	glLightfv(GL_LIGHT0, GL_POSITION, l1.position);

	for (int i = 0; i < cnt; ++i)
	{
		glUseProgram(Program);

		glm::mat4 normal = glm::transpose(glm::inverse(view * objs[i].model));
		float m[16], v[16], p[16], n[16];
		mat4_to_pointer(objs[i].model, m);
		mat4_to_pointer(view, v);
		mat4_to_pointer(projection, p);
		mat4_to_pointer(normal, n);

		glUniform3fv(eye, 1, eye_pos);
		glUniformMatrix4fv(glGetUniformLocation(Program, "model"), 1, GL_FALSE, m);
		glUniformMatrix4fv(glGetUniformLocation(Program, "view"), 1, GL_FALSE, v);
		glUniformMatrix4fv(glGetUniformLocation(Program, "projection"), 1, GL_FALSE, p);
		glUniformMatrix4fv(glGetUniformLocation(Program, "normal"), 1, GL_FALSE, n);

		glUniform1f(glGetUniformLocation(Program, "obj_diff"), objs[i].diffuse);
		glUniform1f(glGetUniformLocation(Program, "obj_spec"), objs[i].specular);
		glUniform1f(glGetUniformLocation(Program, "obj_refl"), objs[i].reflect);

		glUniform1f(glGetUniformLocation(Program, "ambient"), ambient);
		glUniform3fv(glGetUniformLocation(Program, "l0_dir"), 1, l0.direction);
		glUniform1f(glGetUniformLocation(Program, "l0_diff"), l0.diffuse);
		glUniform1f(glGetUniformLocation(Program, "l0_spec"), l0.specular);
		glUniform1f(glGetUniformLocation(Program, "l1_diff"), l1.diffuse);
		glUniform1f(glGetUniformLocation(Program, "l1_spec"), l1.specular);
		glUniform3fv(glGetUniformLocation(Program, "l2_pos"), 1, l2.position);
		glUniform3fv(glGetUniformLocation(Program, "l2_dir"), 1, l2.direction);
		glUniform1f(glGetUniformLocation(Program, "l2_phi"), l2.phi);
		glUniform1f(glGetUniformLocation(Program, "l2_diff"), l2.diffuse);
		glUniform1f(glGetUniformLocation(Program, "l2_spec"), l2.specular);

		glEnableClientState(GL_VERTEX_ARRAY);
		glBindBuffer(GL_ARRAY_BUFFER, VBO[i]);
		glVertexPointer(3, GL_FLOAT, 0, NULL);

		glEnableClientState(GL_NORMAL_ARRAY);
		glBindBuffer(GL_ARRAY_BUFFER, NBO[i]);
		glNormalPointer(GL_FLOAT, 0, NULL);

		glEnableClientState(GL_TEXTURE_COORD_ARRAY);
		glBindBuffer(GL_ARRAY_BUFFER, TBO[i]);
		glTexCoordPointer(2, GL_FLOAT, 0, NULL);

		glBindTexture(GL_TEXTURE_2D, texture[i]);
		glUniform1i(glGetUniformLocation(Program, "obj_texture"), 0);
		glEnable(GL_TEXTURE_2D);

		glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, IBO[i]);

		glDrawElements(GL_TRIANGLES, objs[i].indices.size() * sizeof(GLuint), GL_UNSIGNED_INT, NULL);

		glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, 0);
		glDisableClientState(GL_VERTEX_ARRAY);
		glDisableClientState(GL_NORMAL_ARRAY);
		glDisableClientState(GL_TEXTURE_COORD_ARRAY);
		glDisable(GL_TEXTURE_2D);

		//! Отключаем шейдерную программу  
		glUseProgram(0);
		checkOpenGLerror();

		glFlush();
	}
	glutSwapBuffers();
}

void Special(int key, int x, int y)
{
	switch (key)
	{
	case GLUT_KEY_LEFT:
		eye_ang_hor -= 5;
		break;
	case GLUT_KEY_RIGHT:
		eye_ang_hor += 5;
		break;
	case GLUT_KEY_UP:
		eye_ang_vert += 5;
		break;
	case GLUT_KEY_DOWN:
		eye_ang_vert -= 5;
		break;
	case GLUT_KEY_PAGE_UP:
		eye_dist -= 1;
		break;
	case GLUT_KEY_PAGE_DOWN:
		eye_dist += 1;
		break;
	}

	double ang_vert_r = eye_ang_vert / 180 * 3.1416;
	double ang_hor_r = eye_ang_hor / 180 * 3.1416;
	eye_pos[0] = eye_dist * std::cos(ang_vert_r) * std::cos(ang_hor_r);
	eye_pos[1] = eye_dist * std::cos(ang_vert_r) * std::sin(ang_hor_r);
	eye_pos[2] = eye_dist * std::sin(ang_vert_r);

	view = glm::lookAt(glm::vec3(eye_pos[0], eye_pos[1], eye_pos[2]), glm::vec3(0, 0, 0), glm::vec3(0, 0, 1));
	glutPostRedisplay();
}

void Keyboard(unsigned char key, int x, int y)
{
	/*switch (key)
	{
	case 'w':
		if (light_ang_vert < 180)
			light_ang_vert += 5;
		break;
	case 's':
		if (light_ang_vert > -180)
			light_ang_vert -= 5;
		break;
	case 'a':
		light_ang_hor -= 5;
		break;
	case 'd':
		light_ang_hor += 5;
		break;
	case 'q':
		light_dist--;
		break;
	case 'z':
		light_dist++;
		break;
	}

	double ang_vert_r = light_ang_vert / 180 * 3.1416;
	double ang_hor_r = light_ang_hor / 180 * 3.1416;
	light_pos[0] = light_dist * std::cos(ang_vert_r) * std::cos(ang_hor_r);
	light_pos[1] = light_dist * std::cos(ang_vert_r) * std::sin(ang_hor_r);
	light_pos[2] = light_dist * std::sin(ang_vert_r);
	std::cout << light_pos[0] << " " << light_pos[1] << " " << light_pos[2] << "\n";*/

	switch (key)
	{
	case 'a':
		l1.position[0]--;
		break;
	case 'd':
		l1.position[0]++;
		break;
	case 'w':
		l1.position[1]++;
		break;
	case 's':
		l1.position[1]--;
		break;
	case 'q':
		l1.position[2]++;
		break;
	case 'z':
		l1.position[2]--;
		break;
	case '0':
		if (l0.diffuse == 0)
			l0.diffuse = l0.specular = 1.2;
		else
			l0.diffuse = l0.specular = 0;
		break;
	case '1':
		if (l1.diffuse == 0)
			l1.diffuse = 1, l1.specular = 1.5;
		else
			l1.diffuse = l1.specular = 0;
		break;
	case '2':
		if (l2.diffuse == 0)
			l2.diffuse = 2, l2.specular = 0.5;
		else
			l2.diffuse = l2.specular = 0;
		break;
	}

	std::cout << l1.position[0] << " " << l1.position[1] << " " << l1.position[2] << "\n";
	glutPostRedisplay();
}

int main(int argc, char **argv)
{
	glutInit(&argc, argv);
	glutInitDisplayMode(GLUT_RGBA | GLUT_ALPHA | GLUT_DOUBLE);
	glutInitWindowSize(600, 600);
	glutCreateWindow("MY SUPER SCENE");

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
	glutSpecialFunc(Special);
	glutKeyboardFunc(Keyboard);
	glutMainLoop();

	//! Освобождение ресурсов  
	freeShader();
	freeBuffers();
}
