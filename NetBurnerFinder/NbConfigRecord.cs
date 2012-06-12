using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using EnumHelper;
using NetBurnerFinder;

namespace Syncor.NetBurnerFinder
{
    //This is the lower level "raw" NetBurner view of the config record. Most users
    //Should use the more C# friendly ConfigRecord view. 
    public class NbConfigRecord
    {
        public Int32 Length { get; set; }
        public IPAddress DefaultIpAddress { get; set; }
        public IPAddress DefaultIpMask { get; set; }
        public IPAddress DefaultIpGateway { get; set;  }

        public IPAddress IpTftpServer { get; set; }
        public Int32 BaudRate { get; set; }
        public byte BootWaitSeconds { get; set; }
        public bool EnableBootToApplication { get; set; }
        public byte ExceptionAction { get; set; }
        public string TftpFileToLoad { get; set; }
        public List<byte> MacAddress { get; set; }
        public byte NbSerialPort { get; set; }
        public IPAddress DefaultDnsAddress { get; set; }
        public List<byte> CoreMacAddress { get; set; }
        public NbInterfaceType InterfaceType { get; set; }
        public bool ConnectedDirectlyTx { get; set; }

        public IPAddress ActiveIpAddress { get; set; }
        public IPAddress ActiveIpMask { get; set; }
        public IPAddress ActiveIpGateway { get; set; }
        public IPAddress ActiveIpDns { get; set; }

        public bool QuietBoot { get; set; }

        public Int32 CheckSum { get; set; }
        public bool ValidRecordSet { get; private set; }

        public override string ToString()
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("Length:         {0}\n", Length);
                sb.AppendFormat("Default IP:     {0}\n", DefaultIpAddress);
                sb.AppendFormat("Default Mask:   {0}\n", DefaultIpMask);
                sb.AppendFormat("Default Gateway:{0}\n", DefaultIpGateway);
                sb.AppendFormat("IP TFTP Server: {0}\n", IpTftpServer);
                sb.AppendFormat("Baud Rate:      {0}\n", BaudRate);
                sb.AppendFormat("Boot Wait(secs):{0}\n", BootWaitSeconds);
                sb.AppendFormat("Boot to app    :{0}\n", EnableBootToApplication);
                sb.AppendFormat("TFTP Filename  :{0}\n", TftpFileToLoad.Substring(0, 79));
                sb.AppendFormat("MAC Address:    {0}\n", BitConverter.ToString(MacAddress.ToArray()));
                sb.AppendFormat("Serial Port:    {0}\n", NbSerialPort);
                sb.AppendFormat("DNS             {0}\n", DefaultDnsAddress);
                sb.AppendFormat("Core MAC Addr:  {0}\n", BitConverter.ToString(MacAddress.ToArray()));
                sb.AppendFormat("Interface Type: {0}\n", InterfaceType.GetDescription());
                sb.AppendFormat("Connect Direct: {0}\n", ConnectedDirectlyTx);
                sb.AppendFormat("Active IP:      {0}\n", ActiveIpAddress);
                sb.AppendFormat("Active Mask:    {0}\n", ActiveIpMask);
                sb.AppendFormat("Active Gate:    {0}\n", ActiveIpGateway);
                sb.AppendFormat("Active DNS:     {0}\n", ActiveIpDns);
                sb.AppendFormat("Quiet Boot:     {0}\n", QuietBoot);
                sb.AppendFormat("Checksum:       {0}\n", CheckSum);
                return sb.ToString();
            }
            catch (Exception)
            {
                return "Invalid data in configuration record.";
            }
        }

        //static string GetIpFormattedAddress(List<byte> data)
        //{
        //    return String.Format("{0}.{1}.{2}.{3}", data[0], data[1], data[2], data[3]);
        //}

        public void SetConfigRecord(byte[] byteData)
        {
            try
            {
                Length = byteData.GetInt32();
                byteData = byteData.Skip(4).ToArray();

                DefaultIpAddress = byteData.GetIpAddress();
                byteData = byteData.Skip(4).ToArray();

                DefaultIpMask = byteData.GetIpAddress();
                byteData = byteData.Skip(4).ToArray();

                DefaultIpGateway = byteData.GetIpAddress();
                byteData = byteData.Skip(4).ToArray();

                IpTftpServer = byteData.GetIpAddress();
                byteData = byteData.Skip(4).ToArray();

                BaudRate = byteData.GetInt32();
                byteData = byteData.Skip(4).ToArray();

                BootWaitSeconds = byteData[0];
                byteData = byteData.Skip(1).ToArray();

                EnableBootToApplication = byteData[0] != 0;
                byteData = byteData.Skip(1).ToArray();

                ExceptionAction = byteData[0];
                byteData = byteData.Skip(1).ToArray();

                TftpFileToLoad = byteData.GetString();
                byteData = byteData.Skip(80).ToArray();

                MacAddress = byteData.GetSixByteAddress();
                byteData = byteData.Skip(6).ToArray();

                NbSerialPort = byteData[0];
                byteData = byteData.Skip(1).ToArray();

                DefaultDnsAddress = byteData.GetIpAddress();
                byteData = byteData.Skip(4).ToArray();

                CoreMacAddress = byteData.GetSixByteAddress(); ;
                byteData = byteData.Skip(6).ToArray();

                InterfaceType = (NbInterfaceType)byteData[0];
                byteData = byteData.Skip(1).ToArray();

                ConnectedDirectlyTx = byteData[0] != 0xAA;
                byteData = byteData.Skip(1).ToArray();

                ActiveIpAddress = byteData.GetIpAddress();
                byteData = byteData.Skip(4).ToArray();

                ActiveIpMask = byteData.GetIpAddress();
                byteData = byteData.Skip(4).ToArray();

                ActiveIpGateway = byteData.GetIpAddress();
                byteData = byteData.Skip(4).ToArray();

                ActiveIpDns = byteData.GetIpAddress();
                byteData = byteData.Skip(4).ToArray();

                byteData = byteData.Skip(3).ToArray(); //Unused 3 bytes.

                QuietBoot = byteData[0] != 0;
                byteData = byteData.Skip(1).ToArray();

                CheckSum = byteData.GetUInt16();

                ValidRecordSet = true;
            }
            catch (Exception)
            {
                ValidRecordSet = false;
            }

        }

    }
}
