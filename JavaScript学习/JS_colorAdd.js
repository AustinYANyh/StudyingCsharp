function colorAdd_loadData(data, thisDom)
{
    thisDom._pg_colorAdd_eb_colorTest27.loadData(data);
}

function colorAdd_cancel(thisDom)
{
    $(thisDom).parent().hide();
    //var parent=thisDom.parentElement.parentElement;
    //var pgData=thisDom._pg_colorAdd_eb_colorTest27.data;
    //orui_datagrid_add(parent,pgData);
    orui_allowedClick($(thisDom).parent());
}

function colorAdd_beforeCancel(thisDom)
{

}

function colorAdd_confirm(thisDom)
{
    //$(thisDom).parent().hide();
    var _thisDom=thisDom;
    var parent=_thisDom.parentElement.parentElement;
    var data=_thisDom._pg_colorAdd_eb_colorTest27.data;

    orui.ajaxhelper.getId("color_id",1,function(res)
        {
            res=JSON.parse(res);
            data.id=res.ID;
            orui_datagrid_add(parent,data);
        }
    );
    orui_allowedClick($(thisDom).parent());
}

function colorAdd_beforeConfirm(thisDom) {

}

//��ʼ������
function colorAdd_init(parentDom){
    //��ñ����ڵİ���DIV��e.g. thidDOM == thisDIV
    var _this = parentDom.querySelector("div");;
    GUI.Pages.colorAdd = _this;

    _this._parent = parentDom;
    parentDom._this = _this;

    //���Infos
    var _info_str = _this.getAttribute("infos");
    var _infos = JSON.parse(_info_str);
    var _div_class = null;
    var _div_class_entityBias = null;
    var _div_class_infos = null;
    var _div_controlType = null;
    var _div_class_init_func = null;
    //#region ��Ҫ��������Ҫ��ӣ��޸ĵ���Ҫ����
    //����ÿ���ӿؼ���_this
    //������һ���ؼ�����classNameΪclass1��div����
        _div_class = _this.querySelector("._pg_colorAdd_eb_colorTest27").children[0];
    //����������ط�����
    _div_controlType = _div_class.getAttribute("_controlType");
    _div_class._parentDom = _this;
    _div_class_init_func = eval(_div_controlType);
    _div_class_init_func(_div_class);
    
    _this._pg_colorAdd_eb_colorTest27 = _div_class;
    GUI.Doms._pg_colorAdd_eb_colorTest27 = _div_class;
    _div_class = new Object();

    //#endregion


    //#region ���������Ҫ�޸����ƵĲ���

    //����loadData
    _this.loadData = function(data)
    {
        var __this = _this;
        colorAdd_loadData(data, __this);//ע���������»���__this
    }

        //Important������������������Add����Edit��Ҫ�����ж�!!!!!!!!!!
    _this.cancel = function()
    {
        var __this = _this;
        colorAdd_cancel(__this);
    }
    
    _this.beforeCancel = function () {
        var __this = _this;
        colorAdd_beforeCancel(__this);
    }
    _this.confirm = function () {
        var __this = _this;
        colorAdd_confirm(__this);
    }
    _this.beforeConfirm = function () {
        var __this = _this;
        colorAdd_beforeConfirm(__this);
    }

    //bind button click event
    var _div_footer = _this.querySelector(".orui_popup_footer");
    _div_footer.querySelectorAll("button")[0].onclick = _this.cancel;
    _div_footer.querySelectorAll("button")[1].onclick = _this.confirm;

    //endregion
}

