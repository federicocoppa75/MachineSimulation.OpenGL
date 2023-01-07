using ObjectTK.Shaders.Variables;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machine._3D.Views.Programs
{
    internal interface IProgram
    {
        VertexAttrib InPosition { get; }
        VertexAttrib InNormal { get; }
        Uniform<Matrix4> ModelViewProjectionMatrix { get; }
        Uniform<Vector3> viewPos { get; }
        UniformStruct<Material> material { get; }

        void Use();
    }
}
