function achievementEditor_loadData(data, thisDom)
{
    thisDom._pg_achievementEditor_eb_achievementTest5.loadData(data);
}

function achievementEditor_cancel(thisDom)
{
    $(thisDom).parent().hide();
    orui_allowedClick($(thisDom).parent());
}

function achievementEditor_beforeCancel(thisDom)
{

}

function achievementEditor_confirm(thisDom)
{
    //$(thisDom).parent().hide();
    //更新成绩表
    var parent=thisDom.parentElement.parentElement;
    var pgData=thisDom._pg_achievementEditor_eb_achievementTest5.data;
    var _data = GUI.Doms._dg_index_eb_achievementTest5.getDataSet();
    var data = GUI.Doms._dg_index_eb_studentTest5.getSelectRowData();

    try
    {
        data.totalScore=0;
        orui_datagrid_edit(parent,pgData);
        for(var i=0;i<_data.length;++i)
        {
            data.totalScore+=_data[i].fraction;
        }

        data = GUI.Doms._dg_index_eb_studentTest5.getDataSet();
        orui_datagrid_edit(GUI.Doms._dg_index_eb_studentTest5,data);
    }
    catch(e)
    {
        console.log("没有选中学生表中的信息!");
    }

    orui_allowedClick($(thisDom).parent());
}

function achievementEditor_beforeConfirm(thisDom) {

}

//初始化函数
function achievementEditor_init(parentDom){
    //获得本窗口的包含DIV，e.g. thidDOM == thisDIV
    var _this = parentDom.querySelector("div");
    GUI.Pages.achievementEditor = _this;

    _this._parent = parentDom;
    parentDom._this = _this;

    //获得Infos
    var _info_str = _this.getAttribute("infos");
    var _infos = JSON.parse(_info_str);
    var _div_class = null;
    var _div_class_entityBias = null;
    var _div_class_infos = null;
    var _div_controlType = null;
    var _div_class_init_func = null;
    //#region 需要界面组需要添加，修改的主要部分
    //设置每个子控件到_this
    //假设有一个控件放在className为class1的div下面
        _div_class = _this.querySelector("._pg_achievementEditor_eb_achievementTest5").children[0];
    //必须在这个地方调用
    _div_controlType = _div_class.getAttribute("_controlType");
    _div_class._parentDom = _this;
    _div_class_init_func = eval(_div_controlType);
    _div_class_init_func(_div_class);
    
    _this._pg_achievementEditor_eb_achievementTest5 = _div_class;
    GUI.Doms._pg_achievementEditor_eb_achievementTest5 = _div_class;
    _div_class.addOnEndEditListener(achievementEditor_pg_endEdit);
    _div_class = new Object();

    //#endregion


    //#region 界面组仅需要修改名称的部分

    //创建loadData
    _this.loadData = function(data)
    {
        var __this = _this;
        achievementEditor_loadData(data, __this);//注意有两个下划线__this
    }

        //Important！！！！！！！！是Add还是Edit需要做好判断!!!!!!!!!!
    _this.cancel = function()
    {
        var __this = _this;
        achievementEditor_cancel(__this);
    }
    
    _this.beforeCancel = function () {
        var __this = _this;
        achievementEditor_beforeCancel(__this);
    }
    _this.confirm = function () {
        var __this = _this;
        achievementEditor_confirm(__this);
    }
    _this.beforeConfirm = function () {
        var __this = _this;
        achievementEditor_beforeConfirm(__this);
    }

    //bind button click event
    var _div_footer = _this.querySelector(".orui_popup_footer");
    _div_footer.querySelectorAll("button")[0].onclick = _this.cancel;
    _div_footer.querySelectorAll("button")[1].onclick = _this.confirm;

    //endregion
}

function achievementEditor_pg_endEdit(data,orgValue,newValue,info)
{
    if(info.classField=='fraction' || info.classField=='totalScore')
    {
        var passLine=0.6;
        if((data.fraction / data.totalScore) > passLine)
            data.isPass=0;
        else
            data.isPass=1;
    }

    GUI.Doms._pg_achievementEditor_eb_achievementTest5.loadData(data);
}