using Machine.ViewModels.GeometryExtensions.Math;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Point3D = OpenTK.Mathematics.Vector3;
using Vector3D = OpenTK.Mathematics.Vector3;

namespace Machine.ViewModels.GeometryExtensions.Helpers
{
    abstract class BaseColliderHelper
    {
        public bool Intersect(out double distance)
        {
            var result = false;
            var matrix1 = GetColliderChainTransformation();
            var matrix2 = GetPanelChainTransformation();
            var colliderPoints = GetColliderPoints();
            var colliderDirection = GetColliderDirection();
            var panelCenter = new Point3D();
            var panelSize = GetPanelSize();

            distance = 0.0;
            colliderPoints = matrix1.Transform(colliderPoints);
            colliderDirection = matrix1.TransformDirection(colliderDirection);
            panelCenter = matrix2.Transform(panelCenter);
            var panel = GetPanel(panelCenter, panelSize);

            foreach (var item in colliderPoints)
            {
                var ray = new Ray(item, colliderDirection);

                if (panel.Intersects(ref ray, out Vector3 v))
                {
                    // a volte Intersect restituisce la posizione del collider invece della posizione dell'intersezione,
                    // va calcolato il punto di intersezione in altro modo
                    if ((ray.Position - v).IsZero()) v = GetPanelIntersection(ref panel, ref ray);

                    distance = GetDistance(v - item, colliderDirection);
                    result = true;
                    break;
                }
            }

            return result;
        }

        private Vector3 GetPanelIntersection(ref Box3 panel, ref Ray ray)
        {
            var size = new Vector3(panel.Size.X / 2.0f, panel.Size.Y / 2.0f, panel.Size.Z / 2.0f);
            var s = size * ray.Direction;
            var v = new Vector3();

            v.X = (ray.Direction.X == 0.0f) ? ray.Position.X : panel.Center.X - s.X;
            v.Y = (ray.Direction.Y == 0.0f) ? ray.Position.Y : panel.Center.Y - s.Y;
            v.Z = (ray.Direction.Z == 0.0f) ? ray.Position.Z : panel.Center.Z - s.Z;

            return v;
        }

        private Box3 GetPanel(Point3D panelCenter, Vector3 panelSize)
        {
            var v1 = new Vector3D(-panelSize.X / 2.0f, -panelSize.Y / 2.0f, -panelSize.Z / 2.0f);
            var v2 = new Vector3D(panelSize.X / 2.0f, panelSize.Y / 2.0f, panelSize.Z / 2.0f);
            var v = panelCenter - new Point3D();
            var min = v + v1;
            var max = v + v2;

            return new Box3(min, max);
        }
        protected abstract Matrix4 GetColliderChainTransformation();
        protected abstract Matrix4 GetPanelChainTransformation();
        protected abstract Point3D[] GetColliderPoints();
        protected abstract Vector3D GetColliderDirection();
        protected abstract Vector3 GetPanelSize();
        protected double GetDistance(Vector3D v, Vector3D direction)
        {
            if (direction.X != 0.0) return v.X;
            else if (direction.Y != 0.0) return v.Y;
            else if (direction.Z != 0.0) return v.Z;
            else throw new ArgumentException();
        }
    }
}
