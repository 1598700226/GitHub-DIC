using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MyDIC.UI
{
    public partial class CalibrationDataInput : Form
    {
        public bool m_bool_lastStep = false;
        public bool m_bool_nextStep = false;
        public bool m_bool_Finish = false;
        public bool m_bool_Cancel = false;
        public int m_int_return = 0; //1代表上一步，2代表下一步，3代表完成，4代表取消

        public CalibrationDataInput(string strPath, string strData)
        {
            InitializeComponent();
            this.txtXmlPath.Text = strPath;
            this.txtXmlData.Text = strData;
        }

        private void CalibrationDataInput_Load(object sender, EventArgs e)
        {
            m_bool_lastStep = false;
            m_bool_nextStep = false;
            m_bool_Finish = false;
            m_bool_Cancel = true;
            m_int_return = 4; 
        }

        private void btn_lastStep_Click(object sender, EventArgs e)
        {
            m_bool_lastStep = true;
            m_bool_nextStep = false;
            m_bool_Finish = false;
            m_bool_Cancel = false;
            m_int_return = 1;
            this.Close();
        }

        private void btn_nextStep_Click(object sender, EventArgs e)
        {
            m_bool_lastStep = false;
            m_bool_nextStep = true;
            m_bool_Finish = false;
            m_bool_Cancel = false;
            m_int_return = 2;
            this.Close();
        }

        private void btn_Finish_Click(object sender, EventArgs e)
        {
            m_bool_lastStep = false;
            m_bool_nextStep = false;
            m_bool_Finish = true;
            m_bool_Cancel = false;
            m_int_return = 3;
            this.Close();
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            m_bool_lastStep = false;
            m_bool_nextStep = false;
            m_bool_Finish = false;
            m_bool_Cancel = true;
            m_int_return = 4;
            this.Close();
        }
    }
}
