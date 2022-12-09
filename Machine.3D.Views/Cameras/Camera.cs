using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Machine._3D.Views.Cameras
{
    public class Camera
    {
        bool _firstMouseMove = true;
        System.Windows.Point _mousePosition;
        private Func<System.Windows.Point> _getMousePosition;
        private Func<System.Windows.Size> _getViewSize;

        public CameraState State;
        public CameraState DefaultState;
        protected CameraBehavior Behavior;
 
        public float MouseMoveSpeed = 0.05f;
        public float MouseWheelSpeed = 0.005f;
        //public float MoveSpeed = 60;

        public Camera()
        {
            State = new CameraState();
            DefaultState = new CameraState();
        }

        public void ResetToDefault()
        {
            State.Position = DefaultState.Position;
            State.LookAt = DefaultState.LookAt;
            State.Up = DefaultState.Up;
            Update();
        }

        public void SetBehavior(CameraBehavior behavior)
        {
            Behavior = behavior;
            Update();
        }

        public void Enable(System.Windows.Controls.UserControl window)
        {
            if (Behavior == null) throw new InvalidOperationException("Can not enable Camera while the Behavior is not set.");

            _getMousePosition = () => Mouse.GetPosition(window);
            _getViewSize = () => new System.Windows.Size(window.ActualWidth, window.ActualHeight);
            window.MouseMove += Window_MouseMove;
            window.MouseWheel += Window_MouseWheel;
            window.MouseUp += Window_MouseUp;

        }

        private void Window_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            _firstMouseMove = true;
        }

        private void Window_MouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            Behavior.MouseWheelChanged(State, MouseWheelSpeed * e.Delta);
        }

        private void Window_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if(_firstMouseMove)
            {
                _mousePosition = _getMousePosition();
                _firstMouseMove= false;
                return;
            }

            var p = _getMousePosition();
            var d = p - _mousePosition;
            var s = _getViewSize();
            Behavior.SetViewSize(s.Width, s.Height);
            Behavior.MouseMove(State, MouseMoveSpeed * new Vector2((float)d.X, (float)d.Y));

            _mousePosition = p;
        }


        public void Disable(System.Windows.Window window)
        {
            window.MouseMove -= Window_MouseMove;
            window.MouseWheel -= Window_MouseWheel;
            window.MouseUp -= Window_MouseUp;
            _getMousePosition = null;
        }

        public void Update()
        {
            if (Behavior != null) Behavior.Initialize(State);
        }

        //private void UpdateFrame(FrameEventArgs e)
        //{
        //    Behavior.UpdateFrame(State, (float)e.Time * MoveSpeed);
        //}

        /// <summary>
        /// TODO: add smooth transitions for the CameraState variables
        /// </summary>
        public Matrix4 GetCameraTransform()
        {
            // kind of hack: prevent look-at and up directions to be parallel
            if (Math.Abs(Vector3.Dot(State.Up, State.LookAt)) > 0.99999999999) State.LookAt += 0.001f * new Vector3(3, 5, 4);
            return Matrix4.LookAt(State.Position, State.Position + State.LookAt, State.Up);
        }

        public override string ToString()
        {
            return string.Format("({0},{1})", State, Behavior);
        }
    }

}
