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
    }
}
