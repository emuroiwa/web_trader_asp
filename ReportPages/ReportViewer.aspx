<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Main.Master" CodeBehind="ReportViewer.aspx.vb" Inherits="WEBTDS.ReportViewer" %>
<%@ Register TagPrefix="CR" Namespace="CrystalDecisions.Web" Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <script language="javaScript" type="text/javascript" src="/crystalreportviewers13/js/crviewer/crv.js"></script> 
       
      <form id="form1" runat="server"> 
  
      <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
           <div style="overflow:scroll">
 <table style="width:200%"><tr><td style="width:200%">

    
     <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server"  AutoDataBind="true" HasRefreshButton="True"  EnableParameterPrompt="true"/>
     </td></tr></table>  </div>
</form>
</asp:Content>
