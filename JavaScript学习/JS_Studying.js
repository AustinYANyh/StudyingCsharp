<%@ page import="com.oraps.servlet.OperationHelpers" %>
<%@ page contentType="text/html;charset=UTF-8" language="java" %>
<html>
<head>
    <title>JS</title>
</head><body>
    <script>
        function myFunction()
        {
            alert("嘿嘿");
        }
    </script>

    <h1>显示不出主界面</h1>
    <button type="button" onclick="myFunction()">点这里！</button>

    <button type="add" onclick="cAdd.jsp">+</button>
    <button type="editor" onclick="cEditor.jsp">x</button>

    <div>问题出在哪里？</div>

</body>
</html>