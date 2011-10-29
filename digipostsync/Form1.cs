using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Digipostsync.Core;

namespace digipostsync
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void timeToSync(object sender, EventArgs e)
        {
            synctimer.Interval = 1000 * 5 * 60;
            textBox1.Text += "\r\nStarter sync - " + DateTime.Now;
            DigipostSync sync = new DigipostSync();
            sync.FileDownloaded += (uri, filename) => textBox1.Text += "\r\nLaster ned " + filename;
            sync.FileUploaded += filename => textBox1.Text += "\r\nLaster opp " + filename;
            sync.Syncronize(Username.Text, Passord.Text, folder.Text);
            textBox1.Text += "\r\nAvslutter sync - " + DateTime.Now;
            textBox1.Text += "\r\n-------------------------------";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            synctimer.Interval = 1000 * 5;
            synctimer.Enabled = true;
            synctimer.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var result = folderBrowserDialog1.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK) {
                folder.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }


    }
}
