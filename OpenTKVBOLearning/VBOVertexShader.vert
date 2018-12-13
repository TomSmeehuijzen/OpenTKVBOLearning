﻿#version 330

layout (location = 0) in vec3 position;
layout (location = 1) in vec3 color;

uniform mat4 MVP;

out vec4 vertexColor;

void main()
{
	gl_Position = MVP * vec4(position, 1);
	vertexColor = vec4(color, 1);
}