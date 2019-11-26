function index_loadData(data, thisDom)
{

}

function index__dg_index_eb_studentTest3_connection(target,orgSelection,newSelection) {
    var _this = target._parentDom;
    var _target = target;

    //#region界面组需要填空的部分⬇️
    //获取需要联动的下级控件
    //e.g.⬇️
    var subDG = GUI.Doms._dg_index_eb_studentTest3;
    var value = newSelection["id"];
    var filter = "studentId='"+value +"'";

    subDG.setFilter(filter);
    //orui_datagrid_refresh(subDG);

    //#endregion
}

//function index__dg_index_eb_studentTest3_beforeAdd(target,newData) {
//}

//初始化函数
function index_init(parentDom){
    //获得本窗口的包含DIV，e.g. thidDOM == thisDIV
    var _this = parentDom;
    GUI.Pages.index = _this;

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
        _div_class = _this.querySelector("._dg_index_eb_studentTest3").children[0];
    //必须在这个地方调用
    _div_controlType = _div_class.getAttribute("_controlType");
    _div_class._parentDom = _this;
    _div_class_init_func = eval(_div_controlType);
    _div_class_init_func(_div_class);
    
    _this._dg_index_eb_studentTest3 = _div_class;
    GUI.Doms._dg_index_eb_studentTest3 = _div_class;
    _div_class = new Object();
    //写Connection相关的events
    _this._dg_index_eb_studentTest3.addOnSelectionChangedListener(index__dg_index_eb_studentTest3_connection);


    //写特殊events
    //_this._dg_index_eb_studentTest3.addOnBeforeAddListener(index__dg_index_eb_studentTest3_beforeAdd);
    _div_class = _this.querySelector("._dg_index_eb_achievementTest3").children[0];
    //必须在这个地方调用
    _div_controlType = _div_class.getAttribute("_controlType");
    _div_class._parentDom = _this;
    _div_class_init_func = eval(_div_controlType);
    _div_class_init_func(_div_class);
    
    _this._dg_index_eb_achievementTest3 = _div_class;
    GUI.Doms._dg_index_eb_achievementTest3 = _div_class;
    _div_class = new Object();

    //#endregion


    //#region 界面组仅需要修改名称的部分

    //创建loadData
    _this.loadData = function(data)
    {
        var __this = _this;
        index_loadData(data, __this);//注意有两个下划线__this
    }

    
    //endregion
}

