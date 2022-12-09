using Machine._3D.Views.Programs;
using Machine.ViewModels.GeometryExtensions.Interfaces;
using Machine.ViewModels.Interfaces.MachineElements;
using MaterialRemove.Interfaces;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using M3DVG = Machine._3D.Views.Geometries;
using M3DVH = Machine._3D.Views.Helpers;

namespace Machine._3D.Views.Elements
{
    class PanelSectionSurfaceViewModel : IDisposable
    {
        Vector3 _center;
        IMeshProvider _meshProvider;
        private bool disposedValue;

        public bool IsVisible => Geometry != null;

        public bool IsChanged => _meshProvider.IsChanged;

        public Vector3 Center => _center;

        public M3DVG.Mesh Geometry { get; private set; }

        private PanelSectionSurfaceViewModel()
        {

        }

        public static PanelSectionSurfaceViewModel Create(ISectionElement se)
        {
            var mp = se as IMeshProvider;

            if (mp != null)
            {
                var psvm = new PanelSectionSurfaceViewModel()
                {
                    _meshProvider = mp,
                    _center = new Vector3((float)se.CenterX, (float)se.CenterY, (float)se.CenterZ)
                };


                return psvm;
            }
            else
                throw new ArgumentException();
        }

        public void GetMesh(out Vector3[] points, out uint[] indexes, out Vector3[] normals) => _meshProvider.GetMesh(out points, out indexes, out normals);

        protected void UpdateGeometry(BaseProgram program)
        {
            _meshProvider.GetMesh(out Vector3[] points, out uint[] indexes, out Vector3[] normals);
            var vertexes = ToVertexes(points, normals);
            Geometry = new M3DVG.Mesh(vertexes, indexes, program);
        }

        private static M3DVG.Vertex ToVertex(ref Vector3 point, ref Vector3 normal) => new M3DVG.Vertex(point.X, point.Y, point.Z, normal.X, normal.Y, normal.Z);

        private static M3DVG.Vertex[] ToVertexes(Vector3[] points, Vector3[] normals)
        {
            var v = new M3DVG.Vertex[points.Count()];

            for (int i = 0; i < v.Count(); i++) v[i] = ToVertex(ref points[i], ref normals[i]);

            return v;
        }

        #region IDispose implementation
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                    Geometry?.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
