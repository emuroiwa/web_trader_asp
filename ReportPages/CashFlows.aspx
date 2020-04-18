<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Main.Master" CodeBehind="CashFlows.aspx.vb" Inherits="WEBTDS.CashFlows" %>
<%@ Register Assembly="EO.Web" Namespace="EO.Web" TagPrefix="eo" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<head />
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

                      <table style="width:100%"><tr><td style="width:33%">
                          <table style="width:100%">
                             <tr style="padding-bottom:10px">
                                  <td   style="width:20%;padding-bottom:20px">
                                  Pick A Date
                                  </td><td>
                                      <asp:TextBox ID="selectiondate" runat="server" Width="100%"></asp:TextBox>
                                      <ajaxToolkit:CalendarExtender ID="selectiondate_CalendarExtender" runat="server" BehaviorID="selectiondate_CalendarExtender" TargetControlID="selectiondate" />
                                  </td></tr>  <tr style="padding-bottom:10px"><td style="padding-bottom:40px">
                                  Period
                                  </td><td>
<asp:DropDownList ID="cmbDays" runat="server" AutoPostBack="True" class="form-control select2" style="width: 100%;">

                                      </asp:DropDownList>
                                  </td></tr>
                             
                             <tr>
                                 <td>
                   Currency
                                  </td><td>
<asp:DropDownList ID="cmbCurrency" runat="server" AutoPostBack="True" class="form-control select2" style="width: 100%;"></asp:DropDownList>
                                  </td></tr>

                          </table>


                                 </td>

                          <td style="width:33%">
                              <center>
                               <%-- grid for dealscontents --%>
                                <asp:Label ID="Label4" runat="server" Text="Deal Codes" BorderColor="#66FFFF" BackColor="#FFFFCC"></asp:Label> <br />
                                 <eo:grid ID="GridDeals" runat="server" BorderColor="#C7D1DF" 
                                        BorderWidth="1px" ColumnHeaderAscImage="00050303" 
                                        ColumnHeaderDescImage="00050304" ColumnHeaderDividerImage="00050302" 
                                        FixedColumnCount="1" Font-Bold="False" Font-Italic="False" Font-Names="Verdana" 
                                        Font-Overline="False" Font-Size="9pt" Font-Strikeout="False" 
                                        Font-Underline="False" GridLineColor="199, 209, 223" GridLines="Both" 
                                        Height="130px" ItemHeight="19" Width="120px" >
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
                                              <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="Deal_code" 
                                                HeaderText="Deal Code" MinWidth="20" ReadOnly="True" Width="70">
                                               
                                            </eo:CustomColumn>
                                            
                                        </Columns>
                                    </eo:grid></center>

</td>
                          <td style="width:33%">
                              <table>
                                  <tr><td style="padding-bottom:20px">  <asp:Button ID="btnLoad" runat="server" CssClass="btn btn-primary btn-block btn-flats" Height="40px" Text="Load Report" Width="150px" /></td></tr>
                                       <tr><td style="padding-bottom:20px"><div class="btn-group">
                      <button type="button" class="btn btn-info btn-flat">Printable Reportsintable Reports</button>
                      <button type="button" class="btn btn-info btn-flat dropdown-toggle" data-toggle="dropdown">
                        <span class="caret"></span>
                        <span class="sr-only">Toggle Dropdown</span>
                      </button>
                      <ul class="dropdown-menu" role="menu">
                                                  <li><a href="Reportviewer.aspx?report=Cashflowanalysys2">Detailed Maturities Ladder</a></li>

                        <li><a href="ReportViewer.aspx?report=Cashflow">Summaries Maturities Ladder</a></li>
                       
                      </ul>
                    </div></td></tr>
                                       <tr><td ><asp:Button ID="cmdRefresh0" runat="server" CssClass="btn btn-danger btn-block btn-flats" Height="40px" Text="Exit" Width="150px" /></td></tr>
                              </table>


                                 </td>
                             </tr></table></div></div>
                      <table style="width:100%"><tr>
                          <td>
                                <div class="box box-info">
                <div class="box-header with-border">
                  <h3 class="box-title">Assests</h3> 
                     
                </div><!-- /.box-header -->
                <!-- form start -->
             <%-- filter options table contents --%>
                  <div class="box-body">

                     <eo:grid ID="GridIn" runat="server" BorderColor="#C7D1DF" 
                                        BorderWidth="1px" ColumnHeaderAscImage="00050303" 
                                        ColumnHeaderDescImage="00050304" ColumnHeaderDividerImage="00050302" 
                                        FixedColumnCount="1" Font-Bold="False" Font-Italic="False" Font-Names="Verdana" 
                                        Font-Overline="False" Font-Size="9pt" Font-Strikeout="False" 
                                        Font-Underline="False" GridLineColor="199, 209, 223" GridLines="Both" 
                                        Height="350px" ItemHeight="19" Width="100%" >
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
                                            <eo:RowNumberColumn Width="1">
                                            </eo:RowNumberColumn>
                                            <eo:CustomColumn DataField="dealref" HeaderText="Dealer" 
                                                MinWidth="20" ReadOnly="True" Width="200">
                                                
                                            </eo:CustomColumn>
                                            <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="dealamnt" 
                                                HeaderText="Deal Amount" MinWidth="10" ReadOnly="True">
                                                
                                            </eo:CustomColumn>
                                            <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="maturityamount" 
                                                HeaderText="Maturity Amount" MinWidth="10" ReadOnly="True">
                                                
                                            </eo:CustomColumn>
                                            <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="netinterest" 
                                                HeaderText="Net Interest" MinWidth="25" ReadOnly="True" Width="200" >
                                               
                                            </eo:CustomColumn> 
                                            <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="days" 
                                                HeaderText="Days to Maturity" MinWidth="25" ReadOnly="True" Width="200" >
                                               
                                            </eo:CustomColumn>
                                            <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="maturitydate" 
                                                HeaderText="Maturity Date" MinWidth="20" ReadOnly="True" Width="150">
                                               
                                            </eo:CustomColumn>
                                            
                                        </Columns>
                                    </eo:grid>
                  </div></div>
                          </td><td>

                                <div class="box box-info">
                <div class="box-header with-border">
                  <h3 class="box-title">Liablities</h3> 
                     
                </div><!-- /.box-header -->
                <!-- form start -->
             <%-- filter options table contents --%>
                  <div class="box-body">

                          <eo:grid ID="GridOut" runat="server" BorderColor="#C7D1DF" 
                                        BorderWidth="1px" ColumnHeaderAscImage="00050303" 
                                        ColumnHeaderDescImage="00050304" ColumnHeaderDividerImage="00050302" 
                                        FixedColumnCount="1" Font-Bold="False" Font-Italic="False" Font-Names="Verdana" 
                                        Font-Overline="False" Font-Size="9pt" Font-Strikeout="False" 
                                        Font-Underline="False" GridLineColor="199, 209, 223" GridLines="Both" 
                                        Height="350px" ItemHeight="19" Width="100%" >
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
                                            <eo:RowNumberColumn Width="1">
                                            </eo:RowNumberColumn>
                                            <eo:CustomColumn DataField="dealref" HeaderText="Dealer" 
                                                MinWidth="20" ReadOnly="True" Width="200">
                                                
                                            </eo:CustomColumn>
                                            <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="dealamnt" 
                                                HeaderText="Deal Amount" MinWidth="10" ReadOnly="True">
                                                
                                            </eo:CustomColumn>
                                            <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="maturityamount" 
                                                HeaderText="Maturity Amount" MinWidth="10" ReadOnly="True">
                                                
                                            </eo:CustomColumn>
                                            <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="netinterest" 
                                                HeaderText="Net Interest" MinWidth="25" ReadOnly="True" Width="200" >
                                               
                                            </eo:CustomColumn> 
                                            <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="days" 
                                                HeaderText="Days to Maturity" MinWidth="25" ReadOnly="True" Width="200" >
                                               
                                            </eo:CustomColumn>
                                            <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="maturitydate" 
                                                HeaderText="Maturity Date" MinWidth="20" ReadOnly="True" Width="150">
                                               
                                            </eo:CustomColumn>
                                            
                                        </Columns>
                                    </eo:grid>
                  </div></div>
                               </td>

                                                </tr>

                      </table>
        <table style="width:100%"><tr><td>

            
                                <div class="box box-info">
                <div class="box-header with-border">
                  <h3 class="box-title">Net Position</h3> 
                     
                </div><!-- /.box-header -->
                <!-- form start -->
             <%-- filter options table contents --%>
                  <div class="box-body">
<center>
                      <table style="width:60%">
                          <tr>
                          <td style="width:33%"><asp:Label ID="Label1" runat="server" Text="Deal Amount"></asp:Label></td>

                          <td style="width:33%"><asp:Label ID="Label2" runat="server" Text="Maturity Amount"></asp:Label></td>
                          <td style="width:33%"><asp:Label ID="Label3" runat="server" Text="Net Interest"></asp:Label></td>
                         <tr>
                          <td style="width:33%"><asp:Label ID="DealAmount" runat="server" Text=""></asp:Label></td>

                          <td style="width:33%"><asp:Label ID="MaturityAmount" runat="server" Text=""></asp:Label></td>
                          <td style="width:33%"><asp:Label ID="NetInterest" runat="server" Text=""></asp:Label></td>
                                               </tr>                       </tr>

                      </table></center>
                  </div></div>
                   </td></tr></table>
             </ContentTemplate>
</asp:UpdatePanel>                      </form>
</asp:Content>
