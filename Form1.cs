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

        CancellationTokenSource cts;



        public Form1()
        {
            InitializeComponent();
            maskedStartPort.Click += new EventHandler(maskedTextBox_Click);
            maskedEndPort.Click += new EventHandler(maskedTextBox_Click);
            maskedEndPort.MaskInputRejected += new MaskInputRejectedEventHandler(maskedTextBox1_MaskInputRejected);
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
                    await RunPortScannerAsync(int.Parse(maskedStartPort.Text), int.Parse(maskedEndPort.Text), targetIpInput.Text, cts.Token);
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

        

        private void maskedTextBox1_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {
            if(e.Position == maskedStartPort.Mask.Length)
            {
                Console.WriteLine(e.RejectionHint);
            }
            else if(e.RejectionHint.ToString() == "DigitExpected")
            {
                Console.WriteLine("You may only enter numbers.");
            }
        }

        private void maskedTextBox_Click(object sender, EventArgs e)
        {
            maskedStartPort.Select(0, 0);
        }


    }
}
