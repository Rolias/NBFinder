using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Threading;
using Syncor.NetBurnerFinder;
namespace NBFinderWpfDemo
{
    public struct NbInfo
    {
        public List<IPAddress> Addresses { get; set; }
        public List<NbConfigRecord> ConfigRecords { get; set; }
    }

    public class Model
    {
        //public List<NbInfo> FoundNbInfo = new List<NbInfo>(); 
        private readonly NBFinder _nbFinder = new NBFinder();
        readonly BackgroundWorker _bgWorker = new BackgroundWorker();

        public NbInfo FoundNbInfo { get; set; }

        public Model()
        {
            _bgWorker.WorkerSupportsCancellation = true;
            _bgWorker.WorkerReportsProgress = true;
            _bgWorker.ProgressChanged += HandleProgressChanged;
            _bgWorker.DoWork += FindNbWork;
            FindInProgress = false;
        }

        void HandleProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            FoundNbInfo = (NbInfo)e.UserState;
            RaiseNbInfoFoundEvent();       
        }

        public bool FindInProgress { get; set; }

        public void StartFinding()
        {
            _bgWorker.RunWorkerAsync();
            FindInProgress = true;
        }

        public void StopFinding()
        {
            _bgWorker.CancelAsync();
            FindInProgress = false;
        }


        void FindNbWork(object sender, DoWorkEventArgs e)
        {
            const int CHECK_FOR_CANCEL_INTERVAL_MS = 100;
            BackgroundWorker worker = sender as BackgroundWorker;
            if (worker == null) return;
            FindNetBurners();
            while (worker.CancellationPending == false)
            {
                Thread.Sleep(CHECK_FOR_CANCEL_INTERVAL_MS);
            }
            StopFindingNetBurners();
        }

        void FindNetBurners()
        {
            _nbFinder.DeviceFoundEvent += HandleNbFound;
            _nbFinder.Start();
        }

        void StopFindingNetBurners()
        {
            _nbFinder.Stop();
            _nbFinder.DeviceFoundEvent -= HandleNbFound;
        }


        void HandleNbFound(object sender, EventArgs e)
        {
            NBFinder nb = sender as NBFinder;
            if (nb == null) return;
            NbInfo nb_info = new NbInfo { Addresses = nb.FoundDevices, ConfigRecords = nb.FoundConfigRecords };
            _bgWorker.ReportProgress(0, nb_info);
        }

        [field: NonSerialized]
        public event EventHandler<EventArgs> NbInfoFoundEvent = (sender, eventArgs) => { };

        private void RaiseNbInfoFoundEvent()
        {
            EventArgs event_args = new EventArgs();
            NbInfoFoundEvent(this, event_args);
        }
    }
}
