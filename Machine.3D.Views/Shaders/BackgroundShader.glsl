-- Version
#version 140

-- Vertex
#include BackgroundShader.Version
in vec3 InPosition;
in vec3 InColor;
uniform mat4 ModelViewProjectionMatrix; 
out vec3 Color;

void main()
{
	gl_Position = ModelViewProjectionMatrix * vec4(InPosition,1);
	Color = InColor;
}

-- Fragment
#include BackgroundShader.Version
in vec3 Color;
out vec4 FragColor;

void main()
{
	FragColor = vec4(Color, 1.0);
}