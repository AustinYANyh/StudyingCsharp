namespace Csharp_myeverything
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
            this.components = new System.ComponentModel.Container();
            this.EDI_PATH = new System.Windows.Forms.TextBox();
            this.EDI_NAME = new System.Windows.Forms.TextBox();
            this.LAB_PATH = new System.Windows.Forms.Label();
            this.LAB_NAME = new System.Windows.Forms.Label();
            this.BTN_SHOW = new System.Windows.Forms.Button();
            this.BTN_CLEAR = new System.Windows.Forms.Button();
            this.LISTDATA = new System.Windows.Forms.ListView();
            this.HEAD_PATH = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.HEAD_NAME = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.MENU_OPEN = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.打开ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.打开所在路径ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MENU_OPEN.SuspendLayout();
            this.SuspendLayout();
            // 
            // EDI_PATH
            // 
            this.EDI_PATH.Location = new System.Drawing.Point(82, 21);
            this.EDI_PATH.Name = "EDI_PATH";
            this.EDI_PATH.Size = new System.Drawing.Size(185, 24);
            this.EDI_PATH.TabIndex = 0;
            // 
            // EDI_NAME
            // 
            this.EDI_NAME.Location = new System.Drawing.Point(82, 61);
            this.EDI_NAME.Name = "EDI_NAME";
            this.EDI_NAME.Size = new System.Drawing.Size(185, 24);
            this.EDI_NAME.TabIndex = 1;
            this.EDI_NAME.TextChanged += new System.EventHandler(this.EDI_NAME_TextChanged);
            // 
            // LAB_PATH
            // 
            this.LAB_PATH.AutoSize = true;
            this.LAB_PATH.Location = new System.Drawing.Point(16, 28);
            this.LAB_PATH.Name = "LAB_PATH";
            this.LAB_PATH.Size = new System.Drawing.Size(59, 17);
            this.LAB_PATH.TabIndex = 2;
            this.LAB_PATH.Text = "扫描路径:";
            // 
            // LAB_NAME
            // 
            this.LAB_NAME.AutoSize = true;
            this.LAB_NAME.Location = new System.Drawing.Point(16, 68);
            this.LAB_NAME.Name = "LAB_NAME";
            this.LAB_NAME.Size = new System.Drawing.Size(47, 17);
            this.LAB_NAME.TabIndex = 3;
            this.LAB_NAME.Text = "文件名:";
            // 
            // BTN_SHOW
            // 
            this.BTN_SHOW.Location = new System.Drawing.Point(287, 20);
            this.BTN_SHOW.Name = "BTN_SHOW";
            this.BTN_SHOW.Size = new System.Drawing.Size(40, 24);
            this.BTN_SHOW.TabIndex = 4;
            this.BTN_SHOW.Text = "显示";
            this.BTN_SHOW.UseVisualStyleBackColor = true;
            this.BTN_SHOW.Click += new System.EventHandler(this.BTN_SHOW_Click);
            // 
            // BTN_CLEAR
            // 
            this.BTN_CLEAR.Location = new System.Drawing.Point(287, 60);
            this.BTN_CLEAR.Name = "BTN_CLEAR";
            this.BTN_CLEAR.Size = new System.Drawing.Size(40, 24);
            this.BTN_CLEAR.TabIndex = 7;
            this.BTN_CLEAR.Text = "清除";
            this.BTN_CLEAR.UseVisualStyleBackColor = true;
            this.BTN_CLEAR.Click += new System.EventHandler(this.BTN_CLEAR_Click);
            // 
            // LISTDATA
            // 
            this.LISTDATA.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.HEAD_PATH,
            this.HEAD_NAME});
            this.LISTDATA.ContextMenuStrip = this.MENU_OPEN;
            this.LISTDATA.FullRowSelect = true;
            this.LISTDATA.GridLines = true;
            this.LISTDATA.Location = new System.Drawing.Point(20, 99);
            this.LISTDATA.Name = "LISTDATA";
            this.LISTDATA.Size = new System.Drawing.Size(352, 165);
            this.LISTDATA.TabIndex = 8;
            this.LISTDATA.UseCompatibleStateImageBehavior = false;
            this.LISTDATA.View = System.Windows.Forms.View.Details;
            // 
            // HEAD_PATH
            // 
            this.HEAD_PATH.Text = "路径";
            // 
            // HEAD_NAME
            // 
            this.HEAD_NAME.Text = "文件名";
            // 
            // MENU_OPEN
            // 
            this.MENU_OPEN.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.打开ToolStripMenuItem,
            this.打开所在路径ToolStripMenuItem});
            this.MENU_OPEN.Name = "MENU_OPEN";
            this.MENU_OPEN.Size = new System.Drawing.Size(153, 70);
            // 
            // 打开ToolStripMenuItem
            // 
            this.打开ToolStripMenuItem.Name = "打开ToolStripMenuItem";
            this.打开ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.打开ToolStripMenuItem.Text = "打开";
            this.打开ToolStripMenuItem.Click += new System.EventHandler(this.打开ToolStripMenuItem_Click);
            // 
            // 打开所在路径ToolStripMenuItem
            // 
            this.打开所在路径ToolStripMenuItem.Name = "打开所在路径ToolStripMenuItem";
            this.打开所在路径ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.打开所在路径ToolStripMenuItem.Text = "打开所在路径";
            this.打开所在路径ToolStripMenuItem.Click += new System.EventHandler(this.打开所在路径ToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(455, 280);
            this.Controls.Add(this.LISTDATA);
            this.Controls.Add(this.BTN_CLEAR);
            this.Controls.Add(this.BTN_SHOW);
            this.Controls.Add(this.LAB_NAME);
            this.Controls.Add(this.LAB_PATH);
            this.Controls.Add(this.EDI_NAME);
            this.Controls.Add(this.EDI_PATH);
            this.Name = "Form1";
            this.Text = "Form1";
            this.MENU_OPEN.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox EDI_PATH;
        private System.Windows.Forms.TextBox EDI_NAME;
        private System.Windows.Forms.Label LAB_PATH;
        private System.Windows.Forms.Label LAB_NAME;
        private System.Windows.Forms.Button BTN_SHOW;
        private System.Windows.Forms.Button BTN_CLEAR;
        private System.Windows.Forms.ListView LISTDATA;
        private System.Windows.Forms.ColumnHeader HEAD_PATH;
        private System.Windows.Forms.ColumnHeader HEAD_NAME;
        private System.Windows.Forms.ContextMenuStrip MENU_OPEN;
        private System.Windows.Forms.ToolStripMenuItem 打开ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 打开所在路径ToolStripMenuItem;
    }
}

