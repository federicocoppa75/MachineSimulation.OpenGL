using Machine._3D.Views.Programs;
using Machine.ViewModels.Interfaces.MachineElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MDB = Machine.Data.Base;
using OTKM = OpenTK.Mathematics;
using M3DVG = Machine._3D.Views.Geometries;
using Machine.ViewModels.Interfaces.Links;
using System.CodeDom;
using System.Collections.Specialized;
using Machine.ViewModels.Messaging;
using Machine._3D.Views.Messages;
using MVMGEH = Machine.ViewModels.GeometryExtensions.Helpers;

namespace Machine._3D.Views.Elements
{
    internal class ElementViewModel
    {
        private IMachineElement _element;
        public IMachineElement Element
        {
            get => _element;
            set
            {
                if (!ReferenceEquals(_element, value))
                {
                    if (_element != null) DetachEvent(_element);
                    if (value != null) AttachEvent(value);
                    _element = value;
                }
            }
        }

        public M3DVG.Mesh Geometry { get; set; }

        public virtual bool IsVisible => IsVisibleBase() && IsModelFileNameValid();

        protected bool IsVisibleBase() => Element != null &&
                                          Element is IViewElementData ved &&
                                          ved.IsVisible;

        protected bool IsModelFileNameValid() => !string.IsNullOrEmpty(Element.ModelFile);

        public virtual void Draw(BaseProgram program, OTKM.Matrix4 projection, OTKM.Matrix4 view)
        {
            SetMaterial(program, Element.Color);
            OTKM.Matrix4 model = GetChainTransformation();
            program.ModelViewProjectionMatrix.Set(model * view * projection);

            Geometry.Draw(program);
        }

        private void AttachEvent(IMachineElement element)
        {
            var collection = element.Children as INotifyCollectionChanged;

            if (collection != null) collection.CollectionChanged += OnChildrenCollectionChanged;
        }

        private void DetachEvent(IMachineElement element)
        {
            var collection = element.Children as INotifyCollectionChanged;

            if (collection != null) collection.CollectionChanged -= OnChildrenCollectionChanged;
        }

        private void OnChildrenCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    ViewModels.Ioc.SimpleIoc<IMessenger>.GetInstance().Send(new AddChildToElementMessage() { Element = e.NewItems[0] as IMachineElement });
                    break;
                case NotifyCollectionChangedAction.Remove:
                    ViewModels.Ioc.SimpleIoc<IMessenger>.GetInstance().Send(new RemoveChildFromElementMessage() { Element = e.OldItems[0] as IMachineElement });
                    break;
                case NotifyCollectionChangedAction.Reset:
                    foreach (var item in Element.Children) ViewModels.Ioc.SimpleIoc<IMessenger>.GetInstance().Send(new RemoveChildFromElementMessage() { Element = item as IMachineElement });
                    break;
                case NotifyCollectionChangedAction.Replace:
                case NotifyCollectionChangedAction.Move:
                default:
                    throw new ArgumentOutOfRangeException($"Not supported operation ({e.Action})!");
                    break;
            }
        }

        protected OTKM.Matrix4 GetChainTransformation() => MVMGEH.ChainTransformtionHelper.GetChainTransformation(Element);

        protected static Material Convert(MDB.Color color)
        {
            return new Material()
            {
                ambient = new OTKM.Vector3(color.R / 255f, color.G / 255f, color.B / 255f),
                diffuse = new OTKM.Vector3(color.R / 255f, color.G / 255f, color.B / 255f),
                specular = new OTKM.Vector3(0.5f, 0.5f, 0.5f),
                shininess = 32
            };
        }

        protected static void SetMaterial(BaseProgram program, MDB.Color color)
        {
            var m = Convert(color);

            program.MaterialAmbient.Set(m.ambient);
            program.MaterialDiffuse.Set(m.diffuse);
            program.MaterialSpecular.Set(m.specular);
            program.MaterialShininess.Set(m.shininess);
        }
    }
}
