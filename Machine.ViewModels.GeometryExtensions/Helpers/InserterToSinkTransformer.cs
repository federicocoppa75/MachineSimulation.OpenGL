using Machine.ViewModels.GeometryExtensions.Math;
using Machine.ViewModels.Interfaces.Insertions;
using Machine.ViewModels.Interfaces.MachineElements;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using MDB = Machine.Data.Base;

namespace Machine.ViewModels.GeometryExtensions.Helpers
{
    internal class InserterToSinkTransformer : IInserterToSinkTransformer
    {
        IInsertionsSink _sink;
        IInjectorElement _injector;

        public InserterToSinkTransformer(IInsertionsSink sink, IInjectorElement injector)
        {
            _sink = sink;
            _injector = injector;
        }

        public InsertPosition Transform()
        {
            var matrix1 = _sink.GetChainTransformation();

            matrix1.Invert();

            GetInsertPositionAndDirection(matrix1, out Vector3 p, out Vector3 d);

            return new InsertPosition()
            {
                Position = new MDB.Point() { X = p.X, Y = p.Y, Z = p.Z },
                Direction = new MDB.Vector() { X = d.X, Y = d.Y, Z = d.Z },
            };
        }

        private void GetInsertPositionAndDirection(Matrix4 panelMatrix, out Vector3 position, out Vector3 direction)
        {
            var matrix = _injector.GetChainTransformation();
            var p0 = new Vector3((float)_injector.Position.X, (float)_injector.Position.Y, (float)_injector.Position.Z);
            var d0 = new Vector3((float)_injector.Direction.X, (float)_injector.Direction.Y, (float)_injector.Direction.Z);
            var p1 = matrix.Transform(p0);
            var d1 = matrix.TransformDirection(d0);
            var p2 = panelMatrix.Transform(p1);
            var d2 = panelMatrix.TransformDirection(d1);

            position = p2;
            direction = d2;
        }

    }
}
