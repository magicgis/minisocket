using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;



namespace Client
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
            toolStripStatusLabel1.Text = "Ready";
        }
        MiniSocket connection = new MiniSocket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        private void button1_Click(object sender, EventArgs e)
        {
            if (!connection.getConnectionProperty()) // 
            {
                
                connection.connectToHost(textBox1.Text, textBox2.Text);
                button2.Enabled = true;
                toolStripStatusLabel1.Text = "Connected";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (connection.getConnectionProperty())
            {
                try
                {
                    connection.write("toggle");
                    string receivedMessage = connection.read();
                    if (receivedMessage == "ledon")
                    {
                        label3.ForeColor = Color.Lime;
                        label3.Text = "Led On";
                        button2.Text = "Led Off";
                    }
                    else if (receivedMessage == "ledoff")
                    {
                        label3.ForeColor = Color.Gray;
                        label3.Text = "Led Off";
                        button2.Text = "Led On";
                    }
                    connection.StreamFlush();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex);
                }
            }
            else
            {
                toolStripStatusLabel1.Text = "Not connected";
            }
        }
    }
}