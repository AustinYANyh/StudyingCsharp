<%@ page import="com.oraps.servlet.OperationHelpers" %>
<%@ page contentType="text/html;charset=UTF-8" language="java" %>
<html>
<head>
    <meta name="viewport" content="width=device-width, user-scalable=no, initial-scale=1.0, maximum-scale=1.0, minimum-scale=1.0">
    <meta http-equiv="X-UA-Compatible" content="ie=edge">
    <meta charset="UTF-8">
    <title>颜色测试</title>
    <link rel="stylesheet" href="../../basicScripts/orui_style/orui_basic.css">
    <link rel="stylesheet" href="../../basicScripts/orui_style/orui.css">
    <link rel="stylesheet" href="../../basicScripts/orui_style/orui_combobox.css">
    <link rel="stylesheet" href="../../basicScripts/orui_style/orui_tabs.css">
    <link rel="stylesheet" href="../../basicScripts/orui_style/orui_datagrid.css" type="text/css"/>
    <link rel="stylesheet" href="../../basicScripts/orui_style/orui_propertygrid.css">
    <link rel="stylesheet" href="../../basicScripts/orui_style/orui_textbox.css">
    <link rel="stylesheet" href="../../basicScripts/orui_style/orui_pager.css">
    <link rel="stylesheet" href="../../basicScripts/orui_style/orui_popup_model.css">
    <link rel="stylesheet" href="../../basicScripts/orui_style/orui_complexSelectionGrid.css">
    <link rel="stylesheet" href="../../basicScripts/orui_style/orui_groupbox.css">
    <link rel="stylesheet" href="../../basicScripts/orui_style/orui_quickquery.css">
    <link rel="stylesheet" href="../../basicScripts/orui_style/orui_query.css">
    <link rel="stylesheet" href="../../basicScripts/orui_style/orui_datePicker.css">
    <link rel="stylesheet" href="../../basicScripts/orui_style/orui_messagebox.css">
    <script type="text/javascript" src="../../basicScripts/jquery.min.js"></script>
    <script type="text/javascript" src="../../basicScripts/pageDataHandler.js"></script>
    <script type="text/javascript" src="../../scripts/validator.js"></script>
    <script type="text/javascript" src="../../scripts/dateUtils.js"></script>
    <script type="text/javascript" src="../../scripts/OAS_Web_Control_Strs_CN.js"></script>
    <script type="text/javascript" src="../../scripts/stringUtils.js"></script>
    <script type="text/javascript" src="../../scripts/ajaxhelper.js"></script>
    <script type="text/javascript" src="../../basicScripts/sorter.js"></script>
    <script type="text/javascript" src="../../basicScripts/orui_datePicker.js"></script>
    <script type="text/javascript" src="../../basicScripts/orui_quickquery.js"></script>
    <script type="text/javascript" src="../../basicScripts/orui_combobox.js"></script>
    <script type="text/javascript" src="../../basicScripts/orui_tabs.js"></script>
    <script type="text/javascript" src="../../basicScripts/orui_textbox.js"></script>
    <script type="text/javascript" src="../../basicScripts/orui_button.js"></script>
    <script type="text/javascript" src="../../basicScripts/orui_datagrid.js"></script>
    <script type="text/javascript" src="../../basicScripts/orui_propertygrid.js"></script>
    <script type="text/javascript" src="../../basicScripts/orui_cardView.js"></script>
    <script type="text/javascript" src="../../basicScripts/orui_treegrid.js"></script>
    <script type="text/javascript" src="../../basicScripts/orui_pager.js"></script>
    <script type="text/javascript" src="../../basicScripts/orui_complexSelectionGrid.js"></script>
    <script type="text/javascript" src="../../basicScripts/orui_selector.js"></script>
    <script type="text/javascript" src="../../basicScripts/orui_query.js"></script>
    <script type="text/javascript" src="../../basicScripts/orui_msgbox.js"></script>
    <script type="text/javascript" src="../../basicScripts/orui.js"></script>
    <script type="text/javascript" src="../../basicScripts/template.js"></script>
    <script type="text/javascript" src="../../scripts/testcolor/colcoradd.js"></script>
    <script type="text/javascript" src="../../scripts/testcolor/colcoradd_logic.js"></script>
    <script type="text/javascript" src="../../scripts/testcolor/colcoreditor.js"></script>
    <script type="text/javascript" src="../../scripts/testcolor/colcoreditor_logic.js"></script>
    <script type="text/javascript" src="../../scripts/testcolor/index.js"></script>
    <script type="text/javascript" src="../../scripts/testcolor/index_logic.js"></script>
    
    <%
        String site = request.getParameter("site");
        //MUST DO
        //PLEASE FILL IN ui by UI NAME !Must Do
        String ui = "index";

        //Prepare var to load base entity infos in a format [{entityName: xxx, entityBias: bxxx, displayInfos:[{},{},...]}]
        String baseInfos = "[]";//OperationHelpers.getEntitiesByEntityBias("","","");
        //String baseInfos = OperationHelpers.getEntities("testcolor",ui);
        String baseQueryPlan = "[]";//OperationHelpers.getQueryPlan(ui);
    %>
</head><body>
<div class="orui_frame" id="<%=ui%>" infos='<%=baseInfos%>' queryPlan='<%=baseQueryPlan%>'>    <script type="text/javascript">
        window.onload = function (ev) {
            try {
                if (GUI == undefined){
                    GUI = new Object();
                }
            }catch (e) {
                GUI = new Object();
            }
            GUI.Doms = new Object();
            GUI.Pages = new Object();
            try {
                var parentDom = document.getElementById("<%=ui%>");
                <%=ui%>_init(parentDom);
            } catch (e) {
                console.log(e.message + "\r\n" + e.stack)
            }
        }
    </script>
    <div id="<%=ui%>_hidden_div"></div>
    <div class="orui_frame">
        <div class="orui_frame_top" style="height:100%;">
            
            <div class="orui_frame_top_content" style="height:100%">
                <div class="orui_frame_top_content_left _dg_index_eb_colcortest11" style="width: 100%; ">
                    <div _width="100%"
                         _height="100%"
                         _entityName="en_colcortest11"
                         _entityBias="eb_colcortest11"
                         _guiName="testcolor"
                         _filter=""
                         _coreFilter=""
                         _orderBy=""
                         _dataSourceURL="/com/oraps/servlet/serviceServlet"
                         _updateURL="/com/oraps/servlet/serviceServlet"
                         _pageSize="100"
                         _isShowPager="true"
                         _basicButtons='{"query":""}'
                         _customButtons='[]'
                         _readOnly="1"
                         _controlType="orui_datagrid"
                         _infos='<%=OperationHelpers.getEntitiesByEntityBias("testcolor", "index", "eb_colcortest11")%>'
                    ></div>
                </div>
                
                
            </div>
        </div>
        
    </div>
</div>

</body>
</html>
