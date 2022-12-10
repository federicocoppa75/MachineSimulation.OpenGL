using Machine._3D.Views.Interfaces;
using Machine.ViewModels.Base;
using Machine.ViewModels.GeometryExtensions.Materials;
using Machine.ViewModels.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MVMGEM = Machine.ViewModels.GeometryExtensions.Materials;

namespace Machine._3D.Views.ViewModels
{
    internal class PanelMaterialViewModel : BaseViewModel, IPanelMaterials
    {
        public MVMGEM.Material PanelOuter { get; set; }
        public MVMGEM.Material PanelInner { get; set; }

        private string _panelOuterMaterialName;
        public string PanelOuterMaterialName
        {
            get => _panelOuterMaterialName;
            set
            {
                if (Set(ref _panelOuterMaterialName, value, nameof(PanelOuterMaterialName)))
                {
                    PanelOuter = PhongMaterials.Materials.FirstOrDefault(m => string.Compare(m.Name, _panelOuterMaterialName) == 0);
                }
            }
        }

        private string _panelInnerMaterialName;
        public string PanelInnerMaterialName
        {
            get => _panelInnerMaterialName;
            set
            {
                if (Set(ref _panelInnerMaterialName, value, nameof(PanelInnerMaterialName)))
                {
                    PanelInner = PhongMaterials.Materials.FirstOrDefault(m => string.Compare(m.Name, _panelInnerMaterialName) == 0);
                }
            }
        }

        public IEnumerable<String> PanelMaterialsNames => MVMGEM.PhongMaterials.Materials.Select(m => m.Name);


        public PanelMaterialViewModel()
        {
            PanelInnerMaterialName = PhongMaterials.Turquoise.Name;
            PanelOuterMaterialName = PhongMaterials.Orange.Name;

            Machine.ViewModels.Ioc.SimpleIoc<IOptionProvider<string>>.Register("PanelOuterMaterial", new StringOptionProxy(() => PanelMaterialsNames, () => PanelOuterMaterialName, (v) => PanelOuterMaterialName = v));
            Machine.ViewModels.Ioc.SimpleIoc<IOptionProvider<string>>.Register("PanelInnerMaterial", new StringOptionProxy(() => PanelMaterialsNames, () => PanelInnerMaterialName, (v) => PanelInnerMaterialName = v));
            Machine.ViewModels.Ioc.SimpleIoc<IPanelMaterials>.Register(this);
        }
    }
}
