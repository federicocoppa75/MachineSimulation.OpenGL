using ObjectTK.Shaders.Sources;
using ObjectTK.Shaders.Variables;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Machine._3D.Views.Programs
{
    [VertexShaderSource("BaseShader.Vertex")]
    [FragmentShaderSource("BaseShader.Fragment")]
    class BaseProgram : ObjectTK.Shaders.Program, IProgram, IPointLight
    {
        [VertexAttrib(3, VertexAttribPointerType.Float)]
        public VertexAttrib InPosition { get; protected set; }

        [VertexAttrib(3, VertexAttribPointerType.Float)]
        public VertexAttrib InNormal { get; protected set; }

        public Uniform<Matrix4> ModelViewProjectionMatrix { get; protected set; }
        public UniformStruct<Material> material { get; protected set; }
        public UniformStruct<Light> light { get; protected set; }
        public Uniform<Vector3> viewPos { get; protected set; }

        public BaseProgram() : base()
        {
            InitializaStructVariable();
        }

        public override void Link()
        {
            base.Link();

            material.Link(nameof(material));
            light.Link(nameof(light));
        }

        private void InitializaStructVariable()
        {
            material.Initialize();
            light.Initialize();
        }
    }
}
