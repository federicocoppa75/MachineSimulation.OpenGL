using Machine.ViewModels.GeometryExtensions.Helpers;
using Machine.ViewModels.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Machine.ViewModels.Interfaces.Factories;
using Machine.ViewModels.Interfaces.Helpers;
using Machine.ViewModels.Interfaces.MachineElements;

namespace Machine.ViewModels.GeometryExtensions.Factories
{
    public class ColliderHelperFactory : IColliderHelperFactory
    {
        public IColliderHelper GetColliderHelper(IColliderElement collider, IPanelElement panel) => new ColliderHelper(collider, panel);
    }
}
