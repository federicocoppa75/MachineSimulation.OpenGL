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
        public VMUI.IStepsController StepController => ViewModels.Ioc.SimpleIoc<VMUI.IStepsController>.GetInstance();
        public VMUI.IStepsExecutionController StepsExecutionController => ViewModels.Ioc.SimpleIoc<VMUI.IStepsExecutionController>.GetInstance();
        public VMUI.IOptionProvider<VMUI.TimeSpanFactor> TimespanFactor => ViewModels.Ioc.SimpleIoc<VMUI.IOptionProvider<VMUI.TimeSpanFactor>>.GetInstance();
        public VMUI.IOptionProvider<VMUI.SampleTimeOption> SampleTimeOptions => ViewModels.Ioc.SimpleIoc<VMUI.IOptionProvider<VMUI.SampleTimeOption>>.GetInstance();


        public MainViewModel() : base()
        {

        }
    }
}
