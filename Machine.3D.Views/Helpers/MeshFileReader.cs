using Assimp;
using Assimp.Configs;
using Machine._3D.Views.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machine._3D.Views.Helpers
{
    internal class MeshFileReader
    {
        public static void Read(string fileName, out Vertex[] vertexes, out uint[] indexes) 
        {
            using (var importer = new AssimpContext())
            {
                importer.SetConfig(new NormalSmoothingAngleConfig(66f));

                var scene = importer.ImportFile(fileName, PostProcessPreset.TargetRealTimeQuality | PostProcessSteps.GenerateSmoothNormals);

                // leggo solo la prima
                if(scene != null && scene.MeshCount > 0) 
                {
                    var mesh = scene.Meshes[0];
                    var vSize = mesh.VertexCount;
                    
                    indexes = mesh.GetIndices().Select(i => (uint)i).ToArray();
                    vertexes = new Vertex[vSize];

                    for (int i = 0; i < vSize; i++)
                    {
                        var v = mesh.Vertices[i];
                        var n = mesh.Normals[i];
                        vertexes[i] = new Vertex(v.X, v.Y, v.Z, n.X, n.Y, n.Z);
                    }
                }
                else
                {
                    vertexes= new Vertex[0];
                    indexes = new uint[0];
                }
            }
        }
    }
}
