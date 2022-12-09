using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MRVM = MaterialRemove.ViewModels;
using MVMGEI = Machine.ViewModels.GeometryExtensions.Interfaces;
using MVMGEB = Machine.ViewModels.GeometryExtensions.Builders;

namespace MaterialRemove.ViewModels._3D
{
    internal class SectionFaceViewModel : MRVM.SectionFaceViewModel, MVMGEI.IMeshProvider
    {
        static Vector3 _xp = new Vector3( 1,  0,  0);
        static Vector3 _xn = new Vector3(-1,  0,  0);
        static Vector3 _yp = new Vector3( 0,  1,  0);
        static Vector3 _yn = new Vector3( 0, -1,  0);
        static Vector3 _zp = new Vector3( 0,  0,  1);
        static Vector3 _zn = new Vector3( 0,  0, -1);

        bool _initialized;
        SectionSurface _sectionSurface = new SectionSurface();

        public bool IsChanged => _sectionSurface.IsChanged;

        public void GetMesh(out Vector3[] points, out uint[] indexes, out Vector3[] normals)
        {
            if (!_initialized)
            {
                Initialize();
                _initialized = true;
            }

            _sectionSurface.GetMesh(out points, out indexes, out normals);
        }

        protected override void OnActionApplied()
        {
            if (IsCorrupted)
            {
                _sectionSurface.Update(InternalGeometry);
            }
        }

        private void Initialize()
        {
            switch (Orientation)
            {
                case MaterialRemove.Interfaces.Orientation.XPos:
                    Initialize(_xp, _zp);
                    break;
                case MaterialRemove.Interfaces.Orientation.XNeg:
                    Initialize(_xn, _zp);
                    break;
                case MaterialRemove.Interfaces.Orientation.YPos:
                    Initialize(_yp, _zp);
                    break;
                case MaterialRemove.Interfaces.Orientation.YNeg:
                    Initialize(_yn, _zp);
                    break;
                case MaterialRemove.Interfaces.Orientation.ZPos:
                    Initialize(_zp, _yp);
                    break;
                case MaterialRemove.Interfaces.Orientation.ZNeg:
                    Initialize(_zn, _yn);
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"{Orientation} face not managed!");
            }
        }

        private void Initialize(Vector3 normal, Vector3 up)
        {
            var builder = new MVMGEB.MeshBuilder();
            var center = new Vector3((float)CenterX, (float)CenterY, (float)CenterZ);

            builder.AddCubeFace(center, normal, up, 0.0, SizeX, SizeY);
            builder.ToMesh(out Vector3[] points, out uint[] indexes, out Vector3[] normals);
            _sectionSurface.SetMesh(points, indexes, normals);
        }
    }
}
