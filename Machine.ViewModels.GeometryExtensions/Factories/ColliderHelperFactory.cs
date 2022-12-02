using Machine.ViewModels.GeometryExtensions.Helpers;
using Machine.ViewModels.Interfaces;
using Machine.ViewModels.MachineElements;
using Machine.ViewModels.MachineElements.Collider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machine.ViewModels.GeometryExtensions.Factories
{
    public class ColliderHelperFactory : IColliderHelperFactory
    {
        public IColliderHelper GetColliderHelper(ColliderElementViewModel collider, PanelViewModel panel) => new ColliderHelper(collider, panel);
    }
}
