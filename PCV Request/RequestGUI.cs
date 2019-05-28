using System;
using System.Windows.Forms;

namespace PCV_Request
{
    public partial class RequestGUI : Form
    {
        public RequestGUI()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e) //View
        {
            SendCommand("view", textBox2.Text);
        }

        private void Button2_Click(object sender, EventArgs e) //Stop
        {
            SendCommand("stop");
        }

        private void Button3_Click(object sender, EventArgs e) //Disable
        {
            SendCommand("disable");
        }

        private void Button4_Click(object sender, EventArgs e) // Enab
        {
            SendCommand("enable");
        }

        private void Button5_Click(object sender, EventArgs e) // Name
        {
            SendCommand("pname", textBox3.Text);
        }

        private void Button6_Click(object sender, EventArgs e) //Note
        {
            SendCommand("pnote", textBox4.Text);
        }

        private void SendCommand(string Verb, string Arg = ".")
        {
            TCPSender.SendTCP(textBox1.Text, (int)numericUpDown1.Value, Verb + " " + Arg);
        }

        private void Button7_Click(object sender, EventArgs e) // Blackout
        {
            SendCommand("fill","rgb(0,0,0)");
        }

        private void Button8_Click(object sender, EventArgs e) // Resume
        {
            SendCommand("unfill");
        }
    }
}