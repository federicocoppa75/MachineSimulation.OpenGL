using Machine._3D.Views.Elements;
using Machine._3D.Views.Helpers;
using Machine._3D.Views.Messages;
using Machine._3D.Views.Programs;
using Machine._3D.Views.ViewModels;
using Machine.ViewModels;
using Machine.ViewModels.Interfaces.Insertions;
using Machine.ViewModels.Interfaces.Links;
using Machine.ViewModels.Interfaces.MachineElements;
using Machine.ViewModels.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using M3DVG = Machine._3D.Views.Geometries;
using OTKM = OpenTK.Mathematics;

namespace Machine._3D.Views
{
    class MainViewModel : BaseElementsCollectionViewModel
    {
        private BaseProgram _program;
        Dictionary<string, M3DVG.Mesh> _meshMap = new Dictionary<string, M3DVG.Mesh>();
        Dictionary<IMachineElement, ElementViewModel> _elementMap = new Dictionary<IMachineElement, ElementViewModel>();

        public ElementViewModel[] GetElements() => _elementMap.Values.ToArray();

        public IStepsExecutionController StepsExecutionController { get; protected set; }
        public IInvertersController InverterController { get; protected set; } = new InverterControllerViewModel();


        public MainViewModel(BaseProgram program) : base()
        {
            _program = program;

            Messenger.Register<AddChildToElementMessage>(this, msg => AddElement(msg.Element));
            Messenger.Register<RemoveChildFromElementMessage>(this, msg => RemoveElement(msg.Element));
            StepsExecutionController = Machine.ViewModels.Ioc.SimpleIoc<IStepsExecutionController>.TryGetInstance(out IStepsExecutionController controller) ? controller : null;
            Machine.ViewModels.Ioc.SimpleIoc<IInvertersController>.Register(InverterController);
        }

        #region BaseElementsCollectionViewModel abstracts
        protected override void AddElement(IEnumerable<IMachineElement> elements)
        {
            foreach (var item in elements)
            {
                AddElement(item);
            }
        }

        protected override void Clear()
        {
            foreach (var item in _meshMap.Values) item.Dispose();
             _meshMap.Clear();
            _elementMap.Clear();
        }

        protected override void RemoveElement(IEnumerable<IMachineElement> elements)
        {
            foreach (var item in elements)
            {
                RemoveElement(item);
            }
        }
        #endregion

        private void AddElement(IMachineElement element) 
        {
            if (element is IAngularTransmission at)
            {
                AddAngularTransmission(at);
            }
            else if (element is IToolElement te)
            {
                AddToolElement(te);
            }
            else if (element is IPanelElement pe)
            {
                AddPanelElement(pe);
            }
            else if (element is IInjectedObject io)
            {
                AddInjectedElement(io);
            }
            else if (!string.IsNullOrEmpty(element.ModelFile))
            {
                AddSimpleElement(element);
            }

            foreach (var item in element.Children)
            {
                AddElement(item);
            }
        }

        private void RemoveElement(IMachineElement element)
        {
            _elementMap.Remove(element);

            foreach (var item in element.Children)
            {
                RemoveElement(item);
            }

            if(_elementMap.Count == 0)
            {
                foreach (var item in _meshMap) item.Value.Dispose();
                _meshMap.Clear();
            }
        }

        private void AddSimpleElement(IMachineElement element)
        {
            var e = new ElementViewModel() 
            {
                Element = element,
                Geometry = GetElementGeometry(element.ModelFile)
            };

            _elementMap[element] = e;
        }

        private void AddToolElement(IToolElement element)
        {
            var coneModelFile = (element as Machine.ViewModels.MachineElements.ToolViewModel).ConeModelFile;
            var e = new ToolViewModel() 
            {
                Element = element,
                Geometry = GetToolGeometry(element),
                ConeGeometry = GetElementGeometry(coneModelFile)
            };

            _elementMap[element] = e;
        }

        private void AddAngularTransmission(IAngularTransmission at)
        {
            var bodyModelFile = (at as Machine.ViewModels.MachineElements.AngularTransmissionViewModel).BodyModelFile;
            var e = new AngularTransmissionViewModel()
            {
                Element = at,
                Geometry = GetElementGeometry(bodyModelFile)
            };

            _elementMap[at] = e;
        }

        private void AddPanelElement(IPanelElement pe)
        {
            var pvm = new PanelViewModel()
            {
                Element = pe,
                Geometry = GetPanelGeometry(pe)
            };

            _elementMap[pe] = pvm;
        }

        private void AddInjectedElement(IInjectedObject io)
        {
            var ivm = new InsertedViewModel()
            {
                Element = io,
                Geometry = GetInsertedGeometry(io)
            };

            _elementMap[io] = ivm;
        }

        private M3DVG.Mesh GetInsertedGeometry(IInjectedObject io)
        {
            ElementBuilder.Build(io, out M3DVG.Vertex[] vertexes, out uint[] indexes);

            return new M3DVG.Mesh(vertexes, indexes, _program);
        }

        private M3DVG.Mesh GetPanelGeometry(IPanelElement pe)
        {
            ElementBuilder.Build(pe, out M3DVG.Vertex[] vertexes, out uint[] indexes);

            return new M3DVG.Mesh(vertexes, indexes, _program);
        }

        private M3DVG.Mesh GetToolGeometry(IMachineElement element)
        {
            if (_meshMap.TryGetValue(element.Name, out M3DVG.Mesh mesh))
            {
                return mesh;
            }
            else
            {
                ElementBuilder.Build(element, out M3DVG.Vertex[] vertexes, out uint[] indexes);

                var m = new M3DVG.Mesh(vertexes, indexes, _program);
                _meshMap.Add(element.Name, m);

                return m;
            }
        }

        private M3DVG.Mesh GetElementGeometry(string modelFile)
        {
            if (!string.IsNullOrEmpty(modelFile))
            {
                if (_meshMap.TryGetValue(modelFile, out M3DVG.Mesh mesh))
                {
                    return mesh;
                }
                else
                {
                    MeshFileReader.Read(modelFile, out M3DVG.Vertex[] vertexes, out uint[] indexes);

                    var m = new M3DVG.Mesh(vertexes, indexes, _program);

                    _meshMap.Add(modelFile, m);

                    return m;
                }
            }
            else
            {
                return null;
            }
        }
    }
}
