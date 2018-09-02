namespace MyDIC.UI
{
    partial class CalibrationDataInput
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CalibrationDataInput));
            this.txtXmlPath = new System.Windows.Forms.TextBox();
            this.txtXmlData = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btn_Cancel = new System.Windows.Forms.Button();
            this.btn_Finish = new System.Windows.Forms.Button();
            this.btn_nextStep = new System.Windows.Forms.Button();
            this.btn_lastStep = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtXmlPath
            // 
            this.txtXmlPath.Location = new System.Drawing.Point(107, 12);
            this.txtXmlPath.Name = "txtXmlPath";
            this.txtXmlPath.ReadOnly = true;
            this.txtXmlPath.Size = new System.Drawing.Size(469, 21);
            this.txtXmlPath.TabIndex = 0;
            // 
            // txtXmlData
            // 
            this.txtXmlData.BackColor = System.Drawing.SystemColors.ControlLight;
            this.txtXmlData.Location = new System.Drawing.Point(14, 65);
            this.txtXmlData.Multiline = true;
            this.txtXmlData.Name = "txtXmlData";
            this.txtXmlData.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtXmlData.Size = new System.Drawing.Size(562, 255);
            this.txtXmlData.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "标定文件路径：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "文件内容预览";
            // 
            // btn_Cancel
            // 
            this.btn_Cancel.Location = new System.Drawing.Point(501, 326);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Size = new System.Drawing.Size(75, 23);
            this.btn_Cancel.TabIndex = 4;
            this.btn_Cancel.Text = "取消";
            this.btn_Cancel.UseVisualStyleBackColor = true;
            this.btn_Cancel.Click += new System.EventHandler(this.btn_Cancel_Click);
            // 
            // btn_Finish
            // 
            this.btn_Finish.Location = new System.Drawing.Point(420, 326);
            this.btn_Finish.Name = "btn_Finish";
            this.btn_Finish.Size = new System.Drawing.Size(75, 23);
            this.btn_Finish.TabIndex = 4;
            this.btn_Finish.Text = "完成";
            this.btn_Finish.UseVisualStyleBackColor = true;
            this.btn_Finish.Click += new System.EventHandler(this.btn_Finish_Click);
            // 
            // btn_nextStep
            // 
            this.btn_nextStep.Location = new System.Drawing.Point(325, 326);
            this.btn_nextStep.Name = "btn_nextStep";
            this.btn_nextStep.Size = new System.Drawing.Size(75, 23);
            this.btn_nextStep.TabIndex = 4;
            this.btn_nextStep.Text = "下一步>";
            this.btn_nextStep.UseVisualStyleBackColor = true;
            this.btn_nextStep.Click += new System.EventHandler(this.btn_nextStep_Click);
            // 
            // btn_lastStep
            // 
            this.btn_lastStep.Location = new System.Drawing.Point(244, 326);
            this.btn_lastStep.Name = "btn_lastStep";
            this.btn_lastStep.Size = new System.Drawing.Size(75, 23);
            this.btn_lastStep.TabIndex = 4;
            this.btn_lastStep.Text = "<上一步";
            this.btn_lastStep.UseVisualStyleBackColor = true;
            this.btn_lastStep.Click += new System.EventHandler(this.btn_lastStep_Click);
            // 
            // CalibrationDataInput
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(588, 363);
            this.Controls.Add(this.btn_lastStep);
            this.Controls.Add(this.btn_nextStep);
            this.Controls.Add(this.btn_Finish);
            this.Controls.Add(this.btn_Cancel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtXmlData);
            this.Controls.Add(this.txtXmlPath);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CalibrationDataInput";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "标定文件信息";
            this.Load += new System.EventHandler(this.CalibrationDataInput_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtXmlPath;
        private System.Windows.Forms.TextBox txtXmlData;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btn_Cancel;
        private System.Windows.Forms.Button btn_Finish;
        private System.Windows.Forms.Button btn_nextStep;
        private System.Windows.Forms.Button btn_lastStep;
    }
}