using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machine.ViewModels.GeometryExtensions.Math
{
    internal static class Vector3Extensions
    {
        public static bool IsZero(this Vector3 v)
        {
            if (v.X == 0f && v.Y == 0f)
            {
                return v.Z == 0f;
            }

            return false;
            
        }
    }
}
