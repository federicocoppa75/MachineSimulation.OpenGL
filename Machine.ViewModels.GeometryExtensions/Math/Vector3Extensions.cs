using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machine.ViewModels.GeometryExtensions.Math
{
    public static class Vector3Extensions
    {
        public static bool IsZero(this Vector3 v)
        {
            if (v.X == 0f && v.Y == 0f)
            {
                return v.Z == 0f;
            }

            return false;
            
        }

        public static Vector3 GetMinimum(this Vector3 v, Vector3 p)
        {
            var result = new Vector3(v);

            if(p.X < result.X) { result.X = p.X; }
            if(p.Y < result.Y) { result.Y = p.Y; }
            if(p.Z < result.Z) { result.Z = p.Z; }

            return result;
        }

        public static Vector3 GetMaximum(this Vector3 v, Vector3 p)
        {
            var result = new Vector3(v);

            if (p.X > result.X) { result.X = p.X; }
            if (p.Y > result.Y) { result.Y = p.Y; }
            if (p.Z > result.Z) { result.Z = p.Z; }

            return result;
        }

        public static Box3 GetBound(this Vector3[] v)
        {
            var min = v[0];
            var max = v[0];

            for (int i = 1; i < v.Length; i++)
            {
                min = min.GetMinimum(v[i]);
                max = max.GetMaximum(v[i]);
            }

            return new Box3(min, max);
        }
    }
}
