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
    public partial class SingleCameraSetting : Form
    {
        public double m_double_ExposureTime;    //定义相机的曝光时间
        public int m_int_GainLevel;     //定义相机的增益等级
        public int m_int_CameraFrame;       //定义相机的拍摄帧率

        public bool m_bool_IsSnapShot;      //定义相机是否连续采集
        public int m_int_SnapShotNumbers;       //定义相机连续采集张数
        public int m_int_SnapShotTimes;     //定义相机采集的时间间隔

        public bool m_bool_IsOK;

        public SingleCameraSetting()
        {
            m_double_ExposureTime = 0;
            m_int_GainLevel = 0;
            m_int_CameraFrame = 0;
            m_bool_IsSnapShot = false;
            m_int_SnapShotNumbers = 0;
            m_int_SnapShotTimes = 0;

            m_bool_IsOK = false;
            InitializeComponent();
        }
        public SingleCameraSetting(double ExposureTime, int GainLevel, int CameraFrame, bool IsSnapShot, int SnapShotNumbers, int SnapShotTimes)
        {
            m_double_ExposureTime = ExposureTime;
            m_int_GainLevel = GainLevel;
            m_int_CameraFrame = CameraFrame;
            m_bool_IsSnapShot = IsSnapShot;
            m_int_SnapShotNumbers = SnapShotNumbers;
            m_int_SnapShotTimes = SnapShotTimes;
            m_bool_IsOK = false;
            InitializeComponent();
        }

        private void SingleCameraSetting_Load(object sender, EventArgs e)
        {
            m_txt_ExposureTime.Text = (m_double_ExposureTime).ToString();
            m_trackB_GainLevel.Value = m_int_GainLevel;
            m_txt_CameraFrame.Text = (m_int_CameraFrame).ToString();
            m_checkb_IsSnapShot.Checked = m_bool_IsSnapShot;

            if (m_int_SnapShotNumbers < 0)
            {
                m_int_SnapShotNumbers = 0;
            }
            m_txt_SnapShotNumbers.Text = (m_int_SnapShotNumbers).ToString();
            m_txt_SnapShotTimes.Text = (m_int_SnapShotTimes).ToString();
        }

        private void m_btn_OK_Click(object sender, EventArgs e)
        {
            m_bool_IsOK = true;
            m_double_ExposureTime = Convert.ToDouble(m_txt_ExposureTime.Text);
            m_int_GainLevel = m_trackB_GainLevel.Value;
            m_int_CameraFrame = Convert.ToInt32(m_txt_CameraFrame.Text);
            m_bool_IsSnapShot = m_checkb_IsSnapShot.Checked;
            m_int_SnapShotNumbers = Convert.ToInt32(m_txt_SnapShotNumbers.Text);
            m_int_SnapShotTimes = Convert.ToInt32(m_txt_SnapShotTimes.Text);

            this.Close();
        }

        private void m_btn_Cancel_Click(object sender, EventArgs e)
        {
            m_bool_IsOK = false;
            this.Close();
        }

        private void m_checkb_IsSnapShot_CheckedChanged(object sender, EventArgs e)
        {
            if (m_checkb_IsSnapShot.Checked == false)
            {
                m_bool_IsSnapShot = false;

                m_txt_SnapShotNumbers.Enabled = false;
                m_txt_SnapShotTimes.Enabled = false;
                m_txt_SnapShotNumbers.Text = "0";
                m_txt_SnapShotTimes.Text = "0";
            }
            else
            {
                m_bool_IsSnapShot = true;

                m_txt_SnapShotNumbers.Enabled = true;
                m_txt_SnapShotTimes.Enabled = true;
            }
        }

        private void m_trackB_GainLevel_ValueChanged(object sender, EventArgs e)
        {
            m_lab_GainLevel.Text = "当前值:" + this.m_trackB_GainLevel.Value.ToString();
        }

        private void m_btn_ExposureTimePlus_Click(object sender, EventArgs e)
        {
            if ((Convert.ToDouble(m_txt_ExposureTime.Text) + 1000) >= 1000000)
            {
                m_txt_CameraFrame.Text = (1000000).ToString();
                return;
            }
            m_txt_ExposureTime.Text = (Convert.ToDouble(m_txt_ExposureTime.Text) + 1000).ToString();
        }

        private void m_btn_ExposureTimeReduce_Click(object sender, EventArgs e)
        {
            if ((Convert.ToDouble(m_txt_ExposureTime.Text) - 1000) <= 288)
            {
                m_txt_ExposureTime.Text = (288).ToString();
                return;
            }
            m_txt_ExposureTime.Text = (Convert.ToDouble(m_txt_ExposureTime.Text) - 1000).ToString();
        }

        private void m_txt_ExposureTime_Leave(object sender, EventArgs e)
        {
            double dShutterValue = 0.0;              //曝光值
            double dMin = 288.0;                       //最小值
            double dMax = 1000000.0;                       //最大值
            try
            {
                dShutterValue = Convert.ToDouble(m_txt_ExposureTime.Text);
            }
            catch (Exception)
            {
                m_txt_ExposureTime.Text = (30000).ToString();
                return;
            }
            //判断输入值是否在曝光时间的范围内
            //若大于最大值则将曝光值设为最大值
            if (dShutterValue > dMax)
            {
                dShutterValue = dMax;
            }
            //若小于最小值将曝光值设为最小值
            if (dShutterValue < dMin)
            {
                dShutterValue = dMin;
            }
            m_txt_ExposureTime.Text = dShutterValue.ToString();
        }

        private void m_txt_CameraFrame_Leave(object sender, EventArgs e)
        {
            double dGainValue = 0.0;              //帧率值HZ
            double dMin = 1;                       //帧率最小值HZ
            double dMax = 200.0;                       //帧率最大值HZ
            try
            {
                dGainValue = Convert.ToDouble(m_txt_CameraFrame.Text);
            }
            catch (Exception)
            {
                m_txt_CameraFrame.Text = (50).ToString();
                return;
            }
            //判断输入值是否在帧率HZ的范围内
            //若大于最大值则将帧率值设为最大值
            if (dGainValue > dMax)
            {
                dGainValue = dMax;
            }
            //若小于最小值将帧率值设为最小值
            if (dGainValue < dMin)
            {
                dGainValue = dMin;
            }
            m_txt_CameraFrame.Text = dGainValue.ToString();
        }

        private void m_btn_CameraFramePlus_Click(object sender, EventArgs e)
        {
            if ((Convert.ToDouble(m_txt_CameraFrame.Text) + 5) >= 200)
            {
                m_txt_CameraFrame.Text = (200).ToString();
                return;
            }
            m_txt_CameraFrame.Text = (Convert.ToDouble(m_txt_CameraFrame.Text) + 5).ToString();
        }

        private void m_btn_CameraFrameReduce_Click(object sender, EventArgs e)
        {
            if((Convert.ToDouble(m_txt_CameraFrame.Text) - 5) <= 0)
            {
                m_txt_CameraFrame.Text = (1).ToString();
                return;
            }
            m_txt_CameraFrame.Text = (Convert.ToDouble(m_txt_CameraFrame.Text) - 5).ToString();
        }

        private void m_txt_SnapShotNumbers_Leave(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToInt32(m_txt_SnapShotNumbers.Text) <= 0)
                {
                    m_txt_SnapShotNumbers.Text = "0";
                }
            }
            catch (Exception)
            {
                m_txt_SnapShotNumbers.Text = "0"; 
            }
        }

        private void m_txt_SnapShotTimes_Leave(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToInt32(m_txt_SnapShotTimes.Text) <= 0)
                {
                    m_txt_SnapShotTimes.Text = "0";
                }
            }
            catch (Exception)
            {
                m_txt_SnapShotTimes.Text = "0";
            }
        }
    }
}
