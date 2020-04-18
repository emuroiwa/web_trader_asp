<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Main.Master" CodeBehind="report.aspx.vb" Inherits="WEBTDS.report" %>
<%@ Register TagPrefix="CR" Namespace="CrystalDecisions.Web" Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script language="javaScript" type="text/javascript" src="/crystalreportviewers13/js/crviewer/crv.js"></script> 

     <form id="form1" runat="server">
    
    <div>

        <table style="width:100%"><tr><td><center>
        <CR:CrystalReportViewer ID="CrystalReportViewer1" Runat="server" AutoDataBind="True"
            Height="100%" Width="100%" /></center>
       </td></tr></table>
    
    </div>
    </form>
</asp:Content>
