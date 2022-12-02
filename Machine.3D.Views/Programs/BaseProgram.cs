using ObjectTK.Shaders.Sources;
using ObjectTK.Shaders.Variables;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machine._3D.Views.Programs
{
    [VertexShaderSource("BaseShader.Vertex")]
    [FragmentShaderSource("BaseShader.Fragment")]
    class BaseProgram : ObjectTK.Shaders.Program
    {
        [VertexAttrib(3, VertexAttribPointerType.Float)]
        public VertexAttrib InPosition { get; protected set; }

        [VertexAttrib(3, VertexAttribPointerType.Float)]
        public VertexAttrib InNormal { get; protected set; }

        public Uniform<Matrix4> ModelViewProjectionMatrix { get; protected set; }
        public Uniform<Vector3> MaterialAmbient { get; protected set; }
        public Uniform<Vector3> MaterialDiffuse { get; protected set; }
        public Uniform<Vector3> MaterialSpecular { get; protected set; }
        public Uniform<float> MaterialShininess { get; protected set; }
        public Uniform<Vector3> LightPosition { get; protected set; }
        public Uniform<Vector3> LightAmbient { get; protected set; }
        public Uniform<Vector3> LightDiffuse { get; protected set; }
        public Uniform<Vector3> LightSpecular { get; protected set; }
        public Uniform<Vector3> viewPos { get; protected set; }
    }
}
