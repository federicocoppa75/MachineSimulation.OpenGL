-- Version
#version 140

-- Vertex
#include MultiLightShader.Version
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
#include MultiLightShader.Version
in vec3 Normal;
in vec3 FragPos;

struct Material {
    vec4 ambient;
    vec4 diffuse;
    vec4 specular;
    float shininess; //Shininess is the power the specular light is raised to
};
struct DirLight {
    vec3 direction;
    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
};
struct PointLight {
    vec3 position;
    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
    float constant;
    float linear;
    float quadratic;
};
struct SpotLight {
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
uniform SpotLight spotLight;
#define NR_DIR_LIGHT 3
uniform DirLight dirLights[NR_DIR_LIGHT];
#define NR_POINT_LIGHTS 4
uniform PointLight pointLights[NR_POINT_LIGHTS];

uniform vec3 viewPos;
out vec4 FragColor;

vec4 CalcDirLight(DirLight light, vec3 normal, vec3 viewDir);
vec4 CalcPointLight(PointLight light, vec3 normal, vec3 fragPos, vec3 viewDir);
vec4 CalcSpotLight(SpotLight light, vec3 normal, vec3 fragPos, vec3 viewDir);


void main()
{
    //properties
    vec3 norm = normalize(Normal);
    vec3 viewDir = normalize(viewPos - FragPos);

    //phase 1: Directional lighting
    vec4 result;
    vec4 dLight;
    for(int i = 0; i < NR_DIR_LIGHT; i++)
        dLight += CalcDirLight(dirLights[i], norm, viewDir);

    result += dLight / NR_DIR_LIGHT;

    //phase 2: Point lights
    vec4 ptLight;
    for(int i = 0; i < NR_POINT_LIGHTS; i++)
        ptLight += CalcPointLight(pointLights[i], norm, FragPos, viewDir);

    result += ptLight / NR_POINT_LIGHTS;

    //phase 3: Spot light
    result += CalcSpotLight(spotLight, norm, FragPos, viewDir);  
    
    // la componente alpha fa normalizzata a causa della sommatoria dei contributi
    result[3] = result[3] / 3;

    FragColor = result;
}

vec4 CalcDirLight(DirLight light, vec3 normal, vec3 viewDir)
{
    vec3 lightDir = normalize(-light.direction);
    //diffuse shading
    float diff = max(dot(normal, lightDir), 0.0);
    //specular shading
    vec3 reflectDir = reflect(-lightDir, normal);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);
    //combine results
    vec4 ambient  = vec4(light.ambient, 1.0)  * material.ambient;
    vec4 diffuse  = vec4(light.diffuse, 1.0)  * diff * material.diffuse;
    vec4 specular = vec4(light.specular, 1.0) * spec * material.specular;
    return (ambient + diffuse + specular);
}

vec4 CalcPointLight(PointLight light, vec3 normal, vec3 fragPos, vec3 viewDir)
{
    vec3 lightDir = normalize(light.position - fragPos);
    //diffuse shading
    float diff = max(dot(normal, lightDir), 0.0);
    //specular shading
    vec3 reflectDir = reflect(-lightDir, normal);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);
    //attenuation
    float distance    = length(light.position - fragPos) / 1000.0;
    float attenuation = 1.0 / (light.constant + light.linear * distance + light.quadratic * (distance * distance));
    //combine results
    vec4 ambient  = vec4(light.ambient, 1.0)  * material.ambient;
    vec4 diffuse  = vec4(light.diffuse, 1.0)  * diff * material.diffuse;
    vec4 specular = vec4(light.specular, 1.0) * spec * material.specular;
    vec3 a = ambient.xyz  * attenuation;
    vec3 d = diffuse.xyz  * attenuation;
    vec3 s = specular.xyz * attenuation;
    return (vec4(a, ambient[3]) + vec4(d, diffuse[3]) + vec4(s, specular[3]));
} 
vec4 CalcSpotLight(SpotLight light, vec3 normal, vec3 fragPos, vec3 viewDir)
{

    //diffuse shading
    vec3 lightDir = normalize(light.position - FragPos);
    float diff = max(dot(normal, lightDir), 0.0);

    //specular shading
    vec3 reflectDir = reflect(-lightDir, normal);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);

    //attenuation
    float distance    = length(light.position - FragPos) / 1000.0;
    float attenuation = 1.0 / (light.constant + light.linear * distance + light.quadratic * (distance * distance));

    //spotlight intensity
    float theta     = dot(lightDir, normalize(-light.direction));
    float epsilon   = light.cutOff - light.outerCutOff;
    float intensity = clamp((theta - light.outerCutOff) / epsilon, 0.0, 1.0);

    //combine results
    vec4 ambient = vec4(light.ambient, 1.0) * material.ambient;
    vec4 diffuse = vec4(light.diffuse, 1.0) * diff * material.diffuse;
    vec4 specular = vec4(light.specular, 1.0) * spec * material.specular;
    vec3 a = ambient.xyz  * attenuation;
    vec3 d = diffuse.xyz  * attenuation * intensity;
    vec3 s = specular.xyz * attenuation * intensity;
    return (vec4(a, ambient[3]) + vec4(d, diffuse[3]) + vec4(s, specular[3]));
}