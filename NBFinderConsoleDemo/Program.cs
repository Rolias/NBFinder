using System;
using Syncor.NetBurnerFinder;

namespace NBFinderConsoleDemo
{
    class Program
    {
        private static int _numDevicesFound = 0;
        static void Main(string[] args)
        {
            try
            {
                //This is a very simplistic approach to finding devices. I just broadcast
                //the request and wait for responses. It keeps listening until the user 
                //hits return. It never rebroadcasts the message so turning on a NB after
                //starting the app won't find the newly powered on device. 
                // You could take a much more sophisticated approach
                // to checking the FoundDevices property and the WPF Demo shows a multi-threaded
                // approach.
                //Also if you know there will only be one NB on the LAN you could write a find that 
                //just stops after finding the first unit. This has the advantage of being almost 
                //instantaneous to the user. 
                using (NBFinder nb_find = new NBFinder())
                {
                    nb_find.Start();
                    nb_find.DeviceFoundEvent += HandleDeviceFoundEvent;
                    Console.WriteLine("Searching for devices ...");
                    Console.WriteLine("Press return to stop searching...");
                    Console.WriteLine();
                    Console.WriteLine("Found devices:");
                    Console.ReadLine();
                    nb_find.Stop();
                    //Alternately you could wait some amount of time and then iterate through
                    //the found devices.
                    //Thread.Sleep(5000); //Let it search for x seconds

                    //Console.WriteLine();
                    //nb_find.FoundDevices.ForEach(Console.WriteLine);
                    //nb_find.FoundConfigRecords.ForEach(Console.WriteLine);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while trying to find NetBurner devices\n" + ex.Message);

            }
            finally
            {
                Console.WriteLine(" Press any key to exit . . . ");
                Console.ReadKey();
            }
        }

        static void HandleDeviceFoundEvent(object sender, EventArgs e)
        {
            NBFinder nb = sender as NBFinder;
            if (nb == null) return;
            Console.WriteLine(nb.FoundDevices[_numDevicesFound]);
            Console.WriteLine(nb.FoundConfigRecords[_numDevicesFound]);
            ++_numDevicesFound;
        }
    }
}
