using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace htttpDemo
{
    public partial class fMain : Form
    {
        HttpWebRequest req;
        HttpWebResponse res;
        StreamReader sr;

        WebClient wc;

        static string pattern = @"(https?:)?//?[^'""<>]+?\.(jpg|jpeg|gif|png)";
        public fMain()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            req = (HttpWebRequest)HttpWebRequest.Create(tbxAdress.Text);
            res = (HttpWebResponse)req.GetResponse();
            sr = new StreamReader(res.GetResponseStream(), Encoding.Default);
            richTextBox1.Text = sr.ReadToEnd();
            sr.Close();

            wc = new WebClient();
            byte[] urlData = wc.DownloadData(tbxAdress.Text);
            string page = Encoding.UTF8.GetString(urlData);
            richTextBox2.Text = page;

            listBox1.Items.Clear();
            foreach (var item in page.Split(tbxSplit.Text[0]))
            {
                listBox1.Items.Add(item);
            }

            try
            {
                //wc = new WebClient();
                string fileCopy = "C:\\image.jpg";
                string urlString = tbxAdress.Text;
                wc.DownloadFile(urlString, fileCopy);
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                pictureBox1.Image = Image.FromFile(fileCopy);
            }
            catch(Exception ex)
            {
                lblImage.Text = ex.Message;
            }

            //homework
            LoadData();
            try
            {
                wc = new WebClient();

                string input = richTextBox1.Text;
                int cntr = 1;

                int offset = 0;
                int offset_Y = 0;
                string filename = "";
                
                foreach (Match m in Regex.Matches(input, pattern))
                {
                    filename = "images\\image" + (cntr++) + "." + Path.GetExtension(m.Value);
                    wc.DownloadFile(m.Value, filename);
                    PictureBox pb = new PictureBox();
                    pb.Width = 100;
                    pb.Height = 100;
                    pb.Location = new Point(pb.Location.X + offset, pb.Location.Y + offset_Y);
                    offset += 150;
                    if (offset > this.Width - 150)
                    {
                        offset_Y += 150;
                        offset = 0;
                    }
                    pb.SizeMode = PictureBoxSizeMode.Zoom;
                    pb.Image = Image.FromFile(filename);
                    panelHW.Controls.Add(pb);
                }
            }
            catch (Exception ex)
            {
                lblHomeWork.Text = ex.Message;
            }
            wc = null;
        }

        private void tbxSplit_TextChanged(object sender, EventArgs e)
        {
            if (tbxSplit.Text != "")
                tbxSplit.Text = tbxSplit.Text[0].ToString();
            else
                tbxSplit.Text = "|";
        }

        private void LoadData()
        {
           
        }
    }
}
