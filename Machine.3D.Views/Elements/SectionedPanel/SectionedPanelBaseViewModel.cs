using Machine._3D.Views.Programs;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MRI = MaterialRemove.Interfaces;
using M3DVH = Machine._3D.Views.Helpers;
using M3DVG = Machine._3D.Views.Geometries;
using Machine.ViewModels.Interfaces.MachineElements;
using Machine.ViewModels.UI;
using System.Threading;
using Machine._3D.Views.Interfaces;
using MVMGEM = Machine.ViewModels.GeometryExtensions.Materials;
using MaterialRemove.Interfaces;
using Assimp;
using System.Collections.Concurrent;

namespace Machine._3D.Views.Elements.SectionedPanel
{
    internal abstract class SectionedPanelViewModel : ElementViewModel, IDisposable
    {
        struct RawMesh
        {
            public Vector3[] points;
            public Vector3[] normals;
            public uint[] indexes;
        }

        int _processing;
        M3DVG.Mesh _panelMesh;
        private bool disposedValue;

        private ConcurrentDictionary<int, PanelSectionSurfaceViewModel> _sectionSurfaces = new ConcurrentDictionary<int, PanelSectionSurfaceViewModel>();

        private IPanelMaterials _panelMaterials;
        public IPanelMaterials PanelMaterials =>  _panelMaterials ?? (_panelMaterials = Machine.ViewModels.Ioc.SimpleIoc<IPanelMaterials>.GetInstance()); 

        public override bool IsVisible => IsVisibleBase();
        private bool IsChanged() => _sectionSurfaces.Values.Any(s => s.IsChanged);

        public override void Draw(IProgram program, Matrix4 projection, Matrix4 view)
        {
            if (disposedValue) return;
            if (_panelMesh == null)
            {
                CheckUpdateAsync(program);
                return;
            }

            Matrix4 model = GetChainTransformation();
            SetMaterial(program);
            program.ModelViewProjectionMatrix.Set(model * view * projection);

            _panelMesh.Draw();
            CheckUpdateAsync(program);
        }

        protected void SetMaterial(IProgram program) => M3DVH.MaterialHelper.SetMaterial(program, GetMaterial());

        private void CheckUpdate(IProgram program)
        {
            if (disposedValue) return;
            if (!IsChanged()) return;
            if (_sectionSurfaces.Count == 0) return;

            var iLength = 0;
            var pLength = 0;
            var meshes = new List<RawMesh>();

            foreach (var key in _sectionSurfaces.Keys)
            {
                if(_sectionSurfaces.TryGetValue(key, out var section))
                {
                    section.GetMesh(out Vector3[] points, out uint[] idxs, out Vector3[] normals);
                    meshes.Add(new RawMesh { points = points, normals = normals, indexes = idxs });
                    pLength += points.Length;
                    iLength += idxs.Length;
                }
            }

            if (meshes.Count == 0) return;

            var indexes = new uint[iLength];
            var vertexes = new M3DVG.Vertex[pLength];
            var iOffset = 0;
            var pOffset = 0;

            for (int i = 0; i < meshes.Count; i++)
            {
                var mesh = meshes[i];

                for (int j = 0; j < mesh.points.Length; j++) vertexes[pOffset + j] = new M3DVG.Vertex(mesh.points[j], mesh.normals[j]);
                for (int j = 0; j < mesh.indexes.Length; j++) indexes[iOffset + j] = (uint)pOffset + mesh.indexes[j];

                pOffset += mesh.points.Length;
                iOffset += mesh.indexes.Length;
            }

            var oldPanelMesh = _panelMesh;

            Machine.ViewModels.Ioc.SimpleIoc<IDispatcherHelper>.GetInstance().CheckBeginInvokeOnUi(() =>
            {
                _panelMesh = new M3DVG.Mesh(vertexes, indexes, program);
            });

            if (oldPanelMesh != null) oldPanelMesh.Dispose();
        }

        private Task CheckUpdateAsync(IProgram program)
        {
            return Task.Run(() =>
            {
                if (Interlocked.CompareExchange(ref _processing, 1, 0) == 0)
                {
                    CheckUpdate(program);
                    Interlocked.Exchange(ref _processing, 0);
                }
            });
        }

        protected void Add(ISectionElement element)
        {
            _sectionSurfaces.TryAdd(element.Id, PanelSectionSurfaceViewModel.Create(element));
        }

        protected void Remove(ISectionElement element)
        {
            if (_sectionSurfaces.TryRemove(element.Id, out var section)) section.Dispose();
        }

        public abstract void Initialize();
        public abstract void Add(MRI.IPanelSection section);
        public abstract void Remove(MRI.IPanelSection section);
        protected abstract MVMGEM.Material GetMaterial();


        #region IDisposable implementation
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                disposedValue = true;

                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                    foreach (var item in _sectionSurfaces.Values) item.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                _panelMesh = null;
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
