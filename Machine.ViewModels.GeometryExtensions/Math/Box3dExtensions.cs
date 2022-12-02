using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machine.ViewModels.GeometryExtensions.Math
{
    internal static class Box3dExtensions
    {
        public static bool Intersects(this Box3 box, ref Ray ray, out Vector3 point)
        {
            return Collision.RayIntersectsBox(ref ray, ref box, out point);
        }
    }
}
