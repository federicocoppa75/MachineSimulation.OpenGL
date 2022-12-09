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

namespace Machine._3D.Views.Elements
{
    internal class SectionedPanelVieModel : ElementViewModel, IDisposable
    {
        struct RawMesh
        {
            public Vector3[] points;
            public Vector3[] normals;
            public uint[] indexes;
        }

        bool _firstDraw = true;
        object _lockObj = new object();
        M3DVG.Mesh _panelMesh;
        List<PanelSectionSurfaceViewModel> _sectionSurfaces = new List<PanelSectionSurfaceViewModel>();
        private bool disposedValue;

        public override bool IsVisible => IsVisibleBase();

        public override void Draw(BaseProgram program, Matrix4 projection, Matrix4 view)
        {
            if (_panelMesh == null)
            {
                CheckUpdate(program);
                return;
            }

            Matrix4 model = GetChainTransformation();
            M3DVH.MaterialHelper.SetMaterial(program, new Data.Base.Color() { R = 253, G = 131, B = 0, A = 255 });
            program.ModelViewProjectionMatrix.Set(model * view * projection);

            _panelMesh.Draw(program);
            CheckUpdate(program);
        }

        private void CheckUpdate(BaseProgram program)
        {
            if (!IsChanged()) return;
            if (_sectionSurfaces.Count == 0) return;

            var iLength = 0;
            var pLength = 0;
            var meshes = new List<RawMesh>();

            for (int i = 0; i < _sectionSurfaces.Count; i++)
            {
                var section = _sectionSurfaces[i];

                section.GetMesh(out Vector3[] points, out uint[] idxs, out Vector3[] normals);
                meshes.Add(new RawMesh { points = points, normals = normals, indexes = idxs/*, center = section.Center*/ });
                pLength+= points.Length;
                iLength+= idxs.Length;
            }

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

            _panelMesh = new M3DVG.Mesh(vertexes, indexes, program);
        }

        public void Initialize()
        {
            var panel = Element as MRI.IPanel;

            foreach (var section in panel.Sections)
            {
                foreach (var face in section.Faces)
                {
                    _sectionSurfaces.Add(PanelSectionSurfaceViewModel.Create(face));
                }

                _sectionSurfaces.Add(PanelSectionSurfaceViewModel.Create(section.Volume));
            }
        }

        private bool IsChanged() => _sectionSurfaces.Any(s => s.IsChanged);

        #region IDisposable implementation
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                    foreach (var item in _sectionSurfaces) item.Dispose();
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
