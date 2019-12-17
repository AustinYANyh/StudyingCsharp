    //父窗口的代码
    //这个按钮必须为public
    public void BTN_SEARCH_Click(object sender, EventArgs e)
    {
        search se = new search();
        se.StartPosition = FormStartPosition.CenterParent;

        //这里必须这么写,写成se.show()程序无法运行
        se.Show(this);
    }

    //EDI_PATH控件属性Modifiers修改成public

     //弹出的子窗口中的按钮的代码
    private void BTN_SURE_Click(object sender, EventArgs e)
    {
        string path = Search_EDI_PATH.Text.Trim();
        //Form1 frm = new Form1();
        //frm.txt = path;
        //frm.Show();
        //EDI_PATH.Text = path;
        Form1 frm = (Form1)this.Owner;
        frm.EDI_PATH.Text = path;
        this.Close();
    }