using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace myFirstForm
{
    public partial class Form1 : Form
    {
        public string file1, file2, file3, file4, file5, file6, file7;
        public string authorizationCode;
        public List<string> fileContents = new List<string>();
        public List<int> channelNums = new List<int>();
        public List<int> timeIntervals = new List<int>();
        public Form1()
        {
            InitializeComponent();
            label23.Hide();
            label24.Hide();
            label25.Hide();
            label26.Hide();
            label27.Hide();
            label28.Hide();
            label29.Hide();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label23_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                label23.Text = Path.GetFileName(openFileDialog1.FileName);
                label23.Show();

                //Retrieve file contents
                file1 = File.ReadAllText(openFileDialog1.FileName);
                fileContents.Add(file1);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                label24.Text = Path.GetFileName(openFileDialog1.FileName);
                label24.Show();

                //Retrieve file contents
                file2 = File.ReadAllText(openFileDialog1.FileName);
                fileContents.Add(file2);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                label25.Text = Path.GetFileName(openFileDialog1.FileName);
                label25.Show();
                
                file3 = File.ReadAllText(openFileDialog1.FileName);
                fileContents.Add(file3);
            }
        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label22_Click(object sender, EventArgs e)
        {

        }

        private void label31_Click(object sender, EventArgs e)
        {
            
        }

        private void button9_Click(object sender, EventArgs e)
        {
            label31.Text = KeyMessagerClient.twoHourTaskCounts.ToString();
            label32.Text = KeyMessagerClient.sixHourTaskCounts.ToString();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                label26.Text = Path.GetFileName(openFileDialog1.FileName);
                label26.Show();
                
                file4 = File.ReadAllText(openFileDialog1.FileName);
                fileContents.Add(file4);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                label27.Text = Path.GetFileName(openFileDialog1.FileName);
                label27.Show();
                
                file5 = File.ReadAllText(openFileDialog1.FileName);
                fileContents.Add(file5);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                label28.Text = Path.GetFileName(openFileDialog1.FileName);
                label28.Show();
                
                file6 = File.ReadAllText(openFileDialog1.FileName);
                fileContents.Add(file6);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                label29.Text = Path.GetFileName(openFileDialog1.FileName);
                label29.Show();
                
                file7 = File.ReadAllText(openFileDialog1.FileName);
                fileContents.Add(file7);
            }
        }
        private async void button8_Click(object sender, EventArgs e)
        {
            RetrieveAllData();
            authorizationCode = File.ReadAllText("authorizationCode.txt");
            KeyMessagerClient keyMessagerClient = new KeyMessagerClient(fileContents,channelNums,timeIntervals,authorizationCode);
            await keyMessagerClient.InitiateProcess();
        }
        public void RetrieveAllData()
        {
            channelNums.Add(Convert.ToInt32(textBox1.Text));
            channelNums.Add(Convert.ToInt32(textBox4.Text));
            channelNums.Add(Convert.ToInt32(textBox6.Text));
            channelNums.Add(Convert.ToInt32(textBox8.Text));
            channelNums.Add(Convert.ToInt32(textBox10.Text));
            channelNums.Add(Convert.ToInt32(textBox12.Text));
            channelNums.Add(Convert.ToInt32(textBox14.Text));

            timeIntervals.Add(Convert.ToInt32(textBox2.Text));
            timeIntervals.Add(Convert.ToInt32(textBox3.Text));
            timeIntervals.Add(Convert.ToInt32(textBox5.Text));
            timeIntervals.Add(Convert.ToInt32(textBox7.Text));
            timeIntervals.Add(Convert.ToInt32(textBox9.Text));
            timeIntervals.Add(Convert.ToInt32(textBox11.Text));
            timeIntervals.Add(Convert.ToInt32(textBox13.Text));
        }
    }
}
