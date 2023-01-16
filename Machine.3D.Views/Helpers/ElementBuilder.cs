using Machine._3D.Views.Geometries;
using Machine.ViewModels.Interfaces.Insertions;
using Machine.ViewModels.Interfaces.MachineElements;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MVMGEB = Machine.ViewModels.GeometryExtensions.Builders;

namespace Machine._3D.Views.Helpers
{
    internal static class ElementBuilder
    {
        public static Vertex[] BuildVertexes(Vector3[] points, Vector3[] normals)
        {
            Vertex[] vertexes = new Vertex[points.Count()];
            for (int i = 0; i < points.Count(); i++)
            {
                var p = points[i];
                var n = normals[i];

                vertexes[i] = new Vertex(p.X, p.Y, p.Z, n.X, n.Y, n.Z);
            }

            return vertexes;
        }

        public static void Build(IPanelElement panel, out Vertex[] vertexes, out uint[] indexes)
        {
            MVMGEB.ElementBuilder.Build(panel, out Vector3[] points, out indexes, out Vector3[] normals);
            vertexes = BuildVertexes(points, normals);
        }

        public static void Build(IMachineElement tool, out Vertex[] vertexes, out uint[] indexes)
        {
            MVMGEB.ToolBuilder.Build(tool, out Vector3[] points, out indexes, out Vector3[] normals);
            vertexes = BuildVertexes(points, normals);
        }

        public static void Build(IInjectedObject obj, out Vertex[] vertexes, out uint[] indexes)
        {
            MVMGEB.ElementBuilder.Build(obj, out Vector3[] points, out indexes, out Vector3[] normals);
            vertexes = BuildVertexes(points, normals);
        }

        public static float[] BuildGradientValues(Vector3[] points, Vector3 gradient)
        {
            var s = new float[points.Length];
            var g = new float[points.Length];
            float min = 0, max = 0;

            for (int i = 0; i < points.Length; i++)
            {
                s[i] =  Vector3.Dot(points[i], gradient);

                if(i == 0)
                {
                    min = s[i];
                    max = s[i];
                }
                else
                {
                    if (s[i] < min) min = s[i];
                    if (s[i] > max) max = s[i];
                }
            }

            var d = max - min;

            for (int i = 0; i < s.Length; i++)
            {
                g[i] = (s[i] - min) / d;
            }

            return g;
        }
    }
}
