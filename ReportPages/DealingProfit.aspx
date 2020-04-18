<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Main.Master" CodeBehind="DealingProfit.aspx.vb" Inherits="WEBTDS.DealingProfit" %>
<%@ Register Assembly="EO.Web" Namespace="EO.Web" TagPrefix="eo" %>
     
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="ajaxToolkit" %>
     
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <head runat="server" />
    <form id="form1" runat="server">
         <asp:UpdatePanel ID="UpdatePanelAll" UpdateMode="Always" runat="server"><ContentTemplate>

    <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
     <%--  filter options table --%>

    <div class="box box-info">
                <div class="box-header with-border">
       Filter Options</h3> 
                     
                </div><!-- /.box-header -->
                <!-- form start -->
             <%-- filter options table contents --%>
       
          <div class="box-body">
              <center>
              <table style="width:80%"><tr><td style="width:33%">
                  <table style="width:100%"><tr><td>
                      <asp:Label ID="Label2" runat="server" Text="Start Date" BorderColor="#66FFFF" BackColor="#FFFFCC"></asp:Label> <br />
                                  <asp:TextBox ID="dtStart" runat="server" Width="80%"></asp:TextBox>
                                         <ajaxToolkit:CalendarExtender ID="dtStart_CalendarExtender" runat="server" BehaviorID="dtStart_CalendarExtender" TargetControlID="dtStart" />
                             </td></tr>
                      <tr><td>
                          <asp:Label ID="Label3" runat="server" Text="End Date" BorderColor="#66FFFF" BackColor="#FFFFCC"></asp:Label> <br />
                                      <asp:TextBox ID="dtEnd" runat="server"  Width="80%"></asp:TextBox>
                                      <ajaxToolkit:CalendarExtender ID="dtEnd_CalendarExtender" runat="server" BehaviorID="dtEnd_CalendarExtender" TargetControlID="dtEnd" />
                          </td></tr>
                      <tr><td>  <asp:Label ID="Label4" runat="server" Text="Currency" BorderColor="#66FFFF" BackColor="#FFFFCC"></asp:Label> <br />

                          <asp:DropDownList ID="cmbCurrency" runat="server" AutoPostBack="True" class="form-control select2" style="width: 80%;">

                                      </asp:DropDownList></td></tr>
                      <tr><td>  <asp:Label ID="Label1" runat="server" Text="Run To" BorderColor="#66FFFF" BackColor="#FFFFCC"></asp:Label> <br />

                          <asp:DropDownList ID="cmbPeriodEnd" runat="server" AutoPostBack="True" class="form-control select2" style="width: 80%;">
                              <asp:ListItem></asp:ListItem>
                                  <asp:ListItem>Current Business Date</asp:ListItem>
                              <asp:ListItem>End Of Selected Period</asp:ListItem>

                                      </asp:DropDownList></td></tr>
                  </table>

                         </td>
                  <td style="width:33%">
                      <table style="width:100%">
                          <tr><td style="padding-bottom:10px;align-items:center"><center><asp:Button ID="btnDailyDealingProfit" runat="server" Text="Daily Dealing Profit" CssClass="btn btn-primary btn-block btn-flats" Height="40px" Width="250px" /></center></td></tr>
                          <tr><td  style="padding-bottom:10px"><center><asp:Button ID="btnYeartoDateDealingProfit" runat="server" Text="Year to Date Dealing Profit" CssClass="btn btn-primary btn-block btn-flats" Height="40px" Width="250px" /></center></td></tr>
                          <tr><td  style="padding-bottom:10px"><center><asp:Button ID="btnPrintDailyDealingProfitExtract" runat="server" Text="Print Daily Dealing Profit Extract" CssClass="btn btn-primary btn-block btn-flats" Height="40px" Width="250px"  Enabled="false"/></center></td></tr>
                          <tr><td  style="padding-bottom:10px"><center><asp:Button ID="btnPrintYeartoDateDealingProfitExtract" runat="server" Text="Print Year to Date Dealing Profit Extract" CssClass="btn btn-primary btn-block btn-flats" Height="40px" Width="250px" Enabled="false"/></center></td></tr>

                      </table>

                        </td>
                  <td style="width:33%">
                      <table style="width:100%"><tr><td style="width:80%;padding-bottom:10px;background-color:lime">
                          <asp:Label ID="Label5" runat="server" Text="Number of Live Deals [Assets]"></asp:Label>
                          </td>
                          <td><asp:Button ID="LblLiveA" runat="server" Text="" CssClass="btn btn-warning btn-block btn-flats" Height="40px" Width="150px"  Enabled="false"/></td>
                             </tr>
<tr><td style="width:80%;padding-bottom:10px;background-color:lime">Number of Matured Deals [Assets] </td>
                          <td><asp:Button ID="lblMaturedA" runat="server" Text="" CssClass="btn btn-warning btn-block btn-flats" Height="40px" Width="150px"  Enabled="false"/></td>
                             </tr>
<tr><td style="width:80%;padding-bottom:10px;background-color:lime">Number of Live Deals [Liabilities]</td>
                          <td><asp:Button ID="lblLiveL" runat="server" Text="" CssClass="btn btn-warning btn-block btn-flats" Height="40px" Width="150px" Enabled="false" /></td>
                             </tr>
<tr><td style="width:80%;padding-bottom:10px;background-color:lime">Number of Matured Deals [Liabilities]</td>
                          <td><asp:Button ID="lblMaturedL" runat="server" Text="" CssClass="btn btn-warning btn-block btn-flats" Height="40px" Width="150px"  Enabled="false"/></td>
                             </tr>
<tr><td style="width:80%;padding-bottom:10px;background-color:lime">Number of Un-Classified Deals</td>
                          <td><asp:Button ID="lblUnclassified" runat="server" Text="" CssClass="btn btn-warning btn-block btn-flats" Height="40px" Width="150px" Enabled="false" /></td>
                             </tr>

                      </table>

                         </td>
                     </tr></table>
                  </center>
          </div></div>
        
    <div class="box box-info">
                <div class="box-header with-border">
                  <h3 class="box-title">
                      <asp:Label ID="grpDetails" runat="server" Text="Details"></asp:Label></h3> 
                     
                </div><!-- /.box-header -->
                <!-- form start -->
             <%-- filter options table contents --%>
       
          <div class="box-body">
               <eo:grid ID="GridDeals" runat="server" BorderColor="#C7D1DF" 
                                        BorderWidth="1px" ColumnHeaderAscImage="00050303" 
                                        ColumnHeaderDescImage="00050304" ColumnHeaderDividerImage="00050302" 
                                        FixedColumnCount="1" Font-Bold="False" Font-Italic="False" Font-Names="Verdana" 
                                        Font-Overline="False" Font-Size="9pt" Font-Strikeout="False" 
                                        Font-Underline="False" GridLineColor="199, 209, 223" GridLines="Both" 
                                        Height="187px" ItemHeight="19" Width="100%" >
                                        <FooterStyle CssText="padding-bottom:4px;padding-left:4px;padding-right:4px;padding-top:4px;" />
                                      
                                        <ColumnHeaderStyle CssText="background-image:url('00050301');padding-left:8px;padding-top:2px;font-weight: bold;color:white;" />
                                        <ItemStyles>
                                            <eo:GridItemStyleSet>
                                                <ItemStyle CssText="background-color: white" />
                                                <ItemHoverStyle CssText="background-image: url(00050206); background-repeat: repeat-x" />
                                                <SelectedStyle CssText="background-image: url(00050207); background-repeat: repeat-x" />
                                                <CellStyle CssText="padding-left:8px;padding-top:2px; color:#336699;white-space:nowrap;" />
                                            </eo:GridItemStyleSet>
                                        </ItemStyles>
                                        <Columns>
                                            <eo:RowNumberColumn Width="30">
                                            </eo:RowNumberColumn>
                                            <eo:CustomColumn DataField="fullname" HeaderText="Full Name" 
                                                MinWidth="20" ReadOnly="True" Width="200">
                                                
                                            </eo:CustomColumn>
                                            <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="dealreference" 
                                                HeaderText="Deal Reference" MinWidth="10" ReadOnly="True"  Width="200">
                                                
                                            </eo:CustomColumn>
                                            <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="accrual" 
                                                HeaderText="Daily Accural" MinWidth="25" ReadOnly="True" Width="200" >
                                               
                                            </eo:CustomColumn>
                                         
                                            <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="accrual" 
                                                HeaderText="Accrued To Date" MinWidth="20" ReadOnly="True" Width="150">
                                               
                                            </eo:CustomColumn>
                                  <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="dealamount" 
                                                HeaderText="Deal Amount" MinWidth="20" ReadOnly="True" Width="150">
                                               
                                            </eo:CustomColumn>
                                             <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="dealamount" 
                                                HeaderText="Deal Amount" MinWidth="20" ReadOnly="True" Width="150">
                                               
                                            </eo:CustomColumn>
                                             <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="interestrate" 
                                                HeaderText="Interest Rate %" MinWidth="20" ReadOnly="True" Width="150">
                                               
                                            </eo:CustomColumn>
                                             <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="discountrate" 
                                                HeaderText="Discount Rate %" MinWidth="20" ReadOnly="True" Width="150">
                                               
                                            </eo:CustomColumn>
                                             <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="maturityamount" 
                                                HeaderText="Maturity Amount" MinWidth="20" ReadOnly="True" Width="150">
                                               
                                            </eo:CustomColumn>
                                              <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="tenor" 
                                                HeaderText="tenor" MinWidth="20" ReadOnly="True" Width="150">
                                               
                                            </eo:CustomColumn>
                                              <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="daystomaturity" 
                                                HeaderText="Days To Run" MinWidth="20" ReadOnly="True" Width="150">
                                               
                                            </eo:CustomColumn>
                                           
                                        </Columns>
                                    </eo:grid>
              </div></div>
             </ContentTemplate></asp:UpdatePanel>
    </form>
</asp:Content>
