using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialRemove.ViewModels._3D
{
    internal class SectionSurface
    {
        Vector3[] _points;
        Vector3[] _normals;
        uint[] _indexes;
        uint _changeIndex = 1;
        uint _lastChangeIndex = 0;

        public bool IsChanged => _changeIndex != _lastChangeIndex;

        public SectionSurface()
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

        public void Update(g3.DMesh3 mesh)
        {
            GeometryHelper.Convert(mesh, out _points, out _indexes, out _normals);
            _changeIndex++;
        }

        public void SetMesh(Vector3[] points, uint[] indexes, Vector3[] normals)
        {
            _points = points;
            _indexes = indexes;
            _normals = normals;
            _changeIndex++;
        }
    }
}
