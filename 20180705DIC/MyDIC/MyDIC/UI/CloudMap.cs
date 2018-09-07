using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace MyDIC.UI
{
    public class CloudMap
    {
        public double max_diff;
        public double min_diff;
        public double[] drawPtValue;
        public Point[] drawPt;

        /// <summary>
        /// 云图数据构造函数
        /// </summary>
        /// <param name="min">最小位移</param>
        /// <param name="max">最大位移</param>
        /// <param name="PtValue">每点的位移数据数组</param>
        /// <param name="Pt">点阵一位数组，位置信息</param>
        public CloudMap(double min, double max, double[] PtValue, Point[] Pt)
        {
            max_diff = max;
            min_diff = min;
            drawPtValue = PtValue;
            drawPt = Pt;
        }

    }
}
