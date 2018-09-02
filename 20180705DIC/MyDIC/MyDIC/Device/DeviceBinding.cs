using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;

namespace MyDIC.Device
{
    class DeviceBinding
    {
        #region API函数
        /// <summary>
        /// 读取INI文件内容
        /// </summary>
        /// <param name="lpAppName">表示INI文件内部根节点的值</param>
        /// <param name="lpKeyName">表示根节点下子标记值</param>
        /// <param name="lpDefault">表示当标记值未设定或者不存在时的默认值</param>
        /// <param name="lpRetrunedString">表示返回读取节点的值</param>
        /// <param name="nSize">表示读取的节点内容的最大容量</param>
        /// <param name="lpFileName">表示文件的全路径</param>
        /// <returns></returns>
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, StringBuilder lpRetrunedString, int nSize, string lpFileName);
        /// <summary>
        /// 向INI文件写数据
        /// </summary>
        /// <param name="mpAppName">表示INI文件内部根节点的值</param>
        /// <param name="mpKeyName">表示将要修改的标记名称</param>
        /// <param name="mpDefault">表示想要修改的内容</param>
        /// <param name="mpFileName">表示INI文件的全路径</param>
        /// <returns></returns>
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string mpAppName, string mpKeyName, string mpDefault, string mpFileName);
        #endregion

        /// <summary>
        /// 软件系统使用时间
        /// </summary>
        /// <param name="uselife">出口参数：true：在使用范围内；false：到期</param>
        public static void SoftWareSystemUsefulLife(out bool uselife)
        {
            string pathINI = System.Windows.Forms.Application.StartupPath + "\\config.ini";
            if (File.Exists(pathINI))
            {
                int _size = 9;
                StringBuilder _returnedString = new StringBuilder();
                StringBuilder _returnedString_num = new StringBuilder();
                int b = GetPrivateProfileString("Date", "SoftwareDate", "", _returnedString, _size, pathINI);
                b = GetPrivateProfileString("Date", "SoftwareDate", "", _returnedString_num, 7, pathINI);
                string serial = DateTime.Now.ToString("yyyyMMdd");
                string dateStr = _returnedString_num.ToString();
                string objectString = _returnedString.ToString();
                if (!dateStr.Equals("201312"))
                {
                    uselife = false;
                    return;
                }
                b = serial.CompareTo(objectString);
                if (b <= 0)
                {
                    uselife = true;
                    return;
                }
            }
            uselife = false;
        }


        /// <summary>
        /// 软件设备识别
        /// </summary>
        /// <param name="device">出口参数：true：型号匹配；false：型号不相符</param>
        public static void SoftWareSystemVerifyDevice(out bool device)
        {
            string pathINI = "C:\\Users\\Admin\\AppData\\Local\\VirtualStore\\Windows\\System32\\MVCCamera.ini";
            if (File.Exists(pathINI))
            {
                int _size = 9;
                StringBuilder _returnedString = new StringBuilder();
                StringBuilder _returnedString_num = new StringBuilder();
                //DvcNum
                int b = GetPrivateProfileString("MVC-F-DVC", "Camera0_SN", "", _returnedString, _size, pathINI);
                b = GetPrivateProfileString("MVC-F-DVC", "DvcNum", "", _returnedString_num, 2, pathINI);
                //this.textBox1.Text = _returnedString.ToString();
                string serial = "45200135";
                string number = _returnedString_num.ToString();
                int d = 0;
                bool f = int.TryParse(number, out d);
                string objectString = _returnedString.ToString();
                //int num=_returnedString_num
                if (serial.Equals(objectString))
                {
                    device = true;
                    return;
                }
            }
            device = false;
        }
    }
}
