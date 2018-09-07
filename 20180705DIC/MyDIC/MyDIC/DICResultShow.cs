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
        public DICResultShow(List<double> ptValue)
        {
            InitializeComponent();
            double[] xData = new double[ptValue.Count];
            for (int i = 0; i < ptValue.Count; i++)
            {
                xData[i] = i + 1;
            }
            chart1.Series[0].LegendText = "偏差值";
            chart1.Series[0].Points.DataBindXY(xData, ptValue);

        }

        private void DICResultShow_Load(object sender, EventArgs e)
        {

        }
    }
}
