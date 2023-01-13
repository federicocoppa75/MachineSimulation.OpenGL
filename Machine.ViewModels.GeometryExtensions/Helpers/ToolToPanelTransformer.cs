using Machine.ViewModels.GeometryExtensions.Math;
using Machine.ViewModels.Interfaces.MachineElements;
using Machine.ViewModels.Interfaces.Tools;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using MDB = Machine.Data.Base;
using MDIT = Machine.Data.Interfaces.Tools;
using MRI = MaterialRemove.Interfaces;
using SMath = System.Math;
using MVMIB = Machine.ViewModels.Interfaces.Bridge;


namespace Machine.ViewModels.GeometryExtensions.Helpers
{
    internal class ToolToPanelTransformer : IToolToPanelTransformer
    {
        const int _nSections = 24;
        IPanelElement _panel;
        IEnumerable<IToolElement> _tools;

        public ToolToPanelTransformer(IPanelElement panel, IEnumerable<IToolElement> tools)
        {
            _panel = panel;
            _tools = tools;
        }

        public IList<ToolPosition> Transform()
        {
            var positions = new List<ToolPosition>();
            var matrix1 = _panel.GetChainTransformation();

            matrix1.Invert();

            foreach (var tool in _tools)
            {
                GetToolPositionAndDirection(tool, matrix1, out Vector3 tp, out Vector3 d2);

                positions.Add(new ToolPosition()
                {
                    Point = new MDB.Point() { X = tp.X, Y = tp.Y, Z = tp.Z },
                    Direction = new MDB.Vector() { X = d2.X, Y = d2.Y, Z = d2.Z },
                    Radius = tool.WorkRadius,
                    Length = tool.UsefulLength
                });

            }

            return positions;
        }

        public void TransformAndApplay()
        {
            var matrix1 = _panel.GetChainTransformation();

            matrix1.Invert();

            foreach (var tool in _tools)
            {
                GetToolPositionAndDirection(tool, matrix1, out Vector3 tp, out Vector3 d2);
                ApplayTool(tool, tp, d2);
            }
        }

        public Task<int> TransformAndApplayAsync()
        {
            return Task.Run(async () =>
            {
                var matrix1 = _panel.GetChainTransformation();
                var tasks = new List<Task>();

                matrix1.Invert();

                foreach (var tool in _tools)
                {
                    tasks.Add(Task.Run(async () =>
                    {
                        GetToolPositionAndDirection(tool, matrix1, out Vector3 tp, out Vector3 d2);
                        await ApplayToolAsync(tool, tp, d2);
                    }));
                }

                await Task.WhenAll(tasks);

                return 0;
            });
        }

        public Task<IList<ToolPosition>> TransformAsync()
        {
            throw new NotImplementedException();
        }

        private void GetToolPositionAndDirection(IToolElement tool, Matrix4 panelMatrix, out Vector3 position, out Vector3 direction)
        {
            var matrix = tool.GetChainTransformation();
            var p1 = matrix.Transform(new Vector3());
            var d1 = matrix.TransformDirection(new Vector3(0, 0, -1));
            var p2 = panelMatrix.Transform(p1);
            var d2 = panelMatrix.TransformDirection(d1);

            position = p2 + d2 * (float)tool.WorkLength;
            direction = d2;
        }

        private Task<int> ApplayToolAsync(IToolElement tool, Vector3 position, Vector3 direction)
        {
            var t = (tool as MVMIB.IToolDataProxy).Tool;

            //if (t is MDT.DiskTool dt) return ApplyToolAsync(dt, position, direction);
            //else if (t is MDT.DiskOnConeTool doct) return ApplyToolAsync(doct, position, direction);
            if (t is MDIT.IPointedTool pt) return ApplyToolAsync(pt, position, direction);
            else if (t is MDIT.ICountersinkTool ct) return ApplyToolAsync(ct, position, direction);
            else if (t is MDIT.IDiskOnConeTool doc) return ApplyToolAsync(doc, position, direction);
            else return ApplyToolAsync(t, position, direction);
        }

        private void ApplayTool(IToolElement tool, Vector3 position, Vector3 direction)
        {
            var t = (tool as MVMIB.IToolDataProxy).Tool;

            //if (t is MDT.DiskTool dt) ApplyTool(dt, position, direction);
            //else if (t is MDT.DiskOnConeTool doct) ApplyTool(doct, position, direction);
            if (t is MDIT.IPointedTool pt) ApplyTool(pt, position, direction);
            else if (t is MDIT.ICountersinkTool ct) ApplyTool(ct, position, direction);
            else ApplyTool(t, position, direction);
        }

        private void ApplyTool(MDIT.IPointedTool t, Vector3 position, Vector3 direction)
        {
            var tca = new MRI.ToolConeActionData()
            {
                X = (float)position.X,
                Y = (float)position.Y,
                Z = (float)position.Z,
                Orientation = ToOrientation(direction),
                Length = (float)t.ConeHeight,
                MaxRadius = (float)t.Diameter / 2.0f,
                MinRadius = 0.0f
            };

            if (_panel is MRI.IPanel panel) panel.ApplyAction(tca);
        }

        private void ApplyTool(MDIT.ICountersinkTool t, Vector3 position, Vector3 direction)
        {
            var position2 = position - direction * (float)t.Length3;

            var ta = new MRI.ToolActionData()
            {
                X = (float)position.X,
                Y = (float)position.Y,
                Z = (float)position.Z,
                Orientation = ToOrientation(direction),
                Length = (float)t.GetTotalLength(),
                Radius = (float)t.Diameter1 / 2.0f,
            };
            var tca = new MRI.ToolConeActionData()
            {
                X = (float)position2.X,
                Y = (float)position2.Y,
                Z = (float)position2.Z,
                Orientation = ToOrientation(direction),
                Length = (float)t.Length2,
                MaxRadius = (float)t.Diameter2 / 2.0f,
                MinRadius = (float)t.Diameter1 / 2.0f
            };

            if (_panel is MRI.IPanel panel)
            {
                panel.ApplyAction(ta);
                panel.ApplyAction(tca);
            }
        }

        private void ApplyTool(MDIT.ITool t, Vector3 position, Vector3 direction)
        {
            var ta = new MRI.ToolActionData()
            {
                X = (float)position.X,
                Y = (float)position.Y,
                Z = (float)position.Z,
                Orientation = ToOrientation(direction),
                Length = (float)t.GetTotalLength(),
                Radius = (float)t.GetTotalDiameter() / 2.0f
            };

            if (_panel is MRI.IPanel panel) panel.ApplyAction(ta);
        }

        private Task<int> ApplyToolAsync(MDIT.ITool t, Vector3 position, Vector3 direction)
        {
            return Task.Run(async () =>
            {
                var ta = new MRI.ToolActionData()
                {
                    X = (float)position.X,
                    Y = (float)position.Y,
                    Z = (float)position.Z,
                    Orientation = ToOrientation(direction),
                    Length = (float)t.GetTotalLength(),
                    Radius = (float)t.GetTotalDiameter() / 2.0f
                };

                if (_panel is MRI.IPanel panel) await panel.ApplyActionAsync(ta);

                return 0;
            });
        }

        private Task<int> ApplyToolAsync(MDIT.IDiskTool dt, Vector3 position, Vector3 direction)
        {
            return Task.Run(async () =>
            {
                var radius = dt.Diameter / 2.0;
                var sa = 360.0 / _nSections;                                // ampiezza angolare delle sezioni
                var sh = dt.CuttingRadialThickness;                         // altezza sezione;
                var sw = radius * sa * (SMath.PI * 2.0) / 360.0;             // larghezza sezione
                var sl = dt.CuttingThickness;                               // linghezza sezione
                var orientation = ToOrientation(direction);
                var n = direction;
                var r = GetRadial(orientation);
                var p = new Vector3(position.X,
                                    position.Y,
                                    position.Z) - n * (float)(sl / 2.0);
                var tasks = new List<Task>();

                for (int i = 0; i < _nSections; i++)
                {
                    var matrix = Matrix4.CreateFromAxisAngle(n, (float)(sa * i));
                    var radial = matrix.Transform(r);
                    var sc = p + radial * (float)(radius - sh / 2.0);
                    var tsad = new MRI.ToolSectionActionData()
                    {
                        PX = (float)sc.X,
                        PY = (float)sc.Y,
                        PZ = (float)sc.Z,
                        DX = (float)radial.X,
                        DY = (float)radial.Y,
                        DZ = (float)radial.Z,
                        L = (float)sl,
                        W = (float)sw,
                        H = (float)sh,
                        FixBaseAx = orientation
                    };

                    if (_panel is MRI.IPanel panel) tasks.Add(panel.ApplyActionAsync(tsad));
                }

                await Task.WhenAll(tasks);

                return 0;
            });
        }

        // private Task<int> ApplyToolAsync(MDT.DiskOnConeTool doct, Point3D position, Vector3D direction) => ApplyToolAsync(doct as MDT.DiskTool, position, direction);
        private Task<int> ApplyToolAsync(MDIT.IDiskOnConeTool doct, Vector3 position, Vector3 direction)
        {
            return Task.Run(async () =>
            {
                var ta = new MRI.ToolActionData()
                {
                    X = (float)position.X,
                    Y = (float)position.Y,
                    Z = (float)position.Z,
                    Orientation = ToOrientation(direction),
                    Length = (float)doct.CuttingThickness,
                    Radius = (float)doct.Diameter / 2.0f
                };

                if (_panel is MRI.IPanel panel) await panel.ApplyActionAsync(ta);

                return 0;
            });
        }

        private Task<int> ApplyToolAsync(MDIT.IPointedTool pt, Vector3 position, Vector3 direction)
        {
            return Task.Run(async () =>
            {
                var tca = new MRI.ToolConeActionData()
                {
                    X = (float)position.X,
                    Y = (float)position.Y,
                    Z = (float)position.Z,
                    Orientation = ToOrientation(direction),
                    Length = (float)pt.ConeHeight,
                    MaxRadius = (float)pt.Diameter / 2.0f,
                    MinRadius = 0.0f
                };

                if (_panel is MRI.IPanel panel) await panel.ApplyActionAsync(tca);

                return 0;
            });
        }

        private Task<int> ApplyToolAsync(MDIT.ICountersinkTool ct, Vector3 position, Vector3 direction)
        {
            return Task.Run(async () =>
            {
                var position2 = position - direction * (float)ct.Length3;

                var ta = new MRI.ToolActionData()
                {
                    X = (float)position.X,
                    Y = (float)position.Y,
                    Z = (float)position.Z,
                    Orientation = ToOrientation(direction),
                    Length = (float)ct.GetTotalLength(),
                    Radius = (float)ct.Diameter1 / 2.0f,
                };
                var tca = new MRI.ToolConeActionData()
                {
                    X = (float)position2.X,
                    Y = (float)position2.Y,
                    Z = (float)position2.Z,
                    Orientation = ToOrientation(direction),
                    Length = (float)ct.Length2,
                    MaxRadius = (float)ct.Diameter2 / 2.0f,
                    MinRadius = (float)ct.Diameter1 / 2.0f
                };

                if (_panel is MRI.IPanel panel)
                {
                    var tasks = new Task[]
                    {
                        panel.ApplyActionAsync(ta),
                        panel.ApplyActionAsync(tca)
                    };

                    await Task.WhenAll(tasks);                    
                }

                return 0;
            });
        }


        private static MRI.Orientation ToOrientation(Vector3 direction)
        {
            var xIsNull = IsNull(direction.X);
            var yIsNull = IsNull(direction.Y);
            var zIsNull = IsNull(direction.Z);

            if (xIsNull && yIsNull && !zIsNull)
            {
                return (direction.Z > 0.0) ? MRI.Orientation.ZPos : MRI.Orientation.ZNeg;
            }
            else if (xIsNull && !yIsNull && zIsNull)
            {
                return (direction.Y > 0.0) ? MRI.Orientation.YPos : MRI.Orientation.YNeg;
            }
            else if (!xIsNull && yIsNull && zIsNull)
            {
                return (direction.X > 0.0) ? MRI.Orientation.XPos : MRI.Orientation.XNeg;
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private static Vector3 GetRadial(MRI.Orientation direction)
        {
            switch (direction)
            {
                case MRI.Orientation.XPos:
                case MRI.Orientation.XNeg:
                    return new Vector3(0, 0, 1);
                case MRI.Orientation.YPos:
                case MRI.Orientation.YNeg:
                    return new Vector3(1, 0, 0);
                case MRI.Orientation.ZPos:
                case MRI.Orientation.ZNeg:
                    return new Vector3(1, 0, 0);
                default:
                    throw new NotImplementedException();
            }
        }

        private static bool IsNull(double value, double tolerance = 0.001) => (value < tolerance) && (value > -tolerance);
    }
}
