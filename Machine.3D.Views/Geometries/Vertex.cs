using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Machine._3D.Views.Geometries
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct Vertex
    {
        public Vector3 position; 
        public Vector3 normal;

        public Vertex(float x, float y, float z, float i, float j, float k)
        {
            position = new Vector3(x, y, z);
            normal = new Vector3(i, j, k);
        }

        public Vertex(Vector3 position, Vector3 normal)
        {
            this.position = position;
            this.normal = normal;
        }
    }
}
