using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machine.ViewModels.GeometryExtensions.Math
{
    public static class Box3dExtensions
    {
        public static bool Intersects(this Box3 box, ref Ray ray, out Vector3 point)
        {
            return Collision.RayIntersectsBox(ref ray, ref box, out point);
        }

        public static Vector3[] GetVertexes(this Box3 box) 
        {
            var hSize = box.Size / 2;
            var c = box.Center;

            return new Vector3[] 
            {
                c + new Vector3(-hSize.X, -hSize.Y, -hSize.Z),
                c + new Vector3(-hSize.X,  hSize.Y, -hSize.Z),
                c + new Vector3( hSize.X,  hSize.Y, -hSize.Z),
                c + new Vector3( hSize.X, -hSize.Y, -hSize.Z),
                c + new Vector3(-hSize.X, -hSize.Y,  hSize.Z),
                c + new Vector3(-hSize.X,  hSize.Y,  hSize.Z),
                c + new Vector3( hSize.X,  hSize.Y,  hSize.Z),
                c + new Vector3( hSize.X, -hSize.Y,  hSize.Z)
            };
        }

        public static Box3 Add(this Box3 box, Vector3 point)
        {
            if(box.Contains(point))
            {
                return box;
            }
            else
            {
                var min = box.Min.GetMinimum(point);
                var max = box.Max.GetMaximum(point);

                return new Box3(min, max);
            }
        }

        public static Box3 Add(this Box3 box, Box3 other)
        {
            if(box.Contains(other))
            {
                return box;
            }
            else
            {
                var min = box.Min.GetMinimum(other.Min);
                var max = box.Max.GetMaximum(other.Max);

                return new Box3(min, max);
            }
        }
    }
}
