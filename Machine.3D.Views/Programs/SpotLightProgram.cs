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
    [VertexShaderSource("SpotLightShader.Vertex")]
    [FragmentShaderSource("SpotLightShader.Fragment")]
    class SpotLightProgram : ObjectTK.Shaders.Program, IProgram, ISpotLight
    {
        [VertexAttrib(3, VertexAttribPointerType.Float)]
        public VertexAttrib InPosition { get; protected set; }

        [VertexAttrib(3, VertexAttribPointerType.Float)]
        public VertexAttrib InNormal { get; protected set; }

        public Uniform<Matrix4> ModelViewProjectionMatrix { get; protected set; }
        public UniformStruct<Material> material { get; protected set; }
        public UniformStruct<SpotLight> light { get; protected set; }
        public Uniform<Vector3> viewPos { get; protected set; }

        public SpotLightProgram() : base()
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
