using System;
using System.Windows;

namespace NBFinderWpfDemo
{
    public partial class App : Application
    {
        private MainWindowViewModel _mainWindowVm;

        protected override void OnStartup(StartupEventArgs e)
        {
            try
            {
                base.OnStartup(e);

                MainWindow window = new MainWindow();
                var main_model = new Model();
                _mainWindowVm = new MainWindowViewModel(main_model);
                window.DataContext = _mainWindowVm;
                window.Closed += ApplicationQuitting;
                window.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Current.Shutdown();
            }
        }

        void ApplicationQuitting(object sender, EventArgs e)
        {
            if (_mainWindowVm.IsFindInProgress)
            {
                _mainWindowVm.StopFinding();
            }
            Current.Shutdown();
        }


    }
}
