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

uniform vec3 MaterialAmbient;
uniform vec3 MaterialDiffuse;
uniform vec3 MaterialSpecular;
uniform float MaterialShininess;
uniform vec3 LightPosition;
uniform vec3 LightAmbient;
uniform vec3 LightDiffuse;
uniform vec3 LightSpecular;

uniform vec3 viewPos;
out vec4 FragColor;

struct Material {
    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
    float shininess; //Shininess is the power the specular light is raised to
};
struct Light {
    vec3 position;
    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
};

void main()
{
    Material material;
    Light light;

    material.ambient = MaterialAmbient;
    material.diffuse = MaterialDiffuse;
    material.specular = MaterialSpecular;
    material.shininess = MaterialShininess;
    
    light.position = LightPosition;
    light.ambient = LightAmbient;
    light.diffuse = LightDiffuse;
    light.specular = LightSpecular;  

    //ambient
    vec3 ambient = light.ambient * material.ambient; //Remember to use the material here.

    //diffuse 
    vec3 norm = normalize(Normal);
    vec3 lightDir = normalize(light.position - FragPos);
    float diff = max(dot(norm, lightDir), 0.0);
    vec3 diffuse = light.diffuse * (diff * material.diffuse); //Remember to use the material here.

    //specular
    vec3 viewDir = normalize(viewPos - FragPos);
    vec3 reflectDir = reflect(-lightDir, norm);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);
    vec3 specular = light.specular * (spec * material.specular); //Remember to use the material here.

    //Now the result sum has changed a bit, since we now set the objects color in each element, we now dont have to
    //multiply the light with the object here, instead we do it for each element seperatly. This allows much better control
    //over how each element is applied to different objects.
    vec3 result = ambient + diffuse + specular;
    FragColor = vec4(result, 1.0);
}