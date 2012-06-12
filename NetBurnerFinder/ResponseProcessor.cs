using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Linq;

namespace Syncor.NetBurnerFinder
{
    //Set up an asynchronous Listener that listens for the special config packet that 
    //all NetBurners will send when they receive the broadcast request for it. 
    //We aren't actually going to look at anything in the response except for the 
    //validation string that lets us know the response is from a NetBurner, then we
    //will capture the IP address of the device that sent it and put it in a List<>
    //The listener will keep running until the code that created the NBFinder class
    // closes it or disposes of it. 
    internal class ResponseProcessor
    {
        private readonly UdpClient _udpClient;

        public ResponseProcessor(UdpClient udpClient)
        {
            _udpClient = udpClient;
            FoundNetBurners = new List<IPAddress>();
            FoundConfigRecords = new List<NbConfigRecord>();
        }

        public List<IPAddress> FoundNetBurners { get; private set; }
        public List<NbConfigRecord> FoundConfigRecords { get; private set; }


        public void StartListening()
        {
            try
            {
                _udpClient.BeginReceive(Receive, new object());
            }
            catch (ObjectDisposedException)
            {
                //let this die silently when the socket gets disposed by user.
            }

        }

        private void Receive(IAsyncResult ar)
        {
            try
            {

                IPEndPoint ip = new IPEndPoint(IPAddress.Any, NBFinder.NB_PORT);
                byte[] bytes = _udpClient.EndReceive(ar, ref ip);
                string recv_msg = Encoding.ASCII.GetString(bytes);
                if (IsNetBurnerResponse(recv_msg))
                {
                    FoundNetBurners.Add(ip.Address);
                    AddConfigRecord(bytes);
                    RaiseDeviceFoundEvent();

                }


                StartListening();
            }
            catch (ObjectDisposedException)
            {
                //Do nothing this asynch read loop will end since we don't call StartListening.
                //This exception happens when user closes socket. The way we expect to quit.
            }

        }

        private void AddConfigRecord(byte[] bytes)
        {
            var config_byte_data = bytes.Skip(5).ToArray();
            NbConfigRecord config_record = new NbConfigRecord();
            config_record.SetConfigRecord(config_byte_data);
            FoundConfigRecords.Add(config_record);
        }


        private static bool IsNetBurnerResponse(string incomingMsg)
        {
            const string RESPONSE_EXPECTED = "NETB";
            const int START_POS = 0;
            return incomingMsg.Substring(START_POS, RESPONSE_EXPECTED.Length) == RESPONSE_EXPECTED;
        }


        [field: NonSerialized]
        public event EventHandler<EventArgs> DeviceFoundEvent = (sender, eventArgs) => { };

        private void RaiseDeviceFoundEvent()
        {
            EventArgs event_args = new EventArgs();
            DeviceFoundEvent(this, event_args);
        }  

    }


}
