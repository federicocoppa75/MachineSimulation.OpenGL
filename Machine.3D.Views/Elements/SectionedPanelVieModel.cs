using Machine._3D.Views.Programs;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MRI = MaterialRemove.Interfaces;

namespace Machine._3D.Views.Elements
{
    internal class SectionedPanelVieModel : ElementViewModel, IDisposable
    {
        List<PanelSectionSurfaceViewModel> _sectionSurfaces = new List<PanelSectionSurfaceViewModel>();
        private bool disposedValue;

        public override bool IsVisible => IsVisibleBase();

        public override void Draw(BaseProgram program, Matrix4 projection, Matrix4 view)
        {
            Matrix4 model = GetChainTransformation();

            foreach (var item in _sectionSurfaces)
            {
                item.Draw(program, projection, view, model);
            }
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
