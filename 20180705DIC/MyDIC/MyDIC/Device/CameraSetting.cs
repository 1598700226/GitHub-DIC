using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;

namespace MyDIC.Device
{
    class CameraSetting
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
        /// <param name="mpDefault">表示要修改的内容</param>
        /// <param name="mpFileName">表示INI文件的全路径</param>
        /// <returns></returns>
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string mpAppName, string mpKeyName, string mpDefault, string mpFileName);


        #endregion
        /// <summary>
        /// 修改保存参数
        /// </summary>
        /// <param name="Interval"></param>
        /// <param name="Exposure"></param>
        /// <param name="Gain"></param>
        public static void CameraParamterWriteFunction(int Interval, int Exposure, int Gain)
        {
            long ret;
            string pathINI = System.Windows.Forms.Application.StartupPath + "\\camera.ini";
            if (File.Exists(pathINI))
            {
                string str1 = Interval.ToString();
                ret = WritePrivateProfileString("Camera", "Interval", str1, pathINI);

                string str2 = Exposure.ToString();
                ret = WritePrivateProfileString("Camera", "Exposure", str2, pathINI);

                string str3 = Gain.ToString();
                ret = WritePrivateProfileString("Camera", "Gain", str3, pathINI);

            }
        }


        /// <summary>
        /// 相机参数读取
        /// </summary>
        /// <param name="Interval">采集时间间隔</param>
        /// <param name="Exposure">曝光时间</param>
        /// <param name="Gain">增益</param>
        public static void CameraParameterReadFunction(out int Interval, out int Exposure, out int Gain)
        {
            string pathINI = System.Windows.Forms.Application.StartupPath + "\\camera.ini";
            if (File.Exists(pathINI))
            {
                StringBuilder _Interval = new StringBuilder();
                StringBuilder _Exposure = new StringBuilder();
                StringBuilder _Gain = new StringBuilder();
                int b = GetPrivateProfileString("Camera", "Interval", "", _Interval, 4, pathINI);
                b = GetPrivateProfileString("Camera", "Exposure", "", _Exposure, 4, pathINI);
                b = GetPrivateProfileString("Camera", "Gain", "", _Gain, 3, pathINI);

                string str1 = _Interval.ToString();
                Interval = Convert.ToInt32(str1);
                string str2 = _Exposure.ToString();
                Exposure = Convert.ToInt32(str2);
                string str3 = _Gain.ToString();
                Gain = Convert.ToInt32(str3);
                return;
            }
            Interval = 400;
            Exposure = 400;
            Gain = 8;
        }
    }
}
