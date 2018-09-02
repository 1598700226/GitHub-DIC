using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace MyDIC.UI
{
    public class ControlUI
    {
        public static void m_btn_MouseEnter(object sender) //鼠标移进到控件为橙色
        {
            Control name = sender as Button;
            if (name == null)
                return;
            name.BackColor = Color.Orange;
        }
        public static void m_btn_MouseLeave(object sender) //鼠标离出控件为蓝色
        {
            Control name = sender as Button;
            if (name == null)
                return;
            name.BackColor = Color.LightSteelBlue;
        }
    }
}
