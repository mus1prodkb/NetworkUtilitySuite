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

## Requirements
Windows 10 or later
Visual Studio 2026 or a compatible .NET development environment
.NET 10 SDK
Network access for DNS, public IP, geolocation, speed-test, and diagnostic features
Administrator privileges for the network reset operation

## Getting Started
Clone the repository
git clone https://github.com/mus1prodkb/NetworkUtilitySuite.git
cd NetworkUtilitySuite

## Build the project
dotnet build

## Run the application
bash
dotnet run

Alternatively, open the project in Visual Studio and press:
Ctrl + F5
To run with the debugger attached, press:
F5

## Usage
After starting the application, select an operation by entering a number from 1 through 12.
Local interface report
Displays information about active network adapters, including:
Adapter name
Description
Network interface type
MAC address
IPv4 addresses
Subnet masks
Default gateways
Local subnet scan
Enter the first three octets of the local subnet.
Example:
192.168.1
The scanner checks addresses from:
192.168.1.1
through:
192.168.1.254
Responsive hosts are then tested against commonly used TCP ports, including:
21, 22, 23, 25, 53, 80, 110, 143, 443, 445, 3389, 8080
## DNS lookup
Enter a hostname or domain name:
example.com
The application displays the IPv4 and IPv6 addresses returned by the system DNS resolver.
## Traceroute
Enter a hostname or IP address to display the network hops between the local computer and the destination.
## Public IP and geolocation
The application retrieves the public IP address and requests approximate location information such as:
Country
Region
City
Internet service provider
Geolocation results are approximate and may not represent the user's exact physical location.
## Wake-on-LAN
Enter a target computer's MAC address using either of these formats:
00:11:22:33:44:55
or:
00-11-22-33-44-55
The application sends a Wake-on-LAN magic packet over UDP port 9.
The target computer must support Wake-on-LAN, and the feature must be enabled in the system firmware and network adapter settings.
## Network reset
The reset option executes the following Windows commands:
ipconfig /flushdns
netsh winsock reset
netsh int ip reset
The reset operation may disconnect the computer from the network and may require a system restart.
## Project Structure
NetworkUtilitySuite/
├── NetworkUtilitySuite.csproj
├── Program.cs
├── NetworkActions.cs
├── NetworkDiagnostics.cs
└── NetworkReports.cs
## Program.cs
Contains the application entry point, console menu, session header, and menu selection logic.
## NetworkActions.cs
Contains operations that actively change or affect network behavior, including:
Wake-on-LAN packet transmission
DNS cache flushing
Winsock reset
TCP/IP stack reset
## NetworkDiagnostics.cs
Contains diagnostic functions such as:
Subnet scanning
Common-port checks
DNS lookup
Traceroute
Public IP lookup
Speed testing
Live ping monitoring
Jitter analysis
## NetworkReports.cs
Contains reporting functions for:
Local interfaces
IP addresses
Gateways
MAC addresses
Interface traffic statistics
## *** Permissions and Responsible Use ***
Use this application only on computers and networks that you own or are authorized to administer.
In particular:
Only scan subnets where you have permission.
Do not use port scanning against unauthorized systems.
Do not send Wake-on-LAN packets to devices without authorization.
Be aware that network reset commands can interrupt active connections.
Respect organizational security policies and acceptable-use requirements.
## Limitations
Network interface statistics are cumulative counters provided by Windows.
Traceroute results may contain missing hops when routers filter ICMP traffic.
Some hosts may block ping requests.
Port results can be affected by firewalls and network filtering.
Public IP geolocation is approximate.
Speed-test results depend on the selected server, route, network congestion, and system performance.
Wake-on-LAN generally works only when the target device is connected to a compatible wired or configured wireless network.
The network reset feature is intended for Windows systems.
## Troubleshooting
The application does not build
Verify that the .NET SDK is installed:

bash
dotnet --info

Confirm that the installed SDK supports the target framework specified in the project file.

The subnet scan finds no devices
Check that:

The subnet was entered correctly.
The target devices are powered on.
The local firewall permits the required traffic.
The devices respond to ICMP echo requests.
You are connected to the expected network.
Wake-on-LAN does not work
Check the following:

The MAC address is correct.
Wake-on-LAN is enabled in the target computer's BIOS or UEFI settings.
Wake-on-LAN is enabled in the network adapter properties.
The target computer is connected to power.
The network supports Wake-on-LAN packets.
The target device is on the same local network or the network is configured to forward WoL traffic.
Network reset fails
Run the application from an elevated Administrator terminal or start Visual Studio as Administrator. Restart Windows if the reset commands request it.

License
Choose a license before publishing the repository. For example, to use the MIT License, add a file named LICENSE containing the official MIT License text.

Suggested repository setting:

text


MIT License
Disclaimer
This project is provided for educational, diagnostic, and authorized network-administration purposes. The author is not responsible for damage, service interruption, unauthorized access, or misuse resulting from this software.
