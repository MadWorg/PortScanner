using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PortScanner
{
    public partial class Form1 : Form
    {

        private List<int> openPorts = new List<int>();
        CancellationTokenSource cts;

        public Form1()
        {
            InitializeComponent();      
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private async void button1_Click(object sender, EventArgs e)
        {
            
            if(cts == null)
            {
                cts = new CancellationTokenSource();
                scanButton.Text = "Stop";
                statusDisplay.Text = "Starting scan.";
                

                try
                {
                    statusDisplay.Text = "Scanning.";
                    await RunPortScannerAsync(int.Parse(startPortInput.Text), int.Parse(endPortInput.Text), targetIpInput.Text, cts.Token);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    cts = null;
                }
            }
            else
            {
                scanButton.Text = "Scan";
                cts.Cancel();
                cts = null;
                Console.WriteLine("Scan ended.");
                statusDisplay.Text = "Scan ended.";
            }

            
            
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void startPort_TextChanged(object sender, EventArgs e)
        {

        }

        private void endPort_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        public static async Task RunPortScannerAsync(int startPort, int endPort, string targetIp, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            PortScanner ps = new PortScanner(startPort, endPort, targetIp);
            await ps.ScanAsync(token);
        }

        class PortScanner
        {

            private const int MIN_PORT = 0;
            private const int MAX_PORT = 65535;

            List<int> _openPorts;

            public int MinPort = MIN_PORT;
            public int MaxPort = MAX_PORT;

            public string TargetIP = "127.0.0.1";

            public PortScanner()
            {
                _openPorts = new List<int>();
            }

            public PortScanner(int minPort, int maxPort, string targetIp)
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

                IPAddress ipTest;
                if (!IPAddress.TryParse(targetIp, out ipTest))
                {
                    Console.Error.WriteLine($"IP address is not valid.");
                }


                MinPort = minPort;
                MaxPort = maxPort;
                TargetIP = targetIp;
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
                    _openPorts.Add(port);
                }
            }

            private async Task<bool> ScanPortAsync(int port, CancellationToken token)
            {

                token.ThrowIfCancellationRequested();

                TcpClient tcpScan = new TcpClient();

                await Task.Run(() =>
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

                return false;

            }

        }

    }
}
