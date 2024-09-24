using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;

Console.WriteLine("TCP Client");
TcpListener listener = new TcpListener(IPAddress.Any, 7);
listener.Start();
while (true)
{
    TcpClient socket = listener.AcceptTcpClient();
    IPEndPoint ep = socket.Client.RemoteEndPoint as IPEndPoint;
    Console.WriteLine("Client connected: " + ep.Address);
    Task.Run(()=> HandleClient(socket));
}

void HandleClient(TcpClient socket)
{
    NetworkStream ns = socket.GetStream();
    StreamReader sr = new StreamReader(ns);
    StreamWriter sw = new StreamWriter(ns);
    while (socket.Connected)
    {
        sw.WriteLine("Welcome \nPick between: Add, Subtract or Random");
        sw.Flush();
        string message = sr.ReadLine().ToLower();
        sw.WriteLine("Input numbers");
        sw.Flush();
        int[] nums = sr.ReadLine().Split(' ').Select(n => Convert.ToInt32(n)).ToArray();
        switch (message)
        {
            case "random": sw.WriteLine(RandomNumberGenerator.GetInt32(nums[0], nums[1])); break;
            case "add": sw.WriteLine(nums[0] + nums[1]); break;
            case "subtract": sw.WriteLine(nums[0] - nums[1]); break;
            default: sw.WriteLine("protocol error"); break;
        }
        sw.Flush();
    }
}