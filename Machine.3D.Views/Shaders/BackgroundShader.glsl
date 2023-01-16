-- Version
#version 140

-- Vertex
#include BackgroundShader.Version
in vec4 InPosition;
uniform mat4 ModelViewProjectionMatrix; 
out float Gradient;

void main()
{
	gl_Position = ModelViewProjectionMatrix * vec4(InPosition.xyz,1);
	Gradient = InPosition.w;
}

-- Fragment
#include BackgroundShader.Version
in float Gradient;
uniform vec3 UpColor;
uniform vec3 DwColor;
out vec4 FragColor;

void main()
{
	vec3 color = UpColor * Gradient + DwColor * (1 - Gradient);	
	FragColor = vec4(color, 1.0);
}