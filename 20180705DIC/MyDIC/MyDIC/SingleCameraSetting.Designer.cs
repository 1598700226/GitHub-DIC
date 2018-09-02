namespace MyDIC
{
    partial class SingleCameraSetting
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SingleCameraSetting));
            this.m_group_1 = new System.Windows.Forms.GroupBox();
            this.m_btn_ExposureTimeReduce = new System.Windows.Forms.Button();
            this.m_btn_ExposureTimePlus = new System.Windows.Forms.Button();
            this.m_lab_GainLevel = new System.Windows.Forms.Label();
            this.m_txt_ExposureTime = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.m_trackB_GainLevel = new System.Windows.Forms.TrackBar();
            this.m_group_2 = new System.Windows.Forms.GroupBox();
            this.m_cb_TriggerActivation = new System.Windows.Forms.ComboBox();
            this.m_cb_TriggerSource = new System.Windows.Forms.ComboBox();
            this.m_btn_SoftTriggerCommand = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.m_cb_TriggerMode = new System.Windows.Forms.ComboBox();
            this.m_btn_CameraFrameReduce = new System.Windows.Forms.Button();
            this.m_txt_SnapShotTimes = new System.Windows.Forms.TextBox();
            this.m_txt_SnapShotNumbers = new System.Windows.Forms.TextBox();
            this.m_txt_CameraFrame = new System.Windows.Forms.TextBox();
            this.m_btn_CameraFramePlus = new System.Windows.Forms.Button();
            this.m_checkb_IsSnapShot = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.m_btn_OK = new System.Windows.Forms.Button();
            this.m_btn_Cancel = new System.Windows.Forms.Button();
            this.m_group_1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_trackB_GainLevel)).BeginInit();
            this.m_group_2.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_group_1
            // 
            this.m_group_1.Controls.Add(this.m_btn_ExposureTimeReduce);
            this.m_group_1.Controls.Add(this.m_btn_ExposureTimePlus);
            this.m_group_1.Controls.Add(this.m_lab_GainLevel);
            this.m_group_1.Controls.Add(this.m_txt_ExposureTime);
            this.m_group_1.Controls.Add(this.label2);
            this.m_group_1.Controls.Add(this.label1);
            this.m_group_1.Controls.Add(this.m_trackB_GainLevel);
            this.m_group_1.Location = new System.Drawing.Point(12, 12);
            this.m_group_1.Name = "m_group_1";
            this.m_group_1.Size = new System.Drawing.Size(503, 128);
            this.m_group_1.TabIndex = 0;
            this.m_group_1.TabStop = false;
            this.m_group_1.Text = "基本参数设置";
            // 
            // m_btn_ExposureTimeReduce
            // 
            this.m_btn_ExposureTimeReduce.Location = new System.Drawing.Point(452, 35);
            this.m_btn_ExposureTimeReduce.Name = "m_btn_ExposureTimeReduce";
            this.m_btn_ExposureTimeReduce.Size = new System.Drawing.Size(21, 21);
            this.m_btn_ExposureTimeReduce.TabIndex = 5;
            this.m_btn_ExposureTimeReduce.Text = "-";
            this.m_btn_ExposureTimeReduce.UseVisualStyleBackColor = true;
            this.m_btn_ExposureTimeReduce.Click += new System.EventHandler(this.m_btn_ExposureTimeReduce_Click);
            // 
            // m_btn_ExposureTimePlus
            // 
            this.m_btn_ExposureTimePlus.Location = new System.Drawing.Point(429, 35);
            this.m_btn_ExposureTimePlus.Name = "m_btn_ExposureTimePlus";
            this.m_btn_ExposureTimePlus.Size = new System.Drawing.Size(21, 21);
            this.m_btn_ExposureTimePlus.TabIndex = 5;
            this.m_btn_ExposureTimePlus.Text = "+";
            this.m_btn_ExposureTimePlus.UseVisualStyleBackColor = true;
            this.m_btn_ExposureTimePlus.Click += new System.EventHandler(this.m_btn_ExposureTimePlus_Click);
            // 
            // m_lab_GainLevel
            // 
            this.m_lab_GainLevel.AutoSize = true;
            this.m_lab_GainLevel.Location = new System.Drawing.Point(299, 110);
            this.m_lab_GainLevel.Name = "m_lab_GainLevel";
            this.m_lab_GainLevel.Size = new System.Drawing.Size(59, 12);
            this.m_lab_GainLevel.TabIndex = 4;
            this.m_lab_GainLevel.Text = "当前值：8";
            // 
            // m_txt_ExposureTime
            // 
            this.m_txt_ExposureTime.Location = new System.Drawing.Point(301, 34);
            this.m_txt_ExposureTime.Name = "m_txt_ExposureTime";
            this.m_txt_ExposureTime.Size = new System.Drawing.Size(122, 21);
            this.m_txt_ExposureTime.TabIndex = 3;
            this.m_txt_ExposureTime.Text = "30000";
            this.m_txt_ExposureTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.m_txt_ExposureTime.Leave += new System.EventHandler(this.m_txt_ExposureTime_Leave);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(34, 37);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(191, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "曝光时间：(288.00~1000000.00)us";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(32, 87);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "增益：(0~63)";
            // 
            // m_trackB_GainLevel
            // 
            this.m_trackB_GainLevel.Location = new System.Drawing.Point(151, 77);
            this.m_trackB_GainLevel.Maximum = 63;
            this.m_trackB_GainLevel.Name = "m_trackB_GainLevel";
            this.m_trackB_GainLevel.Size = new System.Drawing.Size(346, 45);
            this.m_trackB_GainLevel.TabIndex = 0;
            this.m_trackB_GainLevel.Value = 8;
            this.m_trackB_GainLevel.ValueChanged += new System.EventHandler(this.m_trackB_GainLevel_ValueChanged);
            // 
            // m_group_2
            // 
            this.m_group_2.Controls.Add(this.m_cb_TriggerActivation);
            this.m_group_2.Controls.Add(this.m_cb_TriggerSource);
            this.m_group_2.Controls.Add(this.m_btn_SoftTriggerCommand);
            this.m_group_2.Controls.Add(this.label7);
            this.m_group_2.Controls.Add(this.label6);
            this.m_group_2.Controls.Add(this.label8);
            this.m_group_2.Controls.Add(this.label9);
            this.m_group_2.Controls.Add(this.m_cb_TriggerMode);
            this.m_group_2.Controls.Add(this.m_btn_CameraFrameReduce);
            this.m_group_2.Controls.Add(this.m_txt_SnapShotTimes);
            this.m_group_2.Controls.Add(this.m_txt_SnapShotNumbers);
            this.m_group_2.Controls.Add(this.m_txt_CameraFrame);
            this.m_group_2.Controls.Add(this.m_btn_CameraFramePlus);
            this.m_group_2.Controls.Add(this.m_checkb_IsSnapShot);
            this.m_group_2.Controls.Add(this.label5);
            this.m_group_2.Controls.Add(this.label4);
            this.m_group_2.Controls.Add(this.label10);
            this.m_group_2.Controls.Add(this.label3);
            this.m_group_2.Location = new System.Drawing.Point(13, 147);
            this.m_group_2.Name = "m_group_2";
            this.m_group_2.Size = new System.Drawing.Size(502, 210);
            this.m_group_2.TabIndex = 1;
            this.m_group_2.TabStop = false;
            this.m_group_2.Text = "拍摄设置";
            // 
            // m_cb_TriggerActivation
            // 
            this.m_cb_TriggerActivation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_cb_TriggerActivation.FormattingEnabled = true;
            this.m_cb_TriggerActivation.Location = new System.Drawing.Point(374, 167);
            this.m_cb_TriggerActivation.Name = "m_cb_TriggerActivation";
            this.m_cb_TriggerActivation.Size = new System.Drawing.Size(113, 20);
            this.m_cb_TriggerActivation.TabIndex = 20;
            // 
            // m_cb_TriggerSource
            // 
            this.m_cb_TriggerSource.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_cb_TriggerSource.FormattingEnabled = true;
            this.m_cb_TriggerSource.Location = new System.Drawing.Point(374, 81);
            this.m_cb_TriggerSource.Name = "m_cb_TriggerSource";
            this.m_cb_TriggerSource.Size = new System.Drawing.Size(113, 20);
            this.m_cb_TriggerSource.TabIndex = 16;
            // 
            // m_btn_SoftTriggerCommand
            // 
            this.m_btn_SoftTriggerCommand.Location = new System.Drawing.Point(374, 123);
            this.m_btn_SoftTriggerCommand.Name = "m_btn_SoftTriggerCommand";
            this.m_btn_SoftTriggerCommand.Size = new System.Drawing.Size(113, 23);
            this.m_btn_SoftTriggerCommand.TabIndex = 18;
            this.m_btn_SoftTriggerCommand.Text = "发送软触发命令";
            this.m_btn_SoftTriggerCommand.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(298, 171);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 12);
            this.label7.TabIndex = 19;
            this.label7.Text = "触发极性";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(310, 128);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 12);
            this.label6.TabIndex = 17;
            this.label6.Text = "软触发";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(310, 87);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(41, 12);
            this.label8.TabIndex = 15;
            this.label8.Text = "触发源";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(298, 43);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(53, 12);
            this.label9.TabIndex = 13;
            this.label9.Text = "触发模式";
            // 
            // m_cb_TriggerMode
            // 
            this.m_cb_TriggerMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_cb_TriggerMode.FormattingEnabled = true;
            this.m_cb_TriggerMode.Location = new System.Drawing.Point(374, 41);
            this.m_cb_TriggerMode.Name = "m_cb_TriggerMode";
            this.m_cb_TriggerMode.Size = new System.Drawing.Size(113, 20);
            this.m_cb_TriggerMode.TabIndex = 14;
            // 
            // m_btn_CameraFrameReduce
            // 
            this.m_btn_CameraFrameReduce.Location = new System.Drawing.Point(256, 40);
            this.m_btn_CameraFrameReduce.Name = "m_btn_CameraFrameReduce";
            this.m_btn_CameraFrameReduce.Size = new System.Drawing.Size(21, 21);
            this.m_btn_CameraFrameReduce.TabIndex = 5;
            this.m_btn_CameraFrameReduce.Text = "-";
            this.m_btn_CameraFrameReduce.UseVisualStyleBackColor = true;
            this.m_btn_CameraFrameReduce.Click += new System.EventHandler(this.m_btn_CameraFrameReduce_Click);
            // 
            // m_txt_SnapShotTimes
            // 
            this.m_txt_SnapShotTimes.Enabled = false;
            this.m_txt_SnapShotTimes.Location = new System.Drawing.Point(181, 167);
            this.m_txt_SnapShotTimes.Name = "m_txt_SnapShotTimes";
            this.m_txt_SnapShotTimes.Size = new System.Drawing.Size(96, 21);
            this.m_txt_SnapShotTimes.TabIndex = 3;
            this.m_txt_SnapShotTimes.Text = "0";
            this.m_txt_SnapShotTimes.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.m_txt_SnapShotTimes.Leave += new System.EventHandler(this.m_txt_SnapShotTimes_Leave);
            // 
            // m_txt_SnapShotNumbers
            // 
            this.m_txt_SnapShotNumbers.Enabled = false;
            this.m_txt_SnapShotNumbers.Location = new System.Drawing.Point(181, 119);
            this.m_txt_SnapShotNumbers.Name = "m_txt_SnapShotNumbers";
            this.m_txt_SnapShotNumbers.Size = new System.Drawing.Size(96, 21);
            this.m_txt_SnapShotNumbers.TabIndex = 3;
            this.m_txt_SnapShotNumbers.Text = "0";
            this.m_txt_SnapShotNumbers.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.m_txt_SnapShotNumbers.Leave += new System.EventHandler(this.m_txt_SnapShotNumbers_Leave);
            // 
            // m_txt_CameraFrame
            // 
            this.m_txt_CameraFrame.Location = new System.Drawing.Point(108, 40);
            this.m_txt_CameraFrame.Name = "m_txt_CameraFrame";
            this.m_txt_CameraFrame.Size = new System.Drawing.Size(106, 21);
            this.m_txt_CameraFrame.TabIndex = 3;
            this.m_txt_CameraFrame.Text = "1";
            this.m_txt_CameraFrame.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.m_txt_CameraFrame.Leave += new System.EventHandler(this.m_txt_CameraFrame_Leave);
            // 
            // m_btn_CameraFramePlus
            // 
            this.m_btn_CameraFramePlus.Location = new System.Drawing.Point(233, 40);
            this.m_btn_CameraFramePlus.Name = "m_btn_CameraFramePlus";
            this.m_btn_CameraFramePlus.Size = new System.Drawing.Size(21, 21);
            this.m_btn_CameraFramePlus.TabIndex = 5;
            this.m_btn_CameraFramePlus.Text = "+";
            this.m_btn_CameraFramePlus.UseVisualStyleBackColor = true;
            this.m_btn_CameraFramePlus.Click += new System.EventHandler(this.m_btn_CameraFramePlus_Click);
            // 
            // m_checkb_IsSnapShot
            // 
            this.m_checkb_IsSnapShot.AutoSize = true;
            this.m_checkb_IsSnapShot.Location = new System.Drawing.Point(36, 83);
            this.m_checkb_IsSnapShot.Name = "m_checkb_IsSnapShot";
            this.m_checkb_IsSnapShot.Size = new System.Drawing.Size(72, 16);
            this.m_checkb_IsSnapShot.TabIndex = 0;
            this.m_checkb_IsSnapShot.Text = "连续拍摄";
            this.m_checkb_IsSnapShot.UseVisualStyleBackColor = true;
            this.m_checkb_IsSnapShot.CheckedChanged += new System.EventHandler(this.m_checkb_IsSnapShot_CheckedChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(34, 170);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(107, 12);
            this.label5.TabIndex = 2;
            this.label5.Text = "采集间隔：(ms/张)";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(34, 122);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 2;
            this.label4.Text = "采集张数：";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(214, 44);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(17, 12);
            this.label10.TabIndex = 2;
            this.label10.Text = "HZ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(34, 43);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "相机帧率：";
            // 
            // m_btn_OK
            // 
            this.m_btn_OK.Location = new System.Drawing.Point(359, 363);
            this.m_btn_OK.Name = "m_btn_OK";
            this.m_btn_OK.Size = new System.Drawing.Size(75, 23);
            this.m_btn_OK.TabIndex = 2;
            this.m_btn_OK.Text = "确定";
            this.m_btn_OK.UseVisualStyleBackColor = true;
            this.m_btn_OK.Click += new System.EventHandler(this.m_btn_OK_Click);
            // 
            // m_btn_Cancel
            // 
            this.m_btn_Cancel.Location = new System.Drawing.Point(440, 363);
            this.m_btn_Cancel.Name = "m_btn_Cancel";
            this.m_btn_Cancel.Size = new System.Drawing.Size(75, 23);
            this.m_btn_Cancel.TabIndex = 3;
            this.m_btn_Cancel.Text = "取消";
            this.m_btn_Cancel.UseVisualStyleBackColor = true;
            this.m_btn_Cancel.Click += new System.EventHandler(this.m_btn_Cancel_Click);
            // 
            // SingleCameraSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(527, 394);
            this.Controls.Add(this.m_btn_Cancel);
            this.Controls.Add(this.m_btn_OK);
            this.Controls.Add(this.m_group_2);
            this.Controls.Add(this.m_group_1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SingleCameraSetting";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "单相机参数设置";
            this.Load += new System.EventHandler(this.SingleCameraSetting_Load);
            this.m_group_1.ResumeLayout(false);
            this.m_group_1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_trackB_GainLevel)).EndInit();
            this.m_group_2.ResumeLayout(false);
            this.m_group_2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox m_group_1;
        private System.Windows.Forms.Button m_btn_ExposureTimeReduce;
        private System.Windows.Forms.Button m_btn_ExposureTimePlus;
        private System.Windows.Forms.Label m_lab_GainLevel;
        private System.Windows.Forms.TextBox m_txt_ExposureTime;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TrackBar m_trackB_GainLevel;
        private System.Windows.Forms.GroupBox m_group_2;
        private System.Windows.Forms.Button m_btn_CameraFrameReduce;
        private System.Windows.Forms.TextBox m_txt_SnapShotTimes;
        private System.Windows.Forms.TextBox m_txt_SnapShotNumbers;
        private System.Windows.Forms.TextBox m_txt_CameraFrame;
        private System.Windows.Forms.Button m_btn_CameraFramePlus;
        private System.Windows.Forms.CheckBox m_checkb_IsSnapShot;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox m_cb_TriggerActivation;
        private System.Windows.Forms.ComboBox m_cb_TriggerSource;
        private System.Windows.Forms.Button m_btn_SoftTriggerCommand;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox m_cb_TriggerMode;
        private System.Windows.Forms.Button m_btn_OK;
        private System.Windows.Forms.Button m_btn_Cancel;
        private System.Windows.Forms.Label label10;
    }
}