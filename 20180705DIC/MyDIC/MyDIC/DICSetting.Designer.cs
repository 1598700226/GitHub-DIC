namespace MyDIC
{
    partial class DICSetting
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.m_txt_RelateSize = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.m_txt_ExtendSize = new System.Windows.Forms.TextBox();
            this.m_txt_SetSize = new System.Windows.Forms.TextBox();
            this.m_btn_Cancel = new System.Windows.Forms.Button();
            this.m_btn_OK = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.m_txt_RelateSize);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.m_txt_ExtendSize);
            this.groupBox1.Controls.Add(this.m_txt_SetSize);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(320, 155);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "相关计算参数设置";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(101, 27);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(0, 12);
            this.label5.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 68);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(161, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "相关计算区域大小（像素）：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(26, 113);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(137, 12);
            this.label3.TabIndex = 1;
            this.label3.Text = "搜索扩展区域（像素）：";
            // 
            // m_txt_RelateSize
            // 
            this.m_txt_RelateSize.Location = new System.Drawing.Point(231, 65);
            this.m_txt_RelateSize.Name = "m_txt_RelateSize";
            this.m_txt_RelateSize.Size = new System.Drawing.Size(62, 21);
            this.m_txt_RelateSize.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(26, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(125, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "像素点间隔（像素）：";
            // 
            // m_txt_ExtendSize
            // 
            this.m_txt_ExtendSize.Location = new System.Drawing.Point(231, 110);
            this.m_txt_ExtendSize.Name = "m_txt_ExtendSize";
            this.m_txt_ExtendSize.Size = new System.Drawing.Size(62, 21);
            this.m_txt_ExtendSize.TabIndex = 0;
            // 
            // m_txt_SetSize
            // 
            this.m_txt_SetSize.Location = new System.Drawing.Point(231, 24);
            this.m_txt_SetSize.Name = "m_txt_SetSize";
            this.m_txt_SetSize.Size = new System.Drawing.Size(62, 21);
            this.m_txt_SetSize.TabIndex = 0;
            // 
            // m_btn_Cancel
            // 
            this.m_btn_Cancel.Location = new System.Drawing.Point(257, 172);
            this.m_btn_Cancel.Name = "m_btn_Cancel";
            this.m_btn_Cancel.Size = new System.Drawing.Size(75, 23);
            this.m_btn_Cancel.TabIndex = 9;
            this.m_btn_Cancel.Text = "取消";
            this.m_btn_Cancel.UseVisualStyleBackColor = true;
            this.m_btn_Cancel.Click += new System.EventHandler(this.m_btn_Cancel_Click);
            // 
            // m_btn_OK
            // 
            this.m_btn_OK.Location = new System.Drawing.Point(176, 172);
            this.m_btn_OK.Name = "m_btn_OK";
            this.m_btn_OK.Size = new System.Drawing.Size(75, 23);
            this.m_btn_OK.TabIndex = 8;
            this.m_btn_OK.Text = "确定";
            this.m_btn_OK.UseVisualStyleBackColor = true;
            this.m_btn_OK.Click += new System.EventHandler(this.m_btn_OK_Click);
            // 
            // DICSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(342, 201);
            this.Controls.Add(this.m_btn_Cancel);
            this.Controls.Add(this.m_btn_OK);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DICSetting";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DIC参数设置";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox m_txt_RelateSize;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox m_txt_ExtendSize;
        private System.Windows.Forms.TextBox m_txt_SetSize;
        private System.Windows.Forms.Button m_btn_Cancel;
        private System.Windows.Forms.Button m_btn_OK;
    }
}