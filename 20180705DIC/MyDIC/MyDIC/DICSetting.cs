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
    public partial class DICSetting : Form
    {
        public int m_int_ExtendSize = 0;
        public int m_int_RelateSize = 0;
        public int m_int_SetSize = 0;
        public bool isOK = false;

        public DICSetting()
        {
            InitializeComponent();

            m_txt_ExtendSize.Leave += new EventHandler(CheckLeaveNumber_Leave);
            m_txt_RelateSize.Leave += new EventHandler(CheckLeaveNumber_Leave);
            m_txt_SetSize.Leave += new EventHandler(CheckLeaveNumber_Leave);
        }
        public DICSetting(int ExtendSize, int RelateSize,int SetSize)
        {
            InitializeComponent();
            m_txt_ExtendSize.Text = ExtendSize.ToString();
            m_txt_RelateSize.Text = RelateSize.ToString();
            m_txt_SetSize.Text = SetSize.ToString();
            m_txt_ExtendSize.Leave += new EventHandler(CheckLeaveNumber_Leave);
            m_txt_RelateSize.Leave += new EventHandler(CheckLeaveNumber_Leave);
            m_txt_SetSize.Leave += new EventHandler(CheckLeaveNumber_Leave);
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
            m_int_ExtendSize = Convert.ToInt32(m_txt_ExtendSize.Text);
            m_int_RelateSize = Convert.ToInt32(m_txt_RelateSize.Text);
            m_int_SetSize = Convert.ToInt32(m_txt_SetSize.Text);
            this.Close();
        }

        private void m_btn_Cancel_Click(object sender, EventArgs e)
        {
            isOK = false;
            this.Close();
        }
    }
}
