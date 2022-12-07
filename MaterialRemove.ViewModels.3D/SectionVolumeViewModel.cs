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
        Vector3[] _points;
        Vector3[] _normals;
        uint[] _indexes;
        uint _changeIndex = 1;
        uint _lastChangeIndex = 0;

        public bool IsChanged => _changeIndex != _lastChangeIndex;

        public SectionVolumeViewModel()
        {
            _points = new Vector3[0];
            _normals = new Vector3[0];
            _indexes = new uint[0];
        }

        public void GetMesh(out Vector3[] points, out uint[] indexes, out Vector3[] normals)
        {
            points = _points;
            indexes = _indexes;
            normals = _normals;

            _lastChangeIndex = _changeIndex;
        }

        protected override void OnActionApplied()
        {
            if (IsCorrupted)
            {
                GeometryHelper.Convert(InternalGeometry, out _points, out _indexes, out _normals);
                _changeIndex++;
            }
        }
    }
}
