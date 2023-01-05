using Machine._3D.Views.Elements;
using Machine._3D.Views.Helpers;
using Machine._3D.Views.Programs;
using Machine._3D.Views.ViewModels;
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
    public struct Material : IFieldValueProvider
    {
        public Vector4 ambient;
        public Vector4 diffuse;
        public Vector4 specular;
        public float shininess;

        void IFieldValueProvider.SetFieldsValues(IFieldValueSetter setter)
        {
            setter.Set(nameof(ambient), ambient);
            setter.Set(nameof(diffuse), diffuse);
            setter.Set(nameof(specular), specular);
            setter.Set(nameof(shininess), shininess);
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Light : IFieldValueProvider
    {
        public Vector3 position;
        public Vector3 ambient;
        public Vector3 diffuse;
        public Vector3 specular;

        void IFieldValueProvider.SetFieldsValues(IFieldValueSetter setter)
        {
            setter.Set(nameof(position), position);
            setter.Set(nameof(ambient), ambient);
            setter.Set(nameof(diffuse), diffuse);
            setter.Set(nameof(specular), specular);
        }
    }



    /// <summary>
    /// Interaction logic for MachineView.xaml
    /// </summary>
    public partial class MachineView : UserControl
    {
        RenderProcessCaller _renderProcesssCaller = new RenderProcessCaller(); 
        int _frames;
        double _elapsed;
        private bool _ctrlLoaded = false;
        private BaseProgram _baseProgram;

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

        private PanelMaterialViewModel _panelMaterial = new PanelMaterialViewModel();

        public MachineView()
        {
            InitializeComponent();

            Machine.ViewModels.Ioc.SimpleIoc<IColliderHelperFactory>.Register<ColliderHelperFactory>();
            Machine.ViewModels.Ioc.SimpleIoc<IInserterToSinkTransformerFactory>.Register<InserterToSinkTransformerFactory>();
            Machine.ViewModels.Ioc.SimpleIoc<IProcessCaller>.Register(_renderProcesssCaller);

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
            _baseProgram = ProgramFactory.Create<BaseProgram>();
            _baseProgram.Use();

            DataContext = new MainViewModel(_baseProgram);

            Camera = new Cameras.Camera();
            //Camera.SetBehavior(new Cameras.ThirdPersonBehavior());
            Camera.SetBehavior(new Cameras.TrackballBehavior());
            Camera.DefaultState.Position.Z = 1000;
            Camera.ResetToDefault();
            Camera.Enable(this);
            ResetMatrices();

            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);
            GL.Enable(EnableCap.FramebufferSrgb);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            _ctrlLoaded = true;
        }

        private void OnGlViewCtrlUnloaded(object sender, RoutedEventArgs e)
        {
            _baseProgram.Dispose();
        }

        private void OnGlViewCtrlRender(TimeSpan obj)
        {
            Time(obj);
            GL.ClearColor(Color4.Aqua);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            if (_ctrlLoaded)
            {
                SetupPerspective();
                // calculate the MVP matrix and set it to the shaders uniform
                _baseProgram.viewPos.Set(Camera.State.Position);

                _light.position = Camera.State.Position - Camera.State.LookAt * 1000;
                
                _baseProgram.light.Set(_light);

                var elements = (DataContext as MainViewModel).GetElements();

                foreach (var item in elements)
                {
                    if(!item.IsVisible) continue;

                    item.Draw(_baseProgram, Projection, View);
                }

                //_program.Use();
            }

            GL.Finish();

            if(_ctrlLoaded ) _renderProcesssCaller.NotifyRendered();
        }

        private void OnGlViewCtrlKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.R) { Camera.ResetToDefault(); }
            else if(e.Key == Key.E) { Camera.SetByViewBox((DataContext as MainViewModel).GetContentBox()); }
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
