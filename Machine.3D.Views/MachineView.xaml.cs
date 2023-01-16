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
using Machine.ViewModels.Interfaces.Factories;
using Machine.ViewModels.GeometryExtensions.Builders;
using M3DVG = Machine._3D.Views.Geometries;
using Machine.ViewModels.GeometryExtensions.Materials;
using Machine._3D.Views.Geometries;
using SWM = System.Windows.Media;
using Machine._3D.Views.Interfaces;
//using System.Configuration;

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

    public struct DirectionalLight : IFieldValueProvider
    {
        public Vector3 direction;
        public Vector3 ambient;
        public Vector3 diffuse;
        public Vector3 specular;

        void IFieldValueProvider.SetFieldsValues(IFieldValueSetter setter)
        {
            setter.Set(nameof(direction), direction);
            setter.Set(nameof(ambient), ambient);
            setter.Set(nameof(diffuse), diffuse);
            setter.Set(nameof(specular), specular);
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct PointLight : IFieldValueProvider
    {
        public Vector3 position;
        public Vector3 ambient;
        public Vector3 diffuse;
        public Vector3 specular;
        public float constant;
        public float linear;
        public float quadratic;

        void IFieldValueProvider.SetFieldsValues(IFieldValueSetter setter)
        {
            setter.Set(nameof(position), position);
            setter.Set(nameof(ambient), ambient);
            setter.Set(nameof(diffuse), diffuse);
            setter.Set(nameof(specular), specular);
            setter.Set(nameof(constant), constant);
            setter.Set(nameof(linear), linear);
            setter.Set(nameof(quadratic), quadratic);
        }
    }

    public struct SpotLight : IFieldValueProvider
    {
        public Vector3 position;
        public Vector3 direction;
        public Vector3 ambient;
        public Vector3 diffuse;
        public Vector3 specular;
        public float cutOff;
        public float outerCutOff;
        public float constant;
        public float linear;
        public float quadratic;

        void IFieldValueProvider.SetFieldsValues(IFieldValueSetter setter)
        {
            setter.Set(nameof(position), position);
            setter.Set(nameof(direction), direction);
            setter.Set(nameof(ambient), ambient);
            setter.Set(nameof(diffuse), diffuse);
            setter.Set(nameof(specular), specular);
            setter.Set(nameof(cutOff), cutOff);
            setter.Set(nameof(outerCutOff), outerCutOff);
            setter.Set(nameof(constant), constant);
            setter.Set(nameof(linear), linear);
            setter.Set(nameof(quadratic), quadratic);
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
        private IProgram _program;
        private BackgroundProgram _bckGrdProgram;

        protected Cameras.Camera Camera;
        protected Matrix4 View;
        protected Matrix4 Projection;

        private float _depthNear = 0.1f;
        private float _depthFar = 20000;
        private float _fov = MathHelper.PiOver4;
        private M3DVG.Background _background;
        //private M3DVG.Mesh _background;
        private Matrix4 _lastView;
        private double _lastWidth;
        private double _lastHeight;
        private bool _backgroundColorChanged;

        protected DirectionalLight _directionalLight = new DirectionalLight()
        {
            direction = new Vector3(-3.0f, -2.0f, -1.0f),//-Vector3.UnitZ,
            ambient = new Vector3(0.2f),
            diffuse = new Vector3(0.5f),
            specular = new Vector3(1.0f),
        };

        protected PointLight _pointlight = new PointLight()
        {
            position = new Vector3(0, 0, 1000),
            ambient = new Vector3(0.2f),
            diffuse = new Vector3(0.5f),
            specular = new Vector3(1.0f),
            constant = 1.0f,
            linear = 0.09f,
            quadratic = 0.032f,
        };

        protected DirectionalLight[] _directionalLights = new DirectionalLight[]
        {
            new DirectionalLight() 
            {
                direction = new Vector3(-2.0f, -3.0f, -1.0f),
                ambient = new Vector3(0.1f),
                diffuse = new Vector3(0.3f),
                specular = new Vector3(1.0f),
            },
            new DirectionalLight()
            {
                direction = new Vector3(-2.0f, 3.0f, -1.0f),
                ambient = new Vector3(0.1f),
                diffuse = new Vector3(0.3f),
                specular = new Vector3(1.0f),
            },
            new DirectionalLight()
            {
                direction = new Vector3(3.5f, -0.5f, -1.0f),
                ambient = new Vector3(0.1f),
                diffuse = new Vector3(0.3f),
                specular = new Vector3(1.0f),
            },
        };

        protected PointLight[] _pointlights = new PointLight[]
        {
            new PointLight()
            {
                position = new Vector3(2000, -1000, 1000),
                ambient = new Vector3(0.2f),
                diffuse = new Vector3(0.5f),
                specular = new Vector3(1.0f),
                constant = 1.0f,
                linear = 0.09f,
                quadratic = 0.032f
            },
            new PointLight()
            {
                position = new Vector3(-2000, 1000, 1000),
                ambient = new Vector3(0.2f),
                diffuse = new Vector3(0.5f),
                specular = new Vector3(1.0f),
                constant = 1.0f,
                linear = 0.18f,
                quadratic = 0.032f
            },
            new PointLight()
            {
                position = new Vector3(-2000, -1000, 1000),
                ambient = new Vector3(0.2f),
                diffuse = new Vector3(0.5f),
                specular = new Vector3(1.0f),
                constant = 1.0f,
                linear = 0.09f,
                quadratic = 0.032f
            },
            new PointLight()
            {
                position = new Vector3(2000, 1000, 1000),
                ambient = new Vector3(0.2f),
                diffuse = new Vector3(0.5f),
                specular = new Vector3(1.0f),
                constant = 1.0f,
                linear = 0.09f,
                quadratic = 0.032f
            }
        };

        protected SpotLight _spotLight = new SpotLight()
        {
            position = new Vector3(0, 0, 1000),
            direction = -Vector3.UnitZ,
            ambient = new Vector3(0.2f),
            diffuse = new Vector3(0.5f),
            specular = new Vector3(1.0f),
            cutOff = MathF.Cos(MathHelper.DegreesToRadians(45.0f)),
            outerCutOff = MathF.Cos(MathHelper.DegreesToRadians(90.0f)),
            constant = 1.0f,
            linear = 0.09f,
            quadratic = 0.032f,
        };

        private PanelMaterialViewModel _panelMaterial = new PanelMaterialViewModel();

        private BackgroundColor BackgroudColor { get; set; } = new BackgroundColor() { Start = Colors.LightGray, Stop = Colors.LightCyan };


        public MachineView()
        {
            InitializeComponent();

            Machine.ViewModels.Ioc.SimpleIoc<IColliderHelperFactory>.Register<ColliderHelperFactory>();
            Machine.ViewModels.Ioc.SimpleIoc<IInserterToSinkTransformerFactory>.Register<InserterToSinkTransformerFactory>();
            Machine.ViewModels.Ioc.SimpleIoc<IProcessCaller>.Register(_renderProcesssCaller);
            Machine.ViewModels.Ioc.SimpleIoc<IBackgroundColor>.Register(BackgroudColor);

            ProgramFactory.BasePath = "Shaders/";
            ProgramFactory.Extension = "glsl";  

            var settings = new GLWpfControlSettings()
            {
                MajorVersion = 4,
                MinorVersion = 3
            };

            glViewCtrl.Start(settings);
            BackgroudColor.PropertyChanged += (s, e) => _backgroundColorChanged = true;
        }

        private void OnGlViewCtrlLoaded(object sender, RoutedEventArgs e)
        {
            //_program = ProgramFactory.Create<PointLightProgram>();
            //_program = ProgramFactory.Create<SpotLightProgram>();
            //_program = ProgramFactory.Create<DirectionalLightProgram>();
            _bckGrdProgram = ProgramFactory.Create<BackgroundProgram>();
            _program = ProgramFactory.Create<MultiLightProgram>();
            _program.Use();

            DataContext = new MainViewModel(_program);

            Camera = new Cameras.Camera();
            //Camera.SetBehavior(new Cameras.ThirdPersonBehavior());
            Camera.SetBehavior(new Cameras.TrackballBehavior());
            Camera.DefaultState.Position = Vector3.UnitZ * 1000;
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
            (_program as IDisposable)?.Dispose();
        }

        private void OnGlViewCtrlRender(TimeSpan obj)
        {
            Time(obj);
            GL.ClearColor(Color4.Aqua);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            if (_ctrlLoaded)
            {
                SetupPerspective();
                SetBackground(Projection);

                _program.Use();

                // calculate the MVP matrix and set it to the shaders uniform
                _program.viewPos.Set(Camera.State.Position);

                SetLight();

                var elements = (DataContext as MainViewModel).GetElements();

                foreach (var item in elements)
                {
                    if(!item.IsVisible) continue;

                    item.Draw(_program, Projection, View);
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
            Projection = Matrix4.CreatePerspectiveFieldOfView(_fov, (float)aspectRatio, _depthNear, _depthFar);
            View = Camera.GetCameraTransform();
        }

        private void SetLight()
        {
            //SetPointLight();
            //SetSpotLight();
            //SetDirectionalLight();
            SetMultiLight();
        }

        private void SetMultiLight()
        {
            _spotLight.position = Camera.State.Position;
            _spotLight.direction = Camera.State.LookAt;

            var p = _program as IMultiLight;

            //p.dirLight.Set(_directionalLight);
            p.spotLight.Set(_spotLight);
            p.dirLights.Set(_directionalLights);
            p.pointLights.Set(_pointlights);
        }

        private void SetPointLight()
        {
            _pointlight.position = Camera.State.Position;

            (_program as IPointLight).light.Set(_pointlight);
        }

        private void SetSpotLight()
        {
            _spotLight.position = Camera.State.Position;
            _spotLight.direction = Camera.State.LookAt;

            (_program as ISpotLight).light.Set(_spotLight);
        }

        private void SetDirectionalLight()
        {
            _directionalLight.direction= Camera.State.LookAt;

            (_program as IDirectionalLight).light.Set(_directionalLight);
        }

        private void SetBackground(Matrix4 projection)
        {
            _bckGrdProgram.Use();

            if (_background == null) SetupBackground();
            else if(IsWindowChanged()) UpdateBackgroundGeometry();

            _bckGrdProgram.ModelViewProjectionMatrix.Set(projection);

            _background.Draw();
        }

        private void SetupBackground()
        {
            GetBackgroundMesh(out var vertexes, out var indexes);

            _background = new M3DVG.Background(vertexes, indexes, _bckGrdProgram);
            UpdateLast();
        }

        private void UpdateBackgroundGeometry()
        {
            GetBackgroundMesh(out var vertexes, out var indexes);

            _background.UpdatePosition(vertexes);
            UpdateLast();
        }

        private Matrix4 UpdateBackgroundOrientation()
        {
            _lastView = View;

            return Matrix4.Invert(View);
        }

        private void GetBackgroundMesh(out Vertex[] vertexes, out uint[] indexes)
        {
            var builder = new MeshBuilder();
            var aspectRatio = ActualWidth / ActualHeight;

            builder.AddBackground(Camera.State.Position, 
                                  Camera.State.LookAt, 
                                  Camera.State.Up, 
                                  _fov, 
                                  _depthFar, 
                                  (float)aspectRatio,
                                  ToVector(BackgroudColor.Start),
                                  ToVector(BackgroudColor.Stop));
            builder.ToMesh(out var points, out indexes, out var normals);

            vertexes = Helpers.ElementBuilder.BuildVertexes(points, normals);
        }

        private bool IsViewStateChanged()
        {
            if(!_lastView.Equals(View)) return true;
            else return false;
        }

        private bool IsWindowChanged()
        {
            if (_lastWidth != ActualWidth) return true;
            else if (_lastHeight != ActualHeight) return true;
            else if (_backgroundColorChanged) return true;
            else return false;
        }

        private void UpdateLast()
        {
            _lastView = View;
            _lastWidth = ActualWidth;
            _lastHeight = ActualHeight;
            _backgroundColorChanged = false;
        }

        private static Vector3 ToVector(SWM.Color color) => new Vector3(color.R / 255.0f, color.G / 255.0f, color.B / 255.0f);
    }
}
