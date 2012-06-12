NBFinder
========

NetBurner device finder .NET .dll written in C# and example console and WPF applications that use it.

## The Core Library ##
The core .dll **NetBurnerFinder.dll**  is used by creating a new Syncor.NetBurderFinder.NbFinder object. Then calling the Start() method on that object sends out the NetBurner UDP broadcast message and automatically starts an asynchronous listener. This listens for any responses. If a response has the proper NetBurner message header it logs the IP address of that device and then attempts to parse the reaminder of the message into a NetBurner configuration structure. It raises an event for each device found.
 
The configuration record stores all IP addresses in .NET IPAddress classes. The Mac adresses are stored in a List<byte> container. All other values are stored in .NET primitives bool, byte, and Int32, except for the InterfaceType which is stored in a custom C# enumeration.

All members of the NBConfig record are available as public properties. **ToString()** is overridden to provide a formatted output string of all the config values. Although both the getter and setter are public, the .DLL currently only supports reading the configuration record. There is no facility for writing a modified configuration record back into the NetBurner. There is no reason this couldn't be done fairly easily, and may be added by a contributor in the future.

## Example Usage ##
The remainder of this code base consists of two sample projects one for a simple console application and one for a slightly more complicated WPF application. Both sample applications only read the configuration record. The WPF shows how an application could be written in an asynchrous manner to use the .dll by handling the raised event. 