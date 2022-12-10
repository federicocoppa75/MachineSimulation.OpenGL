using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MVMGEM = Machine.ViewModels.GeometryExtensions.Materials;


namespace Machine._3D.Views.Interfaces
{
    public interface IPanelMaterials
    {
        MVMGEM.Material PanelOuter { get; set; }
        MVMGEM.Material PanelInner { get; set; }
    }
}
