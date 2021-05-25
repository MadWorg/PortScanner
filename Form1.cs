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
    public partial class Form1 : Form, IOutputAccess
    {

        public string OutputText { get { return portResult.Text; } set { portResult.Text += value; } }

        private CancellationTokenSource _cts;

        private Form1 form;

        public Form1()
        {
            InitializeComponent();
            maskedStartPort.Click += new EventHandler(maskedTextBox_Click);
            maskedEndPort.Click += new EventHandler(maskedTextBox_Click);
            maskedEndPort.MaskInputRejected += new MaskInputRejectedEventHandler(maskedTextBox1_MaskInputRejected);
            form = this;
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
            
            

            if(_cts == null)
            {
                _cts = new CancellationTokenSource();
                scanButton.Text = "Stop";
                statusDisplay.Text = "Starting scan.";
                portResult.Clear();

                try
                {
                    statusDisplay.Text = "Scanning.";
                    await RunPortScannerAsync(int.Parse(maskedStartPort.Text), int.Parse(maskedEndPort.Text), targetIpInput.Text, _cts.Token, form);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    _cts = null;
                    Console.WriteLine("Scan concluded.");
                    scanButton.Text = "Scan";
                    statusDisplay.Text = "Scan concluded, open ports are listed below.";
                }
            }
            else
            {
                scanButton.Text = "Scan";
                _cts.Cancel();
                _cts = null;
                Console.WriteLine("Scan interrupted.");
                statusDisplay.Text = "Scan concluded, open ports are listed below.";
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

        public static async Task RunPortScannerAsync(int startPort, int endPort, string targetIp, CancellationToken token, Form1 form)
        {
            token.ThrowIfCancellationRequested();
            PortScanner ps = new PortScanner(startPort, endPort, targetIp, form);
            await ps.ScanAsync(token);
        }

        private void maskedTextBox1_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {
            if(e.Position == maskedStartPort.Mask.Length)
            {
                Console.WriteLine(e.RejectionHint);
            }
            else if(e.RejectionHint.ToString() == "DigitExpected")
            {
                Console.WriteLine("You may only enter numbers.");
                statusDisplay.Text = "You may only enter digits.";
            }
        }

        private void maskedTextBox_Click(object sender, EventArgs e)
        {
            maskedStartPort.Select(0, 0);
        }


    }
}
