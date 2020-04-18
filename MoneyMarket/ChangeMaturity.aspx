
<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Main.Master" CodeBehind="ChangeMaturity.aspx.vb" Inherits="WEBTDS.ChangeMaturity" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="ajaxToolkit" %>
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
                  <h3 class="box-title">Change Maturity Date</h3>  
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
                                          <td style="width: 147px">
                                              <asp:Label ID="Label47" runat="server" Font-Size="Small" Text="Maturity Date"></asp:Label>
                                          </td>
                                          <td style="width: 398px">
                                              <asp:TextBox ID="dtMaturity" runat="server" Font-Size="Small" required="required" title="Please Enter Start Date" Width="75%" AutoPostBack="true"></asp:TextBox>
                                              
                                              <ajaxToolkit:CalendarExtender ID="dtMaturity_CalendarExtender" runat="server" BehaviorID="txtStartDateM_CalendarExtender" TargetControlID="dtMaturity" />
                                              
                                          </td>
                                          <td style="align-content:center ;padding-bottom:10px">
                                                    <asp:Button ID="btnValidate" runat="server" CssClass="btn btn-primary btn-block btn-flats" Height="40px" Text="Validate" Width="150px" />
                                                    <asp:Label ID="lblCaptureType" runat="server" Enabled="False" Visible="False"></asp:Label>
                                                </td>
                                      </tr>
                                      <tr>
                                          <td style="width: 147px"> 
                                              <asp:Label ID="Label42" runat="server" Font-Size="Small" Text="Value Date"></asp:Label>
                                          </td>
                                          <td style="width: 398px" >
                                              <asp:TextBox ID="InterestValueDate" runat="server" AutoPostBack="True" Font-Size="Small" required="required" title="Please Enter Maturity Date" Width="75%" Enabled="False"></asp:TextBox>
                                              
                                              <ajaxToolkit:CalendarExtender ID="InterestValueDate_CalendarExtender" runat="server" BehaviorID="txtMatDateM_CalendarExtender" TargetControlID="InterestValueDate" />
                                              
                                          </td>
                                         <td style="align-content:center ;padding-bottom:10px">
                                                    <asp:Button ID="btnReset" runat="server" CssClass="btn btn-primary btn-block btn-flats" Height="40px" Text="Reset" Width="150px" />
                                                    <asp:Label ID="txtid" runat="server" Enabled="False" Visible="False"></asp:Label>
                                                    <asp:Label ID="txtDiscountRate" runat="server" Enabled="False" Visible="False"></asp:Label>
                                                </td>
                                      </tr>
                                      <tr>
                                          <td style="width: 147px">
                                              <asp:Label ID="Label44" runat="server" Font-Size="Small" Text="Tenor"></asp:Label>
                                          </td>
                                          <td style="width: 398px">
                                              <asp:TextBox ID="txtTenor" runat="server"  Font-Size="Small" min="1" type="number" Width="75%" AutoPostBack="True" ></asp:TextBox>
                                          </td>
                                          <td style="align-content:center ;padding-bottom:10px">
                                                    <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary btn-block btn-flats" Height="40px" Text="Save" Width="150px" Enabled="False" />
                                                    <asp:Label ID="txtDaystomaturity" runat="server" Enabled="False" Visible="False"></asp:Label>
                                                    <asp:Label ID="txtDealtype" runat="server" Enabled="False" Visible="False"></asp:Label>
                                                </td>
                                      </tr>
                                       <tr>
                                          <td style="width: 147px">
                                              <asp:Label ID="Label45" runat="server" Text="Interest Rate"></asp:Label>
                                           </td>
                                          <td style="width: 398px">
                                              <asp:TextBox ID="txtRate" runat="server" Font-Size="Small" required="required" style="margin-left: 0px" title="Please Enter Present value" Width="75%"></asp:TextBox>
                                           </td>
                                          <td style="align-content:center ;padding-bottom:10px">
                                                    <a href="MMDealBlotter.aspx"><asp:Button ID="btnExit" runat="server" CssClass="btn btn-danger btn-block btn-flats" Height="40px" Text="Exit" Width="150px" /></a>
                                                    <asp:Label ID="txtTBID" runat="server" Enabled="False" Visible="False"></asp:Label>
                                                    <asp:Label ID="lblCurrency" runat="server" Enabled="False" Visible="False"></asp:Label>
                                                </td>
                                      </tr>
                                       <tr>
                                          <td style="width: 147px">
                                              <asp:Label ID="Label46" runat="server" Text="Deal Amount"></asp:Label>
                                              <br />
                                           </td>
                                          <td style="width: 398px">
                                              <asp:TextBox ID="txtPrincipalChange" runat="server" Font-Size="Small" required="required" style="margin-left: 0px" title="Please Enter Present value" Width="75%" Enabled="False"></asp:TextBox>
                                              <br />
                                              <br />
                                           </td>
                                          <td>
                                              <asp:Label ID="lblCustomerNumber" runat="server" Enabled="False" Visible="False"></asp:Label>
                                           </td>
                                      </tr>
                                       <tr>
                                          <td style="width: 147px">
                                              <asp:Label ID="Label48" runat="server" Text="Deal Reference"></asp:Label>
                                              <br />
                                           </td>
                                          <td style="width: 398px">
                                              <asp:TextBox ID="lblRef" runat="server" Enabled="False" Font-Size="Small" required="required" style="margin-left: 0px" title="Please Enter Present value" Width="75%"></asp:TextBox>
                                              <br />
                                              <br />
                                           </td>
                                          <td>&nbsp;</td>
                                      </tr>
                                      <tr>
                                          <td>
                                              <asp:Label ID="Label50" runat="server" Text="Reason"></asp:Label>
                                          </td>
                                          <td>
                                              <asp:TextBox ID="txtReason" runat="server" Font-Size="Small" Height="48px" TextMode="MultiLine" Width="75%"></asp:TextBox>
                                              &nbsp;</td>
                                          <td></td>
                                      </tr>
                                  </table>
                                   <hr>
                             <ContentTemplate>
                                <fieldset title="Maturity Details">
                                    <div class="panel box box-success">
                                        <div class="box-header with-border">
                                            <h4 class="box-title"><a data-parent="#accordion" data-toggle="collapse" href="#maturity">Maturity Details </a></h4>
                                        </div>
                                        <div id="maturity" class="panel-collapse collapse">
                                            <div class="box-body">
                                               <table style="width: 90%; height: 178px; table-layout: auto;">
                        <tr>
                            <td style="width: 291px; padding-bottom:20px" >
                                <asp:Label ID="Label14" runat="server" Font-Size="Small" Text="Start Date"></asp:Label>
                            </td>
                            <td style="width: 492px">
                                <asp:TextBox ID="lblStart" runat="server" Enabled="False" Font-Size="Small" Width="75%"></asp:TextBox>
                            </td>

                        <td style="width: 168px; ">
                            <asp:Label ID="Label19" runat="server" Font-Size="Small" Text="Maturity Date" Width="75%"></asp:Label>
                        </td>
                        <td style="width: 369px; ">
                            <asp:TextBox ID="lblMaturityDate" runat="server" Enabled="False" Font-Size="Small" Width="75%"></asp:TextBox>
                        </td>
                        <tr>
                            <td style="width: 291px; padding-bottom:20px" ; >
                                <asp:Label ID="Label15" runat="server" Font-Size="Small" Text="Deal Amount"></asp:Label>
                            </td>
                            <td style="width: 492px">
                                <asp:TextBox ID="LblAmount" runat="server" required="required" title="Please Enter Present value" style="margin-left: 0px" Width="75%" Font-Size="Small" Enabled="False"></asp:TextBox>
                            </td>
                            <td style="width: 168px">
                                <asp:Label ID="Label20" runat="server" Font-Size="Small" Text="Maturity Amount"></asp:Label>
                            </td>
                            <td style="width: 369px">
                                <asp:TextBox ID="lblMaturityAmount" runat="server" Width="75%" Enabled="False" Font-Size="Small"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 28px; width: 291px; padding-bottom:20px">
                                <asp:Label ID="Label16" runat="server" Font-Size="Small" Text="Interest Rate"></asp:Label>
                            </td>
                            <td style="width: 492px; height: 28px">
                                <asp:TextBox ID="lblRate" Width="75%"  required="required" title="Please Enter Interest Rate" runat="server" Font-Size="Small" Enabled="False"></asp:TextBox>
                            </td>
                            <td style="height: 28px; width: 168px;">
                                <asp:Label ID="Label21" runat="server" Font-Size="Small" Text="Net Interest"></asp:Label>
                            </td>
                            <td style="height: 28px; width: 369px;">
                                <asp:TextBox ID="lblNet" runat="server" Width="75%" Enabled="False" Font-Size="Small"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 291px; padding-bottom:20px">
                                <asp:Label ID="Label17" runat="server" Font-Size="Small" Text="Tenor"></asp:Label>
                            </td>
                            <td style="width: 492px">
                                <asp:TextBox ID="lblTenor" runat="server"  type="number" min="1" Width="75%" Font-Size="Small" AutoPostBack="True" Enabled="False" ></asp:TextBox>
                            </td>
                            <td style="width: 168px">
                                <asp:Label ID="Label22" runat="server" Font-Size="Small" Text="Gross Interest"></asp:Label>
                            </td>
                            <td style="width: 369px">
                                <asp:TextBox ID="lblGross" runat="server" Enabled="False" Width="75%" Font-Size="Small"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 291px; ">
                                <asp:Label ID="Label25" runat="server" Font-Size="Small" Text="Tax Amount"></asp:Label>
                                <br />
                            </td>
                            <td style="width: 492px">
                                <asp:TextBox ID="lblTaxAmount" runat="server" Enabled="False" min="1" required="required" title="Please Enter Deal Tenor" type="number" Width="75%" Font-Size="Small"></asp:TextBox>
                            </td>
                            <td style="width: 168px">
                                <asp:Label ID="Label23" runat="server" Font-Size="Small" Text="Acrued To Date"></asp:Label>
                            </td>
                            <td style="width: 369px">
                                <asp:TextBox ID="txtAccrrued" runat="server" Enabled="False" Width="75%" Font-Size="Small"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 291px ; padding-bottom:20px">
                                <asp:Label ID="Label49" runat="server" Font-Size="Small" Text="Tax Rate"></asp:Label>
                            </td>
                            <td style="width: 492px">
                                <asp:TextBox ID="lblTaxRate" runat="server" Enabled="False" Font-Size="Small" min="1" required="required" title="Please Enter Deal Tenor" type="number" Width="75%"></asp:TextBox>
                            </td>
                            <td style="width: 168px">
                                &nbsp;</td>
                            <td style="width: 369px">
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td colspan="4"><hr />
                                </td>
                        </tr>
                    </caption>
                    </td></td>
                        
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

