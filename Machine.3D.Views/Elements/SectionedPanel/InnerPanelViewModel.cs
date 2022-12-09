using Machine._3D.Views.Programs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using M3DVH = Machine._3D.Views.Helpers;
using MRI = MaterialRemove.Interfaces;

namespace Machine._3D.Views.Elements.SectionedPanel
{
    internal class InnerPanelViewModel : SectionedPanelViewModel
    {
        public override void Initialize()
        {
            var panel = Element as MRI.IPanel;

            foreach (var section in panel.Sections)
            {
                _sectionSurfaces.Add(PanelSectionSurfaceViewModel.Create(section.Volume));
            }
        }

        protected override void SetMaterial(BaseProgram program)
        {
            M3DVH.MaterialHelper.SetMaterial(program, new Data.Base.Color() { R = 101, G = 191, B = 176, A = 255 });
        }
    }
}
