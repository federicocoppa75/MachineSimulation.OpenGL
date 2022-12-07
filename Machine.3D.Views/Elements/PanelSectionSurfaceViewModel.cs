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

        public M3DVG.Mesh Geometry { get; private set; }

        private PanelSectionSurfaceViewModel()
        {

        }

        public static PanelSectionSurfaceViewModel Create(ISectionElement se)
        {
            var mp = se as IMeshProvider;

            if (mp != null)
            {
                //if (mp.IsChanged)
                //{
                //    mp.GetMesh(out Vector3[] points, out uint[] indexes, out Vector3[] normals);
                //    var vertexes = ToVertexes(points, normals);
                //    geometry = new Mesh(vertexes, indexes, program);
                //}

                var psvm = new PanelSectionSurfaceViewModel()
                {
                    _meshProvider = mp,
                    //Element = ps as IMachineElement,
                    _center = new Vector3((float)se.CenterX, (float)se.CenterY, (float)se.CenterZ)
                };


                return psvm;
            }
            else
                throw new ArgumentException();
        }

        public void Draw(BaseProgram program, Matrix4 projection, Matrix4 view, Matrix4 panelModel)
        {
            if(_meshProvider.IsChanged)
            {
                M3DVH.MaterialHelper.SetMaterial(program, new Data.Base.Color() { R = 253, G = 131, B = 0, A = 255 });
                var model = Matrix4.CreateTranslation(_center) * panelModel;

                program.ModelViewProjectionMatrix.Set(model * view * projection);

                var g = Geometry;

                UpdateGeometry(program);

                Geometry.Draw(program);

                g?.Dispose();
            }
            else if(Geometry != null)
            {
                M3DVH.MaterialHelper.SetMaterial(program, new Data.Base.Color() { R = 253, G = 131, B = 0, A = 255 });
                var model = Matrix4.CreateTranslation(_center) * panelModel;
                program.ModelViewProjectionMatrix.Set(model * view * projection);
                Geometry.Draw(program);
            }
        }

        private Matrix4 GetChainTransformation()
        {
            throw new NotImplementedException();
        }

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
