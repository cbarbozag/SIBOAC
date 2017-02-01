<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewerPage.aspx.cs" Inherits="Cosevi.SIBOAC.Reports.ViewerPage" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>    
</head>
<body style="width:auto">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <asp:Button ID="btnPrint" runat="server" OnClick="btnPrint_Click" Text="Imprimir" Visible="false" />
&nbsp;<rsweb:ReportViewer ID="ReportViewer1" Height="800px" Width="99%" runat="server" ZoomMode="FullPage"></rsweb:ReportViewer>

        <iframe id="frmPrint" name="IframeName" width="500" 
  height="200" runat="server" 
  style="display: none" runat="server"></iframe>
    </form>
</body>
</html>
