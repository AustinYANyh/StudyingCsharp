namespace FriendLIst
{
    partial class Form1
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
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.LAB_GRP1 = new System.Windows.Forms.Label();
            this.LAB_GRP2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.PIC_USER = new System.Windows.Forms.PictureBox();
            this.LAB_USER = new System.Windows.Forms.Label();
            this.EDI_BELOWUSER = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.PIC_USER)).BeginInit();
            this.SuspendLayout();
            // 
            // LAB_GRP1
            // 
            this.LAB_GRP1.AutoSize = true;
            this.LAB_GRP1.Location = new System.Drawing.Point(18, 122);
            this.LAB_GRP1.Name = "LAB_GRP1";
            this.LAB_GRP1.Size = new System.Drawing.Size(92, 25);
            this.LAB_GRP1.TabIndex = 0;
            this.LAB_GRP1.Text = "分组列表1";
            this.LAB_GRP1.Click += new System.EventHandler(this.LAB_GRP1_Click);
            this.LAB_GRP1.MouseEnter += new System.EventHandler(this.LAB_GRP1_MouseEnter);
            this.LAB_GRP1.MouseLeave += new System.EventHandler(this.LAB_GRP1_MouseLeave);
            // 
            // LAB_GRP2
            // 
            this.LAB_GRP2.AutoSize = true;
            this.LAB_GRP2.Location = new System.Drawing.Point(18, 164);
            this.LAB_GRP2.Name = "LAB_GRP2";
            this.LAB_GRP2.Size = new System.Drawing.Size(94, 25);
            this.LAB_GRP2.TabIndex = 1;
            this.LAB_GRP2.Text = "分组列表2";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(102, 458);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 46);
            this.button1.TabIndex = 3;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Location = new System.Drawing.Point(26, 149);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(246, 15);
            this.panel1.TabIndex = 4;
            this.panel1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseMove);
            // 
            // PIC_USER
            // 
            this.PIC_USER.Location = new System.Drawing.Point(23, 12);
            this.PIC_USER.Name = "PIC_USER";
            this.PIC_USER.Size = new System.Drawing.Size(102, 97);
            this.PIC_USER.TabIndex = 5;
            this.PIC_USER.TabStop = false;
            // 
            // LAB_USER
            // 
            this.LAB_USER.AutoSize = true;
            this.LAB_USER.Location = new System.Drawing.Point(136, 24);
            this.LAB_USER.Name = "LAB_USER";
            this.LAB_USER.Size = new System.Drawing.Size(95, 25);
            this.LAB_USER.TabIndex = 6;
            this.LAB_USER.Text = "LAB_USER";
            // 
            // EDI_BELOWUSER
            // 
            this.EDI_BELOWUSER.BackColor = System.Drawing.SystemColors.Control;
            this.EDI_BELOWUSER.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.EDI_BELOWUSER.Location = new System.Drawing.Point(136, 74);
            this.EDI_BELOWUSER.Name = "EDI_BELOWUSER";
            this.EDI_BELOWUSER.ReadOnly = true;
            this.EDI_BELOWUSER.Size = new System.Drawing.Size(142, 26);
            this.EDI_BELOWUSER.TabIndex = 7;
            this.EDI_BELOWUSER.TabStop = false;
            this.EDI_BELOWUSER.Text = "签名框";
            this.EDI_BELOWUSER.Click += new System.EventHandler(this.EDI_BELOWUSER_Click);
            this.EDI_BELOWUSER.MouseLeave += new System.EventHandler(this.EDI_BELOWUSER_MouseLeave);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(297, 516);
            this.Controls.Add(this.EDI_BELOWUSER);
            this.Controls.Add(this.LAB_USER);
            this.Controls.Add(this.PIC_USER);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.LAB_GRP2);
            this.Controls.Add(this.LAB_GRP1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.PIC_USER)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label LAB_GRP1;
        private System.Windows.Forms.Label LAB_GRP2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox PIC_USER;
        private System.Windows.Forms.Label LAB_USER;
        private System.Windows.Forms.TextBox EDI_BELOWUSER;
    }
}

