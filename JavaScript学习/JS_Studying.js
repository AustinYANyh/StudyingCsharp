<%@ page import="com.oraps.servlet.OperationHelpers" %>
<%@ page contentType="text/html;charset=UTF-8" language="java" %>
<html>
<head>
    <title>JS</title>
</head><body>
    <script>
        function myFunction()
        {
            alert("�ٺ�");
        }
    </script>

    <h1>��ʾ����������</h1>
    <button type="button" onclick="myFunction()">�����</button>

    <button type="add" onclick="cAdd.jsp">+</button>
    <button type="editor" onclick="cEditor.jsp">x</button>

    <div>����������</div>

</body>
</html>