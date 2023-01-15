using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ObjectTK.Shaders.Sources;
using ObjectTK.Shaders.Variables;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Machine._3D.Views.Programs
{
    [VertexShaderSource("BackgroundShader.Vertex")]
    [FragmentShaderSource("BackgroundShader.Fragment")]
    internal class BackgroundProgram : ObjectTK.Shaders.Program
    {
        [VertexAttrib(3, VertexAttribPointerType.Float)]
        public VertexAttrib InPosition { get; protected set; }

        [VertexAttrib(3, VertexAttribPointerType.Float)]
        public VertexAttrib InColor { get; protected set; }

        public Uniform<Matrix4> ModelViewProjectionMatrix { get; protected set; }
    }
}
