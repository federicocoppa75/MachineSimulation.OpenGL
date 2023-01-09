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
    [VertexShaderSource("MultiLightShader.Vertex")]
    [FragmentShaderSource("MultiLightShader.Fragment")]
    internal class MultiLightProgram : ObjectTK.Shaders.Program, IProgram, IMultiLight
    {
        [VertexAttrib(3, VertexAttribPointerType.Float)]
        public VertexAttrib InPosition { get; protected set; }

        [VertexAttrib(3, VertexAttribPointerType.Float)]
        public VertexAttrib InNormal { get; protected set; }

        public Uniform<Matrix4> ModelViewProjectionMatrix { get; protected set; }
        public UniformStruct<Material> material { get; protected set; }
        public UniformStruct<SpotLight> spotLight { get; protected set; }
        public UniformStructArray<DirectionalLight> dirLights { get; protected set; }
        public UniformStructArray<PointLight> pointLights { get; protected set; }

        public Uniform<Vector3> viewPos { get; protected set; }

        public MultiLightProgram() : base()
        {
            InitializaStructVariable();
        }

        public override void Link()
        {
            base.Link();

            material.Link(nameof(material));
            spotLight.Link(nameof(spotLight));
            dirLights.Link(nameof(dirLights));
            pointLights.Link(nameof(pointLights));
        }

        private void InitializaStructVariable()
        {
            material.Initialize();
            spotLight.Initialize();
            dirLights.Initialize(3);
            pointLights.Initialize(4);
        }
    }
}
