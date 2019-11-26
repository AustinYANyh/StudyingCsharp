<%@ page import="com.oraps.servlet.OperationHelpers" %>
<%@ page contentType="text/html;charset=UTF-8" language="java" %>
<%
    String site = request.getParameter("site");
    String ui = "studentAdd";

    String baseInfos = "[]";//OperationHelpers.getEntities("studentTest3",ui);
    String baseQueryPlan = "[]";
%>
<div id = "<%=ui%>" class="studentAdd orui_popup_model" infos='<%=baseInfos%>' queryPlan='<%=baseQueryPlan%>'>
    <div id="<%=ui%>_hidden_div">
    </div>
    <div class="orui_popup_title">学生增加</div>

    <%--XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXPlace you html code here e.g↓--%>
    <div class="orui_popup_center">
        <div class="orui_frame_top" style="height:100%;">
            
            <div class="orui_frame_top_content" style="height:100%">
                <div class="orui_frame_top_content_left _pg_studentAdd_eb_studentTest3" style="width: 100%; height: 100%; ">
                    <div _entityBias="eb_studentTest3"
                         _width="100%"
                         _height="100%"
                         _tabName=""
                         _blockName=""
                         _index=""
                         _controlType="orui_propertygrid"
                         _infos='<%=OperationHelpers.getEntitiesByEntityBias("studentTest3", "studentAdd", "eb_studentTest3")%>'
                    ></div>
                </div>
                
                
            </div>
        </div>
        <div class="orui_frame_bottom" style="height:50%;">
    
    <div class="orui_frame_bottom_content"  style="height:100%">
        @#leftBottom#@
        @#centerBottom#@
        @#rightBottom#@
    </div>
</div>
    </div>
    <div class="orui_popup_footer">
        <button class="orui_button" id="<%=(ui + "_cancel")%>">取消</button>
        <span id="<%=(ui + "_errMsg")%>"></span>
        <button class="orui_button" id="<%=(ui + "_ok")%>">确定</button>
    </div>
</div>

