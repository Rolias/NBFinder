using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Windows.Input;
using System.Windows.Threading;
using Syncor.MvvmLib;
using Syncor.NetBurnerFinder;

namespace NBFinderWpfDemo
{
    internal class MainWindowViewModel : ViewModelBase
    {
        private readonly Model _theModel;

        public MainWindowViewModel(Model theModel)
        {
            _theModel = theModel;
            _theModel.NbInfoFoundEvent += HandleNbInfoFoundEvent;
            FoundDevices = new ObservableCollection<IPAddress>();
            FoundConfigRecords = new ObservableCollection<NbConfigRecord>();
            InitTimer();
        }


        public ObservableCollection<IPAddress> FoundDevices { get; set; }
        public ObservableCollection<NbConfigRecord> FoundConfigRecords { get; set; }
        private readonly DispatcherTimer _timer = new DispatcherTimer();

        #region Timer
        void InitTimer()
        {
            const int FIND_TIMEOUT_MS = 3000;
            _timer.Interval = TimeSpan.FromMilliseconds(FIND_TIMEOUT_MS);
            _timer.Tick += HandleTimerTick;
        }

        void HandleTimerTick(object sender, EventArgs e)
        {
            _timer.Stop();
            if (IsFindInProgress)
            {
                StopFinding();
                CommandManager.InvalidateRequerySuggested();
                RaisePropertyChanged("");
            }
        }

        #endregion

        void HandleNbInfoFoundEvent(object sender, EventArgs e)
        {
            FoundDevices = new ObservableCollection<IPAddress>(_theModel.FoundNbInfo.Addresses);
            FoundConfigRecords = new ObservableCollection<NbConfigRecord>(_theModel.FoundNbInfo.ConfigRecords);
            RaisePropertyChanged("FoundDevices");
        }



        private IPAddress _activeIpAddress;
        public IPAddress ActiveIp
        {
            get { return _activeIpAddress; }
            set
            {
                _activeIpAddress = value;
                _activeConfigRecord = FoundConfigRecords.FirstOrDefault(x => x.ActiveIpAddress.Equals(_activeIpAddress));
                RaisePropertyChanged("ActiveIp");
                RaisePropertyChanged("");
            }
        }

        private NbConfigRecord _activeConfigRecord;

        public string DefaultIpAddress { get { return _activeConfigRecord.DefaultIpAddress.ToString(); } }
        public string ActiveIpAddress { get { return _activeConfigRecord.ActiveIpAddress.ToString(); } }
        public string DefaultIpMask { get { return _activeConfigRecord.DefaultIpMask.ToString(); } }
        public string ActiveIpMask { get { return _activeConfigRecord.ActiveIpMask.ToString(); } }
        public string DefaultIpGateway { get { return _activeConfigRecord.DefaultIpAddress.ToString(); } }
        public string ActiveIpGateway { get { return _activeConfigRecord.ActiveIpGateway.ToString(); } }
        public string DefaultIpDns { get { return _activeConfigRecord.DefaultDnsAddress.ToString(); } }
        public string ActiveIpDns { get { return _activeConfigRecord.ActiveIpDns.ToString(); } }

        public string ConfigRecordAsString { get { return _activeConfigRecord.ToString(); } }

        public bool QuietBoot { get { return _activeConfigRecord.QuietBoot; } }
        public int BootTimeSecs { get { return _activeConfigRecord.BootWaitSeconds; } }

        #region FindCommand

        private RelayCommand _findCommand;

        public ICommand FindCommand
        {
            get { return _findCommand ?? (_findCommand = new RelayCommand(x => FindCommandExecute(), x => FindCommandCanExecute)); }
        }

        private bool FindCommandCanExecute
        {
            get { return !_theModel.FindInProgress; } //replace with logic to tell if command can run 
        }

        private void FindCommandExecute()
        {
            _theModel.StartFinding();
            _timer.Start();

        }

        #endregion

        #region CancelFindCommand

        private RelayCommand _cancelFindCommand;

        public ICommand CancelFindCommand
        {
            get { return _cancelFindCommand ?? (_cancelFindCommand = new RelayCommand(x => CancelFindCommandExecute(), x => CancelFindCommandCanExecute)); }
        }

        private bool CancelFindCommandCanExecute
        {
            get { return IsFindInProgress; } //replace with logic to tell if command can run 
        }

        public bool IsFindInProgress
        {
            get { return _theModel.FindInProgress; }
        }


        private void CancelFindCommandExecute()
        {
            StopFinding();
        }

        public void StopFinding()
        {
            _theModel.StopFinding();
        }

        #endregion

    }
}
