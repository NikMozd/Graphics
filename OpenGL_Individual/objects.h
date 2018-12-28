#ifndef OBJECTS_H
#define OBJECTS_H

#include <vector>
#include <gl/glew.h>
#include <glm/glm.hpp>

struct object
{
	std::vector<GLfloat> vertices;
	std::vector<GLfloat> normals;
	std::vector<GLfloat> textures;
	std::vector<GLuint> indices;
	glm::mat4 model;
	GLfloat diffuse, specular, reflect;
	object clone();
};

std::vector<object> objects();

#endif // !OBJECTS_H
