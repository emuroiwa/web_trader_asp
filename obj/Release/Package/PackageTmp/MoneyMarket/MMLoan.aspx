<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Main.Master" CodeBehind="MMLoan.aspx.vb" Inherits="WEBTDS.MMLoan" %>

<%@ Register Assembly="EO.Web" Namespace="EO.Web" TagPrefix="eo" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<head runat="server" />
    <script>
        function OnItemSelected(grid) {
            //Get the selected item
            var item = grid.getSelectedItem();


            grid.raiseItemCommandEvent(item.getIndex(), "select");
        }
        function OnItemDelete(grid, itemIndex, colIndex, commandName) {
            //Ask user to confirm the delete
            if (window.confirm("Are you sure you want to delete this row?"))
                grid.deleteItem(itemIndex);
            grid.raiseItemCommandEvent(item.getIndex(), "select");
        }
        //<!--JS_SAMPLE_END-->
    </script>
    <script>
        function MsgBox(message) {

            alert(message)
        }

    </script>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <%-- <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>--%>
        <asp:Label ID="lblError" runat="server"></asp:Label>
        <asp:Label ID="lblsuccess" runat="server"></asp:Label>
                  <asp:Label ID="lblwarning" runat="server"> </asp:Label>
        <ajaxToolkit:TabContainer ID="tabLoan" runat="server" ActiveTabIndex="0" Width="100%">
            <ajaxToolkit:TabPanel runat="server" HeaderText="TabPanel1" ID="TabPanel1">
                               <HeaderTemplate >&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Product / Client&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</HeaderTemplate>

               <%-- <asp:UpdatePanel ID="UpdatePanel1" runat="server"></asp:UpdatePanel>--%>
                <ContentTemplate>
                    <table style="width:100%;">
                        <tr>
                            <td style="width: 63px">Customer</td>
                            <td>
                                <asp:DropDownList ID="cmbCustomerLoan" runat="server" class="form-control select2" Width="80%" Font-Size="Small"  AutoPostBack="true">
                                    <asp:ListItem Text="Select Customer" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="width: 120px">Product Portifolio</td>

                            <td><button class="btn btn-block btn-primary disabled" style="width:50%"><asp:Label ID="lblportfolioLoan" runat="server"></asp:Label></button></td>
                        </tr> 
                         <tr>   
                            <td style="width: 63px">Funding Account</td>
                            <td>
                                <asp:TextBox ID="txtDealAccountLoan" runat="server" Enabled="False" Width="80%"></asp:TextBox>
                             </td>
                            <td style="width: 120px">Product</td>
                            <td>
                                        <button class="btn btn-block btn-primary disabled" style="width:50%"><asp:Label ID="lblDescription" runat="server"></asp:Label>             <asp:Label ID="lblDealtypeLoan" runat="server" Font-Bold="True"></asp:Label></button>
                                         
                                     </td>
                                 </tr>
                             </caption>
                        </tr>
                        <tr>
                                                        <td style="width: 63px">&nbsp;</td>

                            <td>
                                <button type="button" class="btn btn-success"><asp:Label ID="txtCurrency" runat="server" Font-Bold="True"></asp:Label></button>
                          <button type="button" class="btn btn-warning"><asp:Label ID="txtIntDays" runat="server" Font-Bold="True" Text="360"></asp:Label></button>
                                      </td>
                            <td style="width: 120px">Receiving Acount</td>
                            <td>
                                <asp:TextBox ID="txtFundingLoan" runat="server" Width="50%" Enabled="False"></asp:TextBox>
                                                        </td>
                        </tr>
                        <tr>
                                                        <td style="width: 63px">&nbsp;</td>

                            <td>&nbsp;</td>
                            <td style="width: 120px">Deal Number</td>
                            <td><button class="btn btn-block btn-primary disabled"  style="width:50%"><asp:Label ID="lblDealNumber" runat="server"></asp:Label></button></td>
                        </tr>
                         <tr>
                                                        <td style="width: 63px">
                                                            <asp:Label ID="txtDashRef" runat="server"></asp:Label>
                                                        </td>

                            <td>
                                <asp:Label ID="txtRollRef" runat="server"></asp:Label>
                                                        </td>
                            <td style="width: 120px"></td>
                            <td></td>
                        </tr>
                    </table>
                    <hr>
                    <asp:UpdatePanel ID="UpdatePanelClient" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="True" >
            <ContentTemplate>
                <fieldset title="Security">
                       <div class="panel box box-success">
                      <div class="box-header with-border">
                        <h4 class="box-title">
                          <a data-toggle="collapse" data-parent="#accordion" href="#history1">
                            Address Details
                          </a>
                        </h4>
                      </div>
                      <div id="history1" class="panel-collapse collapse">
                        <div class="box-body">
                            <asp:Label ID="lblAddress" runat="server" ></asp:Label><br />
                            Customer Age<asp:Label ID="custAge" runat="server" Text=" "></asp:Label>
                        </div>
                      </div>
                    </div>
                        <div class="panel box box-success">
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
                                   <center>
                                     
  <eo:grid ID="GridClientHistory" runat="server" BorderColor="#C7D1DF" 
                                        BorderWidth="1px" ColumnHeaderAscImage="00050303" 
                                        ColumnHeaderDescImage="00050304" ColumnHeaderDividerImage="00050302" 
                                        FixedColumnCount="1" Font-Bold="False" Font-Italic="False" Font-Names="Verdana" 
                                        Font-Overline="False" Font-Size="9pt" Font-Strikeout="False" 
                                        Font-Underline="False" GridLineColor="199, 209, 223" GridLines="Both" 
                                        Height="150px" ItemHeight="19" Width="55%"  ClientSideOnItemCommand="OnItemDelete"><FooterStyle CssText="padding-bottom:4px;padding-left:4px;padding-right:4px;padding-top:4px;" /><ColumnHeaderStyle CssText="background-image:url('00050301');padding-left:8px;padding-top:2px;font-weight: bold;color:white;" /><ItemStyles><eo:GridItemStyleSet><ItemStyle CssText="background-color: white" /><ItemHoverStyle CssText="background-image: url(00050206); background-repeat: repeat-x" /><SelectedStyle CssText="background-image: url(00050207); background-repeat: repeat-x" /><CellStyle CssText="padding-left:8px;padding-top:2px; color:#336699;white-space:nowrap;" /></eo:GridItemStyleSet></ItemStyles><Columns><eo:RowNumberColumn Width="30"></eo:RowNumberColumn><eo:CustomColumn DataField="dealreference" HeaderText="Deal Reference" 
                                                MinWidth="20" ReadOnly="True" Width="200"></eo:CustomColumn><eo:CustomColumn AllowResize="True" AllowSort="True" DataField="tenor" 
                                                HeaderText="Tenor" MinWidth="10" ReadOnly="True"></eo:CustomColumn><eo:CustomColumn AllowResize="True" AllowSort="True" DataField="dealamount" 
                                                HeaderText="Deal Amount" MinWidth="25" ReadOnly="True" Width="200"></eo:CustomColumn><eo:CustomColumn AllowResize="True" AllowSort="True" DataField="startdate" 
                                                HeaderText="Start Date" MinWidth="20" ReadOnly="True" Width="150"></eo:CustomColumn><eo:CustomColumn AllowResize="True" AllowSort="True" DataField="maturitydate" 
                                                HeaderText="Maturity Date" MinWidth="20" ReadOnly="True" Width="150"></eo:CustomColumn></Columns></eo:grid>
                                       </center>
                        </div>
                      </div>
                    </div> </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="cmbCustomerLoan" EventName="SelectedIndexChanged" />
            </Triggers>
        </asp:UpdatePanel>
                        
                         </ContentTemplate>
            </ajaxToolkit:TabPanel>
            <ajaxToolkit:TabPanel runat="server" HeaderText="TabPanel2" ID="TabPanel2">
                            <HeaderTemplate>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Details&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </HeaderTemplate>

              
                <ContentTemplate>
                     <asp:UpdatePanel ID="UpdatePanelAll" runat="server"><ContentTemplate>
                    <center><table style="width: 90%; height: 178px; table-layout: auto;">
                        <tr>
                            <td style="width: 291px; padding-bottom:20px" >
                             Start Date
                            </td>
                            <td style="width: 492px">
                                <asp:TextBox ID="StartDateLoan" runat="server" Width="75%" Font-Size="Small" required="required" title="Please Enter Start Date"></asp:TextBox>
                                <ajaxToolkit:CalendarExtender ID="StartDateLoan_CalendarExtender" runat="server" BehaviorID="TextBox6_CalendarExtender" DaysModeTitleFormat="dd/MM/yyyy" TargetControlID="StartDateLoan" />
                            </td>

                        <td style="width: 168px; ">
                            <asp:Label ID="Label19" runat="server" Font-Size="Small" Text="Maturity Date" Width="75%"></asp:Label>
                        </td>
                        <td style="width: 369px; ">
                            <asp:TextBox ID="MaturityDateLoan" runat="server" Width="75%" required="required" title="Please Enter Maturity Date" AutoPostBack="True"></asp:TextBox>
                            <ajaxToolkit:CalendarExtender ID="MaturityDateLoan_CalendarExtender" runat="server" BehaviorID="TextBox15_CalendarExtender" DaysModeTitleFormat="dd/MM/yyyy" TargetControlID="MaturityDateLoan"   />
                        </td>
                        <tr>
                            <td style="width: 291px; padding-bottom:20px" ; >
                                <asp:Label ID="Label15" runat="server" Font-Size="Small" Text="Present Value"></asp:Label>
                            </td>
                            <td style="width: 492px">
                                <asp:TextBox ID="txtDealAmount" runat="server" required="required" title="Please Enter Present value" style="margin-left: 0px" Width="75%"></asp:TextBox>
                            </td>
                            <td style="width: 168px">
                                <asp:Label ID="Label20" runat="server" Font-Size="Small" Text="Future Value"></asp:Label>
                            </td>
                            <td style="width: 369px">
                                <asp:TextBox ID="txtMaturityAmountLoan" runat="server" Width="75%" Enabled="False" ></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 28px; width: 291px; padding-bottom:20px">
                                <asp:Label ID="Label16" runat="server" Font-Size="Small" Text="Interest Rate"></asp:Label>
                            </td>
                            <td style="width: 492px; height: 28px">
                                <asp:TextBox ID="txtInterestRateLoan" Width="75%"  required="required" title="Please Enter Interest Rate" runat="server"></asp:TextBox>
                            </td>
                            <td style="height: 28px; width: 168px;">
                                <asp:Label ID="Label21" runat="server" Font-Size="Small" Text="Net Interest"></asp:Label>
                            </td>
                            <td style="height: 28px; width: 369px;">
                                <asp:TextBox ID="txtNetInterestLoan" runat="server" Width="75%" Enabled="False" ></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 291px; padding-bottom:20px; height: 38px;">
                                <asp:Label ID="Label17" runat="server" Font-Size="Small" Text="Tenor"></asp:Label>
                            </td>
                            <td style="width: 492px; height: 38px;">
                                <asp:TextBox ID="txtTenorLoan" runat="server"  type="number" min="1" Width="75%"  AutoPostBack="True" ></asp:TextBox>
                            </td>
                            <td style="width: 168px; height: 38px;">
                                <asp:Label ID="Label22" runat="server" Font-Size="Small" Text="Gross Interest"></asp:Label>
                            </td>
                            <td style="width: 369px; height: 38px;">
                                <asp:TextBox ID="txtGrossLoan" runat="server" Enabled="False" Width="75%" ></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 291px; ">
                                &nbsp;</td>
                            <td style="width: 492px">
                                <asp:TextBox ID="txtTaxRateL" runat="server" Enabled="False" min="1" required="required" title="Please Enter Deal Tenor" type="number" Visible="False" Width="75%" Font-Size="Small">0</asp:TextBox>
                            </td>
                            <td style="width: 168px">
                                <asp:Label ID="Label23" runat="server" Font-Size="Small" Text="Total Security"></asp:Label>
                            </td>
                            <td style="width: 369px">
                                <asp:TextBox ID="txtTotalSecurityLoan" runat="server" Enabled="False" Width="75%" ></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 291px ; padding-bottom:20px">
                                <asp:Label ID="Label25" runat="server" Font-Size="Small" Text="Secured"></asp:Label>
                            </td>
                            <td style="width: 492px">
                                <%--<asp:DropDownList ID="cmbSecureLoan" runat="server" Font-Size="Small">
                                    <asp:ListItem></asp:ListItem>
                                    <asp:ListItem>Yes</asp:ListItem>
                                    <asp:ListItem>No</asp:ListItem>
                                </asp:DropDownList>--%>
                            </td>
                            <td style="width: 168px">
                                <asp:Label ID="Label24" runat="server" Font-Size="Small" Text="Tax Amount"></asp:Label>
                            </td>
                            <td style="width: 369px">
                                <asp:TextBox ID="txtTaxAmountL" runat="server" Enabled="False" Width="75%" ></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td colspan="4"><hr />
                                <asp:Label ID="Label1" runat="server" Font-Size="Small" Text="Secure using"></asp:Label>
                                <a data-target="#AttachSecurity" data-toggle="modal">
                                <button class="btn btn-success ">
                                   Deals
                                </button>
                                </a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Label ID="Label2" runat="server" Font-Size="Small" Text="Or  using"></asp:Label>
                                <a data-target="#AttachSecurity2" data-toggle="modal">
                                <button class="btn btn-info">
                                     Other type of security
                                </button>
                                </a></td>
                        </tr>
                    </caption>
                    </td></td>
                        
                    </table>
                         </ContentTemplate></asp:UpdatePanel>
                    <div class="example-modal1 modal" id="AttachSecurity2">
                        <div class="modal-dialog" >
                            <div class="modal-content"><div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                <h4 class="modal-title">Attach Other Security</h4></div>
                                <div class="modal-body"><p><asp:Label ID="lblModalError" runat="server"></asp:Label><asp:UpdatePanel ID="UpdatePanelcmbSecurity" UpdateMode="Conditional" runat="server"><ContentTemplate>
                                    <fieldset title="Security">
                                    <table style="width: 100%; height: 178px; table-layout: auto;">
                                    <tr><td colspan="2">
          <asp:DropDownList ID="cmbSecurity" runat="server" class="form-control select2" Width="100%" AutoPostBack="True"><asp:ListItem Text="Please Select Security" Value="0"></asp:ListItem></asp:DropDownList><hr />

                                        </td></tr>
                                        <tr><td style="width:30%">Currency</td>
                                        <td style="width:70%; padding-bottom:10px ">
                            <asp:TextBox ID="txtCollCurrency" runat="server" Width="70%" Enabled="False"></asp:TextBox></td></tr>
                                        <tr><td style="width:30%">Colletral Value</td>
                                            <td style ="width:70%; padding-bottom:10px ">
                                                
                <asp:TextBox ID="txtValue" runat="server"  Width="70%" Enabled="False"></asp:TextBox>

                                            </td></tr>
                                        <tr><td style="width:30%">Expiry Date</td><td style="width:70%; padding-bottom:10px ">
         <asp:TextBox ID="txtExpiry" runat="server"  Width="70%" Enabled="False"></asp:TextBox><ajaxToolkit:CalendarExtender ID="txtExpiry_CalendarExtender" runat="server" BehaviorID="txtExpiry_CalendarExtender" TargetControlID="txtExpiry" />
 </td></tr>
                                        <tr><td style="width:30%">Amount Availble To Secure</td><td style="width:70%; padding-bottom:10px ">
                         <asp:TextBox ID="txtAvailble" runat="server"  Width="70%" Enabled="False"></asp:TextBox>
                          </td></tr>
                                        <tr><td style="width:30%">Security Amount</td><td style="width:70%; padding-bottom:10px ">
                   <asp:TextBox ID="txtCollateralLoan" runat="server"   Width="70%"></asp:TextBox></td></tr>
                                        <tr><td colspan="2"><asp:Button ID="btnAddCollateralLoan" runat="server" Text="Add" class="btn btn-block btn-success "></asp:Button></td>

                                        </tr></table><hr /></fieldset></ContentTemplate>
                                    <Triggers><asp:AsyncPostBackTrigger ControlID="cmbSecurity" EventName="SelectedIndexChanged" />

                                    </Triggers></asp:UpdatePanel><asp:Label ID="lblAdd" runat="server"></asp:Label>
                        </div><div class="modal-footer">
                            <button type="button" class="btn btn-default pull-left" data-dismiss="modal">Close</button></div></div></div><!-- /.modal-dialog --></div>

                    <div class="example-modal1 modal" id="AttachSecurity">
                        <div class="modal-dialog" >
                            <div class="modal-content"><div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                <h4 class="modal-title">Attach Deals As Security </h4></div>
                                <div class="modal-body"><p><asp:Label ID="Label3" runat="server"></asp:Label><asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Conditional" runat="server"><ContentTemplate>
                                    <fieldset title="Security">
                                    <table style="width: 100%; height: 178px; table-layout: auto;">
                                    <tr><td colspan="2">
                                    <asp:DropDownList ID="cmbTB" runat="server" class="form-control select2" Width="100%" AutoPostBack="True"><asp:ListItem Text="Please Select Security" Value="0"></asp:ListItem></asp:DropDownList><hr /></td></tr>
                                        <tr><td style="width:30%">Currency</td>
                                        <td style="width:70%; padding-bottom:10px "><asp:TextBox ID="lblcurrency1" runat="server" Width="70%" Enabled="false"></asp:TextBox></td></tr>
                                        <tr><td style="width:30%">Purchase Value</td>
                                            <td style ="width:70%; padding-bottom:10px "><asp:TextBox ID="PurValue" runat="server"  Width="70%" Enabled="false"></asp:TextBox></td></tr>
                                        <tr><td style="width:30%">Interest Rate</td>
                                            <td style ="width:70%; padding-bottom:10px "><asp:TextBox ID="IntRate" runat="server"  Width="70%" Enabled="false"></asp:TextBox></td></tr>
                                         <tr><td style="width:30%">Days To Maturity</td><td style="width:70%; padding-bottom:10px ">
                           <asp:TextBox ID="DaysMaturity" runat="server"  Width="70%"></asp:TextBox></td></tr>
                                        <tr><td style="width:30%">Avaliable To Sale</td><td style="width:70%; padding-bottom:10px ">
                           <asp:TextBox ID="AvaliableForSale" runat="server"  Width="70%"></asp:TextBox></td></tr>

                                        <tr><td style="width:30%">Maturity Date</td><td style="width:70%; padding-bottom:10px ">
                                            <asp:TextBox ID="txtMaturityDate" runat="server"  Enabled="false" Width="70%" ></asp:TextBox></td></tr>
                                        <tr><td style="width:30%">Security Amount</td><td style="width:70%; padding-bottom:10px "><asp:TextBox ID="txtSale" runat="server"   Width="70%"></asp:TextBox></td></tr>
                                        <tr><td colspan="2"><asp:Button ID="cmdSale" runat="server" Text="Add" class="btn btn-block btn-success "></asp:Button></td>

                                        </tr></table><hr /></fieldset></ContentTemplate>
                                    <Triggers><asp:AsyncPostBackTrigger ControlID="cmbTB" EventName="SelectedIndexChanged" />

                                    </Triggers></asp:UpdatePanel>
                        </div><div class="modal-footer">
                            <button type="button" class="btn btn-default pull-left" data-dismiss="modal">Close</button></div></div></div><!-- /.modal-dialog --></div>
                    <hr><div class="panel box box-success">
                        <div class="box-header with-border"><h4 class="box-title"><a data-parent="#accordion" data-toggle="collapse" href="#collapseTwo">Security Details </a></h4></div>
                        <div id="collapseTwo" class="panel-collapse collapse-in"><div class="box-body">
                          <center>   <asp:UpdatePanel ID="UpdatePanelr" runat="server"><ContentTemplate><fieldset title="Security">
                                  <eo:CallbackPanel ID="CallbackPanel2" runat="server" Height="150px" Width="1287px" Triggers="{ControlID:txtdealref;Parameter:}">
                                 <eo:grid ID="GridSecurity" runat="server" BorderColor="#C7D1DF" 
                                        BorderWidth="1px" ColumnHeaderAscImage="00050303" 
                                        ColumnHeaderDescImage="00050304" ColumnHeaderDividerImage="00050302" 
                                        FixedColumnCount="1" Font-Bold="False" Font-Italic="False" Font-Names="Verdana" 
                                        Font-Overline="False" Font-Size="9pt" Font-Strikeout="False" 
                                        Font-Underline="False" GridLineColor="199, 209, 223" GridLines="Both" 
                                        Height="150px" ItemHeight="19" Width="55%" OnItemDeleted="GridSecurity_ItemDeleted">
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
                                          
                                            <eo:CustomColumn DataField="id" HeaderText="ID" 
                                                MinWidth="20" ReadOnly="True" Width="200">
                                                
                                            </eo:CustomColumn>
                                            <eo:CustomColumn DataField="security" HeaderText="Security" 
                                                MinWidth="20" ReadOnly="True" Width="200">
                                                
                                            </eo:CustomColumn>
                                            <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="dealref" 
                                                HeaderText="Deal Reference" MinWidth="10" ReadOnly="True">
                                                
                                            </eo:CustomColumn>
                                            <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="amount" 
                                                HeaderText="Amount" MinWidth="25" ReadOnly="True" Width="200">
                                               
                                            </eo:CustomColumn>
                                            <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="expirydate" 
                                                HeaderText="Expiry Date" MinWidth="20" ReadOnly="True" Width="150">
                                               
                                            </eo:CustomColumn>
                                            
                                            <eo:DeleteCommandColumn Name="Delete">
                                            </eo:DeleteCommandColumn>
                                            
                                        </Columns>
                                    </eo:grid>
                                       </center><asp:Label ID="lblTotalSecurity" runat="server" ForeColor="#009900" Font-Bold="True" BackColor="Yellow"></asp:Label>
                                    
                                      <asp:TextBox ID="txtdealref" runat="server" AutoPostBack="true" Enabled="False"  BorderStyle="None"  BackColor="White" ForeColor="White" BorderColor="White"></asp:TextBox>
                                        </eo:CallbackPanel>
                                                                                                                        </fieldset></ContentTemplate></asp:UpdatePanel></center>
                                                                                                  </div></div></div>
                        </center></ContentTemplate>


            </ajaxToolkit:TabPanel>
            <ajaxToolkit:TabPanel ID="TabPanel5" runat="server" HeaderText="TabPanel5">
                             <HeaderTemplate>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Instructions&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</HeaderTemplate>

                
                <ContentTemplate>
                    
                    <table style="width:100%;" >
                        <tr>
                             <td style="width:50%;">
    <table style="width:100%; height:100%"  >
                        <tr>

                             <td style="width: 103px; padding-bottom:10px" >
                                 <asp:Label ID="Label26" runat="server" Text="Deal Inception"></asp:Label>
                             </td>
                            <td>
                                <asp:DropDownList ID="cmbInstructionLoan" runat="server" Width="80%">
                                    <asp:ListItem Text="Select Deal Inception Instruction" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                             </td>
                             
                        </tr>
           <tr>
                             <td style="width: 103px">&nbsp;</td>
                            <td>
                                <asp:TextBox ID="txtOtherLoan" runat="server" TextMode="MultiLine" Height="150px" Width="80%"></asp:TextBox>
                             </td>
                             
                        </tr>
         <tr>
            
                             <td style="width: 103px;padding-top:35px">
                                 <asp:Label ID="Label27" runat="server" Text="Deal Maturity"></asp:Label>
                             </td>
                            <td>
                                <asp:DropDownList ID="cmbInstructionMaturityL" runat="server"  Width="80%" Font-Size="Small">
                                    <asp:ListItem Text="Select Deal Maturity Instruction" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                             </td>
                             
                        </tr>
           <tr>
                             <td style="width: 103px">&nbsp;</td>
                            <td>
                                <asp:TextBox ID="txtOtherMaturity" runat="server" TextMode="MultiLine" Height="150px" Width="80%"></asp:TextBox>
                             </td>
                             
                        </tr>
                           </table>


                             </td>
                                                    <td style="width:30%;" >
    <table style="width:100%;" >
                        <tr>                             
                  <td style="align-content:center ;padding-bottom:10px"   >
                      <asp:Button ID="btnValidate" runat="server" Text="Test"  CssClass="btn btn-primary btn-block btn-flats" Height="40px" Width="150px" />
                            </td>
                        </tr>
                        <tr>      
                 <td style="align-content:center;padding-bottom:10px">
                     <asp:Button ID="btnIntDays" runat="server" Text="Int Day Basis"  CssClass="btn btn-primary btn-block btn-flats" Height="40px" Width="150px" />
                            </td>                            
                        </tr>
                        <tr>                             
                  <td style="align-content:center ;padding-bottom:10px">
                      <asp:Button ID="btnSaveLoan" runat="server" CssClass="btn btn-success btn-block btn-flats" Height="40px" Text="Deal" Width="150px" />
                            </td>
                        </tr>
                        <tr>      
                 <td style="align-content:center ;padding-bottom:10px">
                     <asp:Button ID="btnReset" runat="server" CssClass="btn btn-warning btn-block btn-flats" Height="40px" Text="Reset" Width="150px" />
                            </td>                            
                        </tr>
                        <tr>                             
                  <td style="align-content:center;padding-bottom:10px">
                      <asp:Button ID="cmdCancelLoan" runat="server" CssClass="btn btn-danger btn-block btn-flats" Height="40px" Text="Cancel Deal" Width="150px" />
                            </td>
                        </tr>
                        <tr>      
                 <td style="align-content:center ;padding-bottom:10px">
                     <asp:Button ID="cmdExit" runat="server" CssClass="btn btn-danger btn-block btn-flats" Height="40px" Text="Exit" Width="150px" />
                            </td>                            
                        </tr>
                        <tr>                             
                  <td style="align-content:center;padding-bottom:10px">
                      &nbsp;</td>
                        </tr>
                        <tr>      
                 <td style="align-content:center;padding-bottom:10px">
                     &nbsp;</td>                            
                        </tr>
                           </table>
                        </tr>
                        
                    </table>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>
        </ajaxToolkit:TabContainer>
          <%-- </ContentTemplate>
        </asp:UpdatePanel>--%>
    </form>
   </head>

   
   <%-- <script>function PopupPicker(ctl) {

    var PopupWindow = null;

    PopupWindow = window.open('DatePicker.aspx?ctl=' + ctl, '', 'width=250,height=250');

    PopupWindow.focus();

}</script>
   

    <script>function SetDate(dateValue, ctl) {

    thisForm = window.opener.document.forms[0].elements[ctl].value = dateValue;

    self.close();

}</script>--%>

</asp:Content>
