<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Main.Master" CodeBehind="IncomeExpenses.aspx.vb" Inherits="WEBTDS.IncomeExpenses" %>
<%@ Register Assembly="EO.Web" Namespace="EO.Web" TagPrefix="eo" %>
     
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="ajaxToolkit" %>
     
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<head runat="server" />

          <form id="form1" runat="server">
         <asp:UpdatePanel ID="UpdatePanelAll" UpdateMode="Always" runat="server"><ContentTemplate>

    <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
     <%--  filter options table --%>
<table style="width:100%"><tr><td>
    <div class="box box-info">
                <div class="box-header with-border">
                  <h3 class="box-title">Filter Options</h3> 
                     
                </div><!-- /.box-header -->
                <!-- form start -->
             <%-- filter options table contents --%>
                  <div class="box-body">
                      <table style="width:100%"><tr>
                          <td style="width:33%">  <%-- 1st column table contents --%>
                              <table style="width:100%">
                                  <tr>
                                  <td style="width:20%;padding-bottom:15px" >

                                      Period Start Date
                                         </td>
                                          <td>

                                              <asp:TextBox ID="dtStart" runat="server" Width="80%"></asp:TextBox>
                                              <ajaxToolkit:CalendarExtender ID="dtStart_CalendarExtender" runat="server" BehaviorID="dtStart_CalendarExtender" TargetControlID="dtStart" />
                                         </td>
                                     </tr>
                                     <tr>
                                  <td style="width:20%;padding-bottom:15px">

                                      Period End Date
                                         </td>
                                          <td>
                                              
                                              <asp:TextBox ID="dtEnd" runat="server" Width="80%"></asp:TextBox>
                                              <ajaxToolkit:CalendarExtender ID="dtEnd_CalendarExtender" runat="server" BehaviorID="dtEnd_CalendarExtender" TargetControlID="dtEnd" />
                                         </td>
                                     </tr>
                                   <tr>
                                  <td style="width:20%">
                                      Currency
                                         </td>
                                          <td>
                                              <asp:DropDownList ID="cmbCurrency" runat="server" class="form-control select2" Width="80%"></asp:DropDownList>
                                         </td>
                                     </tr>
                              </table><%-- end 1st column table contents --%>


                                 </td>
                            <td style="width:33%">
                                <%-- grid for dealscontents --%>
                                <asp:Label ID="Label4" runat="server" Text="Deal Codes" BorderColor="#66FFFF" BackColor="#FFFFCC"></asp:Label> <br />
                                 <eo:grid ID="GridDeals" runat="server" BorderColor="#C7D1DF" 
                                        BorderWidth="1px" ColumnHeaderAscImage="00050303" 
                                        ColumnHeaderDescImage="00050304" ColumnHeaderDividerImage="00050302" 
                                        FixedColumnCount="1" Font-Bold="False" Font-Italic="False" Font-Names="Verdana" 
                                        Font-Overline="False" Font-Size="9pt" Font-Strikeout="False" 
                                        Font-Underline="False" GridLineColor="199, 209, 223" GridLines="Both" 
                                        Height="130px" ItemHeight="19" Width="50%" >
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
                                                HeaderText="Deal Code" MinWidth="20" ReadOnly="True" Width="100">
                                               
                                            </eo:CustomColumn>
                                            <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="dealBasictype" 
                                                HeaderText="Structure" MinWidth="20" ReadOnly="True" Width="100">
                                               
                                            </eo:CustomColumn>
                                           
                                        </Columns>
                                    </eo:grid>


                                 </td>
                            <td style="width:33%">
                                 <%-- grid for dealer --%>
                                  <asp:Label ID="Label5" runat="server" Text="Dealers" BorderColor="#66FFFF" BackColor="#FFFFCC"></asp:Label> <br />
                                <eo:grid ID="GridDealer" runat="server" BorderColor="#C7D1DF" 
                                        BorderWidth="1px" ColumnHeaderAscImage="00050303" 
                                        ColumnHeaderDescImage="00050304" ColumnHeaderDividerImage="00050302" 
                                        FixedColumnCount="1" Font-Bold="False" Font-Italic="False" Font-Names="Verdana" 
                                        Font-Overline="False" Font-Size="9pt" Font-Strikeout="False" 
                                        Font-Underline="False" GridLineColor="199, 209, 223" GridLines="Both" 
                                        Height="130px" ItemHeight="19" Width="50%" >
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
                                           
                                             <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="user_id" 
                                                HeaderText="Dealer Name" MinWidth="20" ReadOnly="True" Width="200">
                                               
                                            </eo:CustomColumn>

                                           
                                        </Columns>
                                    </eo:grid>


                                 </td>

                             </tr>
                          <tr><td colspan="3">
                              <%-- Buttons table --%>
                              <hr />
                              <table style="width:100%"><tr>
                                  <td  style="align-content:center">                              <asp:Button ID="bntGet" runat="server"  CssClass="btn btn-primary btn-block btn-flats" Height="40px" Text="Get Details" Width="150px" />
                                  </td>
                                  <td><div class="btn-group">
                      <button type="button" class="btn btn-info btn-flat">Printable Reports</button>
                      <button type="button" class="btn btn-info btn-flat dropdown-toggle" data-toggle="dropdown">
                        <span class="caret"></span>
                        <span class="sr-only">Toggle Dropdown</span>
                      </button>
                      <ul class="dropdown-menu" role="menu">
                        <li><a href="ReportViewer.aspx?report=Income Expense Analysis">Income Expense Analysis</a></li>
                        <li><a href="Reportviewer.aspx?report=Income Expense Return">Income /Expense Report</a></li>
                        <li><a href="Reportviewer.aspx?report=commission return">Commission Return</a></li>
                       
                        <li><a href="Reportviewer.aspx?report=tax return">Tax Return</a></li>
                      </ul>
                    </div></td></tr></table>


                              </td></tr>

                      </table>
                          <%-- end filter options table contents --%>



                      </div></div><!-- end .box-header -->
                <!-- end form start -->




</td></tr>

    <tr><td>
         <div class="box box-info">
                <div class="box-header with-border">
                  <h3 class="box-title">Details</h3> 
                     
                </div><!-- /.box-header -->
                <!-- form start -->
             <%-- filter options table contents --%>
                  <div class="box-body">
<table style="width:100%"><tr><td style="width:80%">
    <asp:Label ID="Label2" runat="server" Text="Detailed" BorderColor="#66FFFF" BackColor="#FFFFCC"></asp:Label> <br />
      <%-- <table id="example1" class="table table-bordered table-striped">
                    <thead>
                      <tr>
                       <th>Deal Structure</th>
                       
                        <th>Currency</th>
                        <th>Deal Reference</th>
                        <th>Maturity Amount</th>
                        <th>Interest Rate</th>
                           <th>Currency</th>
                        <th>Tenor</th>
                        <th>Days To Run</th>
                        <th>Rollover Deal</th>
                                        <th>Start Date</th>
                        <th>Maturity Date</th>
                      </tr>
                    </thead>
                    <tbody>
                        <asp:Label ID="lblCustomerDeals" runat="server" Text=""></asp:Label>
                      </tbody></table>--%>
    <eo:grid ID="GridCustomerDeals" runat="server" BorderColor="#C7D1DF" 
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
              
                                            <eo:RowNumberColumn Width="1">
                                            </eo:RowNumberColumn>
                                            <eo:CustomColumn DataField="dealref" HeaderText="Deal Reference" 
                                                MinWidth="20" ReadOnly="True" Width="200">
                                                
                                            </eo:CustomColumn>
                                            <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="dealamnt" 
                                                HeaderText="Deal Amount" MinWidth="10" ReadOnly="True">
                                                
                                            </eo:CustomColumn>
                                            <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="periodaccrual" 
                                                HeaderText="Period Accrual" MinWidth="25" ReadOnly="True" Width="200" >
                                               
                                            </eo:CustomColumn>
                                            <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="tenor" 
                                                HeaderText="tenor" MinWidth="20" ReadOnly="True" Width="150">
                                               
                                            </eo:CustomColumn>
                                            <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="startdate" 
                                                HeaderText="Start Date" MinWidth="20" ReadOnly="True" Width="150">
                                               
                                            </eo:CustomColumn>
                                             <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="maturitydate" 
                                                HeaderText="Maturity Date" MinWidth="20" ReadOnly="True" Width="150">
                                               
                                            </eo:CustomColumn>
                                             <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="taxamount" 
                                                HeaderText="Tax Amount" MinWidth="20" ReadOnly="True" Width="150">
                                               
                                            </eo:CustomColumn>
                                             <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="comrate" 
                                                HeaderText="Commission Rate" MinWidth="20" ReadOnly="True" Width="150">
                                               
                                            </eo:CustomColumn>
                                             <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="dealstatus" 
                                                HeaderText="Deal Status" MinWidth="20" ReadOnly="True" Width="150">
                                               
                                            </eo:CustomColumn>
                                             <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="dealcapturer" 
                                                HeaderText="dealcapturer" MinWidth="20" ReadOnly="True" Width="150">
                                               
                                            </eo:CustomColumn><eo:CustomColumn AllowResize="True" AllowSort="True" DataField="dealtype" 
                                                HeaderText="dealtype" MinWidth="20" ReadOnly="True" Width="150">
                                               
                                            </eo:CustomColumn>
                                           
                                        </Columns>
                                    </eo:grid>
           </td>
    <td>
        <%-- table summary --%>
        <asp:Label ID="Label3" runat="server" Text="Summary" BorderColor="#66FFFF" BackColor="#FFFFCC"></asp:Label> <br />
        <table style="width:100%">
            <tr><td>
                Interest Income
                   </td>
                <td>
                    <asp:Label ID="lblIntIncome" runat="server"  Font-Bold="True" Text=""></asp:Label>
                   </td>
            </tr>
                <tr><td>
                Interest Expenses
                   </td>
                <td>
                    <asp:Label ID="lnlIntExpense"  Font-Bold="True" runat="server" Text=""></asp:Label>
                   </td>
            </tr>
            <tr><td>
        Commission Earned
                   </td>
                <td>
                    <asp:Label ID="Lblcomm" runat="server" Font-Bold="True" Text=""></asp:Label>
                   </td>
            </tr>
              <tr><td>
      Net Position
                   </td>
                <td>
                    <asp:Label ID="lblNetPos" runat="server" Font-Bold="True" Text=""></asp:Label>
                   </td>
            </tr>
            <tr><td>
    Tax
                   </td>
                <td>
                    <asp:Label ID="lblTax" runat="server" Text="" Font-Bold="True"></asp:Label>
                   </td>
            </tr>

        </table>
         <%-- end table summary --%>
    </td>
    
       </tr></table><hr />
                      <table style="width:100%"><tr><td style="width:50%"> 
                           <asp:Label ID="lbltext1" runat="server" Text="Deal Type Summary" BorderColor="#66FFFF" BackColor="#FFFFCC"></asp:Label> <br />
                          <table style="width:100%"><tr><td style="width:50%">
    <asp:Label ID="Label6" runat="server" Text="Detailed" BorderColor="#66FFFF" BackColor="#FFFFCC"></asp:Label> <br />
      
                          <eo:grid ID="GridDealSum" runat="server" BorderColor="#C7D1DF" 
                                        BorderWidth="1px" ColumnHeaderAscImage="00050303" 
                                        ColumnHeaderDescImage="00050304" ColumnHeaderDividerImage="00050302" 
                                        FixedColumnCount="1" Font-Bold="False" Font-Italic="False" Font-Names="Verdana" 
                                        Font-Overline="False" Font-Size="9pt" Font-Strikeout="False" 
                                        Font-Underline="False" GridLineColor="199, 209, 223" GridLines="Both" 
                                        Height="185px" ItemHeight="19" Width="100%" >
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
                                            <eo:CustomColumn DataField="dealref" HeaderText="Deal Reference" 
                                                MinWidth="20" ReadOnly="True" Width="200">
                                                
                                            </eo:CustomColumn>
                                            <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="dealamnt" 
                                                HeaderText="Deal Amount" MinWidth="10" ReadOnly="True">
                                                
                                            </eo:CustomColumn>
                                            <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="periodaccrual" 
                                                HeaderText="Period Accrual" MinWidth="25" ReadOnly="True" Width="200" >
                                               
                                            </eo:CustomColumn>
                                            <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="taxamount" 
                                                HeaderText="Tax Amount" MinWidth="20" ReadOnly="True" Width="150">
                                               
                                            </eo:CustomColumn>
                                            <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="commision" 
                                                HeaderText="Commision Rate%" MinWidth="20" ReadOnly="True" Width="150">
                                               
                                            </eo:CustomColumn>
                                            
                                        </Columns>
                                    </eo:grid></td>
                          <td style="width:50%">   <asp:Label ID="Label1" runat="server" Text="Dealer Summary" BorderColor="#66FFFF" BackColor="#FFFFCC"></asp:Label> 
                              <br />
                             
                              <eo:grid ID="GridDealerSum" runat="server" BorderColor="#C7D1DF" 
                                        BorderWidth="1px" ColumnHeaderAscImage="00050303" 
                                        ColumnHeaderDescImage="00050304" ColumnHeaderDividerImage="00050302" 
                                        FixedColumnCount="1" Font-Bold="False" Font-Italic="False" Font-Names="Verdana" 
                                        Font-Overline="False" Font-Size="9pt" Font-Strikeout="False" 
                                        Font-Underline="False" GridLineColor="199, 209, 223" GridLines="Both" 
                                        Height="185px" ItemHeight="19" Width="100%" >
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
                                            <eo:CustomColumn DataField="dealer" HeaderText="Dealer" 
                                                MinWidth="20" ReadOnly="True" Width="200">
                                                
                                            </eo:CustomColumn>
                                            <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="income" 
                                                HeaderText="income" MinWidth="10" ReadOnly="True">
                                                
                                            </eo:CustomColumn>
                                            <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="expense" 
                                                HeaderText="Expense" MinWidth="25" ReadOnly="True" Width="200" >
                                               
                                            </eo:CustomColumn>
                                            <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="incexp" 
                                                HeaderText="Income Expense" MinWidth="20" ReadOnly="True" Width="150">
                                               
                                            </eo:CustomColumn>
                                            
                                        </Columns>
                                    </eo:grid></td></tr></table>

                      </div></div>

        </td></tr>
</table>
             <%-- End of filter options table --%>







             </ContentTemplate>
             </asp:UpdatePanel>
          </form>




</asp:Content>
