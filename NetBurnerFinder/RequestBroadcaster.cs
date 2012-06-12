using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Syncor.NetBurnerFinder
{
    //UDP Broadcast to all devices on our subnet the special NetBurner packet
    //that makes any NB module respond with its configuration record. 
    internal class RequestBroadcaster
    {
        private readonly UdpClient _udpClient;

        public RequestBroadcaster(UdpClient udpClient)
        {
            _udpClient = udpClient;
        }

        public void BroadcastRequest()
        {
            IPEndPoint ip = new IPEndPoint(ThisNetworkSegmentAddress, NBFinder.NB_PORT);
            const string NB_REQ_STR = "BURNR";
            byte[] nb_req = Encoding.ASCII.GetBytes(NB_REQ_STR);
            _udpClient.Send(nb_req, nb_req.Length, ip);
        }

        private static IPAddress ThisNetworkSegmentAddress
        {
            get { return IPAddress.Parse("255.255.255.255"); }
        }
    }

} 
