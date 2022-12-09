using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machine.ViewModels.GeometryExtensions.Math
{
    public static class Matrix4Extensions
    {
        public static Vector3 Transform(this Matrix4 m, Vector3 v) => ((new Vector4(v, 1)) * m).Xyz;

        public static Vector3 TransformDirection(this Matrix4 m, Vector3 v) => v * (new Matrix3(m));

        public static Vector3[] Transform(this Matrix4 m, Vector3[] v) 
        {
            var result = new Vector3[v.Length];

            for (int i = 0; i < v.Length; i++)
            {
                result[i] = m.Transform(v[i]);
            }

            return result;
        }

        public static void Append(this ref Matrix4 m, Matrix4 v) => m = v * m;
    }
}
