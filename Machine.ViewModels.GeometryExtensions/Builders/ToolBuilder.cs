using Machine.Data.Interfaces.Tools;
using Machine.ViewModels.Interfaces.Bridge;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MVMIM = Machine.ViewModels.Interfaces.MachineElements;
using SMath = System.Math;


namespace Machine.ViewModels.GeometryExtensions.Builders
{
    public static class ToolBuilder
    {
        public static void Build(MVMIM.IMachineElement tool, out Vector3[] vertexes, out uint[] indexes, out Vector3[] normals)
        {
            var t = tool as IToolDataProxy;

            if (t.Tool is ICountersinkTool cst) Build(cst, out vertexes, out indexes, out normals);
            else if (t.Tool is IDiskOnConeTool dct) Build(dct, out vertexes, out indexes, out normals);
            else if (t.Tool is IDiskTool dt) Build(dt, out vertexes, out indexes, out normals);
            else if (t.Tool is IPointedTool pt) Build(pt, out vertexes, out indexes, out normals);
            else if (t.Tool is ISimpleTool st) Build(st, out vertexes, out indexes, out normals);
            else if (t.Tool is ITwoSectionTool tst) Build(tst, out vertexes, out indexes, out normals);
            else throw new NotImplementedException();
        }

        private static void Build(ITwoSectionTool t, out Vector3[] vertexes, out uint[] indexes, out Vector3[] normals)
        {
            var builder = new MeshBuilder();

            builder.AddCylinder(new Vector3(),
                                new Vector3(0.0f, 0.0f, -(float)t.Length1),
                                t.Diameter1 / 2.0);
            builder.AddCylinder(new Vector3(0.0f, 0.0f, -(float)t.Length1),
                                new Vector3(0.0f, 0.0f, -(float)(t.Length1 + t.Length2)),
                                t.Diameter2 / 2.0);

            builder.ToMesh(out vertexes, out indexes, out normals);
         }

        private static void Build(ISimpleTool t, out Vector3[] vertexes, out uint[] indexes, out Vector3[] normals)
        {
            var builder = new MeshBuilder();

            builder.AddCylinder(new Vector3(), new Vector3(0, 0, -(float)(t.Length)), t.Diameter / 2.0);
            builder.ToMesh(out vertexes, out indexes, out normals);
        }

        private static void Build(IPointedTool t, out Vector3[] vertexes, out uint[] indexes, out Vector3[] normals)
        {
            var builder = new MeshBuilder();

            builder.AddCylinder(new Vector3(),
                                new Vector3(0.0f, 0.0f, -(float)t.StraightLength),
                                t.Diameter / 2.0);
            builder.AddCone(new Vector3(0.0f, 0.0f, -(float)t.StraightLength),
                            new Vector3(0.0f, 0.0f, -(float)(t.StraightLength + t.ConeHeight)),
                            t.Diameter / 2.0,
                            false,
                            20);

            builder.ToMesh(out vertexes, out indexes, out normals);
        }

        private static void Build(IDiskTool t, out Vector3[] vertexes, out uint[] indexes, out Vector3[] normals)
        {
            var builder = new MeshBuilder();
            var d = SMath.Abs(t.BodyThickness - t.CuttingThickness) / 2.0;
            var r1 = t.Diameter / 2.0 - t.CuttingRadialThickness;
            var profile = new[]
            {
                new Vector2d(0.0f, 10.0f),
                new Vector2d(0.0f, r1),
                new Vector2d((- d), r1),
                new Vector2d((- d), (t.Diameter / 2.0)),
                new Vector2d((t.BodyThickness + d), (t.Diameter / 2.0)),
                new Vector2d((t.BodyThickness + d), r1),
                new Vector2d(t.BodyThickness, r1),
                new Vector2d(t.BodyThickness, 10.0f)
            };

            builder.AddRevolvedGeometry(profile.ToList(),
                                        new Vector3(),
                                        new Vector3(0.0f, 0.0f, -1.0f),
                                        100);

            builder.ToMesh(out vertexes, out indexes, out normals);
        }

        private static void Build(IDiskOnConeTool t, out Vector3[] vertexes, out uint[] indexes, out Vector3[] normals)
        {
            var builder = new MeshBuilder();
            var d = SMath.Abs(t.BodyThickness - t.CuttingThickness) / 2.0;
            var r1 = t.Diameter / 2.0 - t.CuttingRadialThickness;
            var p1 = new Vector3(0.0f, 0.0f, -(float)t.PostponemntLength);
            var profile = new[]
            {
                new Vector2d(0.0f, (t.PostponemntDiameter / 2.0)),
                new Vector2d(0.0f, r1),
                new Vector2d((-d), r1),
                new Vector2d((-d), (t.Diameter / 2.0)),
                new Vector2d((t.BodyThickness + d),(t.Diameter / 2.0)),
                new Vector2d((t.BodyThickness + d), r1),
                new Vector2d(t.BodyThickness, r1),
                new Vector2d(t.BodyThickness,(t.PostponemntDiameter / 2.0)),

            };

            builder.AddRevolvedGeometry(profile.ToList(),
                                        p1,
                                        new Vector3(0.0f, 0.0f, -1.0f),
                                        100);
            builder.AddCylinder(new Vector3(),
                                p1,
                                t.PostponemntDiameter,
                                20);

            builder.ToMesh(out vertexes, out indexes, out normals);
        }

        private static void Build(ICountersinkTool t, out Vector3[] vertexes, out uint[] indexes, out Vector3[] normals)
        {
            const double hSvasatore = 10.0;
            var builder = new MeshBuilder();
            var p1 = new Vector3(0.0f, 0.0f, -(float)(t.Length1 - hSvasatore));
            var p12 = new Vector3(0.0f, 0.0f, -(float)t.Length1);
            var p2 = new Vector3(0.0f, 0.0f, -(float)(t.Length1 + t.Length2));
            var p3 = new Vector3(0.0f, 0.0f, -(float)(t.Length1 + t.Length2 + t.Length3));

            builder.AddCylinder(new Vector3(),
                                p1,
                                t.Diameter1 / 2.0);
            builder.AddCylinder(p1,
                                p12,
                                t.Diameter2 / 2.0);
            builder.AddCone(p12,
                            new Vector3(0.0f, 0.0f, -1.0f),
                            t.Diameter2 / 2.0,
                            t.Diameter1 / 2.0,
                            t.Length2,
                            false,
                            false,
                            20);
            builder.AddCylinder(p2,
                                p3,
                                t.Diameter1 / 2.0);

            builder.ToMesh(out vertexes, out indexes, out normals);
        }
    }

}
