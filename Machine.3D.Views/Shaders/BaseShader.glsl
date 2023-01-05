-- Version
#version 140

-- Vertex
#include BaseShader.Version
in vec3 InPosition;
in vec3 InNormal;
uniform mat4 ModelViewProjectionMatrix; 
out vec3 Normal;
out vec3 FragPos;

void main()
{
	gl_Position = ModelViewProjectionMatrix * vec4(InPosition,1);
	Normal = InNormal;
	FragPos = InPosition;
}

-- Fragment
#include BaseShader.Version
in vec3 Normal;
in vec3 FragPos;

struct Material {
    vec4 ambient;
    vec4 diffuse;
    vec4 specular;
    float shininess; //Shininess is the power the specular light is raised to
};
struct Light {
    vec3 position;
    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
};

uniform Material material;
uniform Light light;

uniform vec3 viewPos;
out vec4 FragColor;

void main()
{
    //ambient
    vec4 ambient = vec4(light.ambient, 1.0) * material.ambient; //Remember to use the material here.

    //diffuse 
    vec3 norm = normalize(Normal);
    vec3 lightDir = normalize(light.position - FragPos);
    float diff = max(dot(norm, lightDir), 0.0);
    vec4 diffuse = vec4(light.diffuse, 1.0) * (diff * material.diffuse); //Remember to use the material here.

    //specular
    vec3 viewDir = normalize(viewPos - FragPos);
    vec3 reflectDir = reflect(-lightDir, norm);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);
    vec4 specular = vec4(light.specular, 1.0) * (spec * material.specular); //Remember to use the material here.

    //Now the result sum has changed a bit, since we now set the objects color in each element, we now dont have to
    //multiply the light with the object here, instead we do it for each element seperatly. This allows much better control
    //over how each element is applied to different objects.
    vec4 result = ambient + diffuse + specular;
    FragColor = result;
}