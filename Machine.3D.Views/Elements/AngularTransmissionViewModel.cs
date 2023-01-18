using Machine._3D.Views.Programs;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MVMIME = Machine.ViewModels.Interfaces.MachineElements;

namespace Machine._3D.Views.Elements
{
    internal class AngularTransmissionViewModel : ElementViewModel
    {
        public override bool IsVisible
        {
            get
            {
                var bodyModelFile = (Element as MVMIME.IAngularTransmission).BodyModelFile;

                return Element != null &&
                       !string.IsNullOrEmpty(bodyModelFile) &&
                       Element is MVMIME.IViewElementData ved &&
                       ved.IsVisible;
            }
        }
    }
}
