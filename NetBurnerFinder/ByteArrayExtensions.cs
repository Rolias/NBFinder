using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace NetBurnerFinder
{
   static public class ByteArrayExtensions
    {

       public static string GetString(this byte[] byteData)
        {
            return Encoding.ASCII.GetString(byteData);
        }


       public static IPAddress GetIpAddress(this IEnumerable<byte> byteData)
       {
           return new IPAddress(byteData.Take(4).ToArray());
       }
       public static List<byte> GetFourByteAddress(this byte[] byteData)
        {
            int byte_index = 0;
            return new List<byte> { byteData[byte_index], byteData[++byte_index], byteData[++byte_index], byteData[++byte_index] };

        }

       public static List<byte> GetSixByteAddress(this byte[] byteData)
        {
            int byte_index = 0;
            return new List<byte> { byteData[byte_index], byteData[++byte_index], byteData[++byte_index], byteData[++byte_index], byteData[++byte_index], byteData[++byte_index] };

        }


       public static int GetInt32(this byte[] byteData)
        {
            Int32 raw_data = BitConverter.ToInt32(byteData, 0);
            return IPAddress.NetworkToHostOrder(raw_data);
        }


       public static int GetUInt16(this byte[] byteData)
        {
            UInt16 raw_data = BitConverter.ToUInt16(byteData, 0);
            int swapped = IPAddress.NetworkToHostOrder(raw_data);
            return (UInt16)(swapped >> 16);
        }

    }
}
