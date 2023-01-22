using Assimp;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using Machine.ViewModels.GeometryExtensions.Math;

namespace Machine._3D.Views.Cameras
{
    public class TrackballBehavior : CameraBehavior
    {
        private float _lastDistance;
        private bool _updateLastDistance;
        public Vector3 Origin;        

        public override void Initialize(CameraState state)
        {
            Origin = new Vector3();

            // recalculate look at direction
            state.LookAt = Origin - state.Position;
            state.LookAt.Normalize();
            UpdateLastDistance(state);
        }

        public override void SetOrigin(Vector3 point)
        {
            Origin= point;
            _updateLastDistance= true;
        }

        public override void MouseMove(CameraState state, Vector2 delta)
        {
            //base.MouseMove(state, delta);

            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                if (Keyboard.IsKeyDown(Key.LeftShift))
                {
                    // Pan with mouse
                    HandlePan(state, delta);
                    UpdateDistanceAfterPan(state, 0);
                }
                else
                {
                    var rotatePosition = new Vector2((float)(Width / 2.0), (float)(Height / 2.0));
                    RotateX(state,
                           rotatePosition,
                           rotatePosition + delta,
                           GetCameraTarget(state));

                    //UpdateDistance(state, 0);
                    UpdateDistanceAfterRotate(state, 0);

                }
            }
            else if (Mouse.RightButton == MouseButtonState.Pressed)
            {
                UpdateDistance(state, delta.Y);
            }
            else if (Mouse.MiddleButton == MouseButtonState.Pressed)
            {
                // Pan with mouse
                HandlePan(state, delta);
                UpdateDistanceAfterPan(state, 0);
            }
        }

        public override void MouseWheelChanged(CameraState state, float delta)
        {
            if (delta > 100) delta = 100;

            UpdateDistance(state, -delta);
        }

        public override bool IsPanning()
        {
            bool result = false;

            if ((Mouse.MiddleButton == MouseButtonState.Pressed) ||
                ((Mouse.LeftButton == MouseButtonState.Pressed) && Keyboard.IsKeyDown(Key.LeftShift)))
            {
                result = true;
            }            

            return result;
        }

        protected void UpdateLastDistance(CameraState state)
        {
            _lastDistance = (state.Position - Origin).Length;
            _updateLastDistance = false;
        }

        protected void UpdateDistance(CameraState state, float scale)
        {
            UpdateLastDistance(state);

            state.Position = Origin - (state.Position - Origin).Length * (1 + scale) * state.LookAt;
        }

        protected void UpdateDistanceAfterPan(CameraState state, float scale)
        {
            if (_updateLastDistance) UpdateLastDistance(state);

            state.Position = Origin - _lastDistance * (1 + scale) * state.LookAt;
        }

        protected void UpdateDistanceAfterRotate(CameraState state, float scale)
        {
            //state.Position = Origin - (state.Position - Origin).Length * (1 + scale) * state.LookAt;
            state.Position = Origin - _lastDistance * (1 + scale) * state.LookAt;
        }

        private void Rotate(CameraState state, Vector2 p0, Vector2 p1, Vector3 rotateAround)
        {
            Vector2 delta = p1 - p0;
            Vector3 relativeTarget = rotateAround - GetCameraTarget(state);
            Vector3 relativePosition = rotateAround - state.Position;

            Vector3 up = state.Up;
            Vector3 dir = state.LookAt;
            dir.Normalize();

            Vector3 right = Vector3.Cross(dir, state.Up);
            right.Normalize();

            float d = -0.5f;
            //if (this.CameraMode != CameraMode.Inspect)
            //{
            //    d *= -0.2;
            //}

            d *= 1.0f; //this.RotationSensitivity;

            var m = Matrix4.CreateFromAxisAngle(right, d * delta.Y) * Matrix4.CreateFromAxisAngle(up, d * delta.X);

            //var q1 = new Quaternion(up, d * delta.X);
            //var q2 = new Quaternion(right, d * delta.Y);
            //Quaternion q = q1 * q2;

            //var m = new Matrix3D();
            //m.Rotate(q);

            Vector3 newUpDirection = m.Transform(state.Up);

            Vector3 newRelativeTarget = m.Transform(relativeTarget);
            Vector3 newRelativePosition = m.Transform(relativePosition);

            Vector3 newTarget = rotateAround - newRelativeTarget;
            Vector3 newPosition = rotateAround - newRelativePosition;

            state.LookAt = newTarget - newPosition;
            //if (CameraMode == CameraMode.Inspect)
            {
                state.Position = newPosition;
            }

            state.Up = newUpDirection;
        }

        private void RotateX(CameraState state, Vector2 p0, Vector2 p1, Vector3 rotateAround)
        {
            if (_updateLastDistance) UpdateLastDistance(state);

            Vector3 v1 = ProjectToTrackball(p0, Width, Height);
            Vector3 v2 = ProjectToTrackball(p1, Width, Height);

            // transform the trackball coordinates to view space
            Vector3 viewZ = state.LookAt;
            Vector3 viewX = Vector3.Cross(state.Up, viewZ);
            Vector3 viewY = Vector3.Cross(viewX, viewZ);
            viewX.Normalize();
            viewY.Normalize();
            viewZ.Normalize();
            Vector3 u1 = viewZ * v1.Z + viewX * v1.X + viewY * v1.Y;
            Vector3 u2 = viewZ * v2.Z + viewX * v2.X + viewY * v2.Y;

            // Find the rotation axis and angle
            Vector3 axis = Vector3.Cross(u1, u2);
            double angle = Vector3.CalculateAngle(u1, u2);

            // Create the transform
            //var delta = new Quaternion(axis, -angle * this.RotationSensitivity * 5);
            //var rotate = new RotateTransform3D(new QuaternionRotation3D(delta));
            var rotate = Matrix4.CreateFromAxisAngle(axis, (float)(-angle * 50));

            // Find vectors relative to the rotate-around point
            Vector3 relativeTarget = rotateAround - GetCameraTarget(state);
            Vector3 relativePosition = rotateAround - state.Position;

            // Rotate the relative vectors
            Vector3 newRelativeTarget = rotate.Transform(relativeTarget);
            Vector3 newRelativePosition = rotate.Transform(relativePosition);
            Vector3 newUpDirection = rotate.Transform(state.Up);

            // Find new camera position
            var newTarget = rotateAround - newRelativeTarget;
            var newPosition = rotateAround - newRelativePosition;

            state.LookAt = newTarget - newPosition;
            if (true) //(CameraMode == CameraMode.Inspect)
            {
                state.Position = newPosition;
            }
            state.Up = newUpDirection;
        }

        private static Vector3 GetCameraTarget(CameraState state) => state.Position + state.LookAt;

        private static Vector3 ProjectToTrackball(Vector2 point, double w, double h)
        {
            // Use the diagonal for scaling, making sure that the whole client area is inside the trackball
            double r = Math.Sqrt(w * w + h * h) / 2;
            double x = (point.X - w / 2) / r;
            double y = (h / 2 - point.Y) / r;
            double z2 = 1 - x * x - y * y;
            double z = z2 > 0 ? Math.Sqrt(z2) : 0;

            return new Vector3((float)x, (float)y, (float)z);
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
