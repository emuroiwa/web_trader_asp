<%--<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Main.Master" CodeBehind="delete.aspx.vb" Inherits="WEBTDS.delete" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <form id="form1" runat="server">
<div>
    <asp:GridView DataKeyNames="Id" ID="grdResults" runat="server" AutoGenerateColumns="false">
        <Columns>
            <asp:BoundField DataField="item0" HeaderText="item0" />
            <asp:BoundField DataField="item1" HeaderText="item1" />
            <asp:TemplateField HeaderText="Action">
                <ItemTemplate>
                    <asp:LinkButton ID="lbDelete" runat="server" OnClick="lbDelete_Click" OnClientClick="return fnConfirm();"> Delete</asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
</div>
</form>
<script type="text/javascript">
    function fnConfirm() {
        if (confirm("The item will be deleted. Are you sure want to continue?") == true)
            return true;
        else
            return false;
    }
</script>
</asp:Content>--%>
