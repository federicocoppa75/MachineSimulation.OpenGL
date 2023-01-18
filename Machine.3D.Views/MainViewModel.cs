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
using MRI = MaterialRemove.Interfaces;
using OpenTK.Mathematics;
using Machine.ViewModels.GeometryExtensions.Math;
using Machine.ViewModels.Base.Implementation;

namespace Machine._3D.Views
{
    class MainViewModel : BaseElementsCollectionViewModel
    {
        private IProgram _program;
        Dictionary<string, M3DVG.Mesh> _meshMap = new Dictionary<string, M3DVG.Mesh>();
        Dictionary<IMachineElement, ElementViewModel> _elementMap = new Dictionary<IMachineElement, ElementViewModel>();

        public ObservableCollection<ILinkViewModel> LinearLinks { get; private set; } = new ObservableCollection<ILinkViewModel>();

        public IStepsExecutionController StepsExecutionController { get; protected set; }
        public IInvertersController InverterController { get; protected set; } = new InverterControllerViewModel();

        public MainViewModel(IProgram program) : base()
        {
            _program = program;

            Messenger.Register<AddChildToElementMessage>(this, msg => AddElement(msg.Element));
            Messenger.Register<RemoveChildFromElementMessage>(this, msg => RemoveElement(msg.Element));
            StepsExecutionController = Machine.ViewModels.Ioc.SimpleIoc<IStepsExecutionController>.TryGetInstance(out IStepsExecutionController controller) ? controller : null;
            Machine.ViewModels.Ioc.SimpleIoc<IInvertersController>.Register(InverterController);
        }

        public ElementViewModel[] GetElements() => _elementMap.Values.ToArray();

        public Box3 GetContentBox()
        {
            if(this.Kernel.Machines.Count == 0)
            {
                return new Box3();
            }
            else
            {
                var elements = _elementMap.Values.ToArray();
                var box = elements[0].GetBound();

                for (int i = 1; i < elements.Length; i++)
                {
                    box = box.Add(elements[i].GetBound());
                }

                return box;
            }
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
                if(pe is MRI.IPanel)
                {
                    AddSectionedPanelElement(pe);
                }
                else
                {
                    AddPanelElement(pe);
                }                
            }
            else if (element is IInjectedObject io)
            {
                AddInjectedElement(io);
            }
            else if (!string.IsNullOrEmpty(element.ModelFile))
            {
                AddSimpleElement(element);
            }

            if ((element.LinkToParent != null) && (element.LinkToParent.MoveType == Data.Enums.LinkMoveType.Linear))
            {
                LinearLinks.Add(element.LinkToParent);
            }

            foreach (var item in element.Children)
            {
                AddElement(item);
            }
        }

        private void RemoveElement(IMachineElement element)
        {
            if (!_elementMap.TryGetValue(element, out ElementViewModel vm)) return;

            _elementMap.Remove(element);

            if ((element.LinkToParent != null) && (element.LinkToParent.MoveType == Data.Enums.LinkMoveType.Linear))
            {
                LinearLinks.Remove(element.LinkToParent);
            }

            foreach (var item in element.Children)
            {
                RemoveElement(item);
            }

            if(_elementMap.Count == 0)
            {
                foreach (var item in _meshMap) item.Value.Dispose();
                _meshMap.Clear();
            }

            if(vm is Elements.SectionedPanel.PanelViewModel pvm) pvm.Dispose();
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
            var coneModelFile = (element as Machine.ViewModels.Interfaces.Bridge.IToolDataProxy).Tool.ConeModelFile;
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
            var e = new AngularTransmissionViewModel()
            {
                Element = at,
                Geometry = GetElementGeometry(at.BodyModelFile)
            };

            _elementMap[at] = e;
        }

        private void AddPanelElement(IPanelElement pe)
        {
            var pvm = new Elements.PanelViewModel()
            {
                Element = pe,
                Geometry = GetPanelGeometry(pe)
            };

            _elementMap[pe] = pvm;
        }

        private void AddSectionedPanelElement(IPanelElement pe)
        {
            var pvm = new Elements.SectionedPanel.PanelViewModel() { Element = pe };

            pvm.Initialize();
            _elementMap[pe] = pvm;
        }

        private void AddInjectedElement(IInjectedObject io)
        {
            // pezza per rimediare al fatto che la spina, quando viene creata, non è figlia dell'inseritore
            // l'effetto è quello di vedere una spina nello spazio
            // va sistemato in Machine.ViewModels.MachineElements
            if (io.Parent == null) return;

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
