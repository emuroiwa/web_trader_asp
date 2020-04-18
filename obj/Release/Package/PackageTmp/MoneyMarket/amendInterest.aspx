
<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Main.Master" CodeBehind="amendInterest.aspx.vb" Inherits="WEBTDS.amendInterest" %>
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
                  <h3 class="box-title">Amend Interest</h3>  
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
                                          <td style="width: 175px">
                                              <asp:Label ID="Label32" runat="server" Text="Deal Reference"></asp:Label>
                                          </td>
                                          <td style="width: 298px">
                                              <asp:TextBox ID="txtdealref" runat="server" Font-Size="Small" required="required" title="Please Enter Interest Rate" Width="75%" Enabled="False"></asp:TextBox>
                                          </td>
                                          <td style="align-content:center ;padding-bottom:10px">
                                                    <asp:Button ID="btnValidate" runat="server" CssClass="btn btn-primary btn-block btn-flats" Height="40px" Text="Validate" Width="150px" />
                                                </td>
                                      </tr>
                                      <tr>
                                          <td style="width: 175px">
                                              <asp:Label ID="Label34" runat="server" Text="New Interest Rate"></asp:Label>
                                          </td>
                                          <td style="width: 298px">
                                              <asp:TextBox ID="txtNewInt" runat="server" Font-Size="Small" required="required" title="Please Enter Interest Rate" Width="75%"></asp:TextBox>
                                          </td>
                                          <td style="align-content:center ;padding-bottom:10px">
                                                    <asp:Button ID="btnReset" runat="server" CssClass="btn btn-primary btn-block btn-flats" Height="40px" Text="Reset" Width="150px" Enabled="False" />
                                                </td>
                                      </tr>
                                      <tr>
                                          <td style="width: 175px">
                                              <asp:Label ID="Label33" runat="server" Text="Old Interest Rate"></asp:Label>
                                          </td>
                                          <td style="width: 298px">
                                              <asp:TextBox ID="txtOldInt" runat="server" Enabled="False" Font-Size="Small" required="required" title="Please Enter Interest Rate" Width="75%"></asp:TextBox>
                                          </td>
                                          <td style="align-content:center ;padding-bottom:10px">
                                                    <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary btn-block btn-flats" Height="40px" Text="Save" Width="150px" Enabled="False" />
                                                </td>
                                      </tr>
                                       <tr>
                                          <td style="width: 175px">
                                              <asp:Label ID="Label35" runat="server" Text="Reason"></asp:Label>
                                           </td>
                                          <td style="width: 298px">
                                              <asp:TextBox ID="txtreason" runat="server" Font-Size="Small" required="required" title="Please Enter Interest Rate" Width="75%" TextMode="MultiLine" Height="45px"></asp:TextBox>
                                           </td>
                                          <td style="align-content:center ;padding-bottom:10px">
                                                 <a href="MMDealBlotter.aspx"><asp:Button ID="btnExit" runat="server" CssClass="btn btn-primary btn-block btn-flats" Height="40px" Text="Exit" Width="150px" /></a>
                                            
                                                </td>
                                      </tr>
                                      <tr>
                                          <td style="width: 175px">&nbsp;</td>
                                          <td style="width: 298px">&nbsp;</td>
                                          <td>&nbsp;</td>
                                      </tr>
                                  </table>
                                   <hr>
                             <ContentTemplate>
                                <fieldset title="Interest Details">
                                    <div class="panel box box-success">
                                        <div class="box-header with-border">
                                            <h4 class="box-title"><a data-parent="#accordion" data-toggle="collapse" href="#interest">Interest Details </a></h4>
                                        </div>
                                        <div id="interest" class="panel-collapse collapse">
                                            <div class="box-body">
                                                <table style="width: 90%; height: 178px; table-layout: auto;">
                        
                        </tr>
                                                    <tr>
                                                        <td style="width: 291px; padding-bottom: 20px;" ;="">
                                                            <asp:Label ID="Label38" runat="server" Font-Size="Small" Text="Deal Amount"></asp:Label>
                                                        </td>
                                                        <td style="width: 492px; ">
                                                            <asp:TextBox ID="txtDealAmt" runat="server" Enabled="False" Font-Size="Small" required="required" style="margin-left: 0px" title="Please Enter Present value" Width="75%"></asp:TextBox>
                                                        </td>
                                                        <td style="width: 168px; ">
                                                            <asp:Label ID="Label39" runat="server" Font-Size="Small" Text="Maturity Amount"></asp:Label>
                                                        </td>
                                                        <td style="width: 369px;">
                                                            <asp:TextBox ID="txtMaturity" runat="server" Enabled="False" Font-Size="Small" Width="75%"></asp:TextBox>
                                                        </td>
                                                        <tr>
                                                            <td style="height: 28px; width: 291px; padding-bottom:20px">
                                                                <asp:Label ID="Label24" runat="server" Font-Size="Small" Text="Interest Rate"></asp:Label>
                                                            </td>
                                                            <td style="width: 492px; height: 28px">
                                                                <asp:TextBox ID="txtIntRate" runat="server" Enabled="False" Font-Size="Small" required="required" title="Please Enter Interest Rate" Width="75%"></asp:TextBox>
                                                            </td>
                                                            <td style="height: 28px; width: 168px;">
                                                                <asp:Label ID="Label28" runat="server" Font-Size="Small" Text="Net Interest"></asp:Label>
                                                            </td>
                                                            <td style="height: 28px; width: 369px;">
                                                                <asp:TextBox ID="txtNetInt" runat="server" Enabled="False" Font-Size="Small" Width="75%"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 291px; padding-bottom:20px">
                                                                <asp:Label ID="Label29" runat="server" Font-Size="Small" Text="Tax Amount"></asp:Label>
                                                            </td>
                                                            <td style="width: 492px">
                                                                <asp:TextBox ID="txtTax" runat="server" AutoPostBack="True" Enabled="False" Font-Size="Small" min="1" type="number" Width="75%"></asp:TextBox>
                                                            </td>
                                                            <td style="width: 168px">
                                                                <asp:Label ID="Label30" runat="server" Font-Size="Small" Text="Gross Interest"></asp:Label>
                                                            </td>
                                                            <td style="width: 369px">
                                                                <asp:TextBox ID="txtGrossInt" runat="server" Enabled="False" Font-Size="Small" Width="75%"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 291px; ">
                                                                <asp:Label ID="Label31" runat="server" Font-Size="Small" Text="Acrued To Date"></asp:Label>
                                                            </td>
                                                            <td style="width: 492px">
                                                                <asp:TextBox ID="txtAccrued" runat="server" Enabled="False" Font-Size="Small" min="1" required="required" title="Please Enter Deal Tenor" type="number" Visible="False" Width="75%"></asp:TextBox>
                                                            </td>
                                                            <td style="width: 168px">&nbsp;</td>
                                                            <td style="width: 369px">&nbsp;</td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 291px ; padding-bottom:20px">&nbsp;</td>
                                                            <td style="width: 492px">&nbsp;</td>
                                                            <td style="width: 168px">&nbsp;</td>
                                                            <td style="width: 369px">&nbsp;</td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="4">&nbsp;</td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="4">
                                                                <hr />
                                                            </td>
                                                        </tr>
                                                        </caption>
                                                        <tr>
                                                            <td></td>
                                                            <td>
                                                                &nbsp;</td>
                                                        </tr>
                                                    </tr>
                        
                    </table>
                                            </div>
                                        </div>
                                    </div>
                               </fieldset>
                            </ContentTemplate>
                            </hr>
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


