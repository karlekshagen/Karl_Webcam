using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;
using System.IO;
using System.Data.SqlClient;

namespace Webcam
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        FilterInfoCollection filterInfoCollection;
        VideoCaptureDevice videoCaptureDevice;
        Bitmap bitmap;
        private void PictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void Button1_Click(object sender, EventArgs e)
        {

        }

        private void VideoCaptureDevice_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            bitmap = (Bitmap)eventArgs.Frame.Clone();

            pic.Image = bitmap;
            SavePic();
        }
        private void SavePic()
        {
            while (videoCaptureDevice.IsRunning == true)
            {
                Bitmap current = (Bitmap)bitmap.Clone();
                string filepath = @"C://Bilder";
                string fileName = System.IO.Path.Combine(filepath, DateTime.Now.ToString("yyyyMMdd_hh_mm_ss") + ".png");
                current.Save(fileName);
                current.Dispose();
                break;
            }
        }

        private void Label1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (videoCaptureDevice.IsRunning == true)
                videoCaptureDevice.Stop();
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            filterInfoCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo filterInfo in filterInfoCollection)
                cboCamera.Items.Add(filterInfo.Name);
            cboCamera.SelectedIndex = 0;
            videoCaptureDevice = new VideoCaptureDevice();
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void TextBox2_TextChanged(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection("Data Source=CND8263QKF;Initial Catalog=18IT_Test;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            con.Open();
            try
            {
                if (textBox1.Text != "")
                {
                    SqlCommand cmd = new SqlCommand("Select Texts from dbo.Texter where ID =@ID", con);
                    cmd.Parameters.AddWithValue("@ID", int.Parse(textBox2.Text));
                    SqlDataReader da = cmd.ExecuteReader();
                    while (da.Read())
                    {
                        textBox3.Text = da.GetValue(0).ToString();

                    }
                    con.Close();
                }
            }
            catch
            {
                textBox3.Clear();
            }
        }

        private void Button2_Click_1(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection("Data Source=CND8263QKF;Initial Catalog=18IT_Test;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            con.Open();
            {
                SqlCommand command;
                SqlDataReader dataReader;
                string sql, output = "";

                sql = "SELECT Texts from Texter";
                command = new SqlCommand(sql, con);
                dataReader = command.ExecuteReader();

                dataReader.Read();
                button2.Text = dataReader["Texts"].ToString();
                con.Close();

                videoCaptureDevice = new VideoCaptureDevice(filterInfoCollection[cboCamera.SelectedIndex].MonikerString);
                videoCaptureDevice.NewFrame += VideoCaptureDevice_NewFrame;
                videoCaptureDevice.Start();
            }
        }
    }
}
    
