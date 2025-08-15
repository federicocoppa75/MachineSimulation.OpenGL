using System;
using System.Collections.Generic;
using System.Linq;
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
using MVUI = Machine.Views.UI;
using MVH = Machine.Views.Helpers;
using System.ComponentModel;

namespace Machine.Viewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
            MVUI.DispatcherHelper.Initialize();

            UpdateFromSettings();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            SaveToSettings();
            Settings.Default.Save();
        }

        private void SaveToSettings()
        {
            var vm = DataContext as MainViewModel;

            Settings.Default.AutoStepOver = vm.StepsExecutionController.AutoStepOver;
            Settings.Default.DynamicTransition = vm.StepsExecutionController.DynamicTransition;
            Settings.Default.TimespanFactor = vm.StepsExecutionController.TimeSpanFactor.ToString();
            Settings.Default.MaterialRemove = vm.MaterialRemoveData.Enable;
            Settings.Default.MinimumSampleTime = vm.StepsExecutionController.MinimumSampleTime.ToString();
            Settings.Default.PanelOuterMaterial = vm.PanelOuterMaterial.Value;
            Settings.Default.PanelInnerMaterial = vm.PanelInnerMaterial.Value;
            Settings.Default.PanelFragmentType = vm.MaterialRemoveData.PanelFragment.ToString();
            Settings.Default.SectionDivision = vm.MaterialRemoveData.SectionDivision.ToString();
            Settings.Default.BackgroundColorStart = MVH.MainWindowHelper.Convert(vm.BackgroundColor.Start);
            Settings.Default.BackgroundColorStop = MVH.MainWindowHelper.Convert(vm.BackgroundColor.Stop);
            Settings.Default.SectionsX100mm = vm.MaterialRemoveData.SectionsX100mm.ToString();
            Settings.Default.ParallelComputing = vm.MaterialRemoveData.ParallelComputing;
        }

        private void UpdateFromSettings()
        {
            var vm = DataContext as MainViewModel;

            vm.StepsExecutionController.AutoStepOver = Settings.Default.AutoStepOver;
            vm.StepsExecutionController.DynamicTransition = Settings.Default.DynamicTransition;
            vm.TimespanFactor.TryToParse(Settings.Default.TimespanFactor);
            vm.MaterialRemoveData.Enable = Settings.Default.MaterialRemove;
            vm.SampleTimeOptions.TryToParse(Settings.Default.MinimumSampleTime);
            vm.PanelOuterMaterial.TryToParse(Settings.Default.PanelOuterMaterial);
            vm.PanelInnerMaterial.TryToParse(Settings.Default.PanelInnerMaterial);
            vm.PanelFragmentOptions.TryToParse(Settings.Default.PanelFragmentType);
            vm.SectionDivisionOptions.TryToParse(Settings.Default.SectionDivision);
            vm.BackgroundColor.Start = MVH.MainWindowHelper.Convert(Settings.Default.BackgroundColorStart);
            vm.BackgroundColor.Stop = MVH.MainWindowHelper.Convert(Settings.Default.BackgroundColorStop);
            vm.SectionsPer100mmOptions.TryToParse(Settings.Default.SectionsX100mm);
            vm.MaterialRemoveData.ParallelComputing = Settings.Default.ParallelComputing;
        }

    }
}
