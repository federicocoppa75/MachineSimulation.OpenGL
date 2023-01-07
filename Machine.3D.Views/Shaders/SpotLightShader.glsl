-- Version
#version 140

-- Vertex
#include SpotLightShader.Version
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
#include SpotLightShader.Version
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
    vec3 direction;
    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
    float cutOff;
    float outerCutOff;
    float constant;
    float linear;
    float quadratic;
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

    //attenuation
    float distance    = length(light.position - FragPos) / 1000.0; // conversione da mm a metri
    float attenuation = 1.0 / (light.constant + light.linear * distance + light.quadratic * (distance * distance));

    //spotlight intensity
    //This is how we calculate the spotlight, for a more in depth explanation of how this works. Check out the web tutorials.
    float theta     = dot(lightDir, normalize(-light.direction));
    float epsilon   = light.cutOff - light.outerCutOff;
    float intensity = clamp((theta - light.outerCutOff) / epsilon, 0.0, 1.0); //The intensity, is the lights intensity on a given fragment,


    //When applying the spotlight intensity we want to multiply it.
    vec3 a = ambient.xyz * attenuation;//Remember the ambient is where the light dosen't hit, this means the spotlight shouldn't be applied
    vec3 d = diffuse.xyz * attenuation * intensity;
    vec3 s = specular.xyz * attenuation * intensity;

    vec4 result = vec4(a, ambient[3]) + vec4(d, diffuse[3]) + vec4(s, specular[3]);
    FragColor = result;
}