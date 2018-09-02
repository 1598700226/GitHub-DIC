namespace MyDIC
{
    partial class CalibrationSetting
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
            this.m_btn_Cancel = new System.Windows.Forms.Button();
            this.m_btn_OK = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.laber2 = new System.Windows.Forms.Label();
            this.m_txt_ColLength = new System.Windows.Forms.TextBox();
            this.m_txt_RowLength = new System.Windows.Forms.TextBox();
            this.m_txt_ColCornerNums = new System.Windows.Forms.TextBox();
            this.m_txt_RowCornerNums = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_btn_Cancel
            // 
            this.m_btn_Cancel.Location = new System.Drawing.Point(257, 174);
            this.m_btn_Cancel.Name = "m_btn_Cancel";
            this.m_btn_Cancel.Size = new System.Drawing.Size(75, 23);
            this.m_btn_Cancel.TabIndex = 5;
            this.m_btn_Cancel.Text = "取消";
            this.m_btn_Cancel.UseVisualStyleBackColor = true;
            this.m_btn_Cancel.Click += new System.EventHandler(this.m_btn_Cancel_Click);
            // 
            // m_btn_OK
            // 
            this.m_btn_OK.Location = new System.Drawing.Point(176, 174);
            this.m_btn_OK.Name = "m_btn_OK";
            this.m_btn_OK.Size = new System.Drawing.Size(75, 23);
            this.m_btn_OK.TabIndex = 4;
            this.m_btn_OK.Text = "确定";
            this.m_btn_OK.UseVisualStyleBackColor = true;
            this.m_btn_OK.Click += new System.EventHandler(this.m_btn_OK_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.laber2);
            this.groupBox1.Controls.Add(this.m_txt_ColLength);
            this.groupBox1.Controls.Add(this.m_txt_RowLength);
            this.groupBox1.Controls.Add(this.m_txt_ColCornerNums);
            this.groupBox1.Controls.Add(this.m_txt_RowCornerNums);
            this.groupBox1.Location = new System.Drawing.Point(12, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(320, 155);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "棋盘格参数设置";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(101, 91);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(137, 12);
            this.label6.TabIndex = 2;
            this.label6.Text = "棋盘格每格实际物理长度";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(101, 27);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(101, 12);
            this.label5.TabIndex = 2;
            this.label5.Text = "实际角点存在个数";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(172, 118);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 12);
            this.label4.TabIndex = 1;
            this.label4.Text = "每格宽(mm)：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 118);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 12);
            this.label3.TabIndex = 1;
            this.label3.Text = "每格长(mm)：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "行(个)：";
            // 
            // laber2
            // 
            this.laber2.AutoSize = true;
            this.laber2.Location = new System.Drawing.Point(172, 57);
            this.laber2.Name = "laber2";
            this.laber2.Size = new System.Drawing.Size(53, 12);
            this.laber2.TabIndex = 1;
            this.laber2.Text = "列(个)：";
            // 
            // m_txt_ColLength
            // 
            this.m_txt_ColLength.Location = new System.Drawing.Point(255, 115);
            this.m_txt_ColLength.Name = "m_txt_ColLength";
            this.m_txt_ColLength.Size = new System.Drawing.Size(52, 21);
            this.m_txt_ColLength.TabIndex = 0;
            // 
            // m_txt_RowLength
            // 
            this.m_txt_RowLength.Location = new System.Drawing.Point(100, 115);
            this.m_txt_RowLength.Name = "m_txt_RowLength";
            this.m_txt_RowLength.Size = new System.Drawing.Size(52, 21);
            this.m_txt_RowLength.TabIndex = 0;
            // 
            // m_txt_ColCornerNums
            // 
            this.m_txt_ColCornerNums.Location = new System.Drawing.Point(245, 54);
            this.m_txt_ColCornerNums.Name = "m_txt_ColCornerNums";
            this.m_txt_ColCornerNums.Size = new System.Drawing.Size(62, 21);
            this.m_txt_ColCornerNums.TabIndex = 0;
            // 
            // m_txt_RowCornerNums
            // 
            this.m_txt_RowCornerNums.Location = new System.Drawing.Point(90, 54);
            this.m_txt_RowCornerNums.Name = "m_txt_RowCornerNums";
            this.m_txt_RowCornerNums.Size = new System.Drawing.Size(62, 21);
            this.m_txt_RowCornerNums.TabIndex = 0;
            // 
            // CalibrationSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(341, 206);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.m_btn_Cancel);
            this.Controls.Add(this.m_btn_OK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CalibrationSetting";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "相机标定参数设置";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button m_btn_Cancel;
        private System.Windows.Forms.Button m_btn_OK;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label laber2;
        private System.Windows.Forms.TextBox m_txt_ColLength;
        private System.Windows.Forms.TextBox m_txt_RowLength;
        private System.Windows.Forms.TextBox m_txt_ColCornerNums;
        private System.Windows.Forms.TextBox m_txt_RowCornerNums;

    }
}