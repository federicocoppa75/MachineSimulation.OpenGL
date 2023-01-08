using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machine._3D.Views.Programs
{
    internal interface IMultiLight
    {
        UniformStruct<DirectionalLight> dirLight { get; }
        UniformStruct<SpotLight> spotLight { get; }
        UniformStructArray<PointLight> pointLights { get; }
    }
}
