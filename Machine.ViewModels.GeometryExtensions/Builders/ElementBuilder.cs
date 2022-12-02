using Machine.ViewModels.Interfaces.Insertions;
using Machine.ViewModels.Interfaces.MachineElements;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machine.ViewModels.GeometryExtensions.Builders
{
    public static class ElementBuilder
    {
        public static void Build(IPanelElement panel, out Vector3[] vertexes, out uint[] indexes, out Vector3[] normals)
        {
            var builder = new MeshBuilder();

            builder.AddBox(new Vector3d(), panel.SizeX, panel.SizeY, panel.SizeZ);

            builder.ToMesh(out vertexes, out indexes, out normals);
        }

        public static void Build(IInjectedObject obj, out Vector3[] vertexes, out uint[] indexes, out Vector3[] normals)
        {
            if (obj is IInsertedObject insObj)
            {
                var builder = new MeshBuilder();
                var p1 = new Vector3((float)insObj.Position.X, (float)insObj.Position.Y, (float)insObj.Position.Z);
                var d = new Vector3((float)insObj.Direction.X, (float)insObj.Direction.Y, (float)insObj.Direction.Z);
                var p2 = p1 + d * (float)(insObj.Length);

                builder.AddCylinder(p1, p2, insObj.Diameter / 2.0);

                builder.ToMesh(out vertexes, out indexes, out normals);
            }
            else
            {
                var builder = new MeshBuilder();
                var p1 = new Vector3((float)obj.Position.X, (float)obj.Position.Y, (float)obj.Position.Z);
                var d = new Vector3((float)obj.Direction.X, (float)obj.Direction.Y, (float)obj.Direction.Z);
                var p2 = p1 + d * 20;

                builder.AddCone(p2, p1, 4.0, true, 20);

                builder.ToMesh(out vertexes, out indexes, out normals);
            }
        }
    }
}
