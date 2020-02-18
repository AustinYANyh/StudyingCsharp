namespace chatroom
{
    partial class login
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(login));
            this.LAB_USERNAME = new System.Windows.Forms.Label();
            this.EDI_USERNAME = new System.Windows.Forms.TextBox();
            this.EDI_PASSWD = new System.Windows.Forms.TextBox();
            this.LAB_PASSWD = new System.Windows.Forms.Label();
            this.BTN_LOGON = new System.Windows.Forms.Button();
            this.BTN_LOGIN = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // LAB_USERNAME
            // 
            this.LAB_USERNAME.BackColor = System.Drawing.Color.Transparent;
            this.LAB_USERNAME.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LAB_USERNAME.ForeColor = System.Drawing.Color.Transparent;
            this.LAB_USERNAME.Image = ((System.Drawing.Image)(resources.GetObject("LAB_USERNAME.Image")));
            this.LAB_USERNAME.Location = new System.Drawing.Point(61, 57);
            this.LAB_USERNAME.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LAB_USERNAME.Name = "LAB_USERNAME";
            this.LAB_USERNAME.Size = new System.Drawing.Size(29, 31);
            this.LAB_USERNAME.TabIndex = 0;
            this.LAB_USERNAME.Text = "   ";
            // 
            // EDI_USERNAME
            // 
            this.EDI_USERNAME.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(242)))), ((int)(((byte)(244)))));
            this.EDI_USERNAME.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.EDI_USERNAME.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.EDI_USERNAME.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.EDI_USERNAME.Location = new System.Drawing.Point(112, 57);
            this.EDI_USERNAME.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.EDI_USERNAME.Multiline = true;
            this.EDI_USERNAME.Name = "EDI_USERNAME";
            this.EDI_USERNAME.Size = new System.Drawing.Size(250, 31);
            this.EDI_USERNAME.TabIndex = 0;
            // 
            // EDI_PASSWD
            // 
            this.EDI_PASSWD.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(242)))), ((int)(((byte)(244)))));
            this.EDI_PASSWD.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.EDI_PASSWD.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.EDI_PASSWD.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.EDI_PASSWD.Location = new System.Drawing.Point(112, 119);
            this.EDI_PASSWD.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.EDI_PASSWD.Name = "EDI_PASSWD";
            this.EDI_PASSWD.Size = new System.Drawing.Size(250, 26);
            this.EDI_PASSWD.TabIndex = 1;
            this.EDI_PASSWD.UseSystemPasswordChar = true;
            this.EDI_PASSWD.KeyDown += new System.Windows.Forms.KeyEventHandler(this.EDI_PASSWD_KeyDown);
            // 
            // LAB_PASSWD
            // 
            this.LAB_PASSWD.Image = ((System.Drawing.Image)(resources.GetObject("LAB_PASSWD.Image")));
            this.LAB_PASSWD.Location = new System.Drawing.Point(61, 119);
            this.LAB_PASSWD.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LAB_PASSWD.Name = "LAB_PASSWD";
            this.LAB_PASSWD.Size = new System.Drawing.Size(29, 31);
            this.LAB_PASSWD.TabIndex = 2;
            this.LAB_PASSWD.Text = "  ";
            // 
            // BTN_LOGON
            // 
            this.BTN_LOGON.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BTN_LOGON.Location = new System.Drawing.Point(244, 180);
            this.BTN_LOGON.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.BTN_LOGON.Name = "BTN_LOGON";
            this.BTN_LOGON.Size = new System.Drawing.Size(96, 38);
            this.BTN_LOGON.TabIndex = 3;
            this.BTN_LOGON.Text = "注册";
            this.BTN_LOGON.UseVisualStyleBackColor = true;
            this.BTN_LOGON.Click += new System.EventHandler(this.BTN_LOGON_Click);
            // 
            // BTN_LOGIN
            // 
            this.BTN_LOGIN.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BTN_LOGIN.Location = new System.Drawing.Point(116, 180);
            this.BTN_LOGIN.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.BTN_LOGIN.Name = "BTN_LOGIN";
            this.BTN_LOGIN.Size = new System.Drawing.Size(96, 38);
            this.BTN_LOGIN.TabIndex = 4;
            this.BTN_LOGIN.Text = "登录";
            this.BTN_LOGIN.UseVisualStyleBackColor = true;
            this.BTN_LOGIN.Click += new System.EventHandler(this.BTN_LOGIN_Click);
            // 
            // login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(424, 244);
            this.Controls.Add(this.BTN_LOGIN);
            this.Controls.Add(this.BTN_LOGON);
            this.Controls.Add(this.LAB_PASSWD);
            this.Controls.Add(this.EDI_PASSWD);
            this.Controls.Add(this.EDI_USERNAME);
            this.Controls.Add(this.LAB_USERNAME);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "login";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "登录";
            this.Load += new System.EventHandler(this.login_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label LAB_USERNAME;
        private System.Windows.Forms.TextBox EDI_PASSWD;
        private System.Windows.Forms.Label LAB_PASSWD;
        public System.Windows.Forms.TextBox EDI_USERNAME;
        private System.Windows.Forms.Button BTN_LOGON;
        private System.Windows.Forms.Button BTN_LOGIN;
    }
}