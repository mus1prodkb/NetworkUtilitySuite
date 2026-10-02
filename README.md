# Network Utility Suite

A Windows command-line network diagnostics and administration toolkit written in C# using .NET.

The application provides a menu-driven interface for checking network configuration, testing connectivity, scanning local devices, analyzing latency, sending Wake-on-LAN packets, and performing selected network maintenance tasks.

## Features

- View local network interfaces and default gateways
- Scan a local subnet for responsive devices
- Check common TCP ports
- Run a basic bandwidth download test
- Perform DNS lookups
- Trace network routes using ICMP
- Display public IP address and approximate geolocation
- View network interface statistics
- Send Wake-on-LAN magic packets
- Monitor live ping responses
- Calculate latency, jitter, minimum, average, and maximum response times
- Flush DNS and reset the Windows TCP/IP stack
- Interactive console menu

## Menu

```text
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
