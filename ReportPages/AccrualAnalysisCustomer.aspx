<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Main.Master" CodeBehind="AccrualAnalysisCustomer.aspx.vb" Inherits="WEBTDS.AccrualAnalysisCustomer" %>
<%@ Register Assembly="EO.Web" Namespace="EO.Web" TagPrefix="eo" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<head runat="server" />
          <form id="form1" runat="server">
   <asp:UpdatePanel ID="UpdatePanelAll" UpdateMode="Always" runat="server"><ContentTemplate>

    <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
               <div class="box box-info">
                <div class="box-header with-border">
                  <h3 class="box-title">Filter Options</h3> 
                     
                </div><!-- /.box-header -->
                <!-- form start -->
             <%-- filter options table contents --%>
                  <div class="box-body">
<center>
                      <table style="width:60%;"><tr><td style="width:50%">
                          <table style="width:100%">
                           
                              <tr>
                                  <td style="padding-bottom:10px">
                                  Pick A Date
                                  <td style="padding-bottom:10px">
                                 
                                      <asp:TextBox ID="DateSystem" runat="server" Width="100%"></asp:TextBox>
                                      <ajaxToolkit:CalendarExtender ID="DateSystem_CalendarExtender" runat="server" BehaviorID="DateSystem_CalendarExtender" TargetControlID="DateSystem" />
                                  </td></tr>
                             <tr>
                                 <td>
    Currency
                                  <td style="padding-bottom:10px">
                                  
<asp:DropDownList ID="cmbCurrency" runat="server" AutoPostBack="True" class="form-control select2" style="width: 100%;"></asp:DropDownList>
                                  </td></tr>

                          </table>


                                 </td>

                          <td style="width:50%">
                               <%-- grid for dealscontents --%>
                                <asp:Label ID="Label4" runat="server" Text="Customers" BorderColor="#66FFFF" BackColor="#FFFFCC"></asp:Label> <br />
                                 <eo:grid ID="GridCustomer" runat="server" BorderColor="#C7D1DF" 
                                        BorderWidth="1px" ColumnHeaderAscImage="00050303" 
                                        ColumnHeaderDescImage="00050304" ColumnHeaderDividerImage="00050302" 
                                        FixedColumnCount="1" Font-Bold="False" Font-Italic="False" Font-Names="Verdana" 
                                        Font-Overline="False" Font-Size="9pt" Font-Strikeout="False" 
                                        Font-Underline="False" GridLineColor="199, 209, 223" GridLines="Both" 
                                        Height="130px" ItemHeight="19" Width="84%" >
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
                                            <eo:CheckBoxColumn Width="40">
                                            </eo:CheckBoxColumn>
                                              <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="customer_Number" 
                                                HeaderText="Customer Number" MinWidth="20" ReadOnly="True" Width="70">
                                               
                                            </eo:CustomColumn>
                                            <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="fullName" 
                                                HeaderText="Full Name" MinWidth="20" ReadOnly="True" Width="250">
                                               
                                            </eo:CustomColumn>
                                           
                                        </Columns>
                                    </eo:grid>
</td>
                          <td style="width:33%">
                              <table>
                                
                                  <tr> <td style="padding-bottom:10px">  <asp:Button ID="btnLoad" runat="server" CssClass="btn btn-primary btn-block btn-flats" Height="40px" Text="Load Report" Width="150px" /></td></tr>
                                 
                                       <tr> <td style="padding-bottom:10px"><asp:Button ID="btnPrint" runat="server" CssClass="btn btn-primary btn-block btn-flats" Height="40px" Text="Print" Width="150px"  Enabled="false"/></td></tr>
                                    
                                    
                              </table>


                                 </td>
                             </tr></table></center>
                        </div></div>
            <div class="panel box box-success">
                            <div class="box-header with-border"><h4 class="box-title">
                     
                                    <a data-toggle="collapse" data-parent="#accordion" href="#history12">Liabilities Detail view     </a>
                                   </h4>                      </div>
                             <div id="history12" class="panel-collapse collapse-in">

                   
                      <table style="width:100%;" ><tr>
                          <td>
                              
                             <eo:grid ID="GridAccrualsDetailWorkliabilities" runat="server" BorderColor="#C7D1DF" 
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
                                            <eo:CustomColumn DataField="ProductDescription" HeaderText="Description" 
                                                MinWidth="20" ReadOnly="True" Width="200">
                                                
                                            </eo:CustomColumn>  
                                             <eo:CustomColumn DataField="Dealref" HeaderText="Deal Ref" 
                                                MinWidth="20" ReadOnly="True" Width="200">
                                                
                                            </eo:CustomColumn>
                                            <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="customerName" 
                                                HeaderText="Customer Name" MinWidth="10" ReadOnly="True"  Width="200">
                                                
                                            </eo:CustomColumn>
                                            <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="StartDate" 
                                                HeaderText="Start Date" MinWidth="25" ReadOnly="True" Width="200" >
                                               
                                            </eo:CustomColumn>

                                            <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="MaturityDate" 
                                                HeaderText="Maturity Date" MinWidth="20" ReadOnly="True" Width="150">
                                               
                                            </eo:CustomColumn>
                                              <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="AmountInvested" 
                                                HeaderText="Amount Invested" MinWidth="20" ReadOnly="True" Width="150">
                                               
                                            </eo:CustomColumn>
                                            <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="DiscountRate" 
                                                HeaderText="Discount Rate" MinWidth="20" ReadOnly="True" Width="150">
                                               
                                            </eo:CustomColumn>
                               
                                        
                                             <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="YTM" 
                                                HeaderText="YTM" MinWidth="20" ReadOnly="True" Width="150">
                                               
                                            </eo:CustomColumn>
                                             <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="Tenor" 
                                                HeaderText="Tenor" MinWidth="20" ReadOnly="True" Width="150">
                                               
                                            </eo:CustomColumn>
                                             <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="InterestAmount" 
                                                HeaderText="InterestAmount" MinWidth="20" ReadOnly="True" Width="150">
                                               
                                            </eo:CustomColumn>
                                            <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="DR" 
                                                HeaderText="DR" MinWidth="20" ReadOnly="True" Width="150">
                                               
                                            </eo:CustomColumn>
                                            <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="InterestAccrued" 
                                                HeaderText="InterestAccrued" MinWidth="20" ReadOnly="True" Width="150">
                                               
                                            </eo:CustomColumn>
                                            <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="DTR" 
                                                HeaderText="DTR" MinWidth="20" ReadOnly="True" Width="150">
                                               
                                            </eo:CustomColumn>
                                             <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="MaturityValue" 
                                                HeaderText="Maturity Value" MinWidth="20" ReadOnly="True" Width="150">
                                               
                                            </eo:CustomColumn>
                                             <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="DailyAccrual" 
                                                HeaderText="Daily Accrual" MinWidth="20" ReadOnly="True" Width="150">
                                               
                                            </eo:CustomColumn>
                                             <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="Currency" 
                                                HeaderText="Currency" MinWidth="20" ReadOnly="True" Width="150">
                                               
                                            </eo:CustomColumn>
                                           
                                             <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="dealCode" 
                                                HeaderText="Deal Code" MinWidth="20" ReadOnly="True" Width="150">
                                               
                                            </eo:CustomColumn>
                                           
                                            
                                           
                                        </Columns>
                                    </eo:grid>
                           
                          </td></tr>

                      </table>
                      <hr />
                     
                    
                      
                      </div></div>
                      <div class="panel box box-success">
                            <div class="box-header with-border"><h4 class="box-title">
                     
                                    <a data-toggle="collapse" data-parent="#accordion" href="#history123">Liabilities Summary view </a>
                                   </h4>                      </div>
                             <div id="history123" class="panel-collapse collapse"><div class="box-body"><asp:Label ID="lblAddress" runat="server"></asp:Label><br />

                   
                      <table style="width:100%;" ><tr>
                          <td>
                              
                             <eo:grid ID="GridLiabilitiesSummary" runat="server" BorderColor="#C7D1DF" 
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
                                            <eo:CustomColumn DataField="DealCode" HeaderText="DealCode" 
                                                MinWidth="20" ReadOnly="True" Width="200">
                                                
                                            </eo:CustomColumn>  
                                             <eo:CustomColumn DataField="Currency" HeaderText="Currency" 
                                                MinWidth="20" ReadOnly="True" Width="200">
                                                
                                            </eo:CustomColumn>
                                            <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="SubTotalAmountInvested" 
                                                HeaderText="Total Amount Invested" MinWidth="10" ReadOnly="True"  Width="200">
                                                
                                            </eo:CustomColumn>
                                            <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="AvgDiscountRate" 
                                                HeaderText="Avg Discount Rate" MinWidth="25" ReadOnly="True" Width="200" >
                                               
                                            </eo:CustomColumn>
                                            <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="AvgYTM" 
                                                HeaderText="Avg YTM" MinWidth="20" ReadOnly="True" Width="150">
                                               
                                            </eo:CustomColumn>
                                            <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="AvgDays" 
                                                HeaderText="Avg Days" MinWidth="20" ReadOnly="True" Width="150">
                                               
                                            </eo:CustomColumn>
                               
                                        
                                             <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="subTotalInterestAmount" 
                                                HeaderText="Total Interest Amount" MinWidth="20" ReadOnly="True" Width="150">
                                               
                                            </eo:CustomColumn>
                                             <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="AvgDR" 
                                                HeaderText="Avg DR" MinWidth="20" ReadOnly="True" Width="150">
                                               
                                            </eo:CustomColumn>
                                             <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="SubTotalInterestAccrued" 
                                                HeaderText="Total Interest Accrued" MinWidth="20" ReadOnly="True" Width="150">
                                               
                                            </eo:CustomColumn>
                                             <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="AvgDTR" 
                                                HeaderText="Avg DTR" MinWidth="20" ReadOnly="True" Width="150">
                                               
                                            </eo:CustomColumn>
                                             <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="SubtotalMaturityValue" 
                                                HeaderText="Total Maturity Value" MinWidth="20" ReadOnly="True" Width="150">
                                               
                                            </eo:CustomColumn>
                                             <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="SubTotalDailyAccrual" 
                                                HeaderText="Total Daily Accrual" MinWidth="20" ReadOnly="True" Width="150">
                                               
                                            </eo:CustomColumn>
                                           
                                        </Columns>
                                    </eo:grid>
                           
                          </td></tr>

                      </table>
                      <hr />
                     
                      <table style="width:100%;" ><tr>
                          <td>
                              
                             <eo:grid ID="GridSubTotals" runat="server" BorderColor="#C7D1DF" 
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
                                            <eo:CustomColumn DataField="description" HeaderText="Description" 
                                                MinWidth="20" ReadOnly="True" Width="200">
                                                
                                            </eo:CustomColumn>  
                                             <eo:CustomColumn DataField="Currency" HeaderText="Currency" 
                                                MinWidth="20" ReadOnly="True" Width="200">
                                                
                                            </eo:CustomColumn>
                                            <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="DailySubtotal" 
                                                HeaderText="Daily Subtotal" MinWidth="10" ReadOnly="True"  Width="200">
                                                
                                            </eo:CustomColumn>
                                            <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="AveragedDiscount" 
                                                HeaderText="Avg Discount Rate" MinWidth="25" ReadOnly="True" Width="200" >
                                               
                                            </eo:CustomColumn>

                                            <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="AverageYTM" 
                                                HeaderText="Avg YTM" MinWidth="20" ReadOnly="True" Width="150">
                                               
                                            </eo:CustomColumn>
                                              <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="AverageDTR" 
                                                HeaderText="Avg DTR" MinWidth="20" ReadOnly="True" Width="150">
                                               
                                            </eo:CustomColumn>
                                            <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="AverageDecayedyield" 
                                                HeaderText="Average Decayed yield" MinWidth="20" ReadOnly="True" Width="150">
                                               
                                            </eo:CustomColumn>
                               
                                        
                                             <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="MaturityValue" 
                                                HeaderText="Maturity Value" MinWidth="20" ReadOnly="True" Width="150">
                                               
                                            </eo:CustomColumn>
                                             <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="InterestAmount" 
                                                HeaderText="Interest Amount" MinWidth="20" ReadOnly="True" Width="150">
                                               
                                            </eo:CustomColumn>
                                             <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="InterestAccruals" 
                                                HeaderText="Interest Accruals" MinWidth="20" ReadOnly="True" Width="150">
                                               
                                            </eo:CustomColumn>
                                           
                                            
                                           
                                        </Columns>
                                    </eo:grid>
                            
                          </td></tr>

                      </table>
                      
                      </div></div></div>
               
                       <div class="panel box box-success">
                            <div class="box-header with-border"><h4 class="box-title">
                      
                                    <a data-toggle="collapse" data-parent="#accordion" href="#Assets">Assets Detail view </a>
                                   </h4>                      </div>
                             <div id="Assets" class="panel-collapse collapse"><div class="box-body"><br /
                      <table style="width:100%;" ><tr>
                          <td>
                              
                             <eo:grid ID="GridAccrualsDetailWorkAssets" runat="server" BorderColor="#C7D1DF" 
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
                                            <eo:CustomColumn DataField="ProductDescription" HeaderText="Description" 
                                                MinWidth="20" ReadOnly="True" Width="200">
                                                
                                            </eo:CustomColumn>  
                                             <eo:CustomColumn DataField="Classification" HeaderText="Classification" 
                                                MinWidth="20" ReadOnly="True" Width="200">
                                                
                                            </eo:CustomColumn>
                                            <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="DealCode" 
                                                HeaderText="Deal Code" MinWidth="10" ReadOnly="True"  Width="200">
                                                
                                            </eo:CustomColumn>
                                             <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="Currency" 
                                                HeaderText="Currency" MinWidth="10" ReadOnly="True"  Width="200">
                                                
                                            </eo:CustomColumn>
                                             <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="DealReference" 
                                                HeaderText="Deal Reference" MinWidth="10" ReadOnly="True"  Width="200">
                                                
                                            </eo:CustomColumn>
                                            <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="CustomerName" 
                                                HeaderText="Customer Name" MinWidth="25" ReadOnly="True" Width="200" >
                                               
                                            </eo:CustomColumn>

                                            <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="Nominalamount" 
                                                HeaderText="Nominal Amount" MinWidth="20" ReadOnly="True" Width="150">
                                               
                                            </eo:CustomColumn>
                                              <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="StartDate" 
                                                HeaderText="Start Date" MinWidth="20" ReadOnly="True" Width="150">
                                               
                                            </eo:CustomColumn>
                                                <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="MaturityDate" 
                                                HeaderText="Maturity Date" MinWidth="20" ReadOnly="True" Width="150">
                                               
                                            </eo:CustomColumn>
                                                <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="DiscountRate" 
                                                HeaderText="Discount Rate" MinWidth="20" ReadOnly="True" Width="150">
                                               
                                            </eo:CustomColumn>
                                            <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="YieldRate" 
                                                HeaderText="Yield Rate" MinWidth="20" ReadOnly="True" Width="150">
                                               
                                            </eo:CustomColumn>
                               
                                        
                                             <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="Tenor" 
                                                HeaderText="Tenor" MinWidth="20" ReadOnly="True" Width="150">
                                               
                                            </eo:CustomColumn>
                                             <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="DiscountAmount" 
                                                HeaderText="Discount Amount" MinWidth="20" ReadOnly="True" Width="150">
                                               
                                            </eo:CustomColumn>
                                             <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="AmountInvested" 
                                                HeaderText="AmountInvested" MinWidth="20" ReadOnly="True" Width="150">
                                               
                                            </eo:CustomColumn>
                                            <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="DR" 
                                                HeaderText="DR" MinWidth="20" ReadOnly="True" Width="150">
                                               
                                            </eo:CustomColumn>
                                            <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="DTR" 
                                                HeaderText="DTR" MinWidth="20" ReadOnly="True" Width="150">
                                               
                                            </eo:CustomColumn>
                                            <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="InterestAccrued" 
                                                HeaderText="Interest Accrued" MinWidth="20" ReadOnly="True" Width="150">
                                               
                                            </eo:CustomColumn>
                                             <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="BookValue" 
                                                HeaderText="Book Value" MinWidth="20" ReadOnly="True" Width="150">
                                               
                                            </eo:CustomColumn>
                                              <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="YTMBV" 
                                                HeaderText="YTMBV" MinWidth="20" ReadOnly="True" Width="150">
                                               
                                            </eo:CustomColumn>

                                             <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="WeekendAccrual" 
                                                HeaderText="Weekend Accrual" MinWidth="20" ReadOnly="True" Width="150">
                                               
                                            </eo:CustomColumn>
                                            
                                           
                                            
                                           
                                        </Columns>
                                    </eo:grid>
                           
                          </td></tr>

                      </table>
                     <hr />
                     
                      
                    
</div></div>      </div>      
                         <div class="panel box box-success">
                            <div class="box-header with-border"><h4 class="box-title">
                      
                                    <a data-toggle="collapse" data-parent="#accordion" href="#SubTotalAssets">Assets Summary view </a>
                                   </h4>                      </div>
                             <div id="SubTotalAssets" class="panel-collapse collapse"><div class="box-body"><br /
                      <table style="width:100%;" ><tr>
                          <td>
                              
                        
                                    <eo:grid ID="GridSubTotalAssets" runat="server" BorderColor="#C7D1DF" 
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
                                            <eo:CustomColumn DataField="ProductDescription" HeaderText="Description" 
                                                MinWidth="20" ReadOnly="True" Width="200">
                                                
                                            </eo:CustomColumn>  
                                             <eo:CustomColumn DataField="Classification" HeaderText="Classification" 
                                                MinWidth="20" ReadOnly="True" Width="200">
                                                
                                            </eo:CustomColumn>
                                            <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="DealCode" 
                                                HeaderText="Deal Code" MinWidth="10" ReadOnly="True"  Width="200">
                                                
                                            </eo:CustomColumn>
                                            <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="Currency" 
                                                HeaderText="Currency" MinWidth="25" ReadOnly="True" Width="200" >
                                               
                                            </eo:CustomColumn>

                                            <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="Nominalamount" 
                                                HeaderText="Nominal Amount" MinWidth="20" ReadOnly="True" Width="150">
                                               
                                            </eo:CustomColumn>
                                              <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="AvgDiscountRate" 
                                                HeaderText="Avg DTR" MinWidth="20" ReadOnly="True" Width="150">
                                               
                                            </eo:CustomColumn>
                                            <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="AvgYield" 
                                                HeaderText="Average yield rate" MinWidth="20" ReadOnly="True" Width="150">
                                               
                                            </eo:CustomColumn>
                               
                                        
                                             <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="AvgDays" 
                                                HeaderText="Avg Days" MinWidth="20" ReadOnly="True" Width="150">
                                               
                                            </eo:CustomColumn>
                                             <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="Discount" 
                                                HeaderText="Discount" MinWidth="20" ReadOnly="True" Width="150">
                                               
                                            </eo:CustomColumn>
                                             <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="AmountInvested" 
                                                HeaderText="AmountInvested" MinWidth="20" ReadOnly="True" Width="150">
                                               
                                            </eo:CustomColumn>
                                            <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="DaysRun" 
                                                HeaderText="Days Run" MinWidth="20" ReadOnly="True" Width="150">
                                               
                                            </eo:CustomColumn>
                                            <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="DTR" 
                                                HeaderText="DTR" MinWidth="20" ReadOnly="True" Width="150">
                                               
                                            </eo:CustomColumn>
                                            <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="Bookvalue" 
                                                HeaderText="Book Value" MinWidth="20" ReadOnly="True" Width="150">
                                               
                                            </eo:CustomColumn>
                                             <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="DailyAccrual" 
                                                HeaderText="Daily Accrual" MinWidth="20" ReadOnly="True" Width="150">
                                               
                                            </eo:CustomColumn>
                                             <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="WeekendAccrual" 
                                                HeaderText="Weekend Accrual" MinWidth="20" ReadOnly="True" Width="150">
                                               
                                            </eo:CustomColumn>
                                             <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="InterestAccrued" 
                                                HeaderText="Interest Accrued" MinWidth="20" ReadOnly="True" Width="150">
                                               
                                            </eo:CustomColumn>
                                           
                                            
                                           
                                        </Columns>
                                    </eo:grid>
                          </td></tr>

                      </table>
                      <hr />
                     
                      <table style="width:100%;" ><tr>
                          <td>
                              
                             <eo:grid ID="GridSummarySubtotalsAssets" runat="server" BorderColor="#C7D1DF" 
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
                                            <eo:CustomColumn DataField="description" HeaderText="Description" 
                                                MinWidth="20" ReadOnly="True" Width="200">
                                                
                                            </eo:CustomColumn>  
                                             <eo:CustomColumn DataField="Currency" HeaderText="Currency" 
                                                MinWidth="20" ReadOnly="True" Width="200">
                                                
                                            </eo:CustomColumn>
                                            <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="DailySubtotal" 
                                                HeaderText="Daily Subtotal" MinWidth="10" ReadOnly="True"  Width="200">
                                                
                                            </eo:CustomColumn>
                                            <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="VolumeSubtotal" 
                                                HeaderText="Volume Subtotal" MinWidth="25" ReadOnly="True" Width="200" >
                                               
                                            </eo:CustomColumn>

                                            <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="AveragedDiscount" 
                                                HeaderText="Averaged Discount" MinWidth="20" ReadOnly="True" Width="150">
                                               
                                            </eo:CustomColumn>
                                              <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="AverageYTM" 
                                                HeaderText="Average YTM" MinWidth="20" ReadOnly="True" Width="150">
                                               
                                            </eo:CustomColumn>
                                              <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="AverageDTR" 
                                                HeaderText="Average DTR" MinWidth="20" ReadOnly="True" Width="150">
                                               
                                            </eo:CustomColumn>
                                            <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="AverageDecayedyield" 
                                                HeaderText="Average Decayed yield" MinWidth="20" ReadOnly="True" Width="150">
                                               
                                            </eo:CustomColumn>
                               
                                        
                                             <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="NominalAmount" 
                                                HeaderText="Nominal Amount" MinWidth="20" ReadOnly="True" Width="150">
                                               
                                            </eo:CustomColumn>
                                             <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="AmountInvested" 
                                                HeaderText="Amount Invested" MinWidth="20" ReadOnly="True" Width="150">
                                               
                                            </eo:CustomColumn>
                                             <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="Discount" 
                                                HeaderText="Discount" MinWidth="20" ReadOnly="True" Width="150">
                                               
                                            </eo:CustomColumn>
                                              <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="Discount" 
                                                HeaderText="Discount" MinWidth="20" ReadOnly="True" Width="150">
                                               
                                            </eo:CustomColumn>
                                           
                                            
                                           
                                        </Columns>
                                    </eo:grid>
                           
                          </td></tr>

                      </table>
                    
</div></div>      </div>              
                    

             </ContentTemplate>
             </asp:UpdatePanel>
              </form>
</asp:Content>
