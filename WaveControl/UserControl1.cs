using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WaveControl
{
    public partial class UserControl1 : UserControl
    {

        private ControlDataCenter ctrlData;
        private ControlProperty ctrlProperty;
        int beginIndex = 0;
        int endIndex = 0;
        private List<ControlDataEntity> dataList;
        private List<Point> dataPointChildHeart1;
        private List<Point> dataPointChildHeart2;

        private List<Point> dataPointPalaceCompression;
        private List<Point> dataPointFetalMovement;
        int pix = 10;
        public UserControl1()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);

        }

        private void UserControl1_Load(object sender, EventArgs e)
        {
            ctrlData = new ControlDataCenter();
            ctrlProperty = new ControlProperty();
            dataPointChildHeart1 = new List<Point>();
            dataPointChildHeart2 = new List<Point>();
            dataPointPalaceCompression = new List<Point>();
            dataPointFetalMovement = new List<Point>();
            dataList = new List<ControlDataEntity>();

            Size size = this.Size;
            ctrlProperty.topRect = new Rectangle { X = 0, Y = 0, Width = size.Width, Height = Convert.ToInt32(size.Height * 0.6) };
            ctrlProperty.interHeight = Convert.ToInt32(size.Height * 0.05);
            ctrlProperty.botRect = new Rectangle { X = 0, Y = ctrlProperty.topRect.Height + ctrlProperty.interHeight, Width = size.Width, Height = Convert.ToInt32(size.Height * 0.35) };

        }
        protected override void OnPaint(PaintEventArgs e)
        {
            //开始绘图
            Draw(e);
        }
        public void SetControlSize(Size size)
        {
            this.Size = size;
            ctrlProperty.topRect = new Rectangle { X = 0, Y = 10, Width = size.Width, Height = Convert.ToInt32(size.Height * 0.4) };
            ctrlProperty.interHeight = Convert.ToInt32(size.Height * 0.05);
            ctrlProperty.botRect = new Rectangle { X = 0, Y = ctrlProperty.topRect.Height + ctrlProperty.interHeight, Width = size.Width, Height = Convert.ToInt32(size.Height * 0.4) };

        }
        public void SetChildHeart1Property(Color color, int width)
        {
            ctrlProperty.colorChildHeart1 = color;
            ctrlProperty.lineWidthChildHeart1 = width;
        }
        public void SetChildHeart2Property(Color color, int width)
        {
            ctrlProperty.colorChildHeart2 = color;
            ctrlProperty.lineWidthChildHeart2 = width;
        }
        public void AddSampleData(string[] data)
        {
            //当有采样点的时候添加采样点到缓冲区
            ctrlData.AddData(ControlDataEntity.ConvertEntity(data));
            //确定begin和end索引点
            CalcDrawIndex();
            //首先从缓冲区拿到对应的数据
            dataList = ctrlData.GetDataSequnce(beginIndex, endIndex);
            //转换数据到点
            ConvertDataToPoint();
            //通知客户端进行绘图
            Invalidate();
            //Refresh();

        }
        private void CalcDrawIndex()
        {
            int cnt = (this.Size.Width) / pix;

            if (cnt <= ctrlData.GetSize())
            {
                endIndex = ctrlData.GetSize();
                beginIndex = ctrlData.GetSize() - cnt;
            }
            else
            {
                beginIndex = 0;
                endIndex = ctrlData.GetSize() <= 2 ? ctrlData.GetSize() : ctrlData.GetSize() - 1;
            }

        }

        private void Draw(PaintEventArgs e)
        {
            //测试代码


            //

            DrawTopRect(e);
            DrawTopBackground(e);
            DrawTopWave(e);

            DrawBottomRect(e);
            DrawBottomBackground(e);
            DrawBottomWave(e);
        }
        private void DrawBackHLine(PaintEventArgs e, Rectangle rect)
        {
            Graphics g = e.Graphics; //创建画板,这里的画板是由Form提供的.
            Pen p1 = new Pen(Color.FromArgb(169, 169, 169), 2);
            Pen p2 = new Pen(Color.FromArgb(211, 211, 211), 1);

            int hig = ctrlProperty.topRect.Height / 15;
            int i = 0;
            for (i = 0; i < 15;)
            {
                g.DrawLine(p1, new Point(rect.Left, rect.Top + hig * i), new Point(rect.Right, rect.Top + hig * i));
                i++;
                g.DrawLine(p2, new Point(rect.Left, rect.Top + hig * i), new Point(rect.Right, rect.Top + hig * i));
                i++;
                g.DrawLine(p2, new Point(rect.Left, rect.Top + hig * i), new Point(rect.Right, rect.Top + hig * i));
                i++;

            }
            g.DrawLine(p1, new Point(rect.Left, rect.Top + hig * i), new Point(rect.Right, rect.Top + hig * i));

        }
        private void DrawBackVLine(PaintEventArgs e, string tag)
        {
            if (dataList.Count < 2)
            {
                return;
            }
            Graphics g = e.Graphics; //创建画板,这里的画板是由Form提供的.
            Pen p1 = new Pen(Color.FromArgb(169, 169, 169), 2);
            Pen p2 = new Pen(Color.FromArgb(211, 211, 211), 1);

            Rectangle rect;
            int[] value; 
            if (tag == "top")
            {
                rect = ctrlProperty.topRect;
                value = ControlDataEntity.tValue;
            } else
            {
                rect = ctrlProperty.botRect;
                value = ControlDataEntity.bValue;
            }

            //画竖线
            for (int i = 0; i < dataList.Count; i++)
            {
                if (dataList[i].isDrawVLine)
                {
                    g.DrawLine(p2, new Point(dataPointChildHeart1[i].X, rect.Top), new Point(dataPointChildHeart1[i].X, rect.Bottom));
                }
                if (dataList[i].isDrawText)
                {
                    g.DrawLine(p1, new Point(dataPointChildHeart1[i].X, rect.Top), new Point(dataPointChildHeart1[i].X, rect.Bottom));
                    Brush brush = new SolidBrush(Color.FromArgb(0, 0, 0));
                    Font font = new Font("Arial", 10, FontStyle.Bold);

                    if (tag == "bottom")
                    {
                        g.DrawString(dataList[i].sampleTime, font, brush, dataPointChildHeart1[i].X, rect.Bottom + 10);
                    }

                    int hig = rect.Height / 15 * 3;
                    for (int j = 0; j < ControlDataEntity.tValue.Length; j++)
                    {
                        Brush brush1 = new SolidBrush(Color.FromArgb(128, 128, 128));
                        Font font1 = new Font("Arial", 10, FontStyle.Bold);
                        g.DrawString(value[j].ToString(), font1, brush1, dataPointChildHeart1[i].X - 10, rect.Top + hig * j - 5);
                    }
                }
            }
        }
        private void DrawTopRect(PaintEventArgs e)
        {
            Graphics g = e.Graphics; //创建画板,这里的画板是由Form提供的.
            Pen p = new Pen(Color.FromArgb(169, 169, 169), 2);
        }
        private void DrawTopBackground(PaintEventArgs e)
        {
            Graphics g = e.Graphics; //创建画板,这里的画板是由Form提供的.
            Brush brush = new SolidBrush(Color.FromArgb(255, 255, 255));

            g.FillRectangle(brush, ctrlProperty.topRect);
            DrawBackHLine(e, ctrlProperty.topRect);
            DrawBackVLine(e, "top");
        }
        private void DrawTopWave(PaintEventArgs e)
        {
            if (dataList.Count > 2)
            {
                Graphics g = e.Graphics; //创建画板,这里的画板是由Form提供的.
                                         //绘制胎心1
                Pen p1 = new Pen(ctrlProperty.colorChildHeart1, ctrlProperty.lineWidthChildHeart1);

                //绘图的时候可能需要抽取采样点，因为有重复，暂时不做考虑
                g.DrawLines(p1, dataPointChildHeart1.ToArray());
                //绘制胎心2
                Pen p2 = new Pen(ctrlProperty.colorChildHeart2, ctrlProperty.lineWidthChildHeart2);
                g.DrawLines(p2, dataPointChildHeart2.ToArray());

            }
        }
        public void ConvertDataToPoint()
        {
            if (dataList.Count < 2)
            {
                return;
            }
            //第一步：根据索引点获取需要绘制的数据
            //List<int> dataInt = ctrlData.GetChildHeart1Data(beginIndex, endIndex);
            //第三部：将采样数据转换成坐标点
            dataPointChildHeart1.Clear();
            dataPointChildHeart2.Clear();
            dataPointPalaceCompression.Clear();
            dataPointFetalMovement.Clear();

            int ipos = 0;
            for (int i = 0; i < dataList.Count; i++)
            {
                Point p = new Point();
                p.X = ipos + ctrlProperty.topRect.Left;

                p.Y = ctrlProperty.topRect.Top + (210 - dataList[i].childHeart1) * ctrlProperty.topRect.Height / (210 - 60);
                dataPointChildHeart1.Add(p);
                p.Y = ctrlProperty.topRect.Top + (210 - dataList[i].childHeart2) * ctrlProperty.topRect.Height / (210 - 60);
                dataPointChildHeart2.Add(p);

                p.Y = ctrlProperty.botRect.Top + (100 - dataList[i].palaceCompression) * ctrlProperty.botRect.Height / (100 - 0);
                dataPointPalaceCompression.Add(p);
                p.Y = ctrlProperty.botRect.Top + (100 - dataList[i].fetalMovement) * ctrlProperty.botRect.Height / (100 - 0);
                dataPointFetalMovement.Add(p);
                ipos = ipos + pix;
            }
            for (int i = 0; i < (endIndex - beginIndex); i++)
            {
                DateTime timei = Convert.ToDateTime(dataList[i].sampleTime);
                DateTime time0 = Convert.ToDateTime(ctrlData.datalist[0].sampleTime);

                Int64 seci = timei.Ticks;
                Int64 sec0 = time0.Ticks;

                TimeSpan ts1 = new TimeSpan(seci); //获取当前时间的刻度数
                                                   //执行某操作
                TimeSpan ts2 = new TimeSpan(sec0);

                TimeSpan ts = ts2.Subtract(ts1).Duration(); //时间差的绝对值

                int sec = Convert.ToInt32(ts.TotalSeconds); //执行时间的总秒数
                if (sec % 60 == 0)
                {
                    dataList[i].isDrawText = true;
                    dataList[i].isDrawVLine = false;
                    //ctrlData.datalist[beginIndex + i].isDrawText = true;
                    //ctrlData.datalist[beginIndex + i].isDrawVLine = false;

                }
                else if (sec % 15 == 0)
                {
                    dataList[i].isDrawText = false;
                    dataList[i].isDrawVLine = true;
                    //ctrlData.datalist[beginIndex + i].isDrawText = false;
                    //ctrlData.datalist[beginIndex + i].isDrawVLine = true;
                }

            }
            return;
        }
        private void DrawBottomRect(PaintEventArgs e)
        {
            Graphics g = e.Graphics; //创建画板,这里的画板是由Form提供的.
            Pen p = new Pen(Color.FromArgb(169, 169, 169), 2);
        }
        private void DrawBottomBackground(PaintEventArgs e)
        {
            Graphics g = e.Graphics; //创建画板,这里的画板是由Form提供的.
            Brush brush = new SolidBrush(Color.FromArgb(110, 250, 250));

            g.FillRectangle(brush, ctrlProperty.botRect);
            DrawBackHLine(e, ctrlProperty.botRect);
            DrawBackVLine(e,  "bottom");
        }
        private void DrawBottomWave(PaintEventArgs e)
        {
            if (dataList.Count > 2)
            {
                Graphics g = e.Graphics; //创建画板,这里的画板是由Form提供的.
                                         //绘制胎心1
                Pen p1 = new Pen(ctrlProperty.colorChildHeart1, ctrlProperty.lineWidthChildHeart1);

                //绘图的时候可能需要抽取采样点，因为有重复，暂时不做考虑
                g.DrawLines(p1, dataPointPalaceCompression.ToArray());
                //绘制胎心2
                Pen p2 = new Pen(ctrlProperty.colorChildHeart2, ctrlProperty.lineWidthChildHeart2);
                g.DrawLines(p2, dataPointFetalMovement.ToArray());

            }
        }
    }
}
