﻿using Machine._3D.Views.Helpers;
using Machine._3D.Views.Programs;
using Machine.ViewModels.GeometryExtensions.Factories;
using Machine.ViewModels.Interfaces;
using Machine.ViewModels.Interfaces.Insertions;
using ObjectTK.Shaders;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Machine._3D.Views
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Material
    {
        public Vector3 ambient;
        public Vector3 diffuse;
        public Vector3 specular;
        public float shininess;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Light
    {
        public Vector3 position;
        public Vector3 ambient;
        public Vector3 diffuse;
        public Vector3 specular;
    }



    /// <summary>
    /// Interaction logic for MachineView.xaml
    /// </summary>
    public partial class MachineView : UserControl
    {
        int _frames;
        double _elapsed;
        private bool _ctrlLoaded = false;
        private BaseProgram _program;

        protected Cameras.Camera Camera;
        protected Matrix4 View;
        protected Matrix4 Projection;

        protected Light _light = new Light()
        {
            position = new Vector3(0, 0, 1000),
            ambient = new Vector3(0.2f),
            diffuse = new Vector3(0.5f),
            specular = new Vector3(1)
        };

        public MachineView()
        {
            InitializeComponent();

            ViewModels.Ioc.SimpleIoc<IColliderHelperFactory>.Register<ColliderHelperFactory>();
            ViewModels.Ioc.SimpleIoc<IInserterToSinkTransformerFactory>.Register<InserterToSinkTransformerFactory>();

            ProgramFactory.BasePath = "Shaders/";
            ProgramFactory.Extension = "glsl";  

            var settings = new GLWpfControlSettings()
            {
                MajorVersion = 4,
                MinorVersion = 3
            };

            glViewCtrl.Start(settings);

        }

        private void OnGlViewCtrlLoaded(object sender, RoutedEventArgs e)
        {
            _program = ProgramFactory.Create<BaseProgram>();
            _program.Use();

            DataContext = new MainViewModel(_program);

            Camera = new Cameras.Camera();
            Camera.SetBehavior(new Cameras.ThirdPersonBehavior());
            Camera.DefaultState.Position.Z = 1000;
            Camera.ResetToDefault();
            Camera.Enable(this);
            ResetMatrices();

            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);

            _ctrlLoaded = true;
        }

        private void OnGlViewCtrlUnloaded(object sender, RoutedEventArgs e)
        {
            _program.Dispose();
        }

        private void OnGlViewCtrlRender(TimeSpan obj)
        {
            Time(obj);
            GL.ClearColor(Color4.Aqua);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            if (_ctrlLoaded)
            {
                _program.LightPosition.Set(_light.position);
                _program.LightAmbient.Set(_light.ambient);
                _program.LightDiffuse.Set(_light.diffuse);
                _program.LightSpecular.Set(_light.specular);

                SetupPerspective();
                // calculate the MVP matrix and set it to the shaders uniform
                _program.viewPos.Set(Camera.State.Position);

                var elements = (DataContext as MainViewModel).GetElements();

                foreach (var item in elements)
                {
                    if(!item.IsVisible) continue;

                    item.Draw(_program, Projection, View);
                }

                //_program.Use();
            }

            GL.Finish();
        }

        private void OnGlViewCtrlKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.R) { Camera.ResetToDefault(); }
        }

        private void Time(TimeSpan obj)
        {
            _frames++;
            _elapsed += obj.TotalMilliseconds;

            if (_elapsed > 1000)
            {
                _elapsed -= 1000;
                fpsLabel.Content = $"{_frames} FPS";
                _frames = 0;
            }
        }

        protected void ResetMatrices()
        {
            View = Matrix4.Identity;
            Projection = Matrix4.Identity;
        }

        protected void SetupPerspective()
        {
            // setup perspective projection
            var aspectRatio = ActualWidth / ActualHeight;
            Projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, (float)aspectRatio, 0.1f, 10000);
            View = Camera.GetCameraTransform();
        }
    }
}