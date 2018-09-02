using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MyDIC
{
    public partial class CalibrationSetting : Form
    {
        public int m_int_RowCornerNums = 0;
        public int m_int_ColCornerNums = 0;
        public double m_double_RowLength = 0;
        public double m_double_ColLength = 0;

        public bool isOK = false;

        public CalibrationSetting()
        {
            InitializeComponent();

            m_txt_ColCornerNums.Leave += new EventHandler(CheckLeaveNumber_Leave);
            m_txt_ColLength.Leave += new EventHandler(CheckLeaveNumber_Leave);
            m_txt_RowCornerNums.Leave += new EventHandler(CheckLeaveNumber_Leave);
            m_txt_RowLength.Leave += new EventHandler(CheckLeaveNumber_Leave);

            m_txt_ColCornerNums.Text = m_int_ColCornerNums.ToString();
            m_txt_RowCornerNums.Text = m_int_RowCornerNums.ToString();
            m_txt_ColLength.Text = m_double_ColLength.ToString();
            m_txt_RowLength.Text = m_double_RowLength.ToString();
        }
        public CalibrationSetting(int RowCornerNums, int ColCornerNums, double RowLength, double ColLength)
        {
            InitializeComponent();

            m_txt_ColCornerNums.Leave += new EventHandler(CheckLeaveNumber_Leave);
            m_txt_ColLength.Leave += new EventHandler(CheckLeaveNumber_Leave);
            m_txt_RowCornerNums.Leave += new EventHandler(CheckLeaveNumber_Leave);
            m_txt_RowLength.Leave += new EventHandler(CheckLeaveNumber_Leave);

            m_txt_ColCornerNums.Text = ColCornerNums.ToString();
            m_txt_RowCornerNums.Text = RowCornerNums.ToString();
            m_txt_ColLength.Text = ColLength.ToString();
            m_txt_RowLength.Text = RowLength.ToString();
        }


        private void CheckLeaveNumber_Leave(object sender, EventArgs e)
        {          
            try 
	        {
                if (Convert.ToInt32(((TextBox)sender).Text) <= 0)
                {
                    ((TextBox)sender).Text = "0";
                }
	        }
	        catch (Exception)
	        {

                ((TextBox)sender).Text = "0";
	        }           
        }


        private void m_btn_OK_Click(object sender, EventArgs e)
        {
            isOK = true;
            m_int_ColCornerNums = Convert.ToInt32(m_txt_ColCornerNums.Text);
            m_int_RowCornerNums = Convert.ToInt32(m_txt_RowCornerNums.Text);
            m_double_ColLength = Convert.ToDouble(m_txt_ColLength.Text);
            m_double_RowLength = Convert.ToDouble(m_txt_RowLength.Text);
            this.Close();
        }

        private void m_btn_Cancel_Click(object sender, EventArgs e)
        {
            isOK = false;
            this.Close();
        }
    }
}
