using OpenTK.Mathematics;
//using MGM = MaterialRemove.Geometry.Mesh;

namespace MaterialRemove.ViewModels._3D
{
    internal static class GeometryHelper
    {
        public static void Convert(g3.DMesh3 mesh, out Vector3[] points, out uint[] indexes, out Vector3[] normals)
        {
            if((mesh == null) || (mesh.VertexCount == 0))
            {
                points = new Vector3[0];
                indexes = new uint[0];  
                normals = new Vector3[0];
            }
            else 
            {
                var vCount = mesh.VerticesRefCounts.count;
                var v = mesh.VerticesBuffer;
                var n = mesh.NormalsBuffer;
                points = new Vector3[vCount];
                normals = new Vector3[vCount];

                for (int i = 0; i < vCount; i++)
                {
                    var j = i * 3;
                    points[i] = new Vector3((float)v[j], (float)v[j + 1], (float)v[j + 2]);
                    normals[i] = new Vector3((float)n[j], (float)n[j + 1], (float)n[j + 2]);
                }

                var iCount = mesh.TrianglesRefCounts.count * 3;
                var idxs = mesh.TrianglesBuffer;
                indexes = new uint[iCount];

                for (int i = 0; i < iCount; i++) indexes[i] = (uint)idxs[i];
            }
        }
    }
}
