using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

internal static class NetworkActions
{
    public static void SendWakeOnLan()
    {
        Console.Write("Target MAC address: ");
        string? input = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(input))
            return;

        string cleaned = input
            .Replace(":", "")
            .Replace("-", "")
            .Trim();

        if (cleaned.Length != 12)
        {
            Console.WriteLine("Invalid MAC address.");
            return;
        }

        byte[] mac = Convert.FromHexString(cleaned);
        byte[] packet = new byte[102];

        for (int i = 0; i < 6; i++)
            packet[i] = 0xFF;

        for (int i = 1; i <= 16; i++)
            Buffer.BlockCopy(mac, 0, packet, i * 6, 6);

        using UdpClient client = new();
        client.EnableBroadcast = true;

        client.Send(
            packet,
            packet.Length,
            new IPEndPoint(IPAddress.Broadcast, 9));

        Console.WriteLine("Wake-on-LAN magic packet sent.");
    }

    public static void ResetNetwork()
    {
        Console.WriteLine("""
This operation requires administrator privileges and may disconnect
your computer from the network.

It runs:
  ipconfig /flushdns
  netsh winsock reset
  netsh int ip reset
""");

        Console.Write("Continue? Type RESET: ");

        if (Console.ReadLine() != "RESET")
        {
            Console.WriteLine("Cancelled.");
            return;
        }

        RunCommand("ipconfig", "/flushdns");
        RunCommand("netsh", "winsock reset");
        RunCommand("netsh", "int ip reset");

        Console.WriteLine("\nA restart may be required.");
    }

    private static void RunCommand(string fileName, string arguments)
    {
        using Process process = new()
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = fileName,
                Arguments = arguments,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
                Verb = "runas"
            }
        };

        try
        {
            process.Start();

            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();

            process.WaitForExit();

            Console.WriteLine(output);

            if (!string.IsNullOrWhiteSpace(error))
                Console.WriteLine(error);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{fileName} failed: {ex.Message}");
        }
    }
}
