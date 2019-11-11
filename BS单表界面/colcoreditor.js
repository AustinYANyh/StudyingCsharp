function colcoreditor_loadData(data, thisDom)
{

}

function colcoreditor_cancel(thisDom) {
    $(thisDom).parent().hide();

}

function colcoreditor_beforeCancel(thisDom) {

}

function colcoreditor_confirm(thisDom) {
    $(thisDom).parent().hide();

}

function colcoreditor_beforeConfirm(thisDom) {

}

//初始化函数
function colcoreditor_init(parentDom){
    //获得本窗口的包含DIV，e.g. thidDOM == thisDIV
    var _this = parentDom.querySelector("div");;
    GUI.Pages.colcoreditor = _this;

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
        _div_class = _this.querySelector("._pg_colcoreditor_eb_colcortest11").children[0];
    //必须在这个地方调用
    _div_controlType = _div_class.getAttribute("_controlType");
    _div_class._parentDom = _this;
    _div_class_init_func = eval(_div_controlType);
    _div_class_init_func(_div_class);
    
    _this._pg_colcoreditor_eb_colcortest11 = _div_class;
    GUI.Doms._pg_colcoreditor_eb_colcortest11 = _div_class;
    _div_class = new Object();

    //#endregion


    //#region 界面组仅需要修改名称的部分

    //创建loadData
    _this.loadData = function(data)
    {
        var __this = _this;
        colcoreditor_loadData(data, __this);//注意有两个下划线__this
    }

        //Important！！！！！！！！是Add还是Edit需要做好判断!!!!!!!!!!
    _this.cancel = function()
    {
        var __this = _this;
        colcoreditor_cancel(__this);
    }
    
    _this.beforeCancel = function () {
        var __this = _this;
        colcoreditor_beforeCancel(__this);
    }
    _this.confirm = function () {
        var __this = _this;
        colcoreditor_confirm(__this);
    }
    _this.beforeConfirm = function () {
        var __this = _this;
        colcoreditor_beforeConfirm(__this);
    }

    //bind button click event
    var _div_footer = _this.querySelector(".orui_popup_footer");
    _div_footer.querySelectorAll("button")[0].onclick = _this.cancel;
    _div_footer.querySelectorAll("button")[1].onclick = _this.confirm;

    //endregion
}

