using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machine._3D.Views.Cameras
{
    public abstract class CameraBehavior
    {
        public double Width { get; private set; }
        public double Height { get; private set; }

        public virtual void Initialize(CameraState state) { }
        public virtual void UpdateFrame(CameraState state, float step) { }
        public virtual void MouseMove(CameraState state, Vector2 delta) { }
        public virtual void MouseWheelChanged(CameraState state, float delta) { }
        public virtual void SetOrigin(Vector3 point) { }
        public virtual bool IsPanning() => false;

        public void SetViewSize(double width, double height) 
        { 
            Width = width;
            Height = height;
        }

        /// <summary>
        /// TODO: add possibility to limit the pitch and prevent "flipping over"
        /// </summary>
        protected void HandleFreeLook(CameraState state, Vector2 delta)
        {
            var leftRight = Vector3.Cross(state.Up, state.LookAt);
            var forward = Vector3.Cross(leftRight, state.Up);
            // rotate look at direction
            var rot = Matrix3.CreateFromAxisAngle(state.Up, -delta.X) * Matrix3.CreateFromAxisAngle(leftRight, delta.Y);
            Vector3.TransformRow(in state.LookAt, in rot, out state.LookAt);
            // renormalize to prevent summing up of floating point errors
            state.LookAt.Normalize();
            // flip up vector when pitched more than +/-90� from the forward direction
            if (Vector3.Dot(state.LookAt, forward) < 0) state.Up *= -1;
        }
    }

}
