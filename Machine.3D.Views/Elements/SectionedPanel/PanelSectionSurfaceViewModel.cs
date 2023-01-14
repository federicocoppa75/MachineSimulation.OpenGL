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
        IMeshProvider _meshProvider;
        private bool disposedValue;

        public bool IsChanged => _meshProvider.IsChanged;
        public int Id { get; private set; }

        private PanelSectionSurfaceViewModel()
        {
        }

        public static PanelSectionSurfaceViewModel Create(ISectionElement se)
        {
            var mp = se as IMeshProvider;

            if (mp != null)
            {
                return new PanelSectionSurfaceViewModel() { Id = se.Id, _meshProvider = mp };
            }
            else
                throw new ArgumentException();
        }

        public void GetMesh(out Vector3[] points, out uint[] indexes, out Vector3[] normals) => _meshProvider.GetMesh(out points, out indexes, out normals);

        #region IDispose implementation
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                    (_meshProvider as IDisposable)?.Dispose();
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
