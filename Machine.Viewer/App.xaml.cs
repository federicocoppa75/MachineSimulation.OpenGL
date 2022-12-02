using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

using MVMUI = Machine.ViewModels.UI;
using MDFJ = Machine.DataSource.File.Json;
using MVUI = Machine.Views.UI;
using MW32 = Microsoft.Win32;
using MSFM = Machine.StepsSource.File.Msteps;
using MSFI = Machine.StepsSource.File.Iso;
using MVMI = Machine.ViewModels.Interfaces;
using MSVMI = Machine.Steps.ViewModels.Interfaces;
using MSVME = Machine.Steps.ViewModels.Extensions;
using MVMB = Machine.ViewModels.Base;
using MVMIF = Machine.ViewModels.Interfaces.Factories;
using MRMB = MaterialRemove.Machine.Bridge;
using MRI = MaterialRemove.Interfaces;
using MVMII = Machine.ViewModels.Interfaces.Insertions;
using MVMIns = Machine.ViewModels.Insertions;
using MVMM = Machine.ViewModels.Messaging;
using Machine.ViewModels;

namespace Machine.Viewer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            ViewModels.Ioc.SimpleIoc<MVMI.IKernelViewModel>.Register<KernelViewModel>();
            ViewModels.Ioc.SimpleIoc<MVMM.IMessenger>.Register<MVMM.Messenger>();
            ViewModels.Ioc.SimpleIoc<MVMUI.IDataSource>.Register<MDFJ.DataSource>("File.JSON");
            ViewModels.Ioc.SimpleIoc<MVMUI.IFileDialog>.Register<MVUI.FileDialog<MW32.OpenFileDialog>>("OpenFile");
            ViewModels.Ioc.SimpleIoc<MVMUI.IFileDialog>.Register<MVUI.FileDialog<MW32.SaveFileDialog>>("SaveFile");
            ViewModels.Ioc.SimpleIoc<MVMUI.IOptionProvider>.Register(new MVMUI.RegisteredOptionProvider<MVMUI.IDataSource>() { Name = "DataSource" });
            ViewModels.Ioc.SimpleIoc<MVMUI.IListDialog>.Register<MVUI.ListDialog>();
            ViewModels.Ioc.SimpleIoc<MVMUI.IStepsSource>.Register<MSFM.StepsSource>("File.msteps");
            ViewModels.Ioc.SimpleIoc<MVMUI.IStepsSource>.Register<MSFI.StepsSource>("File.iso");
            ViewModels.Ioc.SimpleIoc<MSVMI.IDurationProvider>.Register<MSVME.DurationProvider>();
            ViewModels.Ioc.SimpleIoc<MSVMI.IBackStepActionFactory>.Register<MSVME.BackStepActionFactory>();
            ViewModels.Ioc.SimpleIoc<MSVMI.IActionExecuter>.Register<MSVME.ActionExecuter>();
            ViewModels.Ioc.SimpleIoc<MVMUI.IDispatcherHelper>.Register<MVUI.DispatcherHelper>();
            ViewModels.Ioc.SimpleIoc<MVMIF.IPanelElementFactory>.Register<Machine.ViewModels.Factories.PanelViewModelFactory>();
            ViewModels.Ioc.SimpleIoc<MVMI.Tools.IToolObserverProvider>.Register<MRMB.ToolsObserverProvider>();
            ViewModels.Ioc.SimpleIoc<MRI.IMaterialRemoveData>.Register<MRMB.MaterialRemoveData>();
            ViewModels.Ioc.SimpleIoc<MVMII.IInsertionsSinkProvider>.Register<MVMIns.InsertionsSinkProvider>();
            //ViewModels.Ioc.SimpleIoc<MVMI.Probing.IProbeFactory>.Register<ViewModels.Probing.ProbeFactory>();
            ViewModels.Ioc.SimpleIoc<MVMUI.IApplicationInformationProvider>.Register(new MVMUI.ApplicationInformationProvider(MVMUI.ApplicationType.MachineViewer));
            ViewModels.Ioc.SimpleIoc<MVMB.ICommandExceptionObserver>.Register<MVUI.SimpleCommandExceptionObserver>();
            ViewModels.Ioc.SimpleIoc<MVMUI.IExceptionObserver>.Register<MVUI.SimpleExceptionObserver>();
            ViewModels.Ioc.SimpleIoc<MVMI.Links.ILinkMovementManager>.Register<MSVME.LinkMovementManager>();
        }
    }
}
