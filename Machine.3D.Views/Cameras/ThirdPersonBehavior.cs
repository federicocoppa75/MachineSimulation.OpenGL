using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Machine._3D.Views.Cameras
{
    public class ThirdPersonBehavior : CameraBehavior
    {
        public Vector3 Origin;

        public ThirdPersonBehavior() : base()
        {
        }

        protected void UpdateDistance(CameraState state, float scale)
        {
            state.Position = Origin - (state.Position - Origin).Length * (1 + scale) * state.LookAt;
        }

        public override void Initialize(CameraState state)
        {
            Origin = new Vector3();

            // recalculate look at direction
            state.LookAt = Origin - state.Position;
            state.LookAt.Normalize();
        }

        public override void MouseMove(CameraState state, Vector2 delta)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed) 
            {
                if (Keyboard.IsKeyDown(Key.LeftShift)) 
                {
                    // Pan with mouse
                    HandlePan(state, delta);

                    UpdateDistance(state, 0);
                }
                else
                {
                    // rotate look direction with mouse
                    HandleFreeLook(state, delta);
                    // recalculate the position
                    UpdateDistance(state, 0);
                }
            }
            if (Mouse.RightButton == MouseButtonState.Pressed)
            {
                UpdateDistance(state, delta.Y);
            }
        }

        public override void MouseWheelChanged(CameraState state, float delta)
        {
            if (delta > 100) delta = 100;
            UpdateDistance(state, -delta);
        }

        
        protected void HandlePan(CameraState state, Vector2 delta)
        {
            var leftRight = Vector3.Cross(state.Up, state.LookAt);

            leftRight.Normalize();

            var d1 = leftRight * delta.X;
            var d2 = state.Up * delta.Y;
            var d = d1 + d2;

            ScalePanStep(state, ref d);

            state.Position += d;
            Origin += d;
        }

        protected void ScalePanStep(CameraState state, ref Vector3 step)
        {
            var d = Math.Abs(Vector3.Dot(state.Position, state.LookAt));
            var v = 5.0f;

            step *= d / v;
        }
    }
}
