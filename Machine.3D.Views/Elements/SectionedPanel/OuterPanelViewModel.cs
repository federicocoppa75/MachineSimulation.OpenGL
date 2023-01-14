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
    internal class OuterPanelViewModel : SectionedPanelViewModel
    {
        public override void Initialize()
        {
            var panel = Element as MRI.IPanel;

            foreach (var section in panel.Sections)
            {
                foreach (var face in section.Faces) Add(face);
            }
        }

        protected override MVMGEM.Material GetMaterial() => PanelMaterials.PanelOuter;

        public override void Add(IPanelSection section)
        {
            foreach (var face in section.Faces) Add(face);
        }

        public override void Remove(IPanelSection section)
        {
            foreach (var face in section.Faces) Remove(face);
        }
    }
}
