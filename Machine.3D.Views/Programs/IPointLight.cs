using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machine._3D.Views.Programs
{
    internal interface IPointLight
    {
        UniformStruct<PointLight> light { get; }
    }
}
