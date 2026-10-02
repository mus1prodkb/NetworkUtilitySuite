using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;

internal static class Program
{
    private static readonly string StartTime =
        DateTime.Now.ToString("MMMM dd, yyyy | HH:mm:ss");

    public static async Task Main()
    {
        Console.BackgroundColor = ConsoleColor.DarkBlue;
        Console.Title = "Network Utility Suite";

        while (true)
        {
            Console.Clear();
            PrintHeader();
            PrintMenu();

            Console.Write("  Select an option (1-12): ");
            string? choice = Console.ReadLine();

            Console.Clear();

            try
            {
                switch (choice)
                {
                    case "1":
                        NetworkReports.ShowInterfacesAndGateway();
                        break;

                    case "2":
                        await NetworkDiagnostics.ScanLocalSubnetAsync();
                        break;

                    case "3":
                        await NetworkDiagnostics.RunSpeedTestAsync();
                        break;

                    case "4":
                        await NetworkDiagnostics.DnsLookupAsync();
                        break;

                    case "5":
                        await NetworkDiagnostics.TraceRouteAsync();
                        break;

                    case "6":
                        await NetworkDiagnostics.ShowPublicIpAsync();
                        break;

                    case "7":
                        NetworkReports.ShowInterfaceStatistics();
                        break;

                    case "8":
                        NetworkActions.SendWakeOnLan();
                        break;

                    case "9":
                        await NetworkDiagnostics.LivePingMonitorAsync();
                        break;

                    case "10":
                        await NetworkDiagnostics.JitterAnalyzerAsync();
                        break;

                    case "11":
                        NetworkActions.ResetNetwork();
                        break;

                    case "12":
                        return;

                    default:
                        Console.WriteLine("Invalid selection.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nOperation failed: {ex.Message}");
            }

            Console.WriteLine("\nPress any key to return to the menu...");
            Console.ReadKey(true);
        }
    }

    private static void PrintHeader()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("""
__________________________________________________________________

                NETWORK UTILITY SUITE 
__________________________________________________________________
""");

      
        Console.WriteLine($"    Session Started: {StartTime}");
        Console.WriteLine();
    }

    private static void PrintMenu()
    {
        Console.WriteLine("""
  1. View Local Interface & Gateway (Report)
  2. Scan Local Subnet & Common Ports (Inventory)
  3. Run Bandwidth Speed Test
  4. DNS Lookup Tool (Domain to IP)
  5. Traceroute (Map Network Hops)
  6. Public IP & Geolocation
  7. Interface Statistics (Live Data)
  8. Wake-on-LAN (Send Magic Packet)
  9. Live Ping Monitor
  10. Jitter & Latency Analyzer (Advanced Report)
  11. Network Reset (Flush DNS & IP Stack)
  12. Exit

===============================================
""");
    }
}
