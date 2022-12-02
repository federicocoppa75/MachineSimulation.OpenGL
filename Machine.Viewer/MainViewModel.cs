using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMUI = Machine.ViewModels.UI;


namespace Machine.Viewer
{
    internal class MainViewModel : ViewModels.MainViewModel
    {
        public VMUI.IOptionProvider DataSource => ViewModels.Ioc.SimpleIoc<VMUI.IOptionProvider>.GetInstance();

        public MainViewModel() : base()
        {

        }
    }
}
