using Machine._3D.Views.Programs;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MVMIM = Machine.ViewModels.Interfaces.MachineElements;
using MVMM = Machine.ViewModels.MachineElements;

namespace Machine._3D.Views.Elements
{
    internal class AngularTransmissionViewModel : ElementViewModel
    {
        public override bool IsVisible
        {
            get
            {
                var bodyModelFile = (Element as MVMM.AngularTransmissionViewModel).BodyModelFile;

                return Element != null &&
                       !string.IsNullOrEmpty(bodyModelFile) &&
                       Element is MVMIM.IViewElementData ved &&
                       ved.IsVisible;
            }
        }
    }
}
