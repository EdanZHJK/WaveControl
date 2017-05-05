using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaveControl
{
    public class ControlProperty
    {
        /// <summary>
        /// 整个绘图
        /// </summary>
        public int interHeight;

        public Rectangle allRect;
        /// <summary>
        /// 顶部Rect
        /// </summary>
        public Rectangle topRect;
        /// <summary>
        /// 底部Rect
        /// </summary>
        public Rectangle botRect;
        /// <summary>
        /// 胎心1相关
        /// </summary>
        public Color colorChildHeart1;
        public int lineWidthChildHeart1;
        /// <summary>
        /// 胎心2相关
        /// </summary>
        public Color colorChildHeart2 = Color.Red;
        public int lineWidthChildHeart2 = 1;

        public ControlProperty()
        {
            interHeight = 10;
            colorChildHeart1 = Color.Blue;
            lineWidthChildHeart1 = 1;
        }
        

    }
}
