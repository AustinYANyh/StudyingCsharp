using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ORAPS.Fabric
{
    public partial class StudentEditor : UserControl
    {
        public StudentEditor()
        {
            InitializeComponent();
            this.Load += StudentEditor_Load;
        }

        DataClient.Client Client = null;
        ORAPS.Helpers.GUI ThisGUI = null;
        private SplitContainer splitContainer1;
        private GUILib.OAS_GridView RGV_Student;
        private Telerik.WinControls.UI.RadGroupBox RGB_Search;
        private Telerik.WinControls.UI.RadButton BTN_Search;
        private Telerik.WinControls.UI.RadButton BTN_Clear;
        private Telerik.WinControls.UI.RadLabel LAB_Name;
        private Telerik.WinControls.UI.RadTextBox TXT_Name;
        private Telerik.WinControls.UI.RadLabel LAB_ID;
        private Telerik.WinControls.UI.RadTextBox TXT_ID;
        ORAPS.Helpers.Action ACT_StudentEditor_Student = null;

        private void StudentEditor_Load(object sender, EventArgs e)
        {
            Client = ORAPS.DataClient.ClientBase.DataSvcClient;
            if (Client == null) return;
            ThisGUI = Client.TargetRole.GetGUIByName("GUI_StudentEditor");
            if (ThisGUI == null) return;
            ACT_StudentEditor_Student = ThisGUI.GetActionByName("ACT_StudentEditor_Student");
            if (ACT_StudentEditor_Student == null) return;

            RGV_Student.SetupView(Client, ThisGUI.Name, ACT_StudentEditor_Student.Name);

            setColName();
        }

        public void setColName()
        {
            LAB_ID.Text = ORAPS.Helpers.HelperStrs._ID;
            LAB_Name.Text = ORAPS.Helpers.HelperStrs._Name;
            BTN_Search.Text = ORAPS.Helpers.HelperStrs._Search;
            BTN_Clear.Text = ORAPS.Helpers.HelperStrs._Clear;
        }
        #region 自动生成
        private void InitializeComponent()
        {
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.RGV_Student = new ORAPS.GUILib.OAS_GridView();
            this.RGB_Search = new Telerik.WinControls.UI.RadGroupBox();
            this.BTN_Search = new Telerik.WinControls.UI.RadButton();
            this.BTN_Clear = new Telerik.WinControls.UI.RadButton();
            this.LAB_Name = new Telerik.WinControls.UI.RadLabel();
            this.TXT_Name = new Telerik.WinControls.UI.RadTextBox();
            this.LAB_ID = new Telerik.WinControls.UI.RadLabel();
            this.TXT_ID = new Telerik.WinControls.UI.RadTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RGB_Search)).BeginInit();
            this.RGB_Search.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BTN_Search)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BTN_Clear)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LAB_Name)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TXT_Name)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LAB_ID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TXT_ID)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.RGB_Search);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.RGV_Student);
            this.splitContainer1.Size = new System.Drawing.Size(589, 407);
            this.splitContainer1.SplitterDistance = 141;
            this.splitContainer1.TabIndex = 0;
            // 
            // RGV_Student
            // 
            this.RGV_Student.AddCtr = null;
            this.RGV_Student.AutoDelete = true;
            this.RGV_Student.BaseFilter = null;
            this.RGV_Student.BatEdit = null;
            this.RGV_Student.ContinueNormalSave = true;
            this.RGV_Student.CoreFilter = "NaN";
            this.RGV_Student.CurrentData = null;
            this.RGV_Student.CurrentSetPage = 1;
            this.RGV_Student.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RGV_Student.ErrorMessage = null;
            this.RGV_Student.FilterString = "";
            this.RGV_Student.GUIName = null;
            this.RGV_Student.Location = new System.Drawing.Point(0, 0);
            this.RGV_Student.Name = "RGV_Student";
            this.RGV_Student.ObjectType = null;
            this.RGV_Student.OrderBy = null;
            this.RGV_Student.ParentConnection = null;
            this.RGV_Student.ParentGrid = null;
            this.RGV_Student.Presaved = true;
            this.RGV_Student.SelectedAdd = false;
            this.RGV_Student.SelectedDel = false;
            this.RGV_Student.SelectedSav = false;
            this.RGV_Student.SelectedUpd = false;
            this.RGV_Student.SetPageSize = 2000;
            this.RGV_Student.ShowAdd = true;
            this.RGV_Student.ShowBatEdit = true;
            this.RGV_Student.ShowCustomFilter = false;
            this.RGV_Student.ShowCustomGrouping = false;
            this.RGV_Student.ShowDBPage = true;
            this.RGV_Student.ShowDelete = true;
            this.RGV_Student.ShowEdit = true;
            this.RGV_Student.ShowExport = true;
            this.RGV_Student.ShowFilter = false;
            this.RGV_Student.ShowGrouping = false;
            this.RGV_Student.ShowImport = true;
            this.RGV_Student.ShowPage = true;
            this.RGV_Student.ShowPageSize = true;
            this.RGV_Student.ShowPrint = true;
            this.RGV_Student.ShowQuery = true;
            this.RGV_Student.ShowRCB = true;
            this.RGV_Student.ShowRefresh = true;
            this.RGV_Student.ShowSave = true;
            this.RGV_Student.ShowSelectAll = true;
            this.RGV_Student.ShowSelectNull = false;
            this.RGV_Student.Size = new System.Drawing.Size(589, 262);
            this.RGV_Student.TabIndex = 0;
            this.RGV_Student.TotalPages = 1;
            this.RGV_Student.TotalRecords = 0;
            // 
            // RGB_Search
            // 
            this.RGB_Search.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this.RGB_Search.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(234)))), ((int)(((byte)(240)))));
            this.RGB_Search.Controls.Add(this.BTN_Search);
            this.RGB_Search.Controls.Add(this.BTN_Clear);
            this.RGB_Search.Controls.Add(this.LAB_Name);
            this.RGB_Search.Controls.Add(this.TXT_Name);
            this.RGB_Search.Controls.Add(this.LAB_ID);
            this.RGB_Search.Controls.Add(this.TXT_ID);
            this.RGB_Search.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RGB_Search.HeaderText = "Search";
            this.RGB_Search.Location = new System.Drawing.Point(0, 0);
            this.RGB_Search.Name = "RGB_Search";
            this.RGB_Search.Size = new System.Drawing.Size(589, 141);
            this.RGB_Search.TabIndex = 1;
            this.RGB_Search.Text = "Search";
            // 
            // BTN_Search
            // 
            this.BTN_Search.Location = new System.Drawing.Point(111, 95);
            this.BTN_Search.Name = "BTN_Search";
            this.BTN_Search.Size = new System.Drawing.Size(122, 24);
            this.BTN_Search.TabIndex = 2;
            this.BTN_Search.Text = "Search";
            // 
            // BTN_Clear
            // 
            this.BTN_Clear.Location = new System.Drawing.Point(330, 95);
            this.BTN_Clear.Name = "BTN_Clear";
            this.BTN_Clear.Size = new System.Drawing.Size(122, 24);
            this.BTN_Clear.TabIndex = 2;
            this.BTN_Clear.Text = "Clear";
            // 
            // LAB_Name
            // 
            this.LAB_Name.Location = new System.Drawing.Point(273, 34);
            this.LAB_Name.Name = "LAB_Name";
            this.LAB_Name.Size = new System.Drawing.Size(36, 18);
            this.LAB_Name.TabIndex = 0;
            this.LAB_Name.Text = "Name";
            // 
            // TXT_Name
            // 
            this.TXT_Name.Location = new System.Drawing.Point(330, 34);
            this.TXT_Name.Name = "TXT_Name";
            this.TXT_Name.Size = new System.Drawing.Size(122, 20);
            this.TXT_Name.TabIndex = 1;
            // 
            // LAB_ID
            // 
            this.LAB_ID.Location = new System.Drawing.Point(42, 34);
            this.LAB_ID.Name = "LAB_ID";
            this.LAB_ID.Size = new System.Drawing.Size(17, 18);
            this.LAB_ID.TabIndex = 0;
            this.LAB_ID.Text = "ID";
            // 
            // TXT_ID
            // 
            this.TXT_ID.Location = new System.Drawing.Point(111, 34);
            this.TXT_ID.Name = "TXT_ID";
            this.TXT_ID.Size = new System.Drawing.Size(122, 20);
            this.TXT_ID.TabIndex = 1;
            // 
            // StudentEditor
            // 
            this.Controls.Add(this.splitContainer1);
            this.Name = "StudentEditor";
            this.Size = new System.Drawing.Size(589, 407);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.RGB_Search)).EndInit();
            this.RGB_Search.ResumeLayout(false);
            this.RGB_Search.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BTN_Search)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BTN_Clear)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LAB_Name)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TXT_Name)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LAB_ID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TXT_ID)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion
    }
}
