#include "objects.h"
#include <glm/gtc/matrix_transform.hpp>
#include <gl/OBJ_Loader.h>

object object::clone()
{
	object res;
	res.vertices = this->vertices;
	res.normals = this->normals;
	res.textures = this->textures;
	res.indices = this->indices;
	res.model = this->model;
	return res;
}

object icosahedron()
{
	std::vector<float> v = {
		-0.4472,  0.6155, -0.4472, //0
		 0.4472,  0.6155, -0.4472, //1
			  0,  0.7608,  0.4472, //2
		-0.7236,  0.2351,  0.4472, //3
		-0.7236, -0.2351, -0.4472, //4
		-0.4472, -0.6155,  0.4472, //5
			  0, -0.7608, -0.4472, //6
		 0.4472, -0.6155,  0.4472, //7
		 0.7236, -0.2351, -0.4472, //8
		 0.7236,  0.2351,  0.4472, //9
			  0,       0,      -1, //10
			  0,       0,       1, //11
			  -0.4472,  0.6155, -0.4472, //0
		 0.4472,  0.6155, -0.4472, //1
			  0,  0.7608,  0.4472, //2
		-0.7236,  0.2351,  0.4472, //3
		-0.7236, -0.2351, -0.4472, //4
		-0.4472, -0.6155,  0.4472, //5
			  0, -0.7608, -0.4472, //6
		 0.4472, -0.6155,  0.4472, //7
		 0.7236, -0.2351, -0.4472, //8
		 0.7236,  0.2351,  0.4472, //9
			  0,       0,      -1, //10
			  0,       0,       1, //11
			  -0.4472,  0.6155, -0.4472, //0
		 0.4472,  0.6155, -0.4472, //1
			  0,  0.7608,  0.4472, //2
		-0.7236,  0.2351,  0.4472, //3
		-0.7236, -0.2351, -0.4472, //4
		-0.4472, -0.6155,  0.4472, //5
			  0, -0.7608, -0.4472, //6
		 0.4472, -0.6155,  0.4472, //7
		 0.7236, -0.2351, -0.4472, //8
		 0.7236,  0.2351,  0.4472, //9
			  0,       0,      -1, //10
			  0,       0,       1, //11
			  -0.4472,  0.6155, -0.4472, //0
		 0.4472,  0.6155, -0.4472, //1
			  0,  0.7608,  0.4472, //2
		-0.7236,  0.2351,  0.4472, //3
		-0.7236, -0.2351, -0.4472, //4
		-0.4472, -0.6155,  0.4472, //5
			  0, -0.7608, -0.4472, //6
		 0.4472, -0.6155,  0.4472, //7
		 0.7236, -0.2351, -0.4472, //8
		 0.7236,  0.2351,  0.4472, //9
			  0,       0,      -1, //10
			  0,       0,       1, //11
			  -0.4472,  0.6155, -0.4472, //0
		 0.4472,  0.6155, -0.4472, //1
			  0,  0.7608,  0.4472, //2
		-0.7236,  0.2351,  0.4472, //3
		-0.7236, -0.2351, -0.4472, //4
		-0.4472, -0.6155,  0.4472, //5
			  0, -0.7608, -0.4472, //6
		 0.4472, -0.6155,  0.4472, //7
		 0.7236, -0.2351, -0.4472, //8
		 0.7236,  0.2351,  0.4472, //9
			  0,       0,      -1, //10
			  0,       0,       1, //11
	};

	std::vector<float> n = {
		-0.3403,  0.4683, -0.2462, //0
		 0.3403,  0.4683, -0.2462, //1
			  0,  0.5789,  0.2462, //2
		-0.5506,  0.1789,  0.2462, //3
		-0.5506, -0.1789, -0.2462, //4
		-0.3403, -0.4683,  0.2462, //5
			  0, -0.5789, -0.2462, //6
		 0.3403, -0.4683,  0.2462, //7
		 0.5506, -0.1789, -0.2462, //8
		 0.5506,  0.1789,  0.2462, //9
			  0,       0, -0.5506, //10
			  0,       0,  0.5506, //11
			  -0.3403,  0.4683, -0.2462, //0
		 0.3403,  0.4683, -0.2462, //1
			  0,  0.5789,  0.2462, //2
		-0.5506,  0.1789,  0.2462, //3
		-0.5506, -0.1789, -0.2462, //4
		-0.3403, -0.4683,  0.2462, //5
			  0, -0.5789, -0.2462, //6
		 0.3403, -0.4683,  0.2462, //7
		 0.5506, -0.1789, -0.2462, //8
		 0.5506,  0.1789,  0.2462, //9
			  0,       0, -0.5506, //10
			  0,       0,  0.5506, //11
			  -0.3403,  0.4683, -0.2462, //0
		 0.3403,  0.4683, -0.2462, //1
			  0,  0.5789,  0.2462, //2
		-0.5506,  0.1789,  0.2462, //3
		-0.5506, -0.1789, -0.2462, //4
		-0.3403, -0.4683,  0.2462, //5
			  0, -0.5789, -0.2462, //6
		 0.3403, -0.4683,  0.2462, //7
		 0.5506, -0.1789, -0.2462, //8
		 0.5506,  0.1789,  0.2462, //9
			  0,       0, -0.5506, //10
			  0,       0,  0.5506, //11
			  -0.3403,  0.4683, -0.2462, //0
		 0.3403,  0.4683, -0.2462, //1
			  0,  0.5789,  0.2462, //2
		-0.5506,  0.1789,  0.2462, //3
		-0.5506, -0.1789, -0.2462, //4
		-0.3403, -0.4683,  0.2462, //5
			  0, -0.5789, -0.2462, //6
		 0.3403, -0.4683,  0.2462, //7
		 0.5506, -0.1789, -0.2462, //8
		 0.5506,  0.1789,  0.2462, //9
			  0,       0, -0.5506, //10
			  0,       0,  0.5506, //11
			  -0.3403,  0.4683, -0.2462, //0
		 0.3403,  0.4683, -0.2462, //1
			  0,  0.5789,  0.2462, //2
		-0.5506,  0.1789,  0.2462, //3
		-0.5506, -0.1789, -0.2462, //4
		-0.3403, -0.4683,  0.2462, //5
			  0, -0.5789, -0.2462, //6
		 0.3403, -0.4683,  0.2462, //7
		 0.5506, -0.1789, -0.2462, //8
		 0.5506,  0.1789,  0.2462, //9
			  0,       0, -0.5506, //10
			  0,       0,  0.5506, //11
	};

	std::vector<float> t = {
		0, 0, //0
		0, 0, //1
		0, 0, //2
		0, 0, //3
		0, 0, //4
		0, 0, //5
		0, 0, //6
		0, 0, //7
		1, 0, //8
		1, 0, //9
		0.5, 1,
		0.5, 1,
		0, 0, //0 + 12
		1, 0, //1
		0, 0, //2
		1, 0, //3
		1, 0, //4
		1, 0, //5
		1, 0, //6
		1, 0, //7
		1, 0, //8
		1, 0, //9
		0.5, 1,
		0.5, 1,
		1, 0, //0 + 24
		0.5, 1, //1
		0, 0, //2
		1, 0, //3
		1, 0, //4
		1, 0, //5
		1, 0, //6
		1, 0, //7
		1, 0, //8
		1, 0, //9
		0.5, 1,
		0.5, 1,
		0.5, 1, //0 + 36
		0, 0, //1
		1, 0, //2
		0.5, 1, //3
		0.5, 1, //4
		0.5, 1, //5
		0.5, 1, //6
		0.5, 1, //7
		0.5, 1, //8
		0.5, 1, //9
		0.5, 1,
		0.5, 1,
		0.5, 1, //0 + 48
		1, 0, //1
		0, 0, //2
		0, 0, //3
		0, 0, //4
		0, 0, //5
		0, 0, //6
		0, 0, //7
		0, 0, //8
		0, 0, //9
		0.5, 1,
		0.5, 1,
	};

	std::vector<unsigned int> i = {
		11, 2, 9, //0
		10, 1, 8, //1
		23, 21, 7, //2
		22, 20, 6, //3
		35, 19, 5, //4
		34, 18, 4, //5
		47, 17, 3, //6
		46, 16, 0, //7
		59, 15, 14, //8
		58, 12, 13, //9
		26, 33, 25, //10
		37, 32, 45, //11
		57, 31, 44, //12
		56, 30, 43, //13
		55, 29, 42, //14
		54, 28, 41, //15
		53, 27, 40, //16
		52, 24, 39, //17
		51, 38, 36, //18
		48, 49, 50  //19
	};

	object res;
	res.vertices = v;
	res.normals = n;
	res.textures = t;
	res.indices = i;

	res.model = glm::mat4(1);

	return res;
}

object cube()
{
	std::vector<float> v = {
		0, 0, 0,
		0, 0, 1,
		0, 1, 0,
		0, 1, 1,
		1, 0, 0,
		1, 0, 1,
		1, 1, 0,
		1, 1, 1,

		0, 0, 0,
		0, 0, 1,
		0, 1, 0,
		0, 1, 1,
		1, 0, 0,
		1, 0, 1,
		1, 1, 0,
		1, 1, 1,

		0, 0, 0,
		0, 0, 1,
		0, 1, 0,
		0, 1, 1,
		1, 0, 0,
		1, 0, 1,
		1, 1, 0,
		1, 1, 1,
	};

	std::vector<float> n = {
		-1, -1, -1,
		-1, -1,  1,
		-1,  1, -1,
		-1,  1,  1,
		 1, -1, -1,
		 1, -1,  1,
		 1,  1, -1,
		 1,  1,  1,

		 -1, -1, -1,
		-1, -1,  1,
		-1,  1, -1,
		-1,  1,  1,
		 1, -1, -1,
		 1, -1,  1,
		 1,  1, -1,
		 1,  1,  1,

		 -1, -1, -1,
		-1, -1,  1,
		-1,  1, -1,
		-1,  1,  1,
		 1, -1, -1,
		 1, -1,  1,
		 1,  1, -1,
		 1,  1,  1,
	};

	std::vector<float> t = {
		0, 0,
		0, 0,
		1, 0,
		1, 0,
		0, 1,
		0, 1,
		1, 1,
		1, 1,

		0, 0, //8
		0, 1,
		1, 0,
		1, 1,
		0, 0,
		0, 1,
		1, 0,
		1, 1,

		0, 0, //16
		1, 0,
		0, 0,
		1, 0,
		0, 1,
		1, 1,
		0, 1,
		1, 1,
	};

	std::vector<unsigned int> i = {
		0, 4, 6,
		0, 6, 2,

		1, 5, 7,
		1, 7, 3,

		12, 14, 15,
		12, 15, 13,

		8, 10, 11,
		8, 11, 9,

		16, 20, 21,
		16, 21, 17,

		18, 22, 23,
		18, 23, 19
	};

	object res;
	res.vertices = v;
	res.normals = n;
	res.textures = t;
	res.indices = i;

	res.model = glm::mat4(1);

	return res;
}


object tetrahedron()
{
	std::vector<float> v = {
		1, 0, 0,
		-0.5, -0.866, 0,
		-0.5, 0.866, 0,
		0, 0, 1,

		1, 0, 0,
		-0.5, -0.866, 0,
		-0.5, 0.866, 0,
		0, 0, 1,

		1, 0, 0,
		-0.5, -0.866, 0,
		-0.5, 0.866, 0,
		0, 0, 1,
	};

	std::vector<float> n = {
		-1.732, 0, 0.732, // 0
		0.866, -1.5, 0.732, // 1
		0.866, 1.5, 0.732, // 2
		0, 0, 1.732, // 3

		-1.732, 0, 0.732, // 0
		0.866, -1.5, 0.732, // 1
		0.866, 1.5, 0.732, // 2
		0, 0, 1.732, // 3

		-1.732, 0, 0.732, // 0
		0.866, -1.5, 0.732, // 1
		0.866, 1.5, 0.732, // 2
		0, 0, 1.732, // 3
	};

	std::vector<float> t = {
		0, 0,
		0.5, 1,
		1, 0,
		1, 0,

		0, 0,
		0.5, 1,
		0.5, 1,
		1, 0,

		0, 0,
		0, 0,
		0.5, 1,
		1, 0,
	};

	std::vector<unsigned int> i = {
		0, 1, 2,
		4, 5, 3,
		8, 6, 7,
		9, 10, 11,
	};

	object res;
	res.vertices = v;
	res.normals = n;
	res.textures = t;
	res.indices = i;

	res.model = glm::mat4(1);

	return res;
}

object wall()
{
	std::vector<float> v = {
		-1, -1, 0,
		-1, 1, 0,
		1, 1, 0,
		1, -1, 0,
	};

	std::vector<float> n = {
		0, 0, 1,
		0, 0, 1,
		0, 0, 1,
		0, 0, 1,
	};

	std::vector<float> t = {
		0, 0,
		0, 1,
		1, 1,
		1, 0,
	};

	std::vector<unsigned int> i = {
		0, 1, 2,
		0, 2, 3,
	};

	object res;
	res.vertices = v;
	res.normals = n;
	res.textures = t;
	res.indices = i;

	res.model = glm::mat4(1);

	return res;
}

object from_file(std::string path)
{
	objl::Loader load;
	load.LoadFile(path);
	auto mesh = load.LoadedMeshes[0];

	unsigned int num_verts = mesh.Vertices.size();
	unsigned int num_inds = mesh.Indices.size();
	

	std::vector<float> v(num_verts * 3);
	std::vector<float> n(num_verts * 3);
	std::vector<float> t(num_verts * 2);
	std::vector<unsigned int> i(num_inds);

	for (int i = 0; i < num_verts; i++) 
	{
		v[3 * i + 0] = mesh.Vertices[i].Position.X;
		v[3 * i + 1] = mesh.Vertices[i].Position.Y;
		v[3 * i + 2] = mesh.Vertices[i].Position.Z;

		n[3 * i + 0] = mesh.Vertices[i].Normal.X;
		n[3 * i + 1] = mesh.Vertices[i].Normal.Y;
		n[3 * i + 2] = mesh.Vertices[i].Normal.Z;

		t[2 * i + 0] = mesh.Vertices[i].TextureCoordinate.X;
		t[2 * i + 1] = mesh.Vertices[i].TextureCoordinate.Y;
	}

	for (int j = 0; j < num_inds; ++j)
		i[j] = mesh.Indices[j];

	object res;
	res.vertices = v;
	res.normals = n;
	res.textures = t;
	res.indices = i;

	res.model = glm::mat4(1);

	return res;
}

std::vector<object> objects()
{
	std::vector<object> res;
	object obj;

	/*object obj1 = icosahedron();
	res.push_back(obj1);

	object obj2 = cube();
	obj2.model = glm::translate(glm::mat4(1), glm::vec3(0, 1, 2));
	res.push_back(obj2);

	object obj3 = tetrahedron();
	obj3.model = glm::translate(glm::mat4(1), glm::vec3(3, 2, 1));
	res.push_back(obj3);*/

	// стены и пол
	obj = wall();
	obj.model = glm::rotate(obj.model, glm::radians(-90.f), glm::vec3(1.0, 0.0, 0.0));
	obj.model = glm::translate(obj.model, glm::vec3(0, 0, -5));
	obj.model = glm::scale(obj.model, glm::vec3(5, 5, 1));
	obj.diffuse = 0.8;
	obj.specular = 0.2;
	obj.reflect = 5;
	res.push_back(obj);

	obj = wall();
	obj.model = glm::rotate(obj.model, glm::radians(90.f), glm::vec3(0.0, 1.0, 0.0));
	obj.model = glm::translate(obj.model, glm::vec3(0, 0, -5));
	obj.model = glm::scale(obj.model, glm::vec3(5, 5, 1));
	obj.diffuse = 0.8;
	obj.specular = 0.2;
	obj.reflect = 5;
	res.push_back(obj);

	obj = wall();
	obj.model = glm::translate(obj.model, glm::vec3(0, 0, -5));
	obj.model = glm::scale(obj.model, glm::vec3(5, 5, 1));
	obj.diffuse = 0.8;
	obj.specular = 0.2;
	obj.reflect = 5;
	res.push_back(obj);

	// ёлка
	/*object obj7 = tetrahedron();
	obj7.model = glm::scale(obj7.model, glm::vec3(2, 2, 2));
	obj7.model = glm::translate(obj7.model, glm::vec3(-1, -2, -2.5));
	res.push_back(obj7);

	object obj8 = tetrahedron();
	obj8.model = glm::scale(obj8.model, glm::vec3(1.5, 1.5, 1.5));
	obj8.model = glm::translate(obj8.model, glm::vec3(-1.3, -2.8, -2));
	res.push_back(obj8);

	object obj9 = tetrahedron();
	obj9.model = glm::translate(obj9.model, glm::vec3(-2.1, -4.3, -1.5));
	res.push_back(obj9);*/

	// олень
	obj = from_file("deer.obj");
	for (int i = 0; i < obj.normals.size(); ++i)
	{
		obj.normals[i] *= -1;
	}
	obj.model = glm::scale(obj.model, glm::vec3(0.004, 0.004, 0.004));
	obj.model = glm::rotate(obj.model, glm::radians(90.f), glm::vec3(1, 0, 0));
	obj.model = glm::translate(obj.model, glm::vec3(500, -1300, 500));
	obj.model = glm::rotate(obj.model, glm::radians(45.f), glm::vec3(0, 1, 0));
	obj.diffuse = 0.5;
	obj.specular = 0.7;
	obj.reflect = 3;
	res.push_back(obj);

	// снеговик
	obj = from_file("ball.obj");
	object obj12 = obj.clone(), obj13 = obj.clone();

	obj.model = glm::scale(obj.model, glm::vec3(0.4, 0.4, 0.4));
	obj.model = glm::translate(obj.model, glm::vec3(-7, 4, -10.5));
	obj.diffuse = 0.9;
	obj.specular = 0.1;
	obj.reflect = 5;
	res.push_back(obj);

	obj12.model = glm::scale(obj12.model, glm::vec3(0.3, 0.3, 0.3));
	obj12.model = glm::translate(obj12.model, glm::vec3(-9.25, 5.25, -10));
	obj12.diffuse = 0.9;
	obj12.specular = 0.1;
	obj12.reflect = 5;
	res.push_back(obj12);

	obj13.model = glm::scale(obj13.model, glm::vec3(0.2, 0.2, 0.2));
	obj13.model = glm::translate(obj13.model, glm::vec3(-13.8, 7.7, -10));
	obj13.diffuse = 0.9;
	obj13.specular = 0.1;
	obj13.reflect = 5;
	res.push_back(obj13);

	// ещё ель
	obj = from_file("pine.obj");
	obj.model = glm::scale(obj.model, glm::vec3(0.02, 0.02, 0.02));
	obj.model = glm::rotate(obj.model, glm::radians(90.f), glm::vec3(0, 0, 1));
	obj.model = glm::translate(obj.model, glm::vec3(50, -50, -270));
	obj.diffuse = 0.8;
	obj.specular = 0.2;
	obj.reflect = 2;
	res.push_back(obj);

	// подарки
	obj = cube();
	obj.model = glm::scale(obj.model, glm::vec3(1.5, 1.5, 1.5));
	obj.model = glm::translate(obj.model, glm::vec3(-0.5, -1.5, -3.3));
	obj.diffuse = 0.5;
	obj.specular = 0.5;
	obj.reflect = 5;
	res.push_back(obj);

	obj = cube();
	obj.model = glm::rotate(obj.model, glm::radians(30.f), glm::vec3(0, 0, 1));
	obj.model = glm::translate(obj.model, glm::vec3(-1.5, 0.5, -5));
	obj.diffuse = 0.7;
	obj.specular = 0.3;
	obj.reflect = 5;
	res.push_back(obj);

	obj = cube();
	obj.model = glm::rotate(obj.model, glm::radians(45.f), glm::vec3(0, 0, 1));
	obj.model = glm::translate(obj.model, glm::vec3(0, -0.5, -5));
	obj.model = glm::scale(obj.model, glm::vec3(1.5, 1.5, 1.5));
	obj.diffuse = 0.9;
	obj.specular = 0.1;
	obj.reflect = 5;
	res.push_back(obj);

	return res;
}