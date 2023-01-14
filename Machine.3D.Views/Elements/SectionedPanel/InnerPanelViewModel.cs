using Machine._3D.Views.Programs;
using MaterialRemove.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using M3DVH = Machine._3D.Views.Helpers;
using MRI = MaterialRemove.Interfaces;
using MVMGEM = Machine.ViewModels.GeometryExtensions.Materials;

namespace Machine._3D.Views.Elements.SectionedPanel
{
    internal class InnerPanelViewModel : SectionedPanelViewModel
    {
        public override void Initialize()
        {
            var panel = Element as MRI.IPanel;

            foreach (var section in panel.Sections)
            {
                if (section.Volume != null) Add(section.Volume);
            }
        }

        protected override MVMGEM.Material GetMaterial() => PanelMaterials.PanelInner;

        public override void Add(IPanelSection section)
        {
            if (section.Volume != null) Add(section.Volume);
        }

        public override void Remove(IPanelSection section)
        {
            if (section.Volume != null) Remove(section.Volume);
        }
    }
}
