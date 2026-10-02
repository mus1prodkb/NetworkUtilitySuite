using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

internal static class NetworkReports
{
    public static void ShowInterfacesAndGateway()
    {
        foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
        {
            if (nic.OperationalStatus != OperationalStatus.Up)
                continue;

            IPInterfaceProperties properties = nic.GetIPProperties();

            Console.WriteLine($"Interface : {nic.Name}");
            Console.WriteLine($"Description: {nic.Description}");
            Console.WriteLine($"Type      : {nic.NetworkInterfaceType}");
            Console.WriteLine($"MAC       : {nic.GetPhysicalAddress()}");

            foreach (UnicastIPAddressInformation address in
                     properties.UnicastAddresses)
            {
                Console.WriteLine($"Address   : {address.Address}");
                Console.WriteLine($"Mask      : {address.IPv4Mask}");
            }

            foreach (GatewayIPAddressInformation gateway in
                     properties.GatewayAddresses)
            {
                Console.WriteLine($"Gateway   : {gateway.Address}");
            }

            Console.WriteLine();
        }
    }

    public static void ShowInterfaceStatistics()
    {
        foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
        {
            if (nic.OperationalStatus != OperationalStatus.Up)
                continue;

            IPv4InterfaceStatistics stats = nic.GetIPv4Statistics();

            Console.WriteLine($"""
Interface : {nic.Name}
Received  : {stats.BytesReceived / 1024d / 1024d:F2} MB
Sent      : {stats.BytesSent / 1024d / 1024d:F2} MB
Packets In: {stats.UnicastPacketsReceived}
Packets Out: {stats.UnicastPacketsSent}
Errors In : {stats.IncomingPacketsWithErrors}
Errors Out: {stats.OutgoingPacketsWithErrors}
""");

            Console.WriteLine();
        }
    }
}
