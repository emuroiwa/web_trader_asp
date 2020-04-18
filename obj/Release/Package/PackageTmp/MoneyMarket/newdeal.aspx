<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Main.Master" CodeBehind="newdeal.aspx.vb" Inherits="WEBTDS.newdeal" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  
    <script>
        //var hook = true;
        //window.onbeforeunload = function () {
        //    if (hook) {
        //        return "Are you sure you have saved all your Data In WEBTDS"
        //    }
        //}



        function mmRedirect(page) {
            var dealcode = document.getElementById("<%=lblDealCode.ClientID%>").innerHTML
            var portifolio = document.getElementById("<%=lblPortifolio.ClientID%>").innerHTML
            var portifolioid = document.getElementById("<%=lblPortifolioID.ClientID%>").innerHTML
            var currency = document.getElementById("<%=lblCurrency.ClientID%>").innerHTML
            var dash = document.getElementById("<%=lblDash.ClientID%>").innerHTML
            var product = document.getElementById("<%=lblProduct.ClientID%>").innerHTML
            window.location = page+".aspx?dealcode=" + dealcode + "&dash=" + dash + "&portifolio=" + portifolio +"&portifolioid=" + portifolioid + "&currency=" + currency + "&product=" + product
        }
         
    </script>
  <script type="text/javascript">
      $(window).unload(function () {
          var html = "<img src='dist/img/loading.gif' />";
          $('#loading').append(html);
      });
</script>

<div id="loading" />
  <form id="form1" runat="server">
       <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
       <!-- Horizontal Form -->
         <asp:UpdatePanel ID="UpdatePanelAll" UpdateMode="Always" runat="server"><ContentTemplate>
              <div class="box box-info">
                <div class="box-header with-border">
                  <h3 class="box-title">New Deal</h3> 
                     <asp:Label ID="lblPortifolioID" runat="server" Text="" ForeColor="White"></asp:Label>
    <asp:Label ID="lblPortifolio" runat="server" ForeColor="White" Text=""></asp:Label>
    <asp:Label ID="lblDash" runat="server" Text="" ForeColor="White"></asp:Label>
    <asp:Label ID="lblProduct" runat="server" Text="" ForeColor="White"></asp:Label>
        <asp:Label ID="lblCurrency" runat="server" Text="" ForeColor="White"></asp:Label>
                </div><!-- /.box-header -->
                <!-- form start -->
             
                  <div class="box-body">
                      <center>
                  <table Class="table table-striped " style="width:75%;""><tr><td><asp:CheckBox ID="chkDash" runat="server" /> DashBoard      |  <asp:CheckBox ID="chkMatured" runat="server" /> Matured  </td></tr></table>
                    <Table ID="Table1" runat="server" style="width:75%; Height:50%" Class="table table-striped" HorizontalAlign="Center" >
            <tr>
                <td style="width:20%">
                   <asp:Label ID="lbltext1" runat="server" Text="Choose Currency" BorderColor="#66FFFF" BackColor="#FFFFCC"></asp:Label> <br />
                      <asp:DropDownList ID="cmbCurrency" runat="server" AutoPostBack="False" class="form-control select2" style="width: 100%;"></asp:DropDownList>
                   <asp:Label ID="lbltext3" runat="server" Text="Choose Deal Type" BorderColor="#66FFFF" BackColor="#FFFFCC"></asp:Label> <br />
                 
                
               
                 
                
                    <asp:RadioButtonList ID="RadioButtonList1" runat="server" AutoPostBack="True" >

<asp:ListItem Value="rdDeposit"> Deposit</asp:ListItem>
<asp:ListItem Value="rdPlacement"> Placement</asp:ListItem>
<asp:ListItem Value="rdSecurity"> Security Purchase</asp:ListItem>

</asp:RadioButtonList>
                         <asp:RequiredFieldValidator ID="RequiredFieldValidatorDealType" runat="server" ControlToValidate="RadioButtonList1" ErrorMessage="Error !! Select Deal Type" ForeColor="Red"></asp:RequiredFieldValidator>
                </td>
                <td  style="width:40%">
                    <asp:UpdatePanel ID="UpdatePanelPortifolio" UpdateMode="Conditional" runat="server"><ContentTemplate>
                        <asp:Label ID="lbltext2" runat="server" BorderColor="#66FFFF" Text="Choose Portifolio" BackColor="#FFFFCC"></asp:Label>
                     <br />   <asp:ListBox ID="lstPortfolio" runat="server"  Height="100%" Width="100%" OnSelectedIndexChanged="lstPortfolio_SelectedIndexChanged" AutoPostBack="true">

                         </asp:ListBox>                                                                               
                    </ContentTemplate><Triggers><asp:AsyncPostBackTrigger ControlID="lstPortfolio" EventName="SelectedIndexChanged" /></Triggers></asp:UpdatePanel> <asp:RequiredFieldValidator ID="RequiredFieldValidatorPortifolio" runat="server" ControlToValidate="lstPortfolio" ErrorMessage="Error !! Select Product Portifolio" ForeColor="Red"></asp:RequiredFieldValidator> </td> 
                <td  style="width:40%"><asp:UpdatePanel ID="UpdatePanelProducts" UpdateMode="Conditional" runat="server"><ContentTemplate>
                       <asp:Label ID="Label1" runat="server" Text="Choose Product" BorderColor="#66FFFF" BackColor="#FFFFCC"></asp:Label> <br /><asp:ListBox ID="lstProducts" runat="server" Height="100%" Width="100%" OnSelectedIndexChanged="lstPortfolio_SelectedIndexChanged" AutoPostBack="true"></asp:ListBox><br />               <a data-target="#DealCodeDetail" data-toggle="modal"  >
                    <asp:Label ID="lblDealCode" runat="server" Text=""></asp:Label></a>  
                                                                                     
                     </ContentTemplate><Triggers><asp:AsyncPostBackTrigger ControlID="lstPortfolio" EventName="SelectedIndexChanged" /></Triggers></asp:UpdatePanel>  </td>
       
            </tr>
        </Table>    </center>       <!-- <button type="button" class="btn btn-block btn-success btn-xs" data-toggle="modal" data-target="#DealCodeDetail">
  Launch demo modal
</button>-->
                  </div><!-- /.box-body -->
                
                  <div class="box-footer" style="width:100%;" > 
                     <asp:Button ID="btncancel" runat="server" Text="Cancel" CssClass="btn btn-default"  /><asp:Button ID="btnnew" runat="server" Text="New   " CssClass="btn btn-info pull-right" />
                    <%--<button type="submit" class="btn btn-default">Cancel</button>
                    <button type="submit" class="btn btn-info pull-right">Sign in</button>--%>
                  </div><!-- /.box-footer -->
              
              </div><!-- /.box --> 

     
                <asp:Label ID="lblDealInfo" runat="server" Text=""></asp:Label>   
         </ContentTemplate></asp:UpdatePanel>

  </form>

</asp:Content>
