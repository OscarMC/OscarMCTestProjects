<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Callbacks.aspx.vb" Inherits="_11_ClientCallbacks_Callbacks" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Sample Web Method</title>
    
    <script language="javascript" type="text/javascript">

        function Button1_onclick() {
            
            var textbox1 = document.getElementById("Text1");
            SampleWebService.CalculateValue(textbox1.value, onCallBack);
        }

        function onCallBack(result) {
        
            var outputDiv = document.getElementById("resultDiv");
            outputDiv.innerHTML = result;
        }

    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <atlas:ScriptManager ID="s1" runat="server">
            <Services>
                <atlas:ServiceReference Path="SampleWebService.asmx" />
            </Services>
        </atlas:ScriptManager>
    
        Some Value: 
        
        <input id="Text1" type="text" />
        
        <input id="Button1" type="button" value="button" onclick="Button1_onclick()" />
    
        <div id="resultDiv" style="font-size:large;margin-top:20px"></div>
    
    </div>
    </form>
</body>
</html>
