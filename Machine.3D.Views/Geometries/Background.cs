using Machine._3D.Views.Programs;
using ObjectTK.Buffers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ObjectTK.Shaders;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Machine._3D.Views.Geometries
{
    internal class Background
    {
        private VertexArray _vao;
        private Buffer<Vertex> _vbo;
        private Buffer<uint> _ebo;

        public Background(Vertex[] vertexes, uint[] indexes, BackgroundProgram program)
        {
            _vbo = new Buffer<Vertex>();
            _vbo.Init(BufferTarget.ArrayBuffer, vertexes);

            _vao = new VertexArray();
            _vao.Bind();
            _vao.BindAttribute(program.InPosition, _vbo);
            _vao.BindAttribute(program.InColor, _vbo, sizeof(float) * 3);

            _ebo = new Buffer<uint>();
            _ebo.Init(BufferTarget.ElementArrayBuffer, indexes);

            _vao.BindElementBuffer(_ebo);
        }

        public void Draw()
        {
            _vao.Bind();
            _vao.DrawElements(PrimitiveType.Triangles, _ebo.ElementCount);
        }

        public void UpdatePosition(Vertex[] vertexes)
        {
            _vbo.SubData(BufferTarget.ArrayBuffer, vertexes);
        }
    }
}
