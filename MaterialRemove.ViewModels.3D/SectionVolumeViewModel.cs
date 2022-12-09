using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MRVM = MaterialRemove.ViewModels;
using MVMGEI = Machine.ViewModels.GeometryExtensions.Interfaces;


namespace MaterialRemove.ViewModels._3D
{
    internal class SectionVolumeViewModel : MRVM.SectionVolumeViewModel, MVMGEI.IMeshProvider
    {
        SectionSurface _sectionSurface = new SectionSurface();

        public bool IsChanged => _sectionSurface.IsChanged;

        public void GetMesh(out Vector3[] points, out uint[] indexes, out Vector3[] normals) => _sectionSurface.GetMesh(out points, out indexes, out normals);

        protected override void OnActionApplied()
        {
            if (IsCorrupted)
            {
                _sectionSurface.Update(InternalGeometry);
            }
        }
    }
}
