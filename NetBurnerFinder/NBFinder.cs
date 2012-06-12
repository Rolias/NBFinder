using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;


namespace Syncor.NetBurnerFinder
{
    //Set ups the classes to send the UDP broadcast asking for config. records and asynchronously 
    //listens for response. If a NetBurner responds the IPAddress of that device is added
    //to the FoundDevices property. User should call Start() and Stop() in pairs. If Start()
    //is called but Stop() is not the UDP listener keeps running. The UDP client is closed
    //when the class is disposed. Supports the IDisposable interface. 
    //Using code can subscribe to our DeviceFoundEvent this class just reraises the event
    //raised by the UDP listener. 
    //Alternately user can poll the FoundDevices or FoundConfigRecords properties to see
    //found information.
    public class NBFinder : IDisposable
    {
        private  ResponseProcessor _responseProcessor;

        private  UdpClient _udpClient;

        public const int NB_PORT = 0x4E42; // NB in ASCII


        public void Start()
        {
            _udpClient = new UdpClient(NB_PORT);
            _responseProcessor = new ResponseProcessor(_udpClient);
            _responseProcessor.DeviceFoundEvent += HandleDeviceFoundEvent;
            RequestBroadcaster rb = new RequestBroadcaster(_udpClient);
            _responseProcessor.StartListening();//This runs asynchronously
            rb.BroadcastRequest();
        }

        public void Stop()
        {
            _responseProcessor.DeviceFoundEvent -= HandleDeviceFoundEvent;
            _udpClient.Close();
        }

        void HandleDeviceFoundEvent(object sender, EventArgs e)
        {
            RaiseDeviceFoundEvent();
        }

        public List<IPAddress> FoundDevices { get { return _responseProcessor.FoundNetBurners; } }
        public List<NbConfigRecord> FoundConfigRecords { get { return _responseProcessor.FoundConfigRecords; } }

        private void Close()
        {
            _udpClient.Close();
            _disposed = true;
        }

        [field: NonSerialized]
        public event EventHandler<EventArgs> DeviceFoundEvent = (sender, eventArgs) => { };

        private void RaiseDeviceFoundEvent()
        {
            EventArgs event_args = new EventArgs();
            DeviceFoundEvent(this, event_args);
        }

        #region Dispose Pattern

        private Boolean _disposed;

        private void Dispose(Boolean disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                //TODO: Managed cleanup code here, while managed refs still valid
            }
            Close();
        }

        public void Dispose()
        {
            // Call the private Dispose(bool) helper and indicate 
            // that we are explicitly disposing
            Dispose(true);

            // Tell the garbage collector that the object doesn't require any
            // cleanup when collected since Dispose was called explicitly.
            GC.SuppressFinalize(this);
        }

        #endregion

    }
}
