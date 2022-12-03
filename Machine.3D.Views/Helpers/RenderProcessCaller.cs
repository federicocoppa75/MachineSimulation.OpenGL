using Machine.ViewModels.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machine._3D.Views.Helpers
{
    internal class RenderProcessCaller : IProcessCaller
    {
        private bool disposedValue;
        object _lockObj = new object();

        public bool Enable { get => true; set => throw new NotImplementedException(); }

        private event EventHandler<DateTime> _processRequest;
        public event EventHandler<DateTime> ProcessRequest
        {
            add 
            { 
                lock(_lockObj)
                {
                    _processRequest += value;
                }                 
            }
            remove 
            {
                lock(_lockObj) 
                {
                    _processRequest -= value;
                }                
            }
        }

        public void NotifyRendered()
        {
            lock(_lockObj)
            {
                _processRequest?.Invoke(this, DateTime.Now);
            }            
        }
     }
}
