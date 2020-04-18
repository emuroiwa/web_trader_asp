<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Main.Master" CodeBehind="MaturedDeposit.aspx.vb" Inherits="WEBTDS.MaturedDeposit" %>

<%@ Register Assembly="EO.Web" Namespace="EO.Web" TagPrefix="eo" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<head   />
    <script>
        function DeleteUsers(id) {

            window.location = "delete.aspx?id=" + id
        }

     

             function getdate(days, dateText) {

                 var dateStr = dateText;
                 var millis = Date.parse(dateStr);
                 var newDate = new Date();
                 newDate.setTime(millis + days * 24 * 60 * 60 * 1000);
                 var newDateStr = "" + (newDate.getMonth() + 1) + "/" + newDate.getDate() + "/" + newDate.getFullYear();
                 return newDateStr;
             }
        function getDateDiff(time1, time2) {
            var date1 = new Date(time1);
            var date2 = new Date(time2);
            var timeDiff = Math.abs(date2.getTime() - date1.getTime());
            var diffDays = Math.ceil(timeDiff / (1000 * 3600 * 24));

            return diffDays;
        }
        function date() {
            var tenor = document.getElementById("<%= txtTenorDeposit.ClientID%>").value;
               var startdate = document.getElementById("<%= StartDateDeposit.ClientID%>").value;
            document.getElementById("<%= MaturityDateDeposit.ClientID %>").value = getdate(tenor,startdate);

               return false;
           } function days() {
               var tenor = document.getElementById("<%= txtTenorDeposit.ClientID%>").value;
               var startdate = document.getElementById("<%= StartDateDeposit.ClientID%>").value;
               var enddate = document.getElementById("<%= MaturityDateDeposit.ClientID%>").value;
               document.getElementById("<%= txtTenorDeposit.ClientID%>").value = getDateDiff(startdate, enddate)

               return false;
           }

        
    </script>

    <%-- Label for alerts --%>

    <style>
        .example-modal1 .modal {
            width: 100%;
            position: relative;
            top: auto;
            bottom: auto;
            right: auto;
            left: auto;
            display: block;
            z-index: 1;
        }

        .example-modal1 .modal {
            width: 100%;
            background: transparent !important;
        }
    </style>
    <%-- end of Label for alerts --%>        <%-- Tab --%>

    <form id="form1" runat="server">
        <%-- Label for alerts --%>

        <asp:Label ID="lblError" runat="server" Text=""></asp:Label>
        <asp:Label ID="lblWarning" runat="server" Text=""></asp:Label>
        <asp:Label ID="lblSuccess" runat="server" Text=""></asp:Label>
        <%-- end of Label for alerts --%>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <%-- Tab --%>
        <ajaxToolkit:TabContainer ID="tabMaturedDeposit" runat="server" ActiveTabIndex="1" Width="100%">
            <ajaxToolkit:TabPanel runat="server" HeaderText="TabPanel1" ID="TabPanel1">
                <HeaderTemplate >&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Product / Client&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</HeaderTemplate>



                <ContentTemplate>
                    <table style="width: 100%;">
                        <tr>
                            <td style="width: 63px">Customer</td>
                            <td>
                                <asp:DropDownList ID="cmbCustomer" runat="server" class="form-control select2" Width="80%" AutoPostBack="True">
                                    <asp:ListItem Text="Please Select Customer" Value="0"></asp:ListItem>
                                </asp:DropDownList></td>
                            <td style="width: 120px">Product Portifolio</td>
                            <td>
                                <button class="btn btn-block btn-primary disabled"  style="width:50%">
                                    <asp:Label ID="lblProduct" runat="server"></asp:Label></button></td>
                        </tr>
                        <tr>
                            <td style="width: 63px">Funding Account</td>
                            <td>
                                <asp:TextBox ID="txtDealAccountDeposit" runat="server" Enabled="False" Width="80%"></asp:TextBox></td>
                            <td style="width: 120px">Product</td>
                            <td>
                                <button class="btn btn-block btn-primary disabled" style="width:50%">
                                    <asp:Label ID="lblPortfolioDeposit" runat="server"></asp:Label><asp:Label ID="lblDealtypeDeposit" runat="server" Font-Bold="True"></asp:Label></button></td>
                        </tr>
                        </caption></tr><tr>
                            <td style="width: 63px">&nbsp;</td>
                            <td>
                                <button type="button" class="btn btn-success">
                                    <asp:Label ID="lblCurrency" runat="server" Font-Bold="True"></asp:Label></button>
                                <button type="button" class="btn btn-warning">
                                        <asp:Label ID="lblDays" runat="server" Font-Bold="True" Text="360"></asp:Label></button></td>
                            <td style="width: 120px">Receiving Acount</td>
                            <td>
                                <asp:TextBox ID="txtRecievingDeposit" runat="server" Width="50%" Enabled="False"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td style="width: 63px">&nbsp;</td>
                            <td>&nbsp;</td>
                            <td style="width: 120px">&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                    </table>
                    <hr>
                    <asp:UpdatePanel ID="UpdatePanelClient" UpdateMode="Conditional" runat="server">
                        <ContentTemplate>
                            <fieldset title="Security">
                                <div class="panel box box-success">
                                    <div class="box-header with-border">
                                        <h4 class="box-title"><a data-toggle="collapse" data-parent="#accordion" href="#history1">Address Details </a></h4>
                                    </div>
                                    <div id="history1" class="panel-collapse collapse">
                                        <div class="box-body">
                                            <asp:Label ID="lblAddress" runat="server"></asp:Label><br />
                                            Customer Age<asp:Label ID="custAge" runat="server" Text=" "></asp:Label></div>
                                    </div>
                                </div>
                                <div class="panel box box-success">
                                    <div class="box-header with-border">
                                        <h4 class="box-title"><a data-toggle="collapse" data-parent="#accordion" href="#history">Client History </a></h4>
                                    </div>
                                    <div id="history" class="panel-collapse collapse in">
                                        <div class="box-body">
                                            <button class="btn btn-success" type="button">Avg Rate
                                                <asp:Label ID="avgRate" runat="server" Text="Label"></asp:Label></button>
                                            <button class="btn btn-success" type="button">Avg Size
                                                    <asp:Label ID="avgSize" runat="server"></asp:Label></button>
                                            <button class="btn btn-success" type="button">Avg Tenor
                                                        <asp:Label ID="avgTenor" runat="server"></asp:Label></button><hr />
                                           
  <eo:grid ID="GridClientHistory" runat="server" BorderColor="#C7D1DF" 
                                        BorderWidth="1px" ColumnHeaderAscImage="00050303" 
                                        ColumnHeaderDescImage="00050304" ColumnHeaderDividerImage="00050302" 
                                        FixedColumnCount="1" Font-Bold="False" Font-Italic="False" Font-Names="Verdana" 
                                        Font-Overline="False" Font-Size="9pt" Font-Strikeout="False" 
                                        Font-Underline="False" GridLineColor="199, 209, 223" GridLines="Both" 
                                        Height="150px" ItemHeight="19" Width="55%">
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
                                            <eo:CustomColumn DataField="dealreference" HeaderText="Deal Reference" 
                                                MinWidth="20" ReadOnly="True" Width="200">
                                                
                                            </eo:CustomColumn>
                                            <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="tenor" 
                                                HeaderText="Tenor" MinWidth="10" ReadOnly="True">
                                                
                                            </eo:CustomColumn>
                                            <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="dealamount" 
                                                HeaderText="Deal Amount" MinWidth="25" ReadOnly="True" Width="200">
                                               
                                            </eo:CustomColumn>
                                            <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="startdate" 
                                                HeaderText="Start Date" MinWidth="20" ReadOnly="True" Width="150">
                                               
                                            </eo:CustomColumn>
                                            <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="maturitydate" 
                                                HeaderText="Maturity Date" MinWidth="20" ReadOnly="True" Width="150">
                                               
                                            </eo:CustomColumn>
                                        </Columns>
                                    </eo:grid>
                                      </div>
                                    </div>
                                </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="cmbCustomer" EventName="SelectedIndexChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                </ContentTemplate>









            </ajaxToolkit:TabPanel>
            <ajaxToolkit:TabPanel runat="server" HeaderText="TabPanel2" ID="TabPanel2">
                
                <HeaderTemplate >&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Details&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</HeaderTemplate>



                <ContentTemplate>
                    <center><table style="width: 90%; height: 178px; table-layout: auto;"><tr><td style="width: 291px; padding-bottom:20px" ><asp:Label ID="Label14" runat="server" Font-Size="Small" Text="Start Date"></asp:Label></td><td style="width: 492px">
    <asp:TextBox ID="StartDateDeposit" runat="server" Width="75%"></asp:TextBox><ajaxToolkit:CalendarExtender ID="StartDateDeposit_CalendarExtender" runat="server" BehaviorID="TextBox6_CalendarExtender" DaysModeTitleFormat="dd/MM/yyyy" TargetControlID="StartDateDeposit" /><td style="width: 168px; "><asp:Label ID="Label19" runat="server" Font-Size="Small" Text="Maturity Date" Width="75%"></asp:Label></td><td style="width: 369px; "><asp:TextBox ID="MaturityDateDeposit" runat="server" required="required" onchange="days()" Width="75%"></asp:TextBox><ajaxToolkit:CalendarExtender ID="MaturityDateDeposit_CalendarExtender" runat="server" BehaviorID="TextBox15_CalendarExtender" DaysModeTitleFormat="dd/MM/yyyy" TargetControlID="MaturityDateDeposit" /></td>
                        <tr><td ;="" style="width: 291px; padding-bottom: 20px;"><asp:Label ID="Label15" runat="server" Font-Size="Small" Text="Present Value"></asp:Label></td><td style="width: 492px"><asp:TextBox ID="txtDealAmountDeposit" runat="server" style="margin-left: 0px" title="Please Enter Present value" Width="75%"></asp:TextBox></td><td style="width: 168px"><asp:Label ID="Label20" runat="server" Font-Size="Small" Text="Future Value"></asp:Label></td><td style="width: 369px"><asp:TextBox ID="txtMaturityAmountDeposit" runat="server" Enabled="False" Text="0" Width="75%"></asp:TextBox></td></tr>
                        <tr><td style="height: 28px; width: 291px; padding-bottom:20px"><asp:Label ID="Label16" runat="server" Font-Size="Small" Text="Interest Rate"></asp:Label></td>
                            <td style="width: 492px; height: 28px"><asp:TextBox ID="txtIntRateDeposit" runat="server" type="number" min="1" required="required" title="Please Enter Interest Rate" Width="75%"></asp:TextBox></td>
                            <td style="height: 28px; width: 168px;"><asp:Label ID="Label21" runat="server" Font-Size="Small" Text="Net Interest"></asp:Label></td><td style="height: 28px; width: 369px;"><asp:TextBox ID="txtNetIntDeposit" runat="server" Enabled="False" Text="0" Width="75%"></asp:TextBox></td></tr>
                        <tr><td style="width: 291px; padding-bottom:20px"><asp:Label ID="Label17" runat="server" Font-Size="Small" Text="Tenor"></asp:Label></td>
                            <td style="width: 492px">
    <asp:TextBox ID="txtTenorDeposit"  onchange="date()" runat="server" min="1" required="required" title="Please Enter Deal Tenor" type="number" Width="75%"></asp:TextBox></td>
                            <td style="width: 168px"><asp:Label ID="Label22" runat="server" Font-Size="Small" Text="Gross Interest"></asp:Label></td>
                            <td style="width: 369px"><asp:TextBox ID="txtGrossInt" runat="server" Enabled="False" Text="0" Width="75%"></asp:TextBox></td></tr>
                        <tr><td style="width: 291px; "><asp:CheckBox ID="chkTaxStatus" runat="server" AutoPostBack="True" Text="Taxable"></asp:CheckBox></td>
        <td style="width: 492px"><asp:UpdatePanel ID="UpdatePanelTaxCode" runat="server" UpdateMode="Conditional"><ContentTemplate><asp:DropDownList ID="cmbtax1" runat="server" class="form-control select2" Enabled="False" Width="75%"><asp:ListItem Text="Zero Tax = 0" Value="0"></asp:ListItem></asp:DropDownList></ContentTemplate><triggers><asp:AsyncPostBackTrigger ControlID="chkTaxStatus" EventName="CheckedChanged"></asp:AsyncPostBackTrigger></triggers></asp:UpdatePanel><td style="width: 168px"><asp:Label ID="Label23" runat="server" Font-Size="Small" Text="Total Security"></asp:Label></td>
        <td style="width: 369px"><asp:TextBox ID="txtTotalSecurityDeposit" runat="server" Enabled="False" Text="0" Width="75%"></asp:TextBox></td>
            </td></tr><tr>
                <td style="width: 291px ; padding-bottom:20px">&nbsp;</td><td style="width: 492px">&nbsp;</td><td style="width: 168px"><asp:Label ID="Label24" runat="server" Font-Size="Small" Text="Tax Amount"></asp:Label></td>
                <td style="width: 369px"><asp:TextBox ID="txtTaxAmountDeposit" runat="server" Enabled="False" Text="0" Width="75%"></asp:TextBox></td></tr>
                        <tr><td colspan="4"><asp:CheckBox ID="chkPledge"   runat="server" Text="Pledge Deal" AutoPostBack="True" Visible="False" /></td></tr>
                        <tr><td colspan="4"><asp:CheckBox ID="chkRollover" runat="server" Text="Rollover deal at chosen frequency"  AutoPostBack="True" Visible="False"/></td></tr>
                        <tr><td colspan="4"></td></tr></td></table>
    <div class="example-modal1 modal" id="AttachSecurity">
        <div class="modal-dialog" ><div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                    <span aria-hidden="true">&times;</span></button><h4 class="modal-title">Attach Security</h4></div><div class="modal-body"><p><asp:Label ID="lblModalError" runat="server"></asp:Label><asp:UpdatePanel ID="UpdatePanelcmbSecurity" UpdateMode="Conditional" runat="server"><ContentTemplate><fieldset title="Security">
                                                                                                                                                     <table style="width: 100%; height: 178px; table-layout: auto;">
                                                                                                                                                         <tr><td colspan="2"><asp:DropDownList ID="cmbSecurity" runat="server" class="form-control select2" Width="100%" AutoPostBack="True"><asp:ListItem Text="Please Select Security" Value="0"></asp:ListItem></asp:DropDownList><hr /></td></tr>
                                                                                                                                                         <tr><td style="width:30%">Currency</td><td style="width:70%; padding-bottom:10px "><asp:TextBox ID="txtCollCurrency" runat="server" Width="70%"></asp:TextBox></td></tr><tr><td style="width:30%">Colletral Value</td><td style ="width:70%; padding-bottom:10px "><asp:TextBox ID="txtValue" runat="server"  Width="70%"></asp:TextBox></td></tr>
                                                                                                                                                         <tr><td style="width:30%">Expiry Date</td><td style="width:70%; padding-bottom:10px "><asp:TextBox ID="txtExpiry" runat="server"  Width="70%"></asp:TextBox><ajaxToolkit:CalendarExtender ID="txtExpiry_CalendarExtender" runat="server" BehaviorID="txtExpiry_CalendarExtender" TargetControlID="txtExpiry" /></td></tr>
                                                                                                                                                         <tr><td style="width:30%">Amount Availble To Secure</td><td style="width:70%; padding-bottom:10px "><asp:TextBox ID="txtAvailble" runat="server"  Width="70%"></asp:TextBox></td></tr>
                                                                                                                                                         <tr><td style="width:30%">Security Amount</td><td style="width:70%; padding-bottom:10px "><asp:TextBox ID="txtCollateralLoan" runat="server"   Width="70%"></asp:TextBox></td></tr>
                                                                                                                                                         <tr><td colspan="2"><asp:Button ID="btnAddCollateralLoan" runat="server" Text="Add" class="btn btn-block btn-success "></asp:Button></td></tr></table>
                                                                                                                                                     <hr /></fieldset></ContentTemplate>
                                                                                                                                                     <Triggers><asp:AsyncPostBackTrigger ControlID="cmbSecurity" EventName="SelectedIndexChanged" /></Triggers></asp:UpdatePanel>
 
                        </div><div class="modal-footer"><button type="button" class="btn btn-default pull-left" data-dismiss="modal">Close</button></div></div>
            <!-- /.modal-content --></div><!-- /.modal-dialog --></div>
    <hr>
                        </center>
                </ContentTemplate>



            </ajaxToolkit:TabPanel>
            <ajaxToolkit:TabPanel ID="TabPanel3" runat="server" HeaderText="TabPanel3">
                <HeaderTemplate>Rollover Details</HeaderTemplate>

                <ContentTemplate>
                    <br />
                    <br />
                    <center><table style="width:80%; padding-bottom:20px" ><tr><td style="width:50%;"><table style="width:100%; height: 100%;"><tr><td style="width:50%; padding-bottom:30px" ><center><asp:DropDownList ID="cmbIntOpt" runat="server" class="form-control select2" Width="80%"><asp:ListItem>Capitalise Interest On Rollover</asp:ListItem><asp:ListItem>Payout Interest On Rollover</asp:ListItem><asp:ListItem>No Action</asp:ListItem></asp:DropDownList></center></td>
                              
                                    </table></td><td style="width:50%;"><table style="width:100%;"><tr style="padding-bottom:15px"><td style="width:10%; padding-bottom:25px">Date</td>
                                        
                                        <td>
                                          
                                                <asp:TextBox ID="dtRollover" runat="server" Width="75%"></asp:TextBox><ajaxToolkit:CalendarExtender ID="dtRollover_CalendarExtender" runat="server" BehaviorID="dtRollover_CalendarExtender" TargetControlID="dtRollover" /></td>

                                                                                                                                                                                                                                                                                                                                                                 </tr><tr><td style="width:10%">Days</td>
                                        <td><asp:TextBox ID="rolloverDays" runat="server" min="1"  title="Please Enter Deal Tenor" type="number" Width="75%" AutoPostBack="True"></asp:TextBox></td>

                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                               </tr></table><tr><td colspan="2" style="padding-top:20px"><center><asp:Button ID="continue" runat="server" Text="Continue" CssClass="btn btn-success btn-block btn-flats" Height="40px" Width="150px"/></center></td></tr>  </tr></table></center>
                    <br />
                    <br />
                </ContentTemplate>









            </ajaxToolkit:TabPanel>
            <ajaxToolkit:TabPanel ID="TabPanel4" runat="server" HeaderText="TabPanel4">
                <HeaderTemplate>Pledge Details</HeaderTemplate>









                <ContentTemplate>
                    <center><asp:UpdatePanel ID="UpdatePanelContract" UpdateMode="Conditional" runat="server"><ContentTemplate><br /><table style="width:100%; padding-bottom:20px"><tr><td colspan="2" style="align-items:center"><center><asp:DropDownList ID="cmbContract" runat="server" class="form-control select2" Width="80%" AutoPostBack="True"><asp:ListItem Text="Please Select Contract" Value="0"></asp:ListItem></asp:DropDownList></center><hr /></td></tr><tr><td style="width:50%;"><table style="width:100%; height: 100%;"><tr><td style="width:20%; padding-bottom:10px">Contract Reference</td><td style="width:50%; padding-bottom:10px"><asp:TextBox ID="ContractReference" runat="server" Width="75%" ></asp:TextBox></td><tr><td style="width:20%; padding-bottom:10px">Pledge Expiry Date</td><td style="width:50%; padding-bottom:10px"><asp:TextBox ID="EndDatePledge" runat="server" Width="75%"></asp:TextBox><ajaxToolkit:CalendarExtender ID="EndDatePledge_CalendarExtender" runat="server" BehaviorID="EndDatePledge_CalendarExtender" TargetControlID="EndDatePledge" /></td></tr><tr><td style="width:20%; padding-bottom:10px">Pledge Amount</td><td style="width:50%; padding-bottom:10px"><asp:TextBox ID="PledgeAmt" runat="server" type="number" min="1"  Width="75%"></asp:TextBox></td></tr></tr></table></td><td style="width:50%;"><table style="width:100%;"><tr><td style="width:30%; padding-bottom:10px">Loan Maturity Date</td><td><asp:TextBox ID="startDatePledge" runat="server"  Width="75%"></asp:TextBox><ajaxToolkit:CalendarExtender ID="startDatePledge_CalendarExtender" runat="server" BehaviorID="startDatePledge_CalendarExtender" TargetControlID="startDatePledge" /></td></tr><tr><td style="width:30%; padding-bottom:10px">Loan Amount</td><td><asp:TextBox ID="txtContractValue" runat="server"  Width="75%" type="number" min="1" ></asp:TextBox></td></tr><tr><td style="width:30%">Loan Purpose</td><td><asp:TextBox ID="txtPurpose"  Width="75%" runat="server"  title="Please Enter Deal Tenor" type="number" Height="90%" TextMode="MultiLine"></asp:TextBox></td></tr></table><tr><td  style="padding-top:10px"><center><asp:Button ID="btnDelete" runat="server" CssClass="btn btn-danger btn-block btn-flats" Height="40px" Text="Delete" Width="150px" /></center></td><td  style="padding-top:10px"><center><asp:Button ID="btnContPledge" runat="server" CssClass="btn btn-success btn-block btn-flats" Height="40px" Text="Continue" Width="150px" /></center></td></tr></td></tr></tr></table></ContentTemplate><Triggers><asp:AsyncPostBackTrigger ControlID="cmbContract" EventName="SelectedIndexChanged" /></Triggers></asp:UpdatePanel><hr />
                        <div class="panel box box-success"><div class="box-header with-border"><h4 class="box-title"><a data-parent="#accordion" data-toggle="collapse" href="#addpledge">Add Pledge </a></h4></div><div id="addpledge" class="panel-collapse collapse">
                            <div class="box-body"><asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table style="width:50%">
                                        <tr><td style="padding-bottom:10px">Contract Reference</td>
                                            <td><asp:TextBox ID="txtContractRefNew" runat="server" Width="75%"></asp:TextBox></td>

                                        </tr><tr>
                                            <td style="padding-bottom:10px">Start Date</td><td><asp:TextBox ID="DateStart" runat="server" Width="75%"></asp:TextBox><ajaxToolkit:CalendarExtender ID="DateStart_CalendarExtender" runat="server" BehaviorID="DateStart_CalendarExtender" TargetControlID="DateStart" /></td>

                                             </tr><tr><td style="padding-bottom:10px">Maturity Date</td><td><asp:TextBox ID="MaturityDate" AutoPostBack="true" runat="server" Width="75%"></asp:TextBox><ajaxToolkit:CalendarExtender ID="MaturityDate_CalendarExtender" runat="server" BehaviorID="MaturityDate_CalendarExtender" TargetControlID="MaturityDate" /></td>

                                                  </tr><tr><td style="padding-bottom:10px">Contract Value</td><td><asp:TextBox ID="txtAmt" runat="server" Width="75%" type="number" min="1" ></asp:TextBox></td></tr><tr><td style="padding-bottom:15px">Purpose</td><td><asp:TextBox ID="txtPurposeNew" runat="server" TextMode="MultiLine" Width="75%"></asp:TextBox></td></tr><tr><td colspan="2" style="padding-bottom:10px"><center><asp:Button ID="btnSaveNew" runat="server" CssClass="btn btn-success btn-block btn-flats" Height="40px" Text="Save" Width="150px" /></center></td></tr></table></ContentTemplate><Triggers><asp:AsyncPostBackTrigger ControlID="btnSaveNew" EventName="Click" /></Triggers></asp:UpdatePanel></div></div></div></center>
                </ContentTemplate>



            </ajaxToolkit:TabPanel>
            <ajaxToolkit:TabPanel ID="TabPanel5" runat="server" HeaderText="TabPanel5">
                
                <HeaderTemplate >&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Instructions&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</HeaderTemplate>
                
                <ContentTemplate>
                    <table style="width: 100%;">
                        <tr>
                            <td style="width: 50%;">
                                <table style="width: 100%; height: 100%">
                                    <tr>
                                        <td style="width: 103px; padding-bottom: 10px">
                                            <asp:Label ID="Label26" runat="server" Text="Deal Inception"></asp:Label></td>
                                        <td>
                                            <asp:DropDownList ID="cmbInstructionsDeposit" runat="server" Width="80%">
                                                <asp:ListItem Text="Select Deal Inception Instruction" Value="0"></asp:ListItem>
                                            </asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 103px">&nbsp;</td>
                                        <td>
                                            <asp:TextBox ID="txtOtherInsDeposit" runat="server" TextMode="MultiLine" Height="150px" Width="80%"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 103px; padding-top: 35px">
                                            <asp:Label ID="Label27" runat="server" Text="Deal Maturity"></asp:Label></td>
                                        <td>
                                            <asp:DropDownList ID="cmbInstructionsDepositM" runat="server" Width="80%">
                                                <asp:ListItem Text="Select Deal Maturity Instruction" Value="0"></asp:ListItem>
                                            </asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 103px">&nbsp;</td>
                                        <td>
                                            <asp:TextBox ID="txtOtherInsDepositM" runat="server" TextMode="MultiLine" Height="150px" Width="80%"></asp:TextBox></td>
                                    </tr>
                                </table>
                            </td>
                            <td style="width: 30%;">
                                <table style="width: 100%;">
                                    <tr>
                                        <td style="align-content: center; padding-bottom: 10px">
                                            <asp:Button ID="btnValidateDeposit" runat="server" Text="Test" CssClass="btn btn-primary btn-block btn-flats" Height="40px" Width="150px" /></td>
                                    </tr>
                                    <tr>
                                        <td style="align-content: center; padding-bottom: 10px">
                                            <asp:Button ID="btnIntDaysBasisDeposit" runat="server" Text="Int Day Basis" CssClass="btn btn-primary btn-block btn-flats" Height="40px" Width="150px" /></td>
                                    </tr>
                                    <%--<tr>
                                        <td style="align-content: center; padding-bottom: 10px">
                                            <asp:Button ID="btnRollOver" runat="server" Text="Rollover Details" CssClass="btn btn-primary btn-block btn-flats" Height="40px" Width="150px" Enabled="False" /></td>
                                    </tr>--%>
                                    <tr>
                                        <td style="align-content: center; padding-bottom: 10px">
                                            <asp:Button ID="cmdSaveDealDeposit" runat="server" Text="Deal" CssClass="btn btn-primary btn-block btn-flats" Height="40px" Width="150px" Enabled="False" /></td>
                                    </tr>
                                    <%--<tr>
                                        <td style="align-content: center; padding-bottom: 10px">
                                            <asp:Button ID="btnPledge" runat="server" CssClass="btn btn-primary btn-block btn-flats" Height="40px" Text="Pledge Details" Width="150px" Enabled="False" /></td>
                                    </tr>--%>
                                    <tr>
                                        <td style="align-content: center; padding-bottom: 10px">
                                            <asp:Button ID="cmdResetDeposit" runat="server" Text="Reset" CssClass="btn btn-primary btn-block btn-flats" Height="40px" Width="150px" /></td>
                                    </tr>
                                    <tr>
                                        <td style="align-content: center; padding-bottom: 10px">
                                            <asp:Button ID="cmdCancelDeposit" runat="server" CssClass="btn btn-danger btn-block btn-flats" Height="40px" Text="Cancel Deal" Width="150px" /></td>
                                    </tr>
                                    <tr>
                                        <td style="align-content: center; padding-bottom: 10px"><a href="../index.aspx">
                                            <asp:Button ID="btnExit" runat="server" Text="Exit" CssClass="btn btn-danger btn-block btn-flats" Height="40px" Width="150px" /></a></td>
                                    </tr>
                                </table>
                        </tr>
                    </table>
                </ContentTemplate>


            </ajaxToolkit:TabPanel>
        </ajaxToolkit:TabContainer>
    </form>
    </head>

</asp:Content>

