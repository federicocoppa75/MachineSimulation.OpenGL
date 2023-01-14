using Machine._3D.Views.Programs;
using Machine.ViewModels.Interfaces.MachineElements;
using MaterialRemove.Interfaces;
using OpenTK.Mathematics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machine._3D.Views.Elements.SectionedPanel
{
    internal class PanelViewModel : ElementViewModel, IDisposable
    {
        private InnerPanelViewModel _innerPanel = new InnerPanelViewModel();
        private OuterPanelViewModel _outerPanel = new OuterPanelViewModel();
        private bool disposedValue;

        public override bool IsVisible => IsVisibleBase();

        public override void Draw(IProgram program, Matrix4 projection, Matrix4 view)
        {
            if(disposedValue) return;
            _innerPanel.Draw(program, projection, view);
            _outerPanel.Draw(program, projection, view);
        }

        public void Initialize()
        {
            _innerPanel.Element = Element;
            _outerPanel.Element = Element;
            _innerPanel.Initialize();
            _outerPanel.Initialize();

            if(Element is IPanel panel) 
            {
                if(panel.Sections is INotifyCollectionChanged ncc)
                {
                    ncc.CollectionChanged += OnSectionsCollectionChanged;
                }
            }
        }

        private void OnSectionsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    AddElements(e.NewItems);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    RemoveElements(e.OldItems);
                    break;
                case NotifyCollectionChangedAction.Replace:
                case NotifyCollectionChangedAction.Move:
                case NotifyCollectionChangedAction.Reset:
                default:
                    throw new InvalidOperationException();
            }
        }

        private void AddElements(IList values) 
        { 
            foreach (var item in values) 
            {
                if(item is IPanelSection section) 
                {
                    _innerPanel.Add(section);
                    _outerPanel.Add(section);
                }
            }
        }
        
        private void RemoveElements(IList values) 
        {
            foreach (var item in values)
            {
                if (item is IPanelSection section)
                {
                    _innerPanel.Remove(section);
                    _outerPanel.Remove(section);
                }
            }
        }

        #region IDispose
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                disposedValue = true;

                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                    _innerPanel.Dispose();
                    _outerPanel.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null                
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
