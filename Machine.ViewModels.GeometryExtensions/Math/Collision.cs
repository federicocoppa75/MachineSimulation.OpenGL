using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machine.ViewModels.GeometryExtensions.Math
{
    internal static class Collision
    {
        public static bool RayIntersectsBox(ref Ray ray, ref Box3 box, out Vector3 point)
        {
            if (!RayIntersectsBox(ref ray, ref box, out float distance))
            {
                point = Vector3.Zero;
                return false;
            }

            point = ray.Position + ray.Direction * distance;
            return true;
        }

        public static bool RayIntersectsBox(ref Ray ray, ref Box3 box, out float distance)
        {
            distance = 0f;
            float num = float.MaxValue;
            if (Utils.IsZero(ray.Direction.X))
            {
                if (ray.Position.X < box.Min.X || ray.Position.X > box.Max.X)
                {
                    distance = 0f;
                    return false;
                }
            }
            else
            {
                float num2 = 1f / ray.Direction.X;
                float num3 = (box.Min.X - ray.Position.X) * num2;
                float num4 = (box.Max.X - ray.Position.X) * num2;
                if (num3 > num4)
                {
                    float num5 = num3;
                    num3 = num4;
                    num4 = num5;
                }

                distance = System.Math.Max(num3, distance);
                num = System.Math.Min(num4, num);
                if (distance > num)
                {
                    distance = 0f;
                    return false;
                }
            }

            if (Utils.IsZero(ray.Direction.Y))
            {
                if (ray.Position.Y < box.Min.Y || ray.Position.Y > box.Max.Y)
                {
                    distance = 0f;
                    return false;
                }
            }
            else
            {
                float num6 = 1f / ray.Direction.Y;
                float num7 = (box.Min.Y - ray.Position.Y) * num6;
                float num8 = (box.Max.Y - ray.Position.Y) * num6;
                if (num7 > num8)
                {
                    float num9 = num7;
                    num7 = num8;
                    num8 = num9;
                }

                distance = System.Math.Max(num7, distance);
                num = System.Math.Min(num8, num);
                if (distance > num)
                {
                    distance = 0f;
                    return false;
                }
            }

            if (Utils.IsZero(ray.Direction.Z))
            {
                if (ray.Position.Z < box.Min.Z || ray.Position.Z > box.Max.Z)
                {
                    distance = 0f;
                    return false;
                }
            }
            else
            {
                float num10 = 1f / ray.Direction.Z;
                float num11 = (box.Min.Z - ray.Position.Z) * num10;
                float num12 = (box.Max.Z - ray.Position.Z) * num10;
                if (num11 > num12)
                {
                    float num13 = num11;
                    num11 = num12;
                    num12 = num13;
                }

                distance = System.Math.Max(num11, distance);
                num = System.Math.Min(num12, num);
                if (distance > num)
                {
                    distance = 0f;
                    return false;
                }
            }

            return true;
        }


    }
}
