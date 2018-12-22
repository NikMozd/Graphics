#include <gl/glew.h>
#include <gl/freeglut.h> 
#include <iostream> 

//! Переменные с индентификаторами ID 
//! ID шейдерной программы 
GLuint Program; 
//! ID атрибута 
//GLint  Attrib_vertex; 
//! ID юниформ переменной цвета 
//GLint  Unif_color; 
//! ID Vertex Buffer Object 
GLuint VBO; 

GLuint light;
GLuint eye;

double light_dist = 5;
double ang_hor = 0, ang_vert = 30;

//! Вершина 
struct vertex 
{  
	GLfloat x;  
	GLfloat y;  
	GLfloat z;
}; 

void Init()
{
	glClearColor(0, 0, 0, 0);
	glEnable(GL_LIGHTING);
	
	float pos[4] = { 5,5,5,1 };
	glLightfv(GL_LIGHT1, GL_POSITION, pos);

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

//! Инициализация шейдеров 
void initShader() 
{  
	//! Исходный код шейдеров  
	const char* vsSource = ""
		"varying vec3 l;\n"
		"varying vec3 n;\n"
		"uniform vec4 lightPos;\n"
		"uniform vec4 eyePos;\n"
		"void main(void) {\n"
		"    vec3 p = vec3(gl_ModelViewMatrix * gl_Vertex);\n"
		"    l = normalize(vec3(gl_LightSource[1].position) - p);\n"
		"    n = normalize(gl_NormalMatrix * gl_Normal);\n"
		"    gl_Position = gl_ModelViewProjectionMatrix * gl_Vertex;\n"
		"}\n"
		;
	const char* fsSource = ""
		"varying vec3 l;\n"
		"varying vec3 n;\n"
		"void main(void) {\n"
		"    const vec4 diffColor = vec4(0.5, 0.0, 0.0, 1.0);\n"
		"    vec3 n2 = normalize(n);\n"
		"    vec3 l2 = normalize(l);\n"
		"    vec4 diff = diffColor * max(dot(n2, l2), 0.0);\n"
		"    gl_FragColor = diff;\n"
		"}\n"
		; 

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
	///! Вытягиваем ID атрибута из собранной программы
	const char* light_name = "lightPos";
	light = glGetUniformLocation(Program, light_name);
	if (light == -1)
	{   
		std::cout << "could not bind uniform " << light_name << std::endl;
		return;
	}  
	//! Вытягиваем ID юниформ
	const char* eye_name = "eyePos";
	eye = glGetUniformLocation(Program, eye_name);
	if (eye == -1)
	{   
		std::cout << "could not bind uniform " << eye_name << std::endl;
		return;
	} 

	checkOpenGLerror(); 
}

//! Инициализация VBO 
void initVBO() 
{  
	glGenBuffers(1, &VBO);
	glBindBuffer(GL_ARRAY_BUFFER, VBO);
	//! Вершины нашего треугольника
	vertex triangle[3] = 
	{   
		{ -1.0f, -1.0f },
		{ 0.0f, 1.0f },
		{ 1.0f, -1.0f }
	};  
	//! Передаем вершины в буфер 
	glBufferData(GL_ARRAY_BUFFER, sizeof(triangle), triangle, GL_STATIC_DRAW); 

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
void freeVBO() 
{  
	glBindBuffer(GL_ARRAY_BUFFER, 0);  
	glDeleteBuffers(1, &VBO);
} 

void resizeWindow(int width, int height) 
{ 
	glViewport(0, 0, width, height);
}

//! Отрисовка 
void render() 
{  
	glClear(GL_COLOR_BUFFER_BIT);

	//gluPerspective(60, 0.5, 1, 10);
	//! Устанавливаем шейдерную программу текущей
	//gluLookAt(5, 5, 5, 0, 0, 0, 0, 0, 1);
	glUseProgram(Program); 

	static float red[4] = { 1.0f, 0.0f, 0.0f, 1.0f };
	//! Передаем юниформ в шейдер
	//glUniform4fv(Unif_color, 1, red); 

	double ang_vert_r = ang_vert / 180 * 3.1416;
	double ang_hor_r = ang_hor / 180 * 3.1416;
	float light_x = light_dist * std::sin(ang_vert_r) * std::cos(ang_hor_r);
	float light_y = light_dist * std::sin(ang_vert_r) * std::sin(ang_hor_r);
	float light_z = light_dist * std::cos(ang_vert_r);
	std::cout << light_x << " " << light_y << " " << light_z << "\n";
	static float light_pos[4] = 
	{ 
		light_x, 
		light_y, 
		light_z, 
		1.0f 
	};
	glUniform4fv(light, 1, light_pos);
	static float eye_pos[4] = { 0.0f, 0.0f, 3.0f, 1.0f };
	glUniform4fv(eye, 1, eye_pos);

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

	/*
	//! Включаем массив атрибутов
	glEnableVertexAttribArray(Attrib_vertex);
	//! Подключаем VBO
	glBindBuffer(GL_ARRAY_BUFFER, VBO);
	//! Указывая pointer 0 при подключенном буфере, мы указываем что данные в VBO
	glVertexAttribPointer(Attrib_vertex, 2, GL_FLOAT, GL_FALSE, 0, 0);
	//! Отключаем VBO
	glBindBuffer(GL_ARRAY_BUFFER, 0);
	//! Передаем данные на видеокарту(рисуем)
	glDrawArrays(GL_TRIANGLES, 0, 3); 

	//! Отключаем массив атрибутов  
	glDisableVertexAttribArray(Attrib_vertex);

	*/

	//! Отключаем шейдерную программу  
	glUseProgram(0); 
	glFlush();
	checkOpenGLerror();
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
	}
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
