

<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Main.Master" CodeBehind="NewSell.aspx.vb" Inherits="WEBTDS.NewSell" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  
    <script>
        //var hook = true;
        //window.onbeforeunload = function () {
        //    if (hook) {
        //        return "Are you sure you have saved all your Data In WEBTDS"
        //    }
        //}



        function mmRedirect(page) {
            var portID = document.getElementById("<%=lblportid.ClientID%>").innerHTML
            var selloption = document.getElementById("<%=lblselloption.ClientID%>").innerHTML
            var currency = document.getElementById("<%=lblCurrency.ClientID%>").innerHTML
            var MatAmount = document.getElementById("<%=lblMatAmount.ClientID%>").innerHTML
            var Days = document.getElementById("<%=lbldays.ClientID%>").innerHTML
            window.location = page + ".aspx?selloption=" + selloption + "&currency=" + currency + "&MatAmount=" + MatAmount + "&Days=" + Days + "&portID=" + portID
        }

    </script>
  <head runat="server">
  <form id="form1" runat="server">
       <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
       <!-- Horizontal Form -->
         <asp:UpdatePanel ID="UpdatePanelAll" UpdateMode="Always" runat="server">
             <contenttemplate>
              <div class="box box-info">
                <div class="box-header with-border">
                  <h3 class="box-title">New Sell</h3> 
                     <asp:Label ID="lblselloption" runat="server" Text="" ForeColor="White"></asp:Label>
                    <asp:Label ID="lblportid" runat="server" Text="" ForeColor="White"></asp:Label>
    <asp:Label ID="lblMatAmount" runat="server" Text="" ForeColor="White"></asp:Label>
    <asp:Label ID="lbldays" runat="server" Text="" ForeColor="White"></asp:Label>
        <asp:Label ID="lblCurrency" runat="server" Text="" ForeColor="White"></asp:Label>
                </div><!-- /.box-header -->
                <!-- form start -->
             
                  <div class="box-body">
                      <center>
                  <table Class="table table-striped " style="width:75%;""><tr><td>&nbsp;</td></tr></table>
                    <Table ID="Table1" runat="server" style="width:75%; Height:20%" Class="table table-striped" HorizontalAlign="Center" >
            <tr>
                <td style="width:20%">
                   <asp:Label ID="lbltext1" runat="server" Text="Choose Currency" BorderColor="#66FFFF" BackColor="#FFFFCC"></asp:Label> <br />
                      <asp:DropDownList ID="cmbCurrency" runat="server" AutoPostBack="False" class="form-control select2" style="width: 100%;">
                          <asp:ListItem Text="Select Currency" Value="0"></asp:ListItem>
                      </asp:DropDownList>
                   <asp:Label ID="lbltext3" runat="server" Text="Choose Sell Type" BorderColor="#66FFFF" BackColor="#FFFFCC"></asp:Label> 
                    <br />
                    <br />
                 
                
                    <asp:RadioButtonList ID="rdSellType" runat="server" AutoPostBack="True">

<asp:ListItem Value="rdSingle"> Single Sell</asp:ListItem>
<asp:ListItem Value="rdMultiple">MultipleSell</asp:ListItem>

</asp:RadioButtonList>
                </td>
                <td  style="width:20%">
                    <center>
                        <asp:Label ID="lbltext2" runat="server" BorderColor="#66FFFF" Text="Choose Sell Option" BackColor="#FFFFCC"></asp:Label>
                        <br />
                        <br />
                     <br />  
                        <asp:RadioButtonList ID="rdOption" runat="server" AutoPostBack="True">
                            <asp:ListItem Value="rdDiscount"> Discount</asp:ListItem>
                            <asp:ListItem Value="rdYield">Yield</asp:ListItem>
                        </asp:RadioButtonList></center>
                    </td> 
                <td  style="width:20%">
                       <br />
                       <br />
                       <table style="width:100%;">
                           <tr>
                               <td style="width: 20%">
                                   <asp:Label ID="Label1" runat="server" Text="Amount Available"></asp:Label>
                               </td>
                               <td style="width: 20%">
                                   <asp:TextBox ID="txtamont" runat="server"></asp:TextBox>
                               </td>
                               <%--<td>&nbsp;</td>--%>
                           </tr>
                           <tr>
                               <td style="width: 20%">&nbsp;</td>
                               <td style="width: 20%"></td>
                           <%--    <td>&nbsp;</td>--%>
                           </tr>
                           <tr>
                               <td style="width: 20%">
                                   <asp:Label ID="Label2" runat="server" Text="Days To Maturity"></asp:Label>
                               </td>
                               <td style="width: 20%">
                                   <asp:TextBox ID="txtdays" runat="server"></asp:TextBox>
                               </td>
                              <%-- <td>&nbsp;</td>--%>
                           </tr>
                       </table>
                       <br />
                       <br /><br />                 
                                                                                     
                     </td>
                <td  style="width:20%">
                       <asp:Label ID="lblport" runat="server" BackColor="#FFFFCC" BorderColor="#66FFFF" Text="Choose Potfolio" Visible="False"></asp:Label>
                       <br />
                       <asp:DropDownList ID="cmbPort" runat="server" AutoPostBack="False" class="form-control select2" style="width: 100%;" Visible="False">
                           <asp:ListItem Text="Select Portfolio" Value="0"></asp:ListItem>
                       </asp:DropDownList>
                       <br />                 
                                                                                     
                       <br />
                                                                                     
                     </td>
       
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

     
         </contenttemplate></asp:UpdatePanel>

  </form>
    </head>
</asp:Content>
