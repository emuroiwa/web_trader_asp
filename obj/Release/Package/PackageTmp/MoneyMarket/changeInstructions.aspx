



<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Main.Master" CodeBehind="changeInstructions.aspx.vb" Inherits="WEBTDS.changeInstructions" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  
    <%--<script>
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
            window.location = page + ".aspx?dealcode=" + dealcode + "&dash=" + dash + "&portifolio=" + portifolio + "&portifolioid=" + portifolioid + "&currency=" + currency + "&product=" + product
        }

    </script>--%>
  <head />
  <form id="form1" runat="server">
       <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
      <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>

       <!-- Horizontal Form -->
              <div class="box box-info">
                <div class="box-header with-border">
                  <h3 class="box-title">Change Deal Instructions</h3>  
    <asp:Label ID="lblError" runat="server"></asp:Label>
        <asp:Label ID="lblsuccess" runat="server"></asp:Label>
                  <asp:Label ID="lblwarning" runat="server"> </asp:Label>
                </div><!-- /.box-header -->
                <!-- form start -->
             
                  <div class="box-body">
                    </HeaderTemplate>

                               <ContentTemplate>
                            <table style="width:100%;">
                                <tr>
                                    <td style="width:50%;">
                                        <table style="width:100%; height:100%">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label28" runat="server" Text="Deal Reference"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtDealref" runat="server" Enabled="False"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 103px; padding-bottom:10px">
                                                    <asp:Label ID="Label26" runat="server" Text="Deal Inception"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="cmbInstructionStart" runat="server" Width="80%">
                                                        <asp:ListItem Text="Select Deal Inception Instruction" Value="0"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 103px">&nbsp;</td>
                                                <td>
                                                    <asp:TextBox ID="txtOtherStart" runat="server" Height="150px" TextMode="MultiLine" Width="80%"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 103px;padding-top:35px">
                                                    <asp:Label ID="Label27" runat="server" Text="Deal Maturity"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="cmbInstructionMaturity" runat="server" Font-Size="Small" Width="80%">
                                                        <asp:ListItem Text="Select Deal Maturity Instruction" Value="0"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 103px">&nbsp;</td>
                                                <td>
                                                    <asp:TextBox ID="txtOtherMaturity" runat="server" Height="150px" TextMode="MultiLine" Width="80%"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td style="width:30%;">
                                        <table style="width:100%;">
                                            <tr>
                                                <td style="align-content:center ;padding-bottom:10px">
                                                    <asp:Button ID="btnUpdate" runat="server" CssClass="btn btn-primary btn-block btn-flats" Height="40px" Text="Update" Width="150px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="align-content:center;padding-bottom:10px">
                                                    <a href="MMDealBlotter.aspx"><asp:Button ID="btnExit" runat="server" CssClass="btn btn-danger btn-block btn-flats" Height="40px" Text="Exit" Width="150px" /></a>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="align-content:center ;padding-bottom:10px">
                                                    &nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td style="align-content:center ;padding-bottom:10px">
                                                    &nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td style="align-content:center;padding-bottom:10px">
                                                    &nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td style="align-content:center ;padding-bottom:10px">
                                                    &nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td style="align-content:center;padding-bottom:10px">&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td style="align-content:center;padding-bottom:10px">&nbsp;</td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>


                         <!-- <button type="button" class="btn btn-block btn-success btn-xs" data-toggle="modal" data-target="#DealCodeDetail">
  Launch demo modal
</button>-->
                  </div><!-- /.box-body -->
                 <%-- <div class="box-footer"> 
                     <asp:Button ID="btncancel" runat="server" Text="Cancel" CssClass="btn btn-default"  /><asp:Button ID="btnnew" runat="server" Text="New   " CssClass="btn btn-info pull-right" />
                    <%--<button type="submit" class="btn btn-default">Cancel</button>
                    <button type="submit" class="btn btn-info pull-right">Sign in</button>--%>
                  </div><!-- /.box-footer --> 
       
         
          </ContentTemplate>
    </asp:UpdatePanel>
  </form>
    </head>
</asp:Content>

