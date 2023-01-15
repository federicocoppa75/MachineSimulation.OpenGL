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
using Machine.ViewModels.GeometryExtensions.Math;

namespace Machine._3D.Views.Geometries
{
    internal class Mesh : IDisposable
    {
        private VertexArray _vao;
        private Buffer<Vertex> _vbo;
        private Buffer<uint> _ebo;
        private bool disposedValue;

        public Mesh(Vertex[] vertexes, uint[] indexes, IProgram program)
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
            if(disposedValue) return;
            _vao.Bind();
            _vao.DrawElements(PrimitiveType.Triangles, _ebo.ElementCount);
        }

        public Box3 GetBound()
        {
            var n = _vbo.ElementCount;

            if(n == 0)
            {
                throw new InvalidProgramException();
            }
            if(n == 1) 
            {
                var v = _vbo.Content[0].position;
                return new Box3(v, v);
            }
            else
            {
                var v = _vbo.Content;
                var min = v[0].position;
                var max = v[0].position;

                for (int i = 1; i < n; i++)
                {
                    min = min.GetMinimum(v[i].position);
                    max = max.GetMaximum(v[i].position);
                }

                return new Box3(min, max);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                disposedValue = true;

                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                    _vao?.Dispose();
                    _ebo?.Dispose();
                    _vbo?.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
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
