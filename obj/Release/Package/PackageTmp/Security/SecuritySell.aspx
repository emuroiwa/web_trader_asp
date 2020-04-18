

<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Main.Master" CodeBehind="SecuritySell.aspx.vb" Inherits="WEBTDS.SecuritySell" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="ajaxToolkit" %>
<%@ Register assembly="EO.Web" namespace="EO.Web" tagprefix="eo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <head runat="server" />

    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:Label ID="lblError" runat="server"></asp:Label>
                <asp:Label ID="lblsuccess" runat="server"></asp:Label>
                <ajaxToolkit:TabContainer ID="TabSecuritySell" runat="server" ActiveTabIndex="1" Width="100%">
                    <ajaxToolkit:TabPanel ID="TabPanel1" runat="server" HeaderText="TabPanel1">
                        <HeaderTemplate>
                            Product / Client
                        </HeaderTemplate>
                        <ContentTemplate>
                            <table style="width:80%;">
                                <tr>
                                    <td class="auto-style1">Customer</td>
                                    <td class="auto-style2">
                                        <asp:DropDownList ID="cmbCustomer" runat="server" class="form-control select2" Font-Size="Small" Width="80%" AutoPostBack="True">
                                            <asp:ListItem Text="Select Customer" Value="0"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 120px">Product Portifolio</td>
                                    <td>
                                        <button class="btn btn-block btn-primary disabled"  style="width:50%" id="test"><asp:Label ID="lblportfolio" runat="server" Visible="False"></asp:Label></button>
                                        <asp:DropDownList ID="cmbportfolio" runat="server" class="form-control select2"  Font-Size="Small" Width="80%" AutoPostBack="True">
                                            <asp:ListItem Text="Select Porfolio" Value="0"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="auto-style1">Funding Account</td>
                                    <td class="auto-style2">
                                        <asp:TextBox ID="txtDealAccountSale" runat="server" Enabled="False" Width="80%"></asp:TextBox>
                                    </td>
                                    <td style="width: 120px">Product</td>
                                    <td>
                                        
                                        <asp:DropDownList ID="cmbproduct" runat="server" class="form-control select2" Font-Size="Small" Width="80%" AutoPostBack="True">
                                            <asp:ListItem Text="Select Product" Value="0"></asp:ListItem>
                                        </asp:DropDownList>
                                        
                                    </td>
                                </tr>
                                </caption>
                                </tr>
                                <tr>
                                    <td class="auto-style1">&nbsp;</td>
                                    <td class="auto-style2">
                                      
                                        <button class="btn btn-success" type="button">
                                            <asp:Label ID="txtCurrency" runat="server" Font-Bold="True"></asp:Label>
                                        </button>
                                        <button class="btn btn-warning" type="button">
                                            <asp:Label ID="txtIntDays" runat="server" Font-Bold="True"></asp:Label>
                                        </button>
                                    </td>
                                    <td style="width: 120px">Receiving Acount</td>
                                    <td>
                                        <asp:TextBox ID="txtSellRecieve" runat="server" Enabled="False" Width="80%"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="auto-style1">&nbsp;</td>
                                   
                                         <td colspan="1" class="auto-style2">
                               </td>
                                    
                                    <td style="width: 120px">
                                        <asp:RadioButtonList ID="RdSellOPT" runat="server" AutoPostBack="True" Font-Bold="True" Font-Size="Small" Enabled="False">
                                            <asp:ListItem Value="discount"> Discount</asp:ListItem>
                                            <asp:ListItem Value="yield"> Yield</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtdealdescSale" runat="server" Visible="False"></asp:TextBox>
                                    </td>
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
                                                <asp:Label ID="lblAddress" runat="server"></asp:Label><br />
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

<ItemHoverStyle CssText="background-image: url(00050206); background-repeat: repeat-x" ></itemhoverstyle>

<SelectedStyle CssText="background-image: url(00050207); background-repeat: repeat-x" ></selectedstyle>

<CellStyle CssText="padding-left:8px;padding-top:2px; color:#336699;white-space:nowrap;" ></cellstyle>
</eo:GridItemStyleSet>
</ItemStyles>

<ColumnHeaderStyle CssText="background-image:url('00050301');padding-left:8px;padding-top:2px;font-weight: bold;color:white;" ></columnheaderstyle>
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
                                            <asp:Label ID="Label29" runat="server" Font-Size="Small" Text="Start Date"></asp:Label>
                                        </td>
                                        <td style="width: 492px">
                                            <asp:TextBox ID="StartDateSale" runat="server" Width="75%" Enabled="False"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender ID="StartDateSale_CalendarExtender" runat="server" BehaviorID="TextBox6_CalendarExtender" DaysModeTitleFormat="dd/MM/yyyy" TargetControlID="StartDateSale" />
                                            <td style="width: 168px; ">
                                                <asp:Label ID="Label30" runat="server" Font-Size="Small" Text="Maturity Date" Width="75%"></asp:Label>
                                            </td>
                                            <td style="width: 369px; ">
                                                <asp:TextBox ID="EndDateSale" runat="server" required="required" Width="75%" Enabled="False"></asp:TextBox>
                                                <ajaxToolkit:CalendarExtender ID="EndDateSale_CalendarExtender" runat="server" BehaviorID="TextBox15_CalendarExtender" DaysModeTitleFormat="dd/MM/yyyy" TargetControlID="EndDateSale" />
                                            </td>
                                            <tr>
                                                <td ;="" style="padding-bottom: 20px;" class="auto-style3">
                                                    <asp:Label ID="Label31" runat="server" Font-Size="Small" Text="Deal Amount"></asp:Label>
                                                </td>
                                                <td class="auto-style4">
                                                    <asp:TextBox ID="DealAmountSale" runat="server" style="margin-left: 0px" title="Please Enter Present value" Width="75%" Enabled="False"></asp:TextBox>
                                                </td>
                                                <td class="auto-style5">
                                                    <asp:Label ID="Label32" runat="server" Font-Size="Small" Text="Future Value"></asp:Label>
                                                </td>
                                                <td class="auto-style6">
                                                    <asp:TextBox ID="txtMaturityAmountSale" runat="server" Enabled="False" Width="75%"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="height: 28px; width: 291px; padding-bottom:20px">
                                                    <asp:Label ID="Label33" runat="server" Font-Size="Small" Text="Discount Rate"></asp:Label>
                                                </td>
                                                <td style="width: 492px; height: 28px">
                                                    <asp:TextBox ID="txtDiscountRateSale" runat="server" min="1" required="required" title="Please Enter Interest Rate" type="number" Width="75%" Enabled="False"></asp:TextBox>
                                                </td>
                                                <td style="height: 28px; width: 168px;">
                                                    <asp:Label ID="Label34" runat="server" Font-Size="Small" Text="Net Interest"></asp:Label>
                                                </td>
                                                <td style="height: 28px; width: 369px;">
                                                    <asp:TextBox ID="netInterestSale" runat="server" Enabled="False" Width="75%"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 291px; padding-bottom:20px">
                                                    <asp:Label ID="Label35" runat="server" Font-Size="Small" Text="Tenor"></asp:Label>
                                                </td>
                                                <td style="width: 492px">
                                                    <asp:TextBox ID="txtTenorSale" runat="server" AutoPostBack="True" min="1" required="required" title="Please Enter Deal Tenor" type="number" Width="75%" Enabled="False"></asp:TextBox>
                                                </td>
                                                <td style="width: 168px">
                                                    <asp:Label ID="Label36" runat="server" Font-Size="Small" Text="Gross Interest"></asp:Label>
                                                </td>
                                                <td style="width: 369px">
                                                    <asp:TextBox ID="txtGrossSale" runat="server" Enabled="False" Width="75%"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 291px; ">
                                                    <asp:CheckBox ID="chkTaxStatus" runat="server" AutoPostBack="True" Text="Taxable" />
                                                    <br />
                                                </td>
                                                <td style="width: 492px">
                                                            <center>
                                                            <table style="width:80%;">
                                                                <tr>
                                                                    <td style="width: 50%">
                                                                        <asp:DropDownList ID="cmbTaxSell" runat="server" AutoPostBack="True" class="form-control select2" Enabled="False" style="margin-left: 0px" Width="50%">
                                                                            <asp:ListItem Text="TAXL" Value="TAXL"></asp:ListItem>
                                                                            <asp:ListItem>TAXT</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                    <td style="width: 50%">
                                                                        <asp:TextBox ID="txtTaxRateSale" runat="server" Width="60%" Enabled="False">0</asp:TextBox>
                                                                    </td>
                                                                   
                                                                </tr>
                                                               
                                                            </table>
                                                                </center>
                                                    <td style="width: 168px">
                                                        <asp:Label ID="Label24" runat="server" Font-Size="Small" Text="Tax Amount"></asp:Label>
                                                    </td>
                                                    <td style="width: 369px">
                                                        <asp:TextBox ID="txtTaxAmountSale" runat="server" Enabled="False" Width="75%"></asp:TextBox>
                                                    </td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 291px ; padding-bottom:20px">
                                                    <asp:Label ID="Label38" runat="server" Font-Size="Medium" Text="Ref"></asp:Label>
                                                </td>
                                                <td style="width: 492px">
                                                    <asp:TextBox ID="lblRef" runat="server" style="margin-left: 0px" title="Please Enter Present value" Width="75%" Enabled="False"></asp:TextBox>
                                                </td>
                                                <td style="width: 168px">
                                                    <asp:Label ID="Label37" runat="server" Font-Size="Small" Text="Yield Rate"></asp:Label>
                                                </td>
                                                <td style="width: 369px">
                                                    <asp:TextBox ID="txtYieldSale" runat="server" Enabled="False" Width="75%"></asp:TextBox>
                                                </td>
                                            </tr>
                                    <tr>
                                                <td style="width: 291px ; padding-bottom:20px">
                                                    <asp:Label ID="Label39" runat="server" Font-Size="Medium" Text="Selling"></asp:Label>
                                                </td>
                                                <td style="width: 492px">
                                                    <asp:TextBox ID="SellingCaption" runat="server" style="margin-left: 0px" title="Please Enter Present value" Width="75%" Enabled="False"></asp:TextBox>
                                                </td>
                                                <td style="width: 168px">
                                                    <asp:TextBox ID="commRate" runat="server" Visible="False">0</asp:TextBox>
                                                </td>
                                                <td style="width: 369px">
                                                    <asp:TextBox ID="TenorOG" runat="server" Visible="False"></asp:TextBox>
                                                    <asp:TextBox ID="lblPurchaseStart" runat="server" Visible="False"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <asp:TextBox ID="lblPurchaseRate" runat="server" Visible="False"></asp:TextBox>
                                                </td>
                                            </tr>
                                           </table>
                                <div class="panel box box-success">
                                    <div class="box-header with-border">
                                        <h4 class="box-title"><a data-parent="#accordion" data-toggle="collapse" href="#collapseTwo">Single Sale </a></h4>
                                    </div>
                                    <div id="collapseTwo" class="panel-collapse collapse">
                                        <div class="box-body">
                                            &nbsp;<table style="width:60%;">
                                                <tr>
                                                    <td style="width:10%;">
                                                        <asp:Label ID="Label40" runat="server" Text="Cost Value"></asp:Label>
                                                    </td>
                                                    <td style="width:20%;">
                                                        <asp:TextBox ID="txtCV" runat="server" Enabled="False" style="margin-left: 0px" title="Please Enter Present value" Width="75%"></asp:TextBox>
                                                    </td>
                                                    <td>&nbsp;</td>
                                                    <td>&nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td>&nbsp;</td>
                                                    <td>&nbsp;</td>
                                                    <td style="width:10%;">
                                                        <asp:Label ID="Label43" runat="server" Text="Capital Gain/Loss"></asp:Label>
                                                    </td>
                                                    <td style="width:20%;">
                                                        <asp:TextBox ID="txtPL" runat="server" Enabled="False" style="margin-left: 0px" title="Please Enter Present value" Width="75%"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width:10%;">
                                                        <asp:Label ID="Label42" runat="server" Text="Present Value"></asp:Label>
                                                    </td>
                                                    <td style="width:20%;">
                                                        <asp:TextBox ID="txtPV" runat="server" Enabled="False" style="margin-left: 0px" title="Please Enter Present value" Width="75%"></asp:TextBox>
                                                    </td>
                                                    <td>&nbsp;</td>
                                                    <td>&nbsp;</td>
                                                </tr>
                                               
                                            </table>
                                        </div>
                                    </div>
                                </div>
                                 <div class="panel box box-success">
                                    <div class="box-header with-border">
                                        <h4 class="box-title"><a data-parent="#accordion" data-toggle="collapse" href="#collapsemulti">Multiple Sale </a></h4>
                                    </div>
                                    <div id="collapsemulti" class="panel-collapse collapse">
                                        <div class="box-body">
                                            <eo:Grid ID="Grid1" runat="server" BorderColor="#C7D1DF" BorderWidth="1px" ColumnHeaderAscImage="00050303" ColumnHeaderDescImage="00050304" ColumnHeaderDividerImage="00050302" FixedColumnCount="1" Font-Bold="False" Font-Italic="False" Font-Names="Verdana" Font-Overline="False" Font-Size="9pt" Font-Strikeout="False" Font-Underline="False" GridLineColor="199, 209, 223" GridLines="Both" Height="85px" ItemHeight="19" Width="1115px">
    <itemstyles>
        <eo:GridItemStyleSet>
            <ItemStyle CssText="background-color: white" />
            <ItemHoverStyle CssText="background-image: url(00050206); background-repeat: repeat-x" />
            <SelectedStyle CssText="background-image: url(00050207); background-repeat: repeat-x" />
            <CellStyle CssText="padding-left:8px;padding-top:2px; color:#336699;white-space:nowrap;" />
        </eo:GridItemStyleSet>
    </itemstyles>
    <ColumnHeaderStyle CssText="background-image:url('00050301');padding-left:8px;padding-top:2px;font-weight: bold;color:white;" />
    <Columns>
        <eo:RowNumberColumn HeaderText="RowNum" Width="50">
        </eo:RowNumberColumn>
        <eo:StaticColumn HeaderText="SecRef" Width="150" DataField="SecRef">
        </eo:StaticColumn>
        <eo:StaticColumn HeaderText="DealRef" Width="150" DataField="dealref">
        </eo:StaticColumn>
        <eo:StaticColumn HeaderText="SaleAmt" Width="150" DataField="SelAmt">
        </eo:StaticColumn>
        <eo:StaticColumn HeaderText="Cost" Width="150" DataField="cost">
        </eo:StaticColumn>
        <eo:StaticColumn HeaderText="Profit" Width="150" DataField="profit">
        </eo:StaticColumn>
        <eo:StaticColumn HeaderText="PresentValue" Width="150" DataField="presentV">
        </eo:StaticColumn>
        <eo:StaticColumn HeaderText="MaturityAmt" Width="150" DataField="matamt">
        </eo:StaticColumn>
    </Columns>
    <columntemplates>
        <eo:TextBoxColumn>
            <TextBoxStyle CssText="BORDER-RIGHT: #7f9db9 1px solid; PADDING-RIGHT: 2px; BORDER-TOP: #7f9db9 1px solid; PADDING-LEFT: 2px; FONT-SIZE: 8.75pt; PADDING-BOTTOM: 1px; MARGIN: 0px; BORDER-LEFT: #7f9db9 1px solid; PADDING-TOP: 2px; BORDER-BOTTOM: #7f9db9 1px solid; FONT-FAMILY: Tahoma" />
        </eo:TextBoxColumn>
        <eo:DateTimeColumn>
            <datepicker controlskinid="None" daycellheight="16" daycellwidth="19" dayheaderformat="FirstLetter" disableddates="" othermonthdayvisible="True" selecteddates="" titleleftarrowimageurl="DefaultSubMenuIconRTL" titlerightarrowimageurl="DefaultSubMenuIcon">
                <PickerStyle CssText="border-bottom-color:#7f9db9;border-bottom-style:solid;border-bottom-width:1px;border-left-color:#7f9db9;border-left-style:solid;border-left-width:1px;border-right-color:#7f9db9;border-right-style:solid;border-right-width:1px;border-top-color:#7f9db9;border-top-style:solid;border-top-width:1px;font-family:Courier New;font-size:8pt;margin-bottom:0px;margin-left:0px;margin-right:0px;margin-top:0px;padding-bottom:1px;padding-left:2px;padding-right:2px;padding-top:2px;" />
                <CalendarStyle CssText="background-color: white; border-right: #7f9db9 1px solid; padding-right: 4px; border-top: #7f9db9 1px solid; padding-left: 4px; font-size: 9px; padding-bottom: 4px; border-left: #7f9db9 1px solid; padding-top: 4px; border-bottom: #7f9db9 1px solid; font-family: tahoma" />
                <TitleStyle CssText="background-color:#9ebef5;font-family:Tahoma;font-size:12px;padding-bottom:2px;padding-left:6px;padding-right:6px;padding-top:2px;" />
                <TitleArrowStyle CssText="cursor:hand" />
                <MonthStyle CssText="font-family: tahoma; font-size: 12px; margin-left: 14px; cursor: hand; margin-right: 14px" />
                <DayHeaderStyle CssText="font-family: tahoma; font-size: 12px; border-bottom: #aca899 1px solid" />
                <DayStyle CssText="font-family: tahoma; font-size: 12px; border-right: white 1px solid; border-top: white 1px solid; border-left: white 1px solid; border-bottom: white 1px solid" />
                <DayHoverStyle CssText="font-family: tahoma; font-size: 12px; border-right: #fbe694 1px solid; border-top: #fbe694 1px solid; border-left: #fbe694 1px solid; border-bottom: #fbe694 1px solid" />
                <TodayStyle CssText="font-family: tahoma; font-size: 12px; border-right: #bb5503 1px solid; border-top: #bb5503 1px solid; border-left: #bb5503 1px solid; border-bottom: #bb5503 1px solid" />
                <SelectedDayStyle CssText="font-family: tahoma; font-size: 12px; background-color: #fbe694; border-right: white 1px solid; border-top: white 1px solid; border-left: white 1px solid; border-bottom: white 1px solid" />
                <DisabledDayStyle CssText="font-family: tahoma; font-size: 12px; color: gray; border-right: white 1px solid; border-top: white 1px solid; border-left: white 1px solid; border-bottom: white 1px solid" />
                <OtherMonthDayStyle CssText="font-family: tahoma; font-size: 12px; color: gray; border-right: white 1px solid; border-top: white 1px solid; border-left: white 1px solid; border-bottom: white 1px solid" />
            </datepicker>
        </eo:DateTimeColumn>
        <eo:MaskedEditColumn>
            <maskededit controlskinid="None" textboxstyle-csstext="BORDER-RIGHT: #7f9db9 1px solid; PADDING-RIGHT: 2px; BORDER-TOP: #7f9db9 1px solid; PADDING-LEFT: 2px; PADDING-BOTTOM: 1px; MARGIN: 0px; BORDER-LEFT: #7f9db9 1px solid; PADDING-TOP: 2px; BORDER-BOTTOM: #7f9db9 1px solid; font-family:Courier New;font-size:8pt;">
            </maskededit>
        </eo:MaskedEditColumn>
        <eo:StaticColumn>
        </eo:StaticColumn>
        <eo:RowNumberColumn>
        </eo:RowNumberColumn>
    </columntemplates>
    <FooterStyle CssText="padding-bottom:4px;padding-left:4px;padding-right:4px;padding-top:4px;" />
                            </eo:Grid>
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
                                                    <asp:DropDownList ID="cmdInstrucSale" runat="server" Width="80%">
                                                        <asp:ListItem Text="Select Deal Inception Instruction" Value="0"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 103px">&nbsp;</td>
                                                <td>
                                                    <asp:TextBox ID="txtOtherSale" runat="server" Height="150px" TextMode="MultiLine" Width="80%"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 103px;padding-top:35px">
                                                    <asp:Label ID="Label27" runat="server" Text="Deal Maturity"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="cmdInstrucSaleM" runat="server" Font-Size="Small" Width="80%">
                                                        <asp:ListItem Text="Select Deal Maturity Instruction" Value="0"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 103px">&nbsp;</td>
                                                <td>
                                                    <asp:TextBox ID="txtOtherSaleM" runat="server" Height="150px" TextMode="MultiLine" Width="80%"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td style="width:30%;">
                                        <table style="width:100%;">
                                            <tr>
                                                <td style="align-content:center ;padding-bottom:10px">
                                                    <asp:Button ID="btnValidateSale" runat="server" CssClass="btn btn-primary btn-block btn-flats" Height="40px" Text="Test" Width="150px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="align-content:center;padding-bottom:10px">
                                                    <asp:Button ID="btnIntDaysPur" runat="server" CssClass="btn btn-primary btn-block btn-flats" Height="40px" Text="Int Day Basis" Width="150px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="align-content:center ;padding-bottom:10px">
                                                    <asp:Button ID="btnSaveSale" runat="server" CssClass="btn btn-success btn-block btn-flats" Height="40px" Text="Deal" Width="150px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="align-content:center ;padding-bottom:10px">
                                                    <asp:Button ID="btnReset" runat="server" CssClass="btn btn-warning btn-block btn-flats" Height="40px" Text="Reset" Width="150px" />
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

   <style>
      .example-modal .modal {
        position: relative;
        top: auto;
        bottom: auto;
        right: auto;
        left: auto;
        display: block;
        z-index: 1;
      }
      .example-modal .modal {
        background: transparent !important;
      }
       .auto-style1 {
           width: 140px;
       }
       .auto-style2 {
           width: 351px;
       }
       .auto-style3 {
           width: 291px;
           height: 39px;
       }
       .auto-style4 {
           width: 492px;
           height: 39px;
       }
       .auto-style5 {
           width: 168px;
           height: 39px;
       }
       .auto-style6 {
           width: 369px;
           height: 39px;
       }
    </style>
  

</asp:Content>

