
<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Main.Master" CodeBehind="SingleSell.aspx.vb" Inherits="WEBTDS.SingleSell" %>

<%@ Register Assembly="EO.Web" Namespace="EO.Web" TagPrefix="eo" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   
 
    <%--  <div class="panel box box-success">
                      <div class="box-header with-border">
                        <h4 class="box-title">
                          <a data-toggle="collapse" data-parent="#accordion" href="#history">
                            Client History
                          </a>
                        </h4>
                      </div>
                      <div id="history" class="panel-collapse collapse-in">
                        <div class="box-body">
                        <button type="button" class="btn btn-success">Avg Rate <asp:Label ID="avgRate" runat="server" Text="Label"></asp:Label></button>     
                            <button type="button" class="btn btn-success">Avg Size <asp:Label ID="avgSize" runat="server"></asp:Label></button>      
                            <button type="button" class="btn btn-success">Avg Tenor <asp:Label ID="avgTenor" runat="server"></asp:Label></button>
                             <hr />
                                 
                                     
  
                                     
                        </div>
                      </div>
                    </div> --%>
 <head />
  <form id="form1" runat="server">
       <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
     <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
 <asp:Label ID="lblError" runat="server"></asp:Label>
        <asp:Label ID="lblsuccess" runat="server"></asp:Label>
                  <asp:Label ID="lblwarning" runat="server"> </asp:Label>
      <%-- <!-- Horizontal Form -->
              <div class="box box-info">
                <div class="box-header with-border">
                  <h3 class="box-title">Securities Master </h3>  
   
                </div><!-- /.box-header -->
                <!-- form start -->
             
                  <div class="box-body">--%>

                <div class="box box-success">
           <div class="box-header with-border">
                  <h3 class="box-title">Single Sell </h3>
                  <div class="box-tools pull-right"> 
                    <%--<button class="btn btn-box-tool" data-widget="remove"><i class="fa fa-times"></i></button>--%>
                      <%--<button class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i></button>--%>
                  </div><!-- /.box-tools -->
                </div><!-- /.box-header -->
                <div class="box-body">

                
                     </div></div> 
                              <ContentTemplate>
                               <center>  
                                   

                                   <table style="width:80%;">
                                     <tr>
                                           <td style="width: 13%">
                                               <asp:Label ID="lbltb" runat="server" Font-Bold="True" Font-Size="Medium" Text="Purchases"></asp:Label>
                                           </td>
                                           <td style="width: 10%">
                                               <asp:Label ID="Label16" runat="server" Text="Currency"></asp:Label>
                                           </td>
                                           <td style="width: 10%">
                                               <asp:TextBox ID="lblCurrency" runat="server" Enabled="False" Font-Size="Small" required="required" style="margin-left: 0px" title="Please Enter Present value" Width="75%"></asp:TextBox>
                                           </td>
                                           <td style="width: 10%">
                                               <asp:Label ID="Label19" runat="server" Text="Days To Maturity"></asp:Label>
                                           </td>
                                           <td style="width: 8%">
                                               <asp:TextBox ID="txtDaysMaturity" runat="server" Enabled="False" Font-Size="Small" required="required" style="margin-left: 0px" title="Please Enter Present value" Width="75%"></asp:TextBox>
                                           </td>
                                           <td style="width: 10%">
                                               <asp:Label ID="lblfv" runat="server" Visible="False"></asp:Label>
                                           </td>
                                          
                                       </tr>
                                           <tr>
                                               <td style="width: 8%; ">
                                                   <asp:DropDownList ID="cmbTB" runat="server" AutoPostBack="True" class="form-control select2" Font-Size="Small" style="margin-left: 0px" Width="98%">
                                                       <asp:ListItem Text="Select TB" Value="0"></asp:ListItem>
                                                   </asp:DropDownList>
                                                   <br />
                                               </td>
                                               <td class="modal-sm" style="width: 15%; ">
                                                   <asp:Label ID="lblDesc" runat="server" Text="Maturity Value"></asp:Label>
                                               </td>
                                               <td style="width: 3%; ">
                                                   <asp:TextBox ID="txtPurValue" runat="server" Enabled="False" Font-Size="Small" required="required" style="margin-left: 0px" title="Please Enter Present value" Width="75%"></asp:TextBox>
                                                   <br />
                                               </td>
                                               <td style="width: 15%; ">
                                                   <asp:Label ID="Label20" runat="server" Text="Interest days basis"></asp:Label>
                                               </td>
                                               <td style="width: 5%; ">
                                                   <asp:TextBox ID="lblBasis" runat="server" Enabled="False" Font-Size="Small" required="required" style="margin-left: 0px" title="Please Enter Present value" Width="75%"></asp:TextBox>
                                               </td>
                                               <td style="width: 10%; ">
                                                   
                                                   <asp:Label ID="lblpv" runat="server" Visible="False"></asp:Label>
                                                   
                                                   <br />
                                                   
                                                   </td>
                                               
                                           </tr>
                                           <tr>
                                               <td style="width: 5%">
                                                   <asp:TextBox ID="selloptionValue" runat="server" Visible="False"></asp:TextBox>
                                               </td>
                                               <td class="modal-sm" style="width: 15%">
                                                   <asp:Label ID="rate" runat="server" Text="Discount Rate"></asp:Label>
                                               </td>
                                               <td style="width: 3%">
                                                   <asp:TextBox ID="txtIntRate" runat="server" Enabled="False" Font-Size="Small" required="required" style="margin-left: 0px" title="Please Enter Present value" Width="75%"></asp:TextBox>
                                               </td>
                                               <td style="width: 10%">
                                                   <asp:Label ID="Label21" runat="server" Text="Tenor"></asp:Label>
                                               </td>
                                               <td style="width: 8%">
                                                   <asp:TextBox ID="lblTenor" runat="server" Enabled="False" Font-Size="Small" required="required" style="margin-left: 0px" title="Please Enter Present value" Width="75%"></asp:TextBox>
                                               </td>
                                               <td style="width: 10%">
                                                   <asp:Label ID="txtmaturity" runat="server" Visible="False"></asp:Label>
                                               </td>
                                              
                                           </tr>
                                           
                                    
                                   </table>
                                    </center>
                                  <hr />
                                   
                <fieldset title="Security">
                       <div class="panel box box-success">
                      <div class="box-header with-border">
                        <h4 class="box-title">
                          <a data-toggle="collapse" data-parent="#accordion" href="#history1">
                       Action Sale
                          </a>
                        </h4>
                      </div>
                      <div id="history1" class="panel-collapse collapse-in">
                        <div class="box-body">
                           <center>
                            <table style="width:75%;">
                                <tr>
                                    <td style="width: 286px">
                                        <asp:Label ID="lblCostDesc2" runat="server"></asp:Label>
                                        <br />
                                    </td>
                                    <td style="width: 459px">
                                        <asp:Label ID="txtAvalableForSale" runat="server" Font-Bold="True" Font-Size="Medium"></asp:Label>
                                        <br />
                                    </td>
                                    <td>
                                        <asp:Label ID="lblPurchaseStart" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 286px">
                                        <asp:Label ID="avalAtPV" runat="server" Text="Available At PV"></asp:Label>
                                        <br />
                                    </td>
                                    <td style="width: 459px">
                                        <asp:Label ID="lblAvalableForSalePV" runat="server" Font-Bold="True" Font-Size="Medium"></asp:Label>
                                        <br />
                                    </td>
                                    <td>
                                        <asp:Label ID="lblDealAmount" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 286px">
                                        <asp:Label ID="Label23" runat="server" Text="Break Even Rate"></asp:Label>
                                        <br />
                                    </td>
                                    <td style="width: 459px">
                                        <asp:Label ID="lblbreakeven" runat="server" Font-Bold="True" Font-Size="Medium"></asp:Label>
                                        <br />
                                    </td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td style="width: 286px">
                                        <asp:Label ID="Label24" runat="server" Text="Sell Option"></asp:Label>
                                        <br />
                                    </td>
                                    <td style="width: 459px">
                                        <asp:Label ID="lblSellingOPT" runat="server" Font-Bold="True" Font-Size="Small">Present Value</asp:Label>
                                    </td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td style="width: 286px">
                                        <asp:Label ID="Label25" runat="server" Text="Sell Amount" Font-Bold="True"></asp:Label>
                                    </td>
                                    <td style="width: 459px">
                                        <asp:TextBox ID="txtSale" runat="server" Font-Size="Small" required="required" style="margin-left: 0px" title="Please Enter Present value" Width="75%"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnValidate" runat="server" CssClass="btn btn-primary btn-block btn-flats" Height="40px" Text="Compute" Width="150px" />
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 286px">
                                        <asp:Label ID="Label26" runat="server" Text="Tenor" Font-Bold="True"></asp:Label>
                                    </td>
                                    <td style="width: 459px">
                                        <asp:TextBox ID="txtSellTenor" runat="server" Font-Size="Small" required="required" style="margin-left: 0px" title="Please Enter Present value" Width="75%"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnSale" runat="server" CssClass="btn btn-success btn-block btn-flats" Height="40px" Text="OK" Width="150px" Enabled="False" />
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 286px">
                                        <asp:Label ID="Label27" runat="server" Text="Sell Rate" Font-Bold="True"></asp:Label>
                                    </td>
                                    <td style="width: 459px">
                                        <asp:TextBox ID="txtSaleRate" runat="server" Font-Size="Small" required="required" style="margin-left: 0px" title="Please Enter Present value" Width="75%"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnReset" runat="server" CssClass="btn btn-primary btn-block btn-flats" Height="40px" Text="Reset" Width="150px" />
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 286px">
                                        <asp:Label ID="Label28" runat="server" Text="Cost" Font-Bold="True"></asp:Label>
                                        <br />
                                    </td>
                                    <td style="width: 459px">
                                        <asp:Label ID="lblCost" runat="server" Font-Bold="True"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Button ID="cmdExit" runat="server" CssClass="btn btn-danger btn-block btn-flats" Height="40px" Text="Exit" Width="150px" />
                                    </td>
                                    <tr>
                                    <td style="width: 286px">
                                        <asp:Label ID="lblprofit" runat="server" Font-Bold="True"></asp:Label>
                                        <br />
                                        </td>
                                    <td style="width: 459px">
                                        <asp:Label ID="txtProfit" runat="server" Font-Bold="True" Font-Size="Medium"></asp:Label>
                                        <br />
                                        <br />
                                        </td>
                                    <td>&nbsp;</td>
                                        <tr>
                                    <td style="width: 286px">
                                        <asp:Label ID="Label30" runat="server" Text="AHPY" Font-Bold="True"></asp:Label>
                                            <br />
                                            </td>
                                    <td style="width: 459px">
                                        <asp:Label ID="AnnualisedHPY" runat="server" Font-Bold="True"></asp:Label>
                                            </td>
                                    <td>
                                        <asp:Label ID="txtSellPV" runat="server" Visible="False"></asp:Label>
                                            </td>
                                </tr>
                                </tr>
                                </tr>
                            </table></center>
                        </div>
                      </div>
                    </div>
                      <%--  <div class="panel box box-success">
                      <div class="box-header with-border">
                        <h4 class="box-title">
                          <a data-toggle="collapse" data-parent="#accordion" href="#history">
                            Client History
                          </a>
                        </h4>
                      </div>
                      <div id="history" class="panel-collapse collapse-in">
                        <div class="box-body">
                        <button type="button" class="btn btn-success">Avg Rate <asp:Label ID="avgRate" runat="server" Text="Label"></asp:Label></button>     
                            <button type="button" class="btn btn-success">Avg Size <asp:Label ID="avgSize" runat="server"></asp:Label></button>      
                            <button type="button" class="btn btn-success">Avg Tenor <asp:Label ID="avgTenor" runat="server"></asp:Label></button>
                             <hr />
                                 
                                     
  
                                     
                        </div>
                      </div>
                    </div> --%>

                               <%--</center>--%>
                                   </ContentTemplate>

                  </ContentTemplate>
    </asp:UpdatePanel>
  </form>
    </head>
              
   
    
         
        

  

   
     
   </asp:Content>

