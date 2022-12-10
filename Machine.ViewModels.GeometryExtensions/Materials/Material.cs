using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machine.ViewModels.GeometryExtensions.Materials
{
    public class Material
    {
        public string Name { get; set; }
        public Vector4 Ambient { get; set; }
        public Vector4 Diffuse { get; set; }
        public Vector4 Specular { get; set; }
        public float Shininess { get; set; }
    }
}
