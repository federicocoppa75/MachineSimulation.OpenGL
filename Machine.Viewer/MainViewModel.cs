using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMUI = Machine.ViewModels.UI;
using MRI = MaterialRemove.Interfaces;
using MRIE = MaterialRemove.Interfaces.Enums;
using MVMIoc = Machine.ViewModels.Ioc;
using M3DVI = Machine._3D.Views.Interfaces;

namespace Machine.Viewer
{
    internal class MainViewModel : ViewModels.MainViewModel
    {
        public VMUI.IOptionProvider DataSource => ViewModels.Ioc.SimpleIoc<VMUI.IOptionProvider>.GetInstance();
        public VMUI.IStepsController StepController => ViewModels.Ioc.SimpleIoc<VMUI.IStepsController>.GetInstance();
        public VMUI.IStepsExecutionController StepsExecutionController => ViewModels.Ioc.SimpleIoc<VMUI.IStepsExecutionController>.GetInstance();
        public VMUI.IOptionProvider<VMUI.TimeSpanFactor> TimespanFactor => ViewModels.Ioc.SimpleIoc<VMUI.IOptionProvider<VMUI.TimeSpanFactor>>.GetInstance();
        public VMUI.IOptionProvider<VMUI.SampleTimeOption> SampleTimeOptions => ViewModels.Ioc.SimpleIoc<VMUI.IOptionProvider<VMUI.SampleTimeOption>>.GetInstance();
        public MRI.IMaterialRemoveData MaterialRemoveData => ViewModels.Ioc.SimpleIoc<MRI.IMaterialRemoveData>.GetInstance();
        public VMUI.IOptionProvider<MRIE.PanelFragment> PanelFragmentOptions => ViewModels.Ioc.SimpleIoc<VMUI.IOptionProvider<MRIE.PanelFragment>>.GetInstance();
        public VMUI.IOptionProvider<MRIE.SectionDivision> SectionDivisionOptions => ViewModels.Ioc.SimpleIoc<VMUI.IOptionProvider<MRIE.SectionDivision>>.GetInstance();
        public VMUI.IOptionProvider<string> PanelOuterMaterial => ViewModels.Ioc.SimpleIoc<VMUI.IOptionProvider<string>>.GetInstance("PanelOuterMaterial");
        public VMUI.IOptionProvider<string> PanelInnerMaterial => ViewModels.Ioc.SimpleIoc<VMUI.IOptionProvider<string>>.GetInstance("PanelInnerMaterial");
        public M3DVI.IBackgroundColor BackgroundColor => ViewModels.Ioc.SimpleIoc<M3DVI.IBackgroundColor>.GetInstance();


        public MainViewModel() : base()
        {
            MVMIoc.SimpleIoc<VMUI.IOptionProvider<MRIE.PanelFragment>>
                .Register(new VMUI.EnumOptionProxy<MRIE.PanelFragment>(() => MaterialRemoveData.PanelFragment,
                                                                        (v) => MaterialRemoveData.PanelFragment = v));

            MVMIoc.SimpleIoc<VMUI.IOptionProvider<MRIE.SectionDivision>>
                .Register(new VMUI.EnumOptionProxy<MRIE.SectionDivision>(() => MaterialRemoveData.SectionDivision,
                                                                        (v) => MaterialRemoveData.SectionDivision = v));
        }
    }
}
