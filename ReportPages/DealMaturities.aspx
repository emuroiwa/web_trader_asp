<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Main.Master" CodeBehind="DealMaturities.aspx.vb" Inherits="WEBTDS.DealMaturities" %>

<%@ Register Assembly="EO.Web" Namespace="EO.Web" TagPrefix="eo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <script type="text/javascript">
         function openModal() {
             $('#customer').modal('show');
         }
</script>
     <form id="form1" runat="server">
         
         <asp:UpdatePanel ID="UpdatePanelAll" UpdateMode="Always" runat="server"><ContentTemplate>

    <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
 <div class="row">
            <div class="col-md-5">
     <div class="box box-info">
                <div class="box-header with-border">
                  <h3 class="box-title">Deal Maturities</h3> 
                     
                </div><!-- /.box-header -->
                <!-- form start -->
             
                  <div class="box-body">
                 

<center>
             <table style="width:100%">

                 <tr><td style="height: 26px;width:10%;padding-bottom:10px">View Options</td><td style="height: 26px">
                     <asp:DropDownList ID="cmbViewOpt" runat="server" class="form-control select2" Width="90%"><asp:ListItem>Display on screen</asp:ListItem><asp:ListItem>Printable Report</asp:ListItem></asp:DropDownList>
                     </td></tr>
                 <tr><td>Type</td><td>
                     <asp:DropDownList ID="cmbType" runat="server" class="form-control select2" Width="90%"><asp:ListItem>Deposit / Placement Deals</asp:ListItem><asp:ListItem>Security Purchases</asp:ListItem><asp:ListItem>Security Sells</asp:ListItem></asp:DropDownList>
                     </td></tr>
             


             </table>
             <hr /><hr />
             
             <table style="width:100%">

                 <tr><td style="height: 26px;width:10%;padding-bottom:15px">Period in Days</td><td style="padding-bottom:15px">
                     <asp:TextBox ID="cmbDays" runat="server"  min="1" required="required" title="Please Days" type="number"  Width="90%" ></asp:TextBox>
                     </td> </tr>
                 <tr><td>Currency</td><td style="width: 683px;padding-bottom:15px">
                     <asp:DropDownList ID="cmbCurrency" runat="server" class="form-control select2" Width="90%"></asp:DropDownList>
                     </td> </tr>
                <tr><td  style="padding-bottom:15px"></td> <td style="height: 26px;width:20%;padding-bottom:15px"><asp:Button ID="btnOk" runat="server" CssClass="btn btn-primary btn-block btn-flats" Height="40px" Text="Get Details" Width="150px" /></td></tr>
             </table> 
             </center>

                      </div>
         </div>

                 <div class="box box-info">
                <div class="box-header with-border">
                  <h3 class="box-title">Deal Maturities</h3> 
                     
                </div><!-- /.box-header -->
                <!-- form start -->
             
                  <div class="box-body">
                 
<table style="width:100%">

                 <tr><td colspan="3">
                    <asp:DropDownList ID="cmbCustomer" runat="server" class="form-control select2" Width="90%" AutoPostBack="True"><asp:ListItem Text="Please Select Customer" Value="0"></asp:ListItem></asp:DropDownList>
                 <hr /> </td></tr>    <tr><td style="height: 26px;width:20%;padding-bottom:10px">
Customer Name
                     </td><td>
                         <asp:Label ID="lblCustomerName" runat="server" Text="" BackColor="#66FFFF"></asp:Label>
                          </td>
                       
                                   </tr>
               <tr><td style="padding-bottom:10px">

                     Customer Number</td><td>
                         <asp:Label ID="lblCustomerNumber" runat="server" Text="" BackColor="#66FFFF"></asp:Label>
                          </td>
                  </tr> <tr>
                         <td colspan="2"><center>
                             <asp:Button ID="btnGetDetails" runat="server" CssClass="btn btn-primary btn-block btn-flats" Height="40px" Text="Get Details" Width="150px" /></center>
                         </td>
                                   </tr>
               
             </table>
            </div></div></div>

          <div class="example-modal1 modal" id="customer" >
                        <div class="modal-dialog" >
                            <div class="modal-content"><div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                <h4 class="modal-title">Maturity Enquiry Customer </h4></div>
                
                             
                                <div class="modal-body">
                                       <table style="width:100%">
                                           <tr><td colspan="2"> <asp:Label ID="Label1" runat="server" Font-Size="Large" text="Number Of Deals That will Mature In This Period "></asp:Label> <asp:Label ID="lblNumberOfDeals" runat="server" Text="" Font-Size="Large" BackColor="Lime"></asp:Label><hr /></td></tr>
                                           <tr><td>Customer Name</td><td>
                                               <asp:Label ID="lblCustomerNameModal" runat="server" Text="Label" Font-Bold="True" BackColor="#66FFFF" Font-Size="Large"></asp:Label></td></tr>
                                              <tr><td>Customer Number</td><td>
                                               <asp:Label ID="lblCustomerNumberModal" runat="server" Text="Label" BackColor="#66FFFF" Font-Bold="True" Font-Size="Large"></asp:Label></td></tr>
                                           <tr><td colspan="2">    <eo:grid ID="GridCustomerDeals" runat="server" BorderColor="#C7D1DF" 
                                        BorderWidth="1px" ColumnHeaderAscImage="00050303" 
                                        ColumnHeaderDescImage="00050304" ColumnHeaderDividerImage="00050302" 
                                        FixedColumnCount="1" Font-Bold="False" Font-Italic="False" Font-Names="Verdana" 
                                        Font-Overline="False" Font-Size="9pt" Font-Strikeout="False" 
                                        Font-Underline="False" GridLineColor="199, 209, 223" GridLines="Both" 
                                        Height="400px" ItemHeight="19" Width="100%" >
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
                                            <eo:CustomColumn DataField="othercharacteristics" HeaderText="Deal Structure" 
                                                MinWidth="20" ReadOnly="True" Width="200">
                                                
                                            </eo:CustomColumn>
                                            <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="currency" 
                                                HeaderText="Currency" MinWidth="10" ReadOnly="True">
                                                
                                            </eo:CustomColumn>
                                            <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="DealReference" 
                                                HeaderText="Deal Reference" MinWidth="25" ReadOnly="True" Width="200" >
                                               
                                            </eo:CustomColumn>
                                            <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="MaturityAmount" 
                                                HeaderText="Maturity Amount" MinWidth="20" ReadOnly="True" Width="150">
                                               
                                            </eo:CustomColumn>
                                            <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="InterestRate" 
                                                HeaderText="Interest Rate%" MinWidth="20" ReadOnly="True" Width="150">
                                               
                                            </eo:CustomColumn>
                                             <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="Tenor" 
                                                HeaderText="Tenor" MinWidth="20" ReadOnly="True" Width="150">
                                               
                                            </eo:CustomColumn>
                                             <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="daystomaturity" 
                                                HeaderText="Days To Run" MinWidth="20" ReadOnly="True" Width="150">
                                               
                                            </eo:CustomColumn>
                                             <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="RolloverDeal" 
                                                HeaderText="Rollover Deal" MinWidth="20" ReadOnly="True" Width="150">
                                               
                                            </eo:CustomColumn>
                                             <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="StartDate" 
                                                HeaderText="Start Date" MinWidth="20" ReadOnly="True" Width="150">
                                               
                                            </eo:CustomColumn>
                                             <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="MaturityDate" 
                                                HeaderText="Maturity Date" MinWidth="20" ReadOnly="True" Width="150">
                                               
                                            </eo:CustomColumn>
                                           
                                        </Columns>
                                    </eo:grid>
                                 </td></tr></table>
                        </div><div class="modal-footer">
                            <button type="button" class="btn btn-default pull-left" data-dismiss="modal">Close</button></div></div></div><!-- /.modal-dialog -->

          </div>

                         <div class="col-md-7">

             <div class="box box-info">
                <div class="box-header with-border">
                  <h3 class="box-title">
                      <asp:Label ID="grpMaturities" runat="server" Text="Deal Maturities Details"></asp:Label></h3> 
                     
                </div><!-- /.box-header -->
                <!-- form start -->
             
                  <div class="box-body">

             <table style="width:100%"><tr><td>
                 <asp:Label ID="lblPeriod" runat="server"  BackColor="#66FFFF" Text=""></asp:Label>      &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                 <asp:Label ID="lblTotal" runat="server"  BackColor="#66FFFF" Text=""></asp:Label>
                 <br />
                  <eo:grid ID="GridDeposit" runat="server" BorderColor="#C7D1DF" 
                                        BorderWidth="1px" ColumnHeaderAscImage="00050303" 
                                        ColumnHeaderDescImage="00050304" ColumnHeaderDividerImage="00050302" 
                                        FixedColumnCount="1" Font-Bold="False" Font-Italic="False" Font-Names="Verdana" 
                                        Font-Overline="False" Font-Size="9pt" Font-Strikeout="False" 
                                        Font-Underline="False" GridLineColor="199, 209, 223" GridLines="Both" 
                                        Height="550px" ItemHeight="19" Width="100%" >
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
                                            <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="customernumber" 
                                                HeaderText="Customer Number" MinWidth="10" ReadOnly="True">
                                                
                                            </eo:CustomColumn>
                                            <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="DealReference" 
                                                HeaderText="Deal Reference" MinWidth="25" ReadOnly="True" Width="200" >
                                               
                                            </eo:CustomColumn>
                                            <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="othercharacteristics" 
                                                HeaderText="Deal Structure" MinWidth="20" ReadOnly="True" Width="150">
                                               
                                            </eo:CustomColumn>
                                            <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="dealAmount" 
                                                HeaderText="deal Amount" MinWidth="20" ReadOnly="True" Width="150">
                                               
                                            </eo:CustomColumn>
                                             <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="maturityamount" 
                                                HeaderText="Maturity Amount" MinWidth="20" ReadOnly="True" Width="150">
                                               
                                            </eo:CustomColumn>
                                             <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="interestrate" 
                                                HeaderText="Interest Rate" MinWidth="20" ReadOnly="True" Width="150">
                                               
                                            </eo:CustomColumn>
                                             <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="intdaysbasis" 
                                                HeaderText="Days Basis" MinWidth="20" ReadOnly="True" Width="150">
                                               
                                            </eo:CustomColumn>
                                             <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="tenor" 
                                                HeaderText="Tenor" MinWidth="20" ReadOnly="True" Width="150">
                                               
                                            </eo:CustomColumn>
                                             <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="daystomaturity" 
                                                HeaderText="Days To Run" MinWidth="20" ReadOnly="True" Width="150">
                                               
                                            </eo:CustomColumn>
                                             <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="StartDate" 
                                                HeaderText="Start Date" MinWidth="20" ReadOnly="True" Width="150">
                                               
                                            </eo:CustomColumn>
                                             <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="MaturityDate" 
                                                HeaderText="Maturity Date" MinWidth="20" ReadOnly="True" Width="150">
                                               
                                            </eo:CustomColumn>

                                                                                         <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="rolloverdeal" 
                                                HeaderText="Rollover" MinWidth="20" ReadOnly="True" Width="150">
                                               
                                            </eo:CustomColumn>
                                             <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="intaccruedtodate" 
                                                HeaderText="Accrued To Date" MinWidth="20" ReadOnly="True" Width="100">
                                               
                                            </eo:CustomColumn>
                                                                                         <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="taxrate" 
                                                HeaderText="Tax Rate" MinWidth="20" ReadOnly="True" Width="100">
                                               
                                            </eo:CustomColumn>
                                             <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="taxamount" 
                                                HeaderText="Tax Amount" MinWidth="20" ReadOnly="True" Width="100">
                                               
                                            </eo:CustomColumn>
                                                                                         <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="dealcapturer" 
                                                HeaderText="Deal Capturer" MinWidth="20" ReadOnly="True" Width="150">
                                               
                                            </eo:CustomColumn>
                                             <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="dealtype" 
                                                HeaderText="Deal Code" MinWidth="20" ReadOnly="True" Width="100">
                                               
                                            </eo:CustomColumn>
                                            <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="currency" 
                                                HeaderText="currency" MinWidth="20" ReadOnly="True" Width="100">
                                               
                                            </eo:CustomColumn>
                                        </Columns>
                                    </eo:grid>
                                     </td></tr></table>    
</div></div></div>
         </ContentTemplate>
             </asp:UpdatePanel>
         </form>
</asp:Content>
