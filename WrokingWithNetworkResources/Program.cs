using System.Net; // IPHostEntry, Dns, IPAddress
using System.Net.NetworkInformation; // Ping, PingReply, IPStatus
using static System.Console;

// p.352 working with network resources
Write("Enter a valid web address: ");
string? url = ReadLine();

if (string.IsNullOrWhiteSpace(url))
{
    url = "https://stackoverflow.com/search?q=securestring";
}

Uri uri = new(url);
WriteLine($"URL : {url} \nScheme : {uri.Scheme} \nPort: {uri.Port} \nHost: {uri.Host} \nPath: {uri.AbsolutePath} \nQuery: {uri.Query} ");
// https://developer.mozilla.org/en-US/docs/Learn/Common_questions/What_is_a_URL
// Scheme : the protocol to be used to access the resource on the Internet.
// Port : The port indicates the technical "gate" used to access the resources on the web server.
// Host : The host name identifies the host that holds the resource.
// Path : The path identifies the specific resource in the host that the web client wants to access.
// Query :  provides a string of information that the resource can use for some purpose 

IPHostEntry entry = Dns.GetHostEntry(uri.Host);
// DNS (Domain Name System) is a hierarchical and decentralized naming system for Internet connected resources. 
WriteLine($"{entry.HostName} has the following IP addresses:");
foreach(IPAddress address in entry.AddressList)
{
    WriteLine($"    {address}({address.AddressFamily})");
}

// p.353 pinging a server
// Ping (Packet Internet Groper) is a method for determining communication latency between two networks.
try
{
    Ping ping = new();
    WriteLine("Pinging server. Please wait...");
    PingReply reply = ping.Send(uri.Host);

    WriteLine($"{uri.Host} was pinged and replied: {reply.Status}");
    if (reply.Status == IPStatus.Success)
    {
        WriteLine("Reply from {0} took {1:N0} ms",
            arg0: reply.Address,
            arg1: reply.RoundtripTime);
    }
}
catch (Exception ex)
{
    WriteLine($"{ex.GetType().ToString()} says {ex.Message}");
}

// 0-40ms : fast
// 41-60ms : normal
// 61 - 100ms : a little slow
// 101- ms : slow

