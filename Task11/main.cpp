#include <gl/glew.h>
#include <gl/freeglut.h>
#include <gl/SOIL.h>
#include <cmath>

static int w = 0, h = 0;

double cam_dist = 20;
double ang_hor = 0, ang_vert = -60;

const double step = 1;
double dist_x = 0, dist_y = 0;
double angle = 0;

float ambient[4] = { 0.3, 0.3, 0.3, 1 };
float amb[] = { 0.8, 0.8, 0.8 };
float dif[] = { 0.2, 0.2, 0.2 };
float emission[] = { 1, 1, 1, 0 };

unsigned int t1, t2;

void Init(void)
{
	glEnable(GL_DEPTH_TEST);
	glEnable(GL_LIGHTING);
	glEnable(GL_COLOR_MATERIAL);
	glClearColor(0, 0, 0, 1);
	glLightModelfv(GL_LIGHT_MODEL_AMBIENT, ambient);
	
	t1 = SOIL_load_OGL_texture("textures/square.bmp", SOIL_LOAD_AUTO, SOIL_CREATE_NEW_ID, 0);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_REPEAT);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_REPEAT);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_NEAREST);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_NEAREST);

	t2 = SOIL_load_OGL_texture("textures/car.bmp", SOIL_LOAD_AUTO, SOIL_CREATE_NEW_ID, 0);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_REPEAT);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_REPEAT);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_NEAREST);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_NEAREST);
}

void Update(void)
{
	// камера и всякие повороты
	glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
	glLoadIdentity();

	double ang_vert_r = ang_vert / 180 * 3.1416;
	double ang_hor_r = ang_hor / 180 * 3.1416;
	double cam_x = cam_dist * std::sin(ang_vert_r) * std::cos(ang_hor_r);
	double cam_y = cam_dist * std::sin(ang_vert_r) * std::sin(ang_hor_r);
	double cam_z = cam_dist * std::cos(ang_vert_r);
	gluLookAt(cam_x, cam_y, cam_z, 0, 0, 0, 0, 0, 1);

	// источники света
	float pos[4] = { 5,3,5,1 };
	float color[4] = { 1,1,1,1 };
	float sp[4] = { 1,1,1,1 };

	glLightfv(GL_LIGHT1, GL_SPECULAR, sp);
	glLightfv(GL_LIGHT5, GL_SPECULAR, sp);
	glLightfv(GL_LIGHT6, GL_SPECULAR, sp);
	glLightfv(GL_LIGHT7, GL_SPECULAR, sp);
	sp[1] = sp[2] = 0;
	glLightfv(GL_LIGHT2, GL_SPECULAR, sp);
	sp[0] = 0; sp[1] = 1;
	glLightfv(GL_LIGHT3, GL_SPECULAR, sp);
	sp[1] = 0; sp[2] = 1;
	glLightfv(GL_LIGHT4, GL_SPECULAR, sp);

	glLightfv(GL_LIGHT1, GL_DIFFUSE, color);
	glLightfv(GL_LIGHT5, GL_DIFFUSE, color);
	glLightfv(GL_LIGHT6, GL_DIFFUSE, color);
	glLightfv(GL_LIGHT7, GL_DIFFUSE, color);
	color[1] = color[2] = 0;
	glLightfv(GL_LIGHT2, GL_DIFFUSE, color);
	color[0] = 0; color[1] = 1;
	glLightfv(GL_LIGHT3, GL_DIFFUSE, color);
	color[1] = 0; color[2] = 1;
	glLightfv(GL_LIGHT4, GL_DIFFUSE, color);

	glLightfv(GL_LIGHT1, GL_POSITION, pos);
	pos[0] = -3; pos[1] = 1; pos[2] = 3;
	glLightfv(GL_LIGHT2, GL_POSITION, pos);
	pos[0] = -6; pos[1] = -5; pos[2] = 5;
	glLightfv(GL_LIGHT3, GL_POSITION, pos);
	pos[0] = 6; pos[1] = -5; pos[2] = 6;
	glLightfv(GL_LIGHT4, GL_POSITION, pos);

	float cam_dir[] = { -cam_x, -cam_y, -cam_z };
	pos[0] = cam_x; pos[1] = cam_y; pos[2] = cam_z;
	glLightfv(GL_LIGHT7, GL_POSITION, pos);
	glLightf(GL_LIGHT5, GL_SPOT_CUTOFF, 60);
	glLightfv(GL_LIGHT5, GL_SPOT_DIRECTION, cam_dir);

	float dir[] = { 0, 1, 0, 1 };
	glTranslatef(dist_x, dist_y, 0);
	glRotatef(angle, 0, 0, 1);
	pos[0] = -0.3; pos[1] = 1; pos[2] = 0.5;
	glLightfv(GL_LIGHT5, GL_POSITION, pos);
	glLightf(GL_LIGHT5, GL_SPOT_CUTOFF, 60);
	glLightfv(GL_LIGHT5, GL_SPOT_DIRECTION, dir);
	pos[0] += 0.6;
	glLightfv(GL_LIGHT6, GL_POSITION, pos);
	glLightf(GL_LIGHT6, GL_SPOT_CUTOFF, 60);
	glLightfv(GL_LIGHT6, GL_SPOT_DIRECTION, dir);
	glRotatef(-angle, 0, 0, 1);
	glTranslatef(-dist_x, -dist_y, 0);
	//glMaterialf(GL_FRONT, GL_SHININESS, 128.0);


	// пол - это лава
	glBindTexture(GL_TEXTURE_2D, t1);
	glEnable(GL_TEXTURE_2D);
	glMaterialfv(GL_FRONT_AND_BACK, GL_AMBIENT, amb);
	glMaterialfv(GL_FRONT_AND_BACK, GL_DIFFUSE, dif);
	glBegin(GL_QUADS);
		glNormal3f(0, 0, 1); glTexCoord2f(0.0, 0.0); glVertex3f(-10, -10, 0);
		glNormal3f(0, 0, 1); glTexCoord2f(0.0, 2.0); glVertex3f(-10.0, 10, 0.0);
		glNormal3f(0, 0, 1); glTexCoord2f(2.0, 2.0); glVertex3f(10.0, 10.0, 0);
		glNormal3f(0, 0, 1); glTexCoord2f(2.0, 0.0); glVertex3f(10.0, -10.0, 0.0);
	glEnd();
	glDisable(GL_TEXTURE_2D);

	// фонари
	GLfloat no_light[] = {0, 0, 0, 1};
	GLfloat light[] = {1, 1, 1, 0};

	glColor3f(0.2, 0, 0.4);
	glTranslatef(5, 3, 0.1);
	glutSolidCylinder(0.1, 5, 10, 3);
	glColor3f(1, 1, 1);
	glTranslatef(0, 0, 5);
	if (glIsEnabled(GL_LIGHT1))
		glMaterialfv(GL_FRONT, GL_EMISSION, light);
	else 
		glMaterialfv(GL_FRONT, GL_EMISSION, no_light);
	glutSolidSphere(0.2, 10, 10);
	glMaterialfv(GL_FRONT, GL_EMISSION, no_light);
	glTranslatef(0, 0, -5);
	glTranslatef(-5, -3, -0.1);

	glColor3f(0.2, 0, 0.4);
	glTranslatef(-3, 1, 0.1);
	glutSolidCylinder(0.1, 3, 10, 3);
	glColor3f(1, 0, 0);
	glTranslatef(0, 0, 3);
	if (glIsEnabled(GL_LIGHT2))
	{
		light[0] = 1; light[1] = light[2] = 0;
		glMaterialfv(GL_FRONT, GL_EMISSION, light);
	}
	else
		glMaterialfv(GL_FRONT, GL_EMISSION, no_light);
	glutSolidSphere(0.2, 10, 10);
	glMaterialfv(GL_FRONT, GL_EMISSION, no_light);
	glTranslatef(0, 0, -3);
	glTranslatef(3, -1, -0.1);

	glColor3f(0.2, 0, 0.4);
	glTranslatef(-6, -5, 0.1);
	glutSolidCylinder(0.1, 5, 10, 3);
	glColor3f(0, 1, 0);
	glTranslatef(0, 0, 5);
	if (glIsEnabled(GL_LIGHT3))
	{
		light[1] = 1; light[0] = light[2] = 0;
		glMaterialfv(GL_FRONT, GL_EMISSION, light);
	}
	else
		glMaterialfv(GL_FRONT, GL_EMISSION, no_light);
	glutSolidSphere(0.2, 10, 10);
	glMaterialfv(GL_FRONT, GL_EMISSION, no_light);
	glTranslatef(0, 0, -5);
	glTranslatef(6, 5, -0.1);

	glColor3f(0.2, 0, 0.4);
	glTranslatef(6, -5, 0.1);
	glutSolidCylinder(0.1, 6, 10, 3);
	glColor3f(0, 0, 1);
	glTranslatef(0, 0, 6);
	if (glIsEnabled(GL_LIGHT4))
	{
		light[2] = 1; light[1] = light[0] = 0;
		glMaterialfv(GL_FRONT, GL_EMISSION, light);
	}
	else
		glMaterialfv(GL_FRONT, GL_EMISSION, no_light);
	glutSolidSphere(0.2, 10, 10);
	glMaterialfv(GL_FRONT, GL_EMISSION, no_light);
	glTranslatef(0, 0, -6);
	glTranslatef(-6, 5, -0.1);

	// каркас машинки
	glTranslatef(dist_x, dist_y, 0);
	glRotatef(angle, 0, 0, 1);

	glColor3f(1, 1, 1);
	glBindTexture(GL_TEXTURE_2D, t2);
	glEnable(GL_TEXTURE_2D);
	glBegin(GL_QUADS);
		glNormal3f(-1, -1, -1); glTexCoord2f(0.0, 0.0); glVertex3f(-0.5, -1, 0.2);
		glNormal3f(-1, 1, -1); glTexCoord2f(0.0, 1.0); glVertex3f(-0.5, 1, 0.2);
		glNormal3f(1, 1, -1); glTexCoord2f(0.5, 1.0); glVertex3f(0.5, 1, 0.2);
		glNormal3f(1, -1, -1); glTexCoord2f(0.5, 0.0); glVertex3f(0.5, -1, 0.2);

		glNormal3f(-1, -1, -1); glTexCoord2f(0.0, 0.0); glVertex3f(-0.5, -1, 0.2);
		glNormal3f(-1, 1, -1); glTexCoord2f(0.0, 1.0); glVertex3f(-0.5, 1, 0.2);
		glNormal3f(-1, 1, 1); glTexCoord2f(0.5, 1.0); glVertex3f(-0.5, 1, 1);
		glNormal3f(-1, -1, 1); glTexCoord2f(0.5, 0.0); glVertex3f(-0.5, -1, 1);

		glNormal3f(1, -1, 1); glTexCoord2f(0.0, 0.0); glVertex3f(0.5, -1, 1);
		glNormal3f(1, 1, 1); glTexCoord2f(0.0, 1.0); glVertex3f(0.5, 1, 1);
		glNormal3f(1, 1, -1); glTexCoord2f(0.5, 1.0); glVertex3f(0.5, 1, 0.2);
		glNormal3f(1, -1, -1); glTexCoord2f(0.5, 0.0); glVertex3f(0.5, -1, 0.2);

		glNormal3f(-1, -1, 1); glTexCoord2f(0.0, 0.0); glVertex3f(-0.5, -1, 1);
		glNormal3f(-1, 1, 1); glTexCoord2f(0.0, 1.0); glVertex3f(-0.5, 1, 1);
		glNormal3f(1, 1, 1); glTexCoord2f(0.5, 1.0); glVertex3f(0.5, 1, 1);
		glNormal3f(1, -1, 1); glTexCoord2f(0.5, 0.0); glVertex3f(0.5, -1, 1);

		glNormal3f(-1, 1, -1); glTexCoord2f(0.0, 0.0); glVertex3f(-0.5, 1, 0.2);
		glNormal3f(1, 1, -1); glTexCoord2f(0.0, 0.5); glVertex3f(0.5, 1, 0.2);
		glNormal3f(1, 1, 1); glTexCoord2f(0.5, 0.5); glVertex3f(0.5, 1, 1);
		glNormal3f(-1, 1, 1); glTexCoord2f(0.5, 0.0); glVertex3f(-0.5, 1, 1);

		glNormal3f(-1, -1, -1); glTexCoord2f(0.0, 0.0); glVertex3f(-0.5, -1, 0.2);
		glNormal3f(1, -1, -1); glTexCoord2f(0.0, 0.5); glVertex3f(0.5, -1, 0.2);
		glNormal3f(1, -1, 1); glTexCoord2f(0.5, 0.5); glVertex3f(0.5, -1, 1);
		glNormal3f(-1, -1, 1); glTexCoord2f(0.5, 0.0); glVertex3f(-0.5, -1, 1);
	glEnd();
	glDisable(GL_TEXTURE_2D);

	// колёса
	glColor3f(0.3, 0, 0);
	glTranslatef(-0.5, 0.8, 0.2);
	glRotatef(90, 0, 1, 0);
	glutSolidTorus(0.1, 0.2, 10, 10);
	glTranslatef(0, -1.6, 0);
	glutSolidTorus(0.1, 0.2, 10, 10);
	glTranslatef(0, 0, 1);
	glutSolidTorus(0.1, 0.2, 10, 10);
	glTranslatef(0, 1.6, 0);
	glutSolidTorus(0.1, 0.2, 10, 10);
	glTranslatef(0, 0, -1);
	glRotatef(-90, 0, 1, 0);
	glTranslatef(0.5, -0.8, -0.2);

	// фары
	glColor3f(1, 1, 1);
	glTranslatef(-0.3, 1, 0.5);
	if (glIsEnabled(GL_LIGHT5))
		glMaterialfv(GL_FRONT, GL_EMISSION, light);
	else
		glMaterialfv(GL_FRONT, GL_EMISSION, no_light);
	glutSolidSphere(0.2, 5, 5);
	glTranslatef(0.6, 0, 0);
	glutSolidSphere(0.2, 5, 5);
	glMaterialfv(GL_FRONT, GL_EMISSION, no_light);
	glTranslatef(-0.3, -1, -0.5);

	glRotatef(-angle, 0, 0, 1);
	glTranslatef(-dist_x, -dist_y, 0);

	
	glFlush();
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
	switch (key)
	{
	case GLUT_KEY_UP:
		dist_x -= step * std::sin(angle / 180 * 3.1416);
		dist_y += step * std::cos(angle / 180 * 3.1416);
		break;
	case GLUT_KEY_DOWN:
		dist_x += step * std::sin(angle / 180 * 3.1416);
		dist_y -= step * std::cos(angle / 180 * 3.1416);
		break;
	case GLUT_KEY_LEFT:
		angle += 5;
		break;
	case GLUT_KEY_RIGHT:
		angle -= 5;
		break;
	}
	glutPostRedisplay();
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
		cam_dist--;
		break;
	case 'z':
		cam_dist++;
		break;
	case '1':
		if (glIsEnabled(GL_LIGHT1))
			glDisable(GL_LIGHT1);
		else
			glEnable(GL_LIGHT1);
		break;
	case '2':
		if (glIsEnabled(GL_LIGHT2))
			glDisable(GL_LIGHT2);
		else
			glEnable(GL_LIGHT2);
		break;
	case '3':
		if (glIsEnabled(GL_LIGHT3))
			glDisable(GL_LIGHT3);
		else
			glEnable(GL_LIGHT3);
		break;
	case '4':
		if (glIsEnabled(GL_LIGHT4))
			glDisable(GL_LIGHT4);
		else
			glEnable(GL_LIGHT4);
		break;
	case '0':
		if (glIsEnabled(GL_LIGHT5))
		{
			glDisable(GL_LIGHT5);
			glDisable(GL_LIGHT6);
		}
		else
		{
			glEnable(GL_LIGHT5);
			glEnable(GL_LIGHT6);
		}
		break;
	case '=':
		if (glIsEnabled(GL_LIGHT7))
			glDisable(GL_LIGHT7);
		else
			glEnable(GL_LIGHT7);
		break;
	}
	glutPostRedisplay();
}

int main(int argc, char * argv[])
{
	glutInit(&argc, argv);
	glutInitWindowPosition(100, 100);
	glutInitWindowSize(800, 600);
	glutInitDisplayMode(GLUT_RGBA | GLUT_DOUBLE);
	glutCreateWindow("OpenGL");
	//glutIdleFunc(Update);
	glutDisplayFunc(Update);
	glutReshapeFunc(Reshape);
	glutSpecialFunc(SpecialKeys);
	glutKeyboardFunc(Keyboard);
	Init();
	glutMainLoop();
	return 0;
}