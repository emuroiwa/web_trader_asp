<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Main.Master" CodeBehind="SecurityPurchase.aspx.vb" Inherits="WEBTDS.SecurityPurchase" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="ajaxToolkit" %>
<%@ Register assembly="EO.Web" namespace="EO.Web" tagprefix="eo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <head />

    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:Label ID="lblError" runat="server"></asp:Label>
                <asp:Label ID="lblsuccess" runat="server"></asp:Label>
                <ajaxToolkit:TabContainer ID="TabSecurityPurchase" runat="server" ActiveTabIndex="0" Width="100%">
                    <ajaxToolkit:TabPanel ID="TabPanel1" runat="server" HeaderText="TabPanel1">
                        <HeaderTemplate>
                            Product / Client
                        </HeaderTemplate>
                        <ContentTemplate>
                            <table style="width:100%;">
                                <tr>
                                    <td style="width: 63px">Customer</td>
                                    <td>
                                        <asp:DropDownList ID="cmbCustomerPur" runat="server" class="form-control select2" Font-Size="Small" Width="80%" AutoPostBack="True">
                                            <asp:ListItem Text="Select Customer" Value="0"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 120px">Product Portifolio</td>
                                    <td>
                                        <button class="btn btn-block btn-primary disabled">
                                            <asp:Label ID="lblportfolioPur" runat="server"></asp:Label>
                                        </button>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 63px">Funding Account</td>
                                    <td>
                                        <asp:TextBox ID="txtPurchaseLoan" runat="server" Enabled="False" Width="75%"></asp:TextBox>
                                    </td>
                                    <td style="width: 120px">Product</td>
                                    <td>
                                        <button class="btn btn-block btn-success disabled">
                                            <asp:Label ID="lblDescriptionPur" runat="server"></asp:Label>
                                            <asp:Label ID="lblDealtypePur" runat="server" Font-Bold="True"></asp:Label>
                                        </button>
                                    </td>
                                </tr>
                                </caption>
                                </tr>
                                <tr>
                                    <td style="width: 63px">Issurer</td>
                                    <td>
                                        <asp:DropDownList ID="cmbIssurer" runat="server" class="form-control select2" Font-Size="Small" Width="80%">
                                            <asp:ListItem Text="Select Issurer" Value="0"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 120px">Receiving Acount</td>
                                    <td>
                                        <asp:TextBox ID="txtDealAccountPurchase" runat="server" Enabled="False" Width="75%"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 63px">Security Reference</td>
                                    <td>
                                        <asp:TextBox ID="txtTBID" runat="server" Width="75%"></asp:TextBox>
                                    </td>
                                    <td style="width: 120px">Deal Number</td>
                                    <td>
                                        <button class="btn btn-block btn-primary disabled">
                                            <asp:Label ID="lblDealNumber" runat="server"></asp:Label>
                                        </button>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 63px">&nbsp;</td>
                                    <td>
                                        <button class="btn btn-success" type="button">
                                            <asp:Label ID="txtCurrency" runat="server" Font-Bold="True"></asp:Label>
                                        </button>
                                        <button class="btn btn-warning" type="button">
                                            <asp:Label ID="txtIntDays" runat="server" Font-Bold="True"></asp:Label>
                                        </button>
                                    </td>
                                    <td style="width: 120px"></td>
                                    <td>
                                        <asp:RadioButtonList ID="RadioDiscYield" runat="server" AutoPostBack="True" Font-Bold="True" Font-Size="Small">
                                            <asp:ListItem Value="discount"> Discount</asp:ListItem>
                                            <asp:ListItem Value="yield"> Yield</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
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
                            <ContentTemplate>
                                <fieldset title="Security">
                                    <div class="panel box box-success">
                                        <div class="box-header with-border">
                                            <h4 class="box-title"><a data-parent="#accordion" data-toggle="collapse" href="#history1">Address Details </a></h4>
                                        </div>
                                        <div id="history1" class="panel-collapse collapse">
                                            <div class="box-body">
                                                <asp:Label ID="lblAddress" runat="server"></asp:Label>
                                                
                                                <br />
                                                 Customer Age<asp:Label ID="custAge" runat="server" Text=" "></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="panel box box-success">
                                        <div class="box-header with-border">
                                            <h4 class="box-title"><a data-parent="#accordion" data-toggle="collapse" href="#history">Client History </a></h4>
                                        </div>
                                        <div id="history" class="panel-collapse collapse">
                                            <div class="box-body">
                                                <button class="btn btn-success" type="button">
                                                    Avg Rate
                                                    <asp:Label ID="avgRate" runat="server" Text="Label"></asp:Label>
                                                </button>
                                                <button class="btn btn-success" type="button">
                                                    Avg Size
                                                    <asp:Label ID="avgSize" runat="server"></asp:Label>
                                                </button>
                                                <button class="btn btn-success" type="button">
                                                    Avg Tenor
                                                    <asp:Label ID="avgTenor" runat="server"></asp:Label>
                                                </button>
                                                <hr />
                                                <eo:grid ID="GridClientHistory" runat="server" BorderColor="#C7D1DF" 
                                        BorderWidth="1px" ColumnHeaderAscImage="00050303" 
                                        ColumnHeaderDescImage="00050304" ColumnHeaderDividerImage="00050302" 
                                        FixedColumnCount="1" Font-Bold="False" Font-Italic="False" Font-Names="Verdana" 
                                        Font-Overline="False" Font-Size="9pt" Font-Strikeout="False" 
                                        Font-Underline="False" GridLineColor="199, 209, 223" GridLines="Both" 
                                        Height="150px" ItemHeight="19" Width="55%"  ClientSideOnItemCommand="OnItemDelete"><ItemStyles>
<eo:GridItemStyleSet>
<ItemStyle CssText="background-color: white" />

<ItemHoverStyle CssText="background-image: url(00050206); background-repeat: repeat-x" />

<SelectedStyle CssText="background-image: url(00050207); background-repeat: repeat-x" />

<CellStyle CssText="padding-left:8px;padding-top:2px; color:#336699;white-space:nowrap;" />
</eo:GridItemStyleSet>
</ItemStyles>

<ColumnHeaderStyle CssText="background-image:url('00050301');padding-left:8px;padding-top:2px;font-weight: bold;color:white;" />
<Columns>
<eo:RowNumberColumn Width="30"></eo:RowNumberColumn>
<eo:CustomColumn DataField="dealreference" HeaderText="Deal Reference" 
                                                MinWidth="20" ReadOnly="True" Width="200"></eo:CustomColumn>
<eo:CustomColumn AllowSort="True" DataField="tenor" 
                                                HeaderText="Tenor" MinWidth="10" ReadOnly="True"></eo:CustomColumn>
<eo:CustomColumn AllowSort="True" DataField="dealamount" 
                                                HeaderText="Deal Amount" MinWidth="25" ReadOnly="True" Width="200"></eo:CustomColumn>
<eo:CustomColumn AllowSort="True" DataField="startdate" 
                                                HeaderText="Start Date" MinWidth="20" ReadOnly="True" Width="150"></eo:CustomColumn>
<eo:CustomColumn AllowSort="True" DataField="maturitydate" 
                                                HeaderText="Maturity Date" MinWidth="20" ReadOnly="True" Width="150"></eo:CustomColumn>
</Columns>

<FooterStyle CssText="padding-bottom:4px;padding-left:4px;padding-right:4px;padding-top:4px;" />
</eo:grid>
                                                </div>
                                        </div>
                                    </div>
                                </fieldset>
                            </ContentTemplate>
                            </hr>
                        </ContentTemplate>
                    </ajaxToolkit:TabPanel>
                    <ajaxToolkit:TabPanel ID="TabPanel2" runat="server" HeaderText="TabPanel2">
                        <HeaderTemplate>
                            Details
                        </HeaderTemplate>
                        <ContentTemplate>
                            <center>
                                <table style="width: 90%; height: 178px; table-layout: auto;">
                                    <tr>
                                        <td style="width: 291px; padding-bottom:20px">
                                            <asp:Label ID="Label14" runat="server" Font-Size="Small" Text="Start Date"></asp:Label>
                                        </td>
                                        <td style="width: 492px">
                                            <asp:TextBox ID="startDatePur" runat="server" Font-Size="Small" required="required" title="Please Enter Start Date" Width="75%"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender ID="startDatePur_CalendarExtender" runat="server" BehaviorID="TextBox6_CalendarExtender" DaysModeTitleFormat="dd/MM/yyyy" TargetControlID="startDatePur" />
                                        </td>
                                        <td style="width: 168px; ">
                                            <asp:Label ID="Label19" runat="server" Font-Size="Small" Text="Maturity Date" Width="75%"></asp:Label>
                                        </td>
                                        <td style="width: 369px; ">
                                            <asp:TextBox ID="MaturityDatePur" runat="server" Font-Size="Small" required="required" title="Please Enter Maturity Date" Width="75%"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender ID="MaturityDatePur_CalendarExtender" runat="server" BehaviorID="TextBox15_CalendarExtender" DaysModeTitleFormat="dd/MM/yyyy" TargetControlID="MaturityDatePur" />
                                        </td>
                                        <tr>
                                            <td ;="" style="width: 291px; padding-bottom:20px">
                                                <asp:Label ID="Label15" runat="server" Font-Size="Small" Text="Face Value"></asp:Label>
                                            </td>
                                            <td style="width: 492px">
                                                <asp:TextBox ID="txtMaturityAmountPur" runat="server" Font-Size="Small" required="required" style="margin-left: 0px" title="Please Enter Present value" Width="75%"></asp:TextBox>
                                            </td>
                                            <td style="width: 168px">
                                                <asp:Label ID="Label20" runat="server" Font-Size="Small" Text="Net Proceeds"></asp:Label>
                                            </td>
                                            <td style="width: 369px">
                                                <asp:TextBox ID="txtDealAmountPur" runat="server" Enabled="False" Font-Size="Small" Width="75%"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 28px; width: 291px; padding-bottom:20px">
                                                <asp:Label ID="Label16" runat="server" Font-Size="Small" Text="Discount Rate"></asp:Label>
                                            </td>
                                            <td style="width: 492px; height: 28px">
                                                <asp:TextBox ID="txtDiscountRatePur" runat="server" Font-Size="Small" required="required" title="Please Enter Interest Rate" Width="75%"></asp:TextBox>
                                            </td>
                                            <td style="height: 28px; width: 168px;">
                                                <asp:Label ID="Label21" runat="server" Font-Size="Small" Text="Yield Rate"></asp:Label>
                                            </td>
                                            <td style="height: 28px; width: 369px;">
                                                <asp:TextBox ID="txtYieldRatePur" runat="server" Enabled="False" Font-Size="Small" Width="75%"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 291px; padding-bottom:20px">
                                                <asp:Label ID="Label17" runat="server" Font-Size="Small" Text="Commission Amount"></asp:Label>
                                            </td>
                                            <td style="width: 492px">
                                                <asp:TextBox ID="txtCommAmount" runat="server" Enabled="False" Font-Size="Small" required="required" title="Please Enter Interest Rate" Width="75%">10</asp:TextBox>
                                            </td>
                                            <td style="width: 168px">
                                                <asp:Label ID="Label22" runat="server" Font-Size="Small" Text="Net Interest"></asp:Label>
                                            </td>
                                            <td style="width: 369px">
                                                <asp:TextBox ID="txtNetInterestPur" runat="server" Enabled="False" Font-Size="Small" Width="75%"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 291px; ">
                                                <asp:Label ID="Label28" runat="server" Font-Size="Small" Text="Tenor"></asp:Label>
                                            </td>
                                            <td style="width: 492px">
                                                <asp:TextBox ID="txtTenorPur" runat="server" Font-Size="Small" min="1" required="required" title="Please Enter Deal Tenor" type="number" Width="75%"></asp:TextBox>
                                            </td>
                                            <td style="width: 168px">
                                                <asp:Label ID="Label23" runat="server" Font-Size="Small" Text="Total Charges"></asp:Label>
                                            </td>
                                            <td style="width: 369px">
                                                <asp:TextBox ID="txtGrossIntPur" runat="server" Enabled="False" Font-Size="Small" Width="75%"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 291px ; padding-bottom:20px">
                                                <asp:CheckBox ID="CheckComRatePur" runat="server" Font-Size="Small" Text="Acceptance Commision Rate" />
                                            </td>
                                            <td style="width: 492px">
                                                <asp:TextBox ID="txtCommRate" runat="server" Enabled="False" Font-Size="Small" required="required" style="margin-left: 0px" title="Please Enter Present value" Width="75%"></asp:TextBox>
                                            </td>
                                            <td style="width: 168px">&nbsp;</td>
                                            <td style="width: 369px">&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">&nbsp;</td>
                                        </tr>
                                    </tr>
                                </table>
                                <hr>
                                <div class="panel box box-success">
                                    <div class="box-header with-border">
                                        <h4 class="box-title"><a data-parent="#accordion" data-toggle="collapse" href="#collapseTwo">Security Details </a></h4>
                                    </div>
                                    <div id="collapseTwo" class="panel-collapse collapse">
                                        <div class="box-body">
                                            <asp:Label ID="Label2" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                                </hr>
                            </center>
                        </ContentTemplate>
                    </ajaxToolkit:TabPanel>
                    <ajaxToolkit:TabPanel ID="TabPanel5" runat="server" HeaderText="TabPanel5">
                        <HeaderTemplate>
                            Instructions
                        </HeaderTemplate>
                        <ContentTemplate>
                            <table style="width:100%;">
                                <tr>
                                    <td style="width:50%;">
                                        <table style="width:100%; height:100%">
                                            <tr>
                                                <td style="width: 103px; padding-bottom:10px">
                                                    <asp:Label ID="Label26" runat="server" Text="Deal Inception"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="cmbInstructionPur" runat="server" Width="80%">
                                                        <asp:ListItem Text="Select Deal Inception Instruction" Value="0"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 103px">&nbsp;</td>
                                                <td>
                                                    <asp:TextBox ID="txtOtherPur" runat="server" Height="150px" TextMode="MultiLine" Width="80%"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 103px;padding-top:35px">
                                                    <asp:Label ID="Label27" runat="server" Text="Deal Maturity"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="cmbInstructionMaturityPur" runat="server" Font-Size="Small" Width="80%">
                                                        <asp:ListItem Text="Select Deal Maturity Instruction" Value="0"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 103px">&nbsp;</td>
                                                <td>
                                                    <asp:TextBox ID="txtOtherMaturityPur" runat="server" Height="150px" TextMode="MultiLine" Width="80%"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td style="width:30%;">
                                        <table style="width:100%;">
                                            <tr>
                                                <td style="align-content:center ;padding-bottom:10px">
                                                    <asp:Button ID="btnValidatePur" runat="server" CssClass="btn btn-primary btn-block btn-flats" Height="40px" Text="Test" Width="150px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="align-content:center;padding-bottom:10px">
                                                    <asp:Button ID="btnIntDaysPur" runat="server" CssClass="btn btn-primary btn-block btn-flats" Height="40px" Text="Int Day Basis" Width="150px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="align-content:center ;padding-bottom:10px">
                                                    <asp:Button ID="btnSavePurchase" runat="server" CssClass="btn btn-primary btn-block btn-flats" Height="40px" Text="Deal" Width="150px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="align-content:center ;padding-bottom:10px">
                                                    <asp:Button ID="btnReset" runat="server" CssClass="btn btn-primary btn-block btn-flats" Height="40px" Text="Reset" Width="150px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="align-content:center;padding-bottom:10px">
                                                    <asp:Button ID="btnCancelPur" runat="server" CssClass="btn btn-danger btn-block btn-flats" Height="40px" Text="Cancel Deal" Width="150px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="align-content:center ;padding-bottom:10px">
                                                    <asp:Button ID="btnExit" runat="server" CssClass="btn btn-danger btn-block btn-flats" Height="40px" Text="Exit" Width="150px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="align-content:center;padding-bottom:10px">&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td style="align-content:center;padding-bottom:10px">&nbsp;</td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </ajaxToolkit:TabPanel>
                </ajaxToolkit:TabContainer>
            </ContentTemplate>
        </asp:UpdatePanel>
             
    </form>
   </head>

   
  

</asp:Content>

