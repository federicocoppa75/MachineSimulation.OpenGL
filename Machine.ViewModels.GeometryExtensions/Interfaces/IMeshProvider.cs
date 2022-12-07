using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machine.ViewModels.GeometryExtensions.Interfaces
{
    public interface IMeshProvider
    {
        bool IsChanged { get; }
        void GetMesh(out Vector3[] points, out uint[] indexes, out Vector3[] normals);
    }
}
