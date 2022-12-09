using Machine._3D.Views.Programs;
using ObjectTK.Buffers;
using ObjectTK.Shaders;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Machine._3D.Views.Geometries
{
    internal class Mesh : IDisposable
    {
        private VertexArray _vao;
        private Buffer<Vertex> _vbo;
        private Buffer<uint> _ebo;
        private bool disposedValue;

        public Mesh(Vertex[] vertexes, uint[] indexes, BaseProgram program)
        {
            _vbo = new Buffer<Vertex>();
            _vbo.Init(BufferTarget.ArrayBuffer, vertexes);

            _vao = new VertexArray();
            _vao.Bind();
            _vao.BindAttribute(program.InPosition, _vbo);
            _vao.BindAttribute(program.InNormal, _vbo, sizeof(float) * 3);

            _ebo = new Buffer<uint>();
            _ebo.Init(BufferTarget.ElementArrayBuffer, indexes);
            
            _vao.BindElementBuffer(_ebo);
        }

        public void Draw()
        {
            _vao.Bind();
            _vao.DrawElements(PrimitiveType.Triangles, _ebo.ElementCount);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                    _vao?.Dispose();
                    _ebo?.Dispose();
                    _vbo?.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
