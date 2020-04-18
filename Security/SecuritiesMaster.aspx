
<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Main.Master" CodeBehind="SecuritiesMaster.aspx.vb" Inherits="WEBTDS.SecuritiesMaster" %>

<%@ Register Assembly="EO.Web" Namespace="EO.Web" TagPrefix="eo" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   
 
    <%--<button class="btn btn-box-tool" data-widget="remove"><i class="fa fa-times"></i></button>--%>
 <head />
  <form id="form1" runat="server">
       <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
     <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
 <asp:Label ID="lblError" runat="server"></asp:Label>
        <asp:Label ID="lblsuccess" runat="server"></asp:Label>
                  <asp:Label ID="lblwarning" runat="server"> </asp:Label>
                <asp:Label ID="lbldealref" runat="server"></asp:Label>
      <%-- <!-- Horizontal Form -->
              <div class="box box-info">
                <div class="box-header with-border">
                  <h3 class="box-title">Securities Master </h3>  
   
                </div><!-- /.box-header -->
                <!-- form start -->
             
                  <div class="box-body">--%>

                <div class="box box-success">
           <div class="box-header with-border">
                  <h3 class="box-title">Securities Master </h3>
                  <div class="box-tools pull-right"> 
                    <%--<button class="btn btn-box-tool" data-widget="remove"><i class="fa fa-times"></i></button>--%>
                      <%--<button class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i></button>--%>
                  </div><!-- /.box-tools -->
                </div><!-- /.box-header -->
                <div class="box-body">

                
                     </div></div> 
                    </HeaderTemplate>
                              <ContentTemplate>
                               <center>  
                                   

                                   <table style="width:80%;">
                                     <tr>
                                           <td style="width: 10%">
                                               <asp:Label ID="Label15" runat="server" Font-Bold="True" Font-Size="Medium" Text="Purchases"></asp:Label>
                                           </td>
                                           <td style="width: 10%">&nbsp;</td>
                                           <td style="width: 122px">
                                               <asp:Label ID="Label16" runat="server" Font-Bold="True" Font-Size="Medium" Text="Filters"></asp:Label>
                                           </td>
                                           <td style="width: 10%"></td>
                                           <td style="width: 8%">
                                               <asp:Label ID="Label9" runat="server" Text="Lower Amount"></asp:Label>
                                           </td>
                                           <td style="width: 10%">
                                               <asp:TextBox ID="txtAmtLower" runat="server"></asp:TextBox>
                                           </td>
                                           <td style="width: 11%">
                                               <asp:Label ID="Label14" runat="server" Text="Days Rmaining Lower"></asp:Label>
                                           </td>
                                           <td style="width: 10%">
                                               <asp:TextBox ID="txtRemLower" runat="server"></asp:TextBox>
                                           </td>
                                           <td>&nbsp;</td>
                                       </tr>
                                           <tr>
                                               <td style="width: 8%; ">
                                                   <br />
                                                   <asp:Label ID="Label3" runat="server" Text="Currency"></asp:Label>
                                               </td>
                                               <td class="modal-sm" style="width: 15%; ">
                                                   <asp:DropDownList ID="cmbcurrency" runat="server" AutoPostBack="True" class="form-control select2" Font-Size="Small" style="margin-left: 0px" Width="80%">
                                                       <asp:ListItem Text="Select Currency" Value="0"></asp:ListItem>
                                                   </asp:DropDownList>
                                               </td>
                                               <td style="width: 3%; ">
                                                   <br />
                                                   <asp:Label ID="Label6" runat="server" Text="Type"></asp:Label>
                                               </td>
                                               <td style="width: 15%; ">
                                                   <asp:DropDownList ID="cmbType" runat="server" AutoPostBack="True" class="form-control select2" Font-Size="Small" style="margin-left: 0px" Width="98%">
                                                       <asp:ListItem Text="Select Type" Value="0"></asp:ListItem>
                                                   </asp:DropDownList>
                                               </td>
                                               <td style="width: 5%; ">
                                                   <asp:TextBox ID="txtmatured" runat="server" Enabled="False" Visible="False"></asp:TextBox>
                                               </td>
                                               <td style="width: 10%; ">
                                                   
                                                   <asp:TextBox ID="lblsellref" runat="server" Enabled="False" Visible="False"></asp:TextBox>
                                                   
                                                   <br />
                                                   
                                                   </td>
                                               <td style="width: 10%; ">
                                                   &nbsp;</td>
                                               <td style="width: 10%; ">
                                                  
                                                  
                                                   <asp:TextBox ID="txtPortfolio" runat="server" Enabled="False" Visible="False"></asp:TextBox>
                                                  
                                                  
                                               </td>
                                               <td style="width: 10%; ">
                                                   <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-primary btn-block btn-flats" Height="40px" style="margin-left: 10px" Text="Search" Width="150px" />
                                               </td>
                                           </tr>
                                           <tr>
                                               <td style="width: 5%">
                                                   <asp:Label ID="Label7" runat="server" Text="TBBAs"></asp:Label>
                                               </td>
                                               <td class="modal-sm" style="width: 15%">
                                                   <asp:DropDownList ID="cmbTBBAS" runat="server" AutoPostBack="True" class="form-control select2" Font-Size="Small" style="margin-left: 0px" Width="80%">
                                                       <asp:ListItem Text="Select TBBA" Value="0"></asp:ListItem>
                                                   </asp:DropDownList>
                                               </td>
                                               <td style="width: 3%"></td>
                                               <td style="width: 10%">
                                                   <asp:TextBox ID="txtTbID" runat="server" Enabled="False" Visible="False"></asp:TextBox>
                                               </td>
                                               <td style="width: 8%">
                                                   <asp:Label ID="Label17" runat="server" Text="Amount Upper"></asp:Label>
                                               </td>
                                               <td style="width: 10%">
                                                   <asp:TextBox ID="txtAmtUpper" runat="server"></asp:TextBox>
                                               </td>
                                               <td style="width: 11%">
                                                   <asp:Label ID="Label11" runat="server" Text="Days Remaining Upper"></asp:Label>
                                               </td>
                                               <td style="width: 10%">
                                                   <asp:TextBox ID="txtRemUpper" runat="server"></asp:TextBox>
                                                   </td>
                                               <td>&nbsp;</td>
                                           </tr>
                                           
                                    
                                   </table>
                                    </center>
                                  <hr />
                                   <div class="row">
            <div class="col-md-6">
              <!-- AREA CHART -->
              <div class="box box-primary">
                <div class="box-header with-border">
                  <h3 class="box-title">Securities Master</h3>
                  <div class="box-tools pull-right">
                    <button class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i></button>
                    <%--<button class="btn btn-box-tool" data-widget="remove"><i class="fa fa-times"></i></button>--%>
                  </div>
                </div>
                <div class="box-body chart-responsive">
                     <eo:Grid ID="GrdMaster" runat="server"  BorderColor ="#C7D1DF" BorderWidth="1px" ColumnHeaderAscImage="00050303" ColumnHeaderDescImage="00050304" ColumnHeaderDividerImage="00050302" FixedColumnCount="1" Font-Bold="False" Font-Italic="False" Font-Names="Verdana" Font-Overline="False" Font-Size="9pt" Font-Strikeout="False" Font-Underline="False" GridLineColor="199, 209, 223" GridLines="Both" Height="102px" ItemHeight="19" Width="768px">
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
                            <eo:RowNumberColumn HeaderText="RowNum">
                            </eo:RowNumberColumn>
                            <eo:StaticColumn DataField="dealreference" HeaderText="Deal Reference" Width="140">
                            </eo:StaticColumn>
                            <eo:StaticColumn DataField="fullname" HeaderText="Customer " Width="140">
                            </eo:StaticColumn>
                            <eo:StaticColumn DataField="PurchaseAmt" HeaderText="Purchase Amount" Width="140">
                            </eo:StaticColumn>
                            <eo:StaticColumn DataField="dealamount" HeaderText="Deal Amount" Width="140">
                            </eo:StaticColumn>
                            <eo:StaticColumn DataField="startdate" HeaderText="Start">
                            </eo:StaticColumn>
                            <eo:StaticColumn DataField="Maturitydate" HeaderText="Matures">
                            </eo:StaticColumn>
                            <eo:StaticColumn DataField="interestrate" HeaderText="YieldRate">
                            </eo:StaticColumn>
                            <eo:StaticColumn DataField="discountrate" HeaderText="DiscountRate">
                            </eo:StaticColumn>
                            <eo:StaticColumn DataField="tenor" HeaderText="Tenor">
                            </eo:StaticColumn>
                            <eo:StaticColumn DataField="daystomaturity" HeaderText="Remain">
                            </eo:StaticColumn>
                            <eo:StaticColumn DataField="intaccruedtodate" HeaderText="AcruedToDate">
                            </eo:StaticColumn>
                            <eo:StaticColumn DataField="maturityamount" HeaderText="Maturity Amt">
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
                <%--  <div class="chart" id="mm" style="height: 300px; position: relative;"></div>--%>
                </div><!-- /.box-body -->
              </div><!-- /.box -->
                <%--  --%>
                
                <%--  --%>
              <!-- DONUT CHART -->
              <div class="box box-danger">
                <div class="box-header with-border">
                  <h3 class="box-title">Sales</h3>
                  <div class="box-tools pull-right">
                    <button class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i></button>

                   
                    <%--<button class="btn btn-box-tool" data-widget="remove"><i class="fa fa-times"></i></button>--%>
                  </div>
                </div>
                <div class="box-body chart-responsive">
                  <%--<div class="chart" id="fx" style="height: 300px; position: relative;"></div>--%>

                    <eo:ContextMenu ID="ContextMenu1" runat="server" CheckIconUrl="OfficeCheckIcon" ControlSkinID="None" Width="200px" ClientSideOnItemClick="OnContextMenuItemClicked">
                        <lookitems>
                            <eo:MenuItem DisabledStyle-CssText="background-color:transparent;border-bottom-style:none;border-left-style:none;border-right-style:none;border-top-style:none;padding-bottom:1px;padding-left:5px;padding-right:5px;padding-top:1px;color:lightgrey" Height="24" HoverStyle-CssText="background-color:#c1d2ee;border-bottom-color:#316ac5;border-bottom-style:solid;border-bottom-width:1px;border-left-color:#316ac5;border-left-style:solid;border-left-width:1px;border-right-color:#316ac5;border-right-style:solid;border-right-width:1px;border-top-color:#316ac5;border-top-style:solid;border-top-width:1px;padding-left:4px;padding-right:4px;padding-top:0px;padding-bottom:0px;" ItemID="_TopLevelItem" NormalStyle-CssText="background-color:transparent;border-bottom-style:none;border-left-style:none;border-right-style:none;border-top-style:none;padding-bottom:1px;padding-left:5px;padding-right:5px;padding-top:1px;" SelectedStyle-CssText="background-color:white;border-bottom-color:#316ac5;border-bottom-style:solid;border-bottom-width:1px;border-left-color:#316ac5;border-left-style:solid;border-left-width:1px;border-right-color:#316ac5;border-right-style:solid;border-right-width:1px;border-top-color:#316ac5;border-top-style:solid;border-top-width:1px;padding-left:4px;padding-right:4px;padding-top:0px;padding-bottom:0px;">
                                <submenu collapseeffect-duration="150" collapseeffect-type="Fade" expandeffect-duration="150" expandeffect-type="Fade" itemspacing="3" lefticoncellwidth="25" sideimage="OfficeXPSideBar" style-csstext="background-color:#fcfcf9;border-bottom-color:#999999;border-bottom-style:solid;border-bottom-width:1px;border-left-color:#999999;border-left-style:solid;border-left-width:1px;border-right-color:#999999;border-right-style:solid;border-right-width:1px;border-top-color:#999999;border-top-style:solid;border-top-width:1px;color:black;cursor:hand;font-family:Tahoma;font-size:8pt;padding-bottom:1px;padding-left:1px;padding-right:1px;padding-top:1px;">
                                </submenu>
                            </eo:MenuItem>
                            <eo:MenuItem IsSeparator="True" ItemID="_Separator" NormalStyle-CssText="background-color:#c5c2b8;height:1px;margin-left:30px;width:1px;">
                            </eo:MenuItem>
                            <eo:MenuItem DisabledStyle-CssText="background-color:transparent;border-bottom-style:none;border-left-style:none;border-right-style:none;border-top-style:none;padding-bottom:1px;padding-left:2px;padding-right:5px;padding-top:1px;color:lightgrey" Height="24" HoverStyle-CssText="background-color:#c1d2ee;border-bottom-color:#316ac5;border-bottom-style:solid;border-bottom-width:1px;border-left-color:#316ac5;border-left-style:solid;border-left-width:1px;border-right-color:#316ac5;border-right-style:solid;border-right-width:1px;border-top-color:#316ac5;border-top-style:solid;border-top-width:1px;padding-left:1px;padding-right:4px;padding-top:0px;" ItemID="_Default" NormalStyle-CssText="background-color:transparent;border-bottom-style:none;border-left-style:none;border-right-style:none;border-top-style:none;padding-bottom:1px;padding-left:2px;padding-right:5px;padding-top:1px;" SelectedStyle-CssText="background-color:white;border-bottom-color:#316ac5;border-bottom-style:solid;border-bottom-width:1px;border-left-color:#316ac5;border-left-style:solid;border-left-width:1px;border-right-color:#316ac5;border-right-style:solid;border-right-width:1px;border-top-color:#316ac5;border-top-style:solid;border-top-width:1px;padding-left:1px;padding-right:4px;padding-top:0px;" Text-Padding-Right="30">
                                <submenu collapseeffect-duration="150" collapseeffect-type="Fade" expandeffect-duration="150" expandeffect-type="Fade" itemspacing="3" lefticoncellwidth="25" sideimage="OfficeXPSideBar" style-csstext="background-color:#fcfcf9;border-bottom-color:#999999;border-bottom-style:solid;border-bottom-width:1px;border-left-color:#999999;border-left-style:solid;border-left-width:1px;border-right-color:#999999;border-right-style:solid;border-right-width:1px;border-top-color:#999999;border-top-style:solid;border-top-width:1px;color:black;cursor:hand;font-family:Tahoma;font-size:8pt;padding-bottom:1px;padding-left:1px;padding-right:1px;padding-top:1px;">
                                </submenu>
                            </eo:MenuItem>
                        </lookitems>
                        <topgroup>
                            <Items>
                                <eo:MenuItem Text-Html="Cancel Sale">
                                </eo:MenuItem>
                            </Items>
                        </topgroup>
                    </eo:ContextMenu>

                    <eo:Grid ID="GrdSale" runat="server" OnItemCommand="GrdSale_ItemCommand" ClientSideOnContextMenu="ShowContextMenu" ClientSideOnItemSelected="OnItemSelected" BorderColor="#C7D1DF" BorderWidth="1px" ColumnHeaderAscImage="00050303" ColumnHeaderDescImage="00050304" ColumnHeaderDividerImage="00050302" FixedColumnCount="1" Font-Bold="False" Font-Italic="False" Font-Names="Verdana" Font-Overline="False" Font-Size="9pt" Font-Strikeout="False" Font-Underline="False" GridLineColor="199, 209, 223" GridLines="Both" Height="109px" ItemHeight="19" Width="762px">
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
                           <eo:RowNumberColumn HeaderText="RowNum">
                           </eo:RowNumberColumn>
                           <eo:StaticColumn HeaderText="DealReference" DataField="dealreference">
                           </eo:StaticColumn>
                           <eo:StaticColumn HeaderText="Customer" DataField="fullname">
                           </eo:StaticColumn>
                           <eo:StaticColumn HeaderText="DealAmt" DataField="dealamount">
                           </eo:StaticColumn>
                           <eo:StaticColumn HeaderText="Start" DataField="startdate">
                           </eo:StaticColumn>
                           <eo:StaticColumn HeaderText="Matures" DataField="Maturitydate">
                           </eo:StaticColumn>
                           <eo:StaticColumn HeaderText="YieldRate" DataField="interestrate">
                           </eo:StaticColumn>
                           <eo:StaticColumn HeaderText="DiscountRate" DataField="discountrate">
                           </eo:StaticColumn>
                           <eo:StaticColumn HeaderText="Tenor" DataField="tenor">
                           </eo:StaticColumn>
                           <eo:StaticColumn HeaderText="Remain" DataField="daystomaturity">
                           </eo:StaticColumn>
                           <eo:StaticColumn HeaderText="AcruedToDate" DataField="intaccruedtodate">
                           </eo:StaticColumn>
                           <eo:StaticColumn HeaderText="MaturityAmt" DataField="maturityamount">
                           </eo:StaticColumn>
                           <eo:StaticColumn DataField="purchaseref" HeaderText="SellType">
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
                           <eo:RowNumberColumn>
                           </eo:RowNumberColumn>
                           <eo:StaticColumn>
                           </eo:StaticColumn>
                       </columntemplates>
                       <FooterStyle CssText="padding-bottom:4px;padding-left:4px;padding-right:4px;padding-top:4px;" />
                    </eo:Grid>
                </div><!-- /.box-body -->
              </div><!-- /.box -->

            </div><!-- /.col (LEFT) -->
            <div class="col-md-6">
             <div class="box box-success">
                <div class="box-header with-border">
                  <h3 class="box-title">Capital Gain</h3>
                  <div class="box-tools pull-right">
                    <button class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i></button>
                    <%--<button class="btn btn-box-tool" data-widget="remove"><i class="fa fa-times"></i></button>--%>
                  </div>
                </div>
                <div class="box-body chart-responsive">
                  <div class="chart" id="bar-chart" style="height: 300px;"></div>
                    <asp:Label ID="lblinter" runat="server" Font-Bold="True" Font-Size="Medium" Text="Graph Interpretation" Visible="False"></asp:Label>
&nbsp;<table style="width:50%;">
                        <tr>
                            <td>
                                <asp:Label ID="Cvalue" runat="server" Text="CostValue" Visible="False"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblcost" runat="server"></asp:Label>
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Pvalue" runat="server" Text="PresentValue" Visible="False"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblvalue" runat="server" Visible="True"></asp:Label>
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Gain" runat="server" Text="Loss/Gain" Visible="False"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblGain" runat="server" Font-Bold="True" ForeColor="Black"></asp:Label>
                            </td>
                            <td>
                                <asp:Image ID="imgGainIcon" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Security/green-spin-arrow-up.gif" Visible="False" />
                                <asp:Image ID="ImaLossIcon" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Security/yellow-red-arrow-down.gif" Visible="False" />
                            </td>
                        </tr>
                    </table>
                </div><!-- /.box-body -->
              </div><!-- /.box -->
              
                    
                       </div>
                      </div>
                    </div>
                </div><!-- /.box-body -->
              </div><!-- /.box -->

            </div><!-- /.col (RIGHT) -->
          </div><!-- /.row -->
                                  
                     

                               <%--</center>--%>
                                   </ContentTemplate>

                  </ContentTemplate>
    </asp:UpdatePanel>
       
  </form>
    </head>
                         <!-- <button type="button" class="btn btn-block btn-success btn-xs" data-toggle="modal" data-target="#DealCodeDetail">
  Launch demo modal
</button>-->
                  <%--</div><!-- /.box-body -->
                 <%-- <div class="box-footer"> 
                     <asp:Button ID="btncancel" runat="server" Text="Cancel" CssClass="btn btn-default"  /><asp:Button ID="btnnew" runat="server" Text="New   " CssClass="btn btn-info pull-right" />
                    <%--<button type="submit" class="btn btn-default">Cancel</button>
                    <button type="submit" class="btn btn-info pull-right">Sign in</button>--%>
                 <%-- </div>--%><!-- /.box-footer --> 
      <script src="../../plugins/jQuery/jQuery-2.1.4.min.js"></script>
    <!-- Bootstrap 3.3.5 -->
    <script src="../../bootstrap/js/bootstrap.min.js"></script>
    <!-- Morris.js charts -->
    <script src="https://cdnjs.cloudflare.com/ajax/libs/raphael/2.1.0/raphael-min.js"></script>
    <script src="../plugins/morris/morris.min.js"></script>
    <!-- FastClick -->
    <script src="../../plugins/fastclick/fastclick.min.js"></script>
    <!-- AdminLTE App -->
    <script src="../../dist/js/app.min.js"></script>
    <!-- AdminLTE for demo purposes -->
    <script src="../../dist/js/demo.js"></script>
    <!-- page script -->
   
    <script>

        Sys.Application.add_init(appl_init)

        function appl_init() {
            var pgRegMgr = Sys.WebForms.PageRequestManager.getInstance()
            pgRegMgr.add_endRequest(pageLoad)
        }
    </script>
     <script>
         function pageLoad() {
             //alert('page !')
             

             <%--alert("Cost" + document.getElementById("<%=lblcost.ClientID%>").innerHTML)--%>
             //BAR CHART
             var bar = new Morris.Bar({
                 element: 'bar-chart',
                 resize: true,
                 data: [
                   { y: 'CostValue', a: document.getElementById("<%=lblcost.ClientID%>").innerHTML },
                  { y: 'PresentValue', b: document.getElementById("<%=lblvalue.ClientID%>").innerHTML },
                  { y: 'Gain/Loss', c: document.getElementById("<%=lblGain.ClientID%>").innerHTML }

                ],
                barColors: ['#00a65a', '#3c8dbc', '#f56954'],
                xkey: 'y',
                ykeys: ['a', 'b', 'c'],
                labels: ['CostValue'],
                labels: ['PresentValue'],
                labels: ['Gain/Loss'],
                hideHover: 'auto'
            });




         }
       
      
    </script>
         
       <script type="text/javascript">
           //<!--JS_SAMPLE_BEGIN-->

           var g_itemIndex = -1;
           var g_cellIndex = -1;

           function ShowContextMenu(e, grid, item, cell) {
               //Save the target cell index
               g_itemIndex = item.getIndex();
               g_cellIndex = cell.getColIndex();

               //Show the context menu
               var menu = eo_GetObject("<%=ContextMenu1.ClientID%>");
              eo_ShowContextMenu(e, "<%=ContextMenu1.ClientID%>");

              //Return true to indicate that we have
              //displayed a context menu
              return true;
          }


          function OnContextMenuItemClicked(e, eventInfo) {
              var grid = eo_GetObject("<%=GrdSale.ClientID%>");

            var item = eventInfo.getItem();
            switch (item.getText()) {

                case "Cancel Sale":
                    var x;
                    //Get the selected item
                    if (confirm("Cancel Sell ?") == true) {
                        x = 1;
                        var gridItem = grid.getItem(g_itemIndex);
           
                        var dealcode = gridItem.getCell(1).getValue()

                        window.open('cancelsale.aspx?dealref=' + dealcode, '_self', false)
                       //alert("kkjfkfk")
                        //$('#cancelsale').modal('show');
                      
                        //var item = grid.getSelectedItem();
                       

                        //grid.raiseItemCommandEvent(item.getIndex(), "Cancel");
                    };
                   
                    document.getElementById("demo").innerHTML = x;

                    
                    break;
           

                case "Save":
                 
            }
        }

        //<!--JS_SAMPLE_END-->
    </script>
    <script>
        function OnItemSelected(grid) {
            //Get the selected item
            var item = grid.getSelectedItem();

          
            grid.raiseItemCommandEvent(item.getIndex(), "select");
        }
        //<!--JS_SAMPLE_END-->
    </script>
  
   <%-- <script>
        function OnItemSelectedM(grid) {
            //Get the selected item
            var item = grid.getSelectedItem();


            grid.raiseItemCommandEvent(item.getIndex(), "select");
        }
        //<!--JS_SAMPLE_END-->
    </script--%>>
  
    
       

   
     
   </asp:Content>

