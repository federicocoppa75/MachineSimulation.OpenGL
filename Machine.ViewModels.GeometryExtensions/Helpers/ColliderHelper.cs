using Machine.ViewModels.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;
using Machine.ViewModels.Interfaces.Helpers;
using Machine.ViewModels.Interfaces.MachineElements;

namespace Machine.ViewModels.GeometryExtensions.Helpers
{
    internal class ColliderHelper : BaseColliderHelper, IColliderHelper
    {
        IColliderElement _collider;
        IPanelElement _panel;

        public ColliderHelper(IColliderElement collider, IPanelElement panel)
        {
            _collider = collider;
            _panel = panel;
        }

        protected override Matrix4 GetColliderChainTransformation() => _collider.GetChainTransformation();

        protected override Vector3 GetColliderDirection() => new Vector3((float)_collider.CollidingDirection.X, (float)_collider.CollidingDirection.Y, (float)_collider.CollidingDirection.Z);

        protected override Vector3[] GetColliderPoints() => _collider.Points.Select(p => new Vector3((float)p.X, (float)p.Y, (float)p.Z)).ToArray();

        protected override Matrix4 GetPanelChainTransformation() => _panel.GetChainTransformation();

        protected override Vector3 GetPanelSize() => new Vector3((float)_panel.SizeX, (float)_panel.SizeY, (float)_panel.SizeZ);
    }
}
