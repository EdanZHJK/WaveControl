using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestPro
{
    public partial class form1 : Form
    {
        public form1()
        {
            InitializeComponent();
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true); // 禁止擦除背景.
            SetStyle(ControlStyles.DoubleBuffer, true); // 双缓冲
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // 获取屏幕分辨率
            //int SH = Screen.PrimaryScreen.Bounds.Height;
            //int SW = Screen.PrimaryScreen.Bounds.Width;
            int width = this.Size.Width;
            int height = this.Size.Height;

            Size size = new Size(width, height);
            userControl11.Location = new Point(0, 0);
            userControl11.SetControlSize(size);
            userControl11.Invalidate();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            TimeSpan ts = DateTime.Now - DateTime.Parse("1970-1-1");
            string time = ts.TotalMilliseconds.ToString();
            time = DateTime.Now.ToString();
            string[] data = new string[5];
            Random r = new Random();
            data[0] = r.Next(80, 120).ToString();
            data[1] = time;
            data[2] = r.Next(100, 180).ToString();
            data[3] = r.Next(20, 80).ToString();
            data[4] = r.Next(10, 90).ToString();

            userControl11.AddSampleData(data);
            //userControl11.Invalidate();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TimeSpan ts = DateTime.Now - DateTime.Parse("1970-1-1");
            string time = ts.TotalMilliseconds.ToString();
            time = DateTime.Now.Ticks.ToString();
            string[] data = new string[5];
            Random r = new Random();
            data[0] = r.Next(80, 120).ToString();
            data[1] = time;

            userControl11.AddSampleData(data);
            //userControl11.Invalidate();
        }
    }
}
