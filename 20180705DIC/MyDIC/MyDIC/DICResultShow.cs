using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using ZedGraph;

namespace MyDIC
{
    public partial class DICResultShow : Form
    {
        public DICResultShow()
        {
            InitializeComponent();
        }
        public DICResultShow(List<PointF> ListPointGridinPolygon, PointF[]ListPointGridinPolygonSub)
        {
            InitializeComponent();
            List<double> yDataOld = new List<double>();
            List<double> yData = new List<double>();
            foreach (PointF item in ListPointGridinPolygon)
            {
                yDataOld.Add(Math.Sqrt(Math.Pow(item.X, 2) + Math.Pow(item.Y, 2)));
            }
            foreach (PointF item in ListPointGridinPolygonSub)
            {
                yData.Add(Math.Sqrt(Math.Pow(item.X, 2) + Math.Pow(item.Y, 2)));
            }
            List<int> xData = new List<int>();
            for (int i = 1; i <= ListPointGridinPolygon.Count(); i++)
            {
                xData.Add(i);
            }

            chart1.Series[0].LegendText = "原始数据点";
            chart1.Series[1].LegendText = "新数据点";
            chart1.Series[0].Points.DataBindXY(xData, yDataOld);
            chart1.Series[1].Points.DataBindXY(xData, yData);
        }

        private void DICResultShow_Load(object sender, EventArgs e)
        {

        }
    }
}
