<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Main.Master" CodeBehind="logout.aspx.vb" Inherits="WEBTDS.logout" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 <script type="text/javascript">
     function RefreshParent() {
         if (window.opener != null && !window.opener.closed) {
             window.opener.location.href = "login.aspx";
             //window.opener.location.reload();
         }
     }
     window.onbeforeunload = RefreshParent;
     window.close()
    </script>
</asp:Content>
