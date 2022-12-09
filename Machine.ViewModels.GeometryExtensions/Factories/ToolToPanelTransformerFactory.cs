using Machine.ViewModels.GeometryExtensions.Helpers;
using Machine.ViewModels.Interfaces.MachineElements;
using Machine.ViewModels.Interfaces.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machine.ViewModels.GeometryExtensions.Factories
{
    public class ToolToPanelTransformerFactory : IToolToPanelTransformerFactory
    {
        public IToolToPanelTransformer GetTransformer(IPanelElement panel, IEnumerable<IToolElement> tools) => new ToolToPanelTransformer(panel, tools);
    }
}
