using Machine._3D.Views.Programs;
using Machine.ViewModels.Interfaces.MachineElements;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machine._3D.Views.Elements
{
    internal class AngularTransmissionViewModel : ElementViewModel
    {
        public override bool IsVisible
        {
            get
            {
                var bodyModelFile = (Element as ViewModels.MachineElements.AngularTransmissionViewModel).BodyModelFile;

                return Element != null &&
                       !string.IsNullOrEmpty(bodyModelFile) &&
                       Element is IViewElementData ved &&
                       ved.IsVisible;
            }
        }
    }
}
