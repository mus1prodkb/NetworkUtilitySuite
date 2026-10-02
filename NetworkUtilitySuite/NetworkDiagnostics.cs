using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text.Json;

internal static class NetworkDiagnostics
{
    private static readonly int[] CommonPorts =
    {
        21, 22, 23, 25, 53, 80, 110, 143,
        443, 445, 3389, 8080
    };

    public static async Task ScanLocalSubnetAsync()
    {
        Console.Write("Enter subnet, for example 192.168.1: ");
        string subnet = Console.ReadLine()?.Trim() ?? "";

        if (!IsValidSubnet(subnet))
        {
            Console.WriteLine("Invalid subnet.");
            return;
        }

        Console.WriteLine("\nScanning hosts...\n");

        using SemaphoreSlim limit = new(64);
        List<Task> tasks = [];

        for (int host = 1; host <= 254; host++)
        {
            string address = $"{subnet}.{host}";
            tasks.Add(ScanHostAsync(address, limit));
        }

        await Task.WhenAll(tasks);
    }

    private static async Task ScanHostAsync(
        string address,
        SemaphoreSlim limit)
    {
        await limit.WaitAsync();

        try
        {
            using Ping ping = new();
            PingReply reply = await ping.SendPingAsync(address, 400);

            if (reply.Status != IPStatus.Success)
                return;

            Console.WriteLine($"{address,-16} {reply.RoundtripTime,4} ms");

            foreach (int port in CommonPorts)
            {
                using TcpClient client = new();

                try
                {
                    Task connection = client.ConnectAsync(address, port);
                    Task completed = await Task.WhenAny(connection,
                        Task.Delay(250));

                    if (completed == connection && client.Connected)
                        Console.WriteLine($"  Open TCP port: {port}");
                }
                catch
                {
                    // Port is closed or filtered.
                }
            }
        }
        catch
        {
            // Host did not respond.
        }
        finally
        {
            limit.Release();
        }
    }

    public static async Task DnsLookupAsync()
    {
        Console.Write("Domain: ");
        string domain = Console.ReadLine()?.Trim() ?? "";

        IPAddress[] addresses = await Dns.GetHostAddressesAsync(domain);

        Console.WriteLine($"\nResults for {domain}:");

        foreach (IPAddress address in addresses)
            Console.WriteLine($"  {address}");
    }

    public static async Task TraceRouteAsync()
    {
        Console.Write("Host: ");
        string host = Console.ReadLine()?.Trim() ?? "";

        IPAddress destination =
            (await Dns.GetHostAddressesAsync(host))
            .First(address => address.AddressFamily == AddressFamily.InterNetwork);

        Console.WriteLine($"\nTracing route to {host} [{destination}]\n");

        using Ping ping = new();

        for (byte ttl = 1; ttl <= 30; ttl++)
        {
            PingOptions options = new(ttl, true);
            byte[] buffer = new byte[32];
            Stopwatch timer = Stopwatch.StartNew();

            PingReply reply = await ping.SendPingAsync(
                destination,
                3000,
                buffer,
                options);

            timer.Stop();

            string hop = reply.Address?.ToString() ?? "*";
            Console.WriteLine($"{ttl,2}  {hop,-18} {timer.ElapsedMilliseconds} ms");

            if (reply.Status == IPStatus.Success)
                break;
        }
    }

    public static async Task LivePingMonitorAsync()
    {
        Console.Write("Host or IP: ");
        string host = Console.ReadLine()?.Trim() ?? "";

        Console.WriteLine("Press Ctrl+C to stop.\n");

        using Ping ping = new();

        while (true)
        {
            Stopwatch timer = Stopwatch.StartNew();
            PingReply reply = await ping.SendPingAsync(host, 2000);
            timer.Stop();

            if (reply.Status == IPStatus.Success)
            {
                Console.WriteLine(
                    $"{DateTime.Now:T}  Reply from {host}: " +
                    $"{reply.RoundtripTime} ms");
            }
            else
            {
                Console.WriteLine(
                    $"{DateTime.Now:T}  {reply.Status}");
            }

            await Task.Delay(1000);
        }
    }

    public static async Task JitterAnalyzerAsync()
    {
        Console.Write("Host or IP: ");
        string host = Console.ReadLine()?.Trim() ?? "";

        Console.Write("Number of samples [20]: ");
        int.TryParse(Console.ReadLine(), out int count);

        if (count <= 0)
            count = 20;

        List<double> samples = [];
        using Ping ping = new();

        for (int i = 0; i < count; i++)
        {
            PingReply reply = await ping.SendPingAsync(host, 3000);

            if (reply.Status == IPStatus.Success)
            {
                samples.Add(reply.RoundtripTime);
                Console.WriteLine($"Sample {i + 1}: {reply.RoundtripTime} ms");
            }
            else
            {
                Console.WriteLine($"Sample {i + 1}: {reply.Status}");
            }

            await Task.Delay(250);
        }

        if (samples.Count == 0)
        {
            Console.WriteLine("No successful samples.");
            return;
        }

        double average = samples.Average();
        double minimum = samples.Min();
        double maximum = samples.Max();

        double jitter = samples
            .Zip(samples.Skip(1), (a, b) => Math.Abs(b - a))
            .DefaultIfEmpty(0)
            .Average();

        Console.WriteLine($"""

Advanced Latency Report
-----------------------
Samples : {samples.Count}
Minimum : {minimum:F2} ms
Average : {average:F2} ms
Maximum : {maximum:F2} ms
Jitter  : {jitter:F2} ms
""");
    }

    public static async Task ShowPublicIpAsync()
    {
        using HttpClient client = new();

        string publicIp = await client.GetStringAsync(
            "https://api.ipify.org");

        Console.WriteLine($"Public IP: {publicIp}");

        try
        {
            string json = await client.GetStringAsync(
                $"https://ipapi.co/{publicIp}/json/");

            using JsonDocument document = JsonDocument.Parse(json);
            JsonElement root = document.RootElement;

            Console.WriteLine($"Country : {Get(root, "country_name")}");
            Console.WriteLine($"Region  : {Get(root, "region")}");
            Console.WriteLine($"City    : {Get(root, "city")}");
            Console.WriteLine($"ISP     : {Get(root, "org")}");
        }
        catch
        {
            Console.WriteLine("Geolocation lookup failed.");
        }
    }

    public static async Task RunSpeedTestAsync()
    {
        Console.WriteLine("""
Speed-test placeholder.

A reliable bandwidth test requires downloading and uploading a
known-size file from a server that permits testing. Do not use
random public files without permission.
""");

        using HttpClient client = new();

        Stopwatch timer = Stopwatch.StartNew();

        using HttpResponseMessage response = await client.GetAsync(
            "https://speed.cloudflare.com/__down?bytes=10000000");

        byte[] data = await response.Content.ReadAsByteArrayAsync();

        timer.Stop();

        double seconds = timer.Elapsed.TotalSeconds;
        double megabits = data.Length * 8 / seconds / 1_000_000;

        Console.WriteLine(
            $"Downloaded: {data.Length / 1024d / 1024d:F2} MB");
        Console.WriteLine($"Elapsed   : {seconds:F2} seconds");
        Console.WriteLine($"Download  : {megabits:F2} Mbps");
    }

    private static bool IsValidSubnet(string subnet)
    {
        string[] parts = subnet.Split('.');

        return parts.Length == 3 &&
               parts.All(part =>
                   int.TryParse(part, out int value) &&
                   value >= 0 &&
                   value <= 255);
    }

    private static string Get(JsonElement element, string property)
    {
        return element.TryGetProperty(property, out JsonElement value)
            ? value.ToString()
            : "Unavailable";
    }
}
