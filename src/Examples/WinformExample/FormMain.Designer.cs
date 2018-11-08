namespace WinformExample
{
    partial class FormMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.checkBoxSuppressMouse = new System.Windows.Forms.CheckBox();
            this.panelSeparator = new System.Windows.Forms.Panel();
            this.radioGlobal = new System.Windows.Forms.RadioButton();
            this.radioApplication = new System.Windows.Forms.RadioButton();
            this.labelWheel = new System.Windows.Forms.Label();
            this.labelMousePosition = new System.Windows.Forms.Label();
            this.textBoxLog = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnPlayback = new System.Windows.Forms.Button();
            this.btnRecord = new System.Windows.Forms.Button();
            this.btnClearLog = new System.Windows.Forms.Button();
            this.checkBoxSupressMouseWheel = new System.Windows.Forms.CheckBox();
            this.radioNone = new System.Windows.Forms.RadioButton();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // checkBoxSuppressMouse
            // 
            this.checkBoxSuppressMouse.AutoSize = true;
            this.checkBoxSuppressMouse.Location = new System.Drawing.Point(165, 45);
            this.checkBoxSuppressMouse.Name = "checkBoxSuppressMouse";
            this.checkBoxSuppressMouse.Size = new System.Drawing.Size(159, 17);
            this.checkBoxSuppressMouse.TabIndex = 13;
            this.checkBoxSuppressMouse.Text = "Suppress Right Mouse Click";
            this.checkBoxSuppressMouse.UseVisualStyleBackColor = true;
            this.checkBoxSuppressMouse.CheckedChanged += new System.EventHandler(this.checkBoxSuppressMouse_CheckedChanged);
            // 
            // panelSeparator
            // 
            this.panelSeparator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelSeparator.BackColor = System.Drawing.Color.White;
            this.panelSeparator.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelSeparator.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panelSeparator.Location = new System.Drawing.Point(6, 34);
            this.panelSeparator.Name = "panelSeparator";
            this.panelSeparator.Size = new System.Drawing.Size(584, 1);
            this.panelSeparator.TabIndex = 11;
            // 
            // radioGlobal
            // 
            this.radioGlobal.AutoSize = true;
            this.radioGlobal.BackColor = System.Drawing.Color.White;
            this.radioGlobal.Checked = true;
            this.radioGlobal.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioGlobal.Location = new System.Drawing.Point(127, 12);
            this.radioGlobal.Name = "radioGlobal";
            this.radioGlobal.Size = new System.Drawing.Size(87, 17);
            this.radioGlobal.TabIndex = 10;
            this.radioGlobal.TabStop = true;
            this.radioGlobal.Text = "Global hooks";
            this.radioGlobal.UseVisualStyleBackColor = false;
            // 
            // radioApplication
            // 
            this.radioApplication.AutoSize = true;
            this.radioApplication.BackColor = System.Drawing.Color.White;
            this.radioApplication.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioApplication.Location = new System.Drawing.Point(12, 12);
            this.radioApplication.Name = "radioApplication";
            this.radioApplication.Size = new System.Drawing.Size(109, 17);
            this.radioApplication.TabIndex = 9;
            this.radioApplication.Text = "Application hooks";
            this.radioApplication.UseVisualStyleBackColor = false;
            // 
            // labelWheel
            // 
            this.labelWheel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelWheel.AutoSize = true;
            this.labelWheel.BackColor = System.Drawing.Color.White;
            this.labelWheel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelWheel.Location = new System.Drawing.Point(9, 70);
            this.labelWheel.Name = "labelWheel";
            this.labelWheel.Size = new System.Drawing.Size(89, 13);
            this.labelWheel.TabIndex = 6;
            this.labelWheel.Text = "Wheel={0:####}";
            // 
            // labelMousePosition
            // 
            this.labelMousePosition.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelMousePosition.AutoSize = true;
            this.labelMousePosition.BackColor = System.Drawing.Color.White;
            this.labelMousePosition.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelMousePosition.Location = new System.Drawing.Point(9, 46);
            this.labelMousePosition.Name = "labelMousePosition";
            this.labelMousePosition.Size = new System.Drawing.Size(125, 13);
            this.labelMousePosition.TabIndex = 2;
            this.labelMousePosition.Text = "x={0:####}; y={1:####}";
            // 
            // textBoxLog
            // 
            this.textBoxLog.BackColor = System.Drawing.Color.White;
            this.textBoxLog.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxLog.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxLog.Location = new System.Drawing.Point(0, 98);
            this.textBoxLog.Multiline = true;
            this.textBoxLog.Name = "textBoxLog";
            this.textBoxLog.ReadOnly = true;
            this.textBoxLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxLog.Size = new System.Drawing.Size(602, 215);
            this.textBoxLog.TabIndex = 10;
            this.textBoxLog.WordWrap = false;
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.White;
            this.groupBox2.Controls.Add(this.btnPlayback);
            this.groupBox2.Controls.Add(this.btnRecord);
            this.groupBox2.Controls.Add(this.btnClearLog);
            this.groupBox2.Controls.Add(this.checkBoxSupressMouseWheel);
            this.groupBox2.Controls.Add(this.radioNone);
            this.groupBox2.Controls.Add(this.checkBoxSuppressMouse);
            this.groupBox2.Controls.Add(this.panelSeparator);
            this.groupBox2.Controls.Add(this.radioGlobal);
            this.groupBox2.Controls.Add(this.radioApplication);
            this.groupBox2.Controls.Add(this.labelWheel);
            this.groupBox2.Controls.Add(this.labelMousePosition);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(602, 98);
            this.groupBox2.TabIndex = 11;
            this.groupBox2.TabStop = false;
            // 
            // btnPlayback
            // 
            this.btnPlayback.Enabled = false;
            this.btnPlayback.Location = new System.Drawing.Point(436, 66);
            this.btnPlayback.Name = "btnPlayback";
            this.btnPlayback.Size = new System.Drawing.Size(75, 21);
            this.btnPlayback.TabIndex = 18;
            this.btnPlayback.Text = "Playback";
            this.btnPlayback.UseVisualStyleBackColor = true;
            this.btnPlayback.Click += new System.EventHandler(this.btnPlayback_Click);
            // 
            // btnRecord
            // 
            this.btnRecord.Location = new System.Drawing.Point(356, 66);
            this.btnRecord.Name = "btnRecord";
            this.btnRecord.Size = new System.Drawing.Size(75, 21);
            this.btnRecord.TabIndex = 17;
            this.btnRecord.Text = "Record";
            this.btnRecord.UseVisualStyleBackColor = true;
            this.btnRecord.Click += new System.EventHandler(this.btnRecord_Click);
            // 
            // btnClearLog
            // 
            this.btnClearLog.Location = new System.Drawing.Point(515, 66);
            this.btnClearLog.Name = "btnClearLog";
            this.btnClearLog.Size = new System.Drawing.Size(75, 21);
            this.btnClearLog.TabIndex = 16;
            this.btnClearLog.Text = "Clear Log";
            this.btnClearLog.UseVisualStyleBackColor = true;
            this.btnClearLog.Click += new System.EventHandler(this.btnClearLog_Click);
            // 
            // checkBoxSupressMouseWheel
            // 
            this.checkBoxSupressMouseWheel.AutoSize = true;
            this.checkBoxSupressMouseWheel.Location = new System.Drawing.Point(165, 66);
            this.checkBoxSupressMouseWheel.Name = "checkBoxSupressMouseWheel";
            this.checkBoxSupressMouseWheel.Size = new System.Drawing.Size(139, 17);
            this.checkBoxSupressMouseWheel.TabIndex = 15;
            this.checkBoxSupressMouseWheel.Text = "Suppress Mouse Wheel";
            this.checkBoxSupressMouseWheel.UseVisualStyleBackColor = true;
            this.checkBoxSupressMouseWheel.CheckedChanged += new System.EventHandler(this.checkBoxSupressMouseWheel_CheckedChanged);
            // 
            // radioNone
            // 
            this.radioNone.AutoSize = true;
            this.radioNone.BackColor = System.Drawing.Color.White;
            this.radioNone.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioNone.Location = new System.Drawing.Point(220, 12);
            this.radioNone.Name = "radioNone";
            this.radioNone.Size = new System.Drawing.Size(51, 17);
            this.radioNone.TabIndex = 14;
            this.radioNone.Text = "None";
            this.radioNone.UseVisualStyleBackColor = false;
            this.radioNone.CheckedChanged += new System.EventHandler(this.radioNone_CheckedChanged);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(602, 313);
            this.Controls.Add(this.textBoxLog);
            this.Controls.Add(this.groupBox2);
            this.Name = "FormMain";
            this.Text = "Mouse and Keyboard Hooks And Simulators Demo";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBoxSuppressMouse;
        private System.Windows.Forms.Panel panelSeparator;
        private System.Windows.Forms.RadioButton radioGlobal;
        private System.Windows.Forms.RadioButton radioApplication;
        private System.Windows.Forms.Label labelWheel;
        private System.Windows.Forms.Label labelMousePosition;
        private System.Windows.Forms.TextBox textBoxLog;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnClearLog;
        private System.Windows.Forms.CheckBox checkBoxSupressMouseWheel;
        private System.Windows.Forms.RadioButton radioNone;
        private System.Windows.Forms.Button btnPlayback;
        private System.Windows.Forms.Button btnRecord;
    }
}

