using Machine.ViewModels.Interfaces.MachineElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machine._3D.Views.Messages
{
    internal class AddChildToElementMessage
    {
        public IMachineElement Element { get; set; }
    }
}
