using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PortScanner
{
    class PortScanner //TODO: pass form into scanner so it can write to output
    {

        private const int MIN_PORT = 0;
        private const int MAX_PORT = 65535;

        List<int> _openPorts;

        public int MinPort = MIN_PORT;
        public int MaxPort = MAX_PORT;

        public string TargetIP = "127.0.0.1";

        private Form1 form;

        public PortScanner()
        {
            _openPorts = new List<int>();
        }

        public PortScanner(int minPort, int maxPort, string targetIp, Form1 form)
        {

            if (minPort < MIN_PORT || minPort > MAX_PORT)
            {
                Console.Error.WriteLine($"Min port must be a number between {MIN_PORT} and {MAX_PORT}");
                return;
            }

            if (maxPort < MIN_PORT || maxPort > MAX_PORT)
            {
                Console.Error.WriteLine($"Max port must be a number between {MIN_PORT} and {MAX_PORT}");
                return;
            }

            if (maxPort < minPort)
            {
                Console.Error.WriteLine($"Max port cannot be smaller than min port.");
                return;
            }

            if (!IPAddress.TryParse(targetIp, out _)) // discards result
            {
                Console.Error.WriteLine($"IP address is not valid.");
            }


            MinPort = minPort;
            MaxPort = maxPort;
            TargetIP = targetIp;
            this.form = form;
            _openPorts = new List<int>();
        }

        public async Task ScanAsync(CancellationToken token)
        {
            for (int port = MinPort; port <= MaxPort; port++)
            {
                token.ThrowIfCancellationRequested();
                await ScanPortRangeAsync(port, token);
            }
        }

        private async Task ScanPortRangeAsync(int port, CancellationToken token)
        {
            if (await ScanPortAsync(port, token))
            {
                form.OutputText = port.ToString();
                _openPorts.Add(port);
            }
        }

        private async Task<bool> ScanPortAsync(int port, CancellationToken token)
        {

            token.ThrowIfCancellationRequested();

            TcpClient tcpScan = new TcpClient();

            return await Task.Run(() =>
            {

                try
                {

                    tcpScan.Connect("127.0.0.1", port);
                    Console.WriteLine($"Port {port} is open.");
                    tcpScan.GetStream().Close();
                    tcpScan.Close();
                    return true;
                }
                catch
                {
                    Console.WriteLine($"Port {port} is not open.");
                    tcpScan.Close();
                    return false;
                }

            });

        }

    }
}
