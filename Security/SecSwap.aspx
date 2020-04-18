

<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Main.Master" CodeBehind="SecuritySwap.aspx.vb" Inherits="WEBTDS.SecuritySwap" %>

<%@ Register Assembly="EO.Web" Namespace="EO.Web" TagPrefix="eo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   
 
    <%--  <div class="chart" id="mm" style="height: 300px; position: relative;"></div>--%>
  <head />
  <form id="form1" runat="server">
       <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
     <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
 
               

                <div class="box box-success">
           <div class="box-header with-border">
               <asp:Label ID="lblError" runat="server"></asp:Label>
        <asp:Label ID="lblsuccess" runat="server"></asp:Label>
                  <asp:Label ID="lblwarning" runat="server"> </asp:Label>
                  <h3 class="box-title">Security Swap </h3>
                  <div class="box-tools pull-right"> 
                    <%--<button class="btn btn-box-tool" data-widget="remove"><i class="fa fa-times"></i></button>--%>
                      <button class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i></button>
                  </div><!-- /.box-tools -->
                </div><!-- /.box-header -->
                <div class="box-body">

                
                     </div></div> 
                    </HeaderTemplate>
                              <ContentTemplate>
                               
                                  <hr />
                                   <div class="row">
            <div class="col-md-6">
              <!-- AREA CHART -->
              <div class="box box-primary">
                <div class="box-header with-border">
                  <h3 class="box-title">Using Other Collerteral</h3>
                  <div class="box-tools pull-right">
                    <button class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i></button>
                    <%--<button class="btn btn-box-tool" data-widget="remove"><i class="fa fa-times"></i></button>--%>
                  </div>
                </div>
                <div class="box-body chart-responsive">
                <%--  <div class="chart" id="mm" style="height: 300px; position: relative;"></div>--%>
                     <table style="width:100%;">
                         <tr>
                             <td style="width: 575px">
                                 <br />
                                 <asp:Label ID="Label10" runat="server" Font-Bold="True" Font-Size="Medium" Text="Available Collateral"></asp:Label>
                                 <br />
                                 <eo:Grid ID="GrdSec1" runat="server" BorderColor="#C7D1DF" BorderWidth="1px" ClientSideOnItemSelected="OnItemSelected" ColumnHeaderAscImage="00050303" ColumnHeaderDescImage="00050304" ColumnHeaderDividerImage="00050302" FixedColumnCount="1" Font-Bold="False" Font-Italic="False" Font-Names="Verdana" Font-Overline="False" Font-Size="9pt" Font-Strikeout="False" Font-Underline="False" GridLineColor="199, 209, 223" GridLines="Both" Height="91px" ItemHeight="19" OnItemCommand="GrdSec1_ItemCommand" Width="726px">
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
                                         <eo:StaticColumn DataField="collateralreference" HeaderText="Sec Reference" Width="250">
                                         </eo:StaticColumn>
                                         <eo:StaticColumn DataField="Ctype" HeaderText="Sec Type" Width="250">
                                         </eo:StaticColumn>
                                         <eo:StaticColumn DataField="collateraldescription" HeaderText="Description" Width="300">
                                         </eo:StaticColumn>
                                     </Columns>
                                     <columntemplates>
                                         <eo:RowNumberColumn>
                                         </eo:RowNumberColumn>
                                         <eo:StaticColumn>
                                         </eo:StaticColumn>
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
                                     </columntemplates>
                                     <FooterStyle CssText="padding-bottom:4px;padding-left:4px;padding-right:4px;padding-top:4px;" />
                                 </eo:Grid>
                                 <br />
                             </td>
                         </tr>
                         <tr>
                             <td style="width: 575px">
                                 <table style="width:100%; height: 170px;">
                                     <tr>
                                         <td style="width: 110px">
                                             <asp:Label ID="lblCustName" runat="server"></asp:Label>
                                         </td>
                                         <td style="width: 174px">&nbsp;</td>
                                         <td style="width: 93px">
                                             <asp:TextBox ID="txtcolDesc" runat="server" Enabled="False" Visible="False"></asp:TextBox>
                                         </td>
                                     </tr>
                                     <tr>
                                         <td style="width: 110px">
                                             <asp:Label ID="Label2" runat="server" Text="Currency"></asp:Label>
                                         </td>
                                         <td style="width: 174px">
                                             <asp:TextBox ID="lblCollCurrency" runat="server" Enabled="False" Font-Size="X-Small" required="required" style="margin-left: 0px" title="Please Enter Present value" Width="75%"></asp:TextBox>
                                             <br />
                                         </td>
                                         <td style="width: 93px">
                                             <asp:TextBox ID="lblRef" runat="server" Enabled="False" Visible="False"></asp:TextBox>
                                         </td>
                                     </tr>
                                     <tr>
                                         <td style="width: 110px">
                                             <asp:Label ID="Label3" runat="server" Text="Colleteral Value"></asp:Label>
                                         </td>
                                         <td style="width: 174px">
                                             <asp:TextBox ID="lblValue" runat="server" Enabled="False" Font-Size="X-Small" required="required" style="margin-left: 0px" title="Please Enter Present value" Width="75%"></asp:TextBox>
                                             <br />
                                         </td>
                                         <td style="width: 93px">
                                             <asp:TextBox ID="lblcustNumber" runat="server" Enabled="False" Visible="False"></asp:TextBox>
                                         </td>
                                     </tr>
                                     <tr>
                                         <td style="width: 110px">
                                             <asp:Label ID="Label4" runat="server" Text="Expiry Date"></asp:Label>
                                         </td>
                                         <td style="width: 174px">
                                             <asp:TextBox ID="lblExpiry" runat="server" Enabled="False" Font-Size="X-Small" required="required" style="margin-left: 0px" title="Please Enter Present value" Width="75%"></asp:TextBox>
                                             <br />
                                         </td>
                                         <td style="width: 93px">
                                             <asp:TextBox ID="lblsecref" runat="server" Enabled="False" Visible="False"></asp:TextBox>
                                         </td>
                                     </tr>
                                     <tr>
                                         <td style="width: 110px">
                                             <asp:Label ID="Label5" runat="server" Text="Amount Available To Secure"></asp:Label>
                                         </td>
                                         <td style="width: 174px">
                                             <asp:TextBox ID="lblCollAvailable" runat="server" Enabled="False" Font-Size="X-Small" required="required" style="margin-left: 0px" title="Please Enter Present value" Width="75%"></asp:TextBox>
                                             <br />
                                         </td>
                                         <td style="width: 93px">
                                             <asp:TextBox ID="txtsectypeD" runat="server" Enabled="False" Visible="False"></asp:TextBox>
                                         </td>
                                     </tr>
                                     <tr>
                                         <td style="width: 110px; height: 17px;">
                                             <asp:Label ID="Label6" runat="server" Text="Security Amount"></asp:Label>
                                         </td>
                                         <td style="width: 174px; height: 17px;">
                                             <asp:TextBox ID="txtCollateralLoan" runat="server" Enabled="False" Font-Size="X-Small" required="required" style="margin-left: 0px" title="Please Enter Present value" Width="75%"></asp:TextBox>
                                         </td>
                                         <td style="height: 17px; width: 93px;"><a href="MoneyMarket/MMDealBlotter.aspx">
                                             <asp:Button ID="btnAdd" runat="server" CssClass="btn btn-primary btn-block btn-flats" Height="40px" Text="Add" Width="150px" />
                                             </a></td>
                                     </tr>
                                 </table>
                             </td>
                         </tr>
                         <tr>
                             <td style="width: 575px">
                                 <asp:Label ID="Label9" runat="server" Font-Bold="True" Font-Size="Medium" Text="Assigned Collateral"></asp:Label>
                                 <br />
                                 <eo:Grid ID="Grdsec2" runat="server" BorderColor="#C7D1DF" BorderWidth="1px" ClientSideOnItemSelected="OnItemSelected1" ColumnHeaderAscImage="00050303" ColumnHeaderDescImage="00050304" ColumnHeaderDividerImage="00050302" FixedColumnCount="1" Font-Bold="False" Font-Italic="False" Font-Names="Verdana" Font-Overline="False" Font-Size="9pt" Font-Strikeout="False" Font-Underline="False" GridLineColor="199, 209, 223" GridLines="Both" Height="82px" ItemHeight="19" OnItemCommand="GrdSec2_ItemCommand" style="margin-right: 0px" Width="715px">
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
                                         <eo:StaticColumn DataField="collateralreference" HeaderText="Sec Ref" Width="300">
                                         </eo:StaticColumn>
                                         <eo:StaticColumn DataField="Ctype" HeaderText="Sec Type" Width="300">
                                         </eo:StaticColumn>
                                         <eo:StaticColumn DataField="securityamount" HeaderText="Amount" Width="300">
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
                                     </columntemplates>
                                     <FooterStyle CssText="padding-bottom:4px;padding-left:4px;padding-right:4px;padding-top:4px;" />
                                 </eo:Grid>
                                 <br />
                             </td>
                         </tr>
                         <tr>
                             <td   </td="">
                                 <table style="width:50%;">
                                     <tr>
                                         <td style="padding: 0; width: 295px">
                                             <asp:Label ID="Label7" runat="server" Text="Deal"></asp:Label>
                                         </td>
                                         <td style="padding: 0; width: 269px">
                                             <asp:Label ID="lblDealValue" runat="server"></asp:Label>
                                         </td>
                                         <td>
                                            
                                             <a href="MoneyMarket/MMDealBlotter.aspx">
                                             <asp:Button ID="btnRemove" runat="server" CssClass="btn btn-primary btn-block btn-flats" Height="40px" Text="Remove" Width="150px" />
                                             </a>
                                             <br />
                                         </td>
                                     </tr>
                                     <tr>
                                         <td style="padding: 0; width: 295px">
                                             <asp:Label ID="Label8" runat="server" Text="Total Security"></asp:Label>
                                         </td>
                                         <td style="padding: 0; width: 269px">
                                             <asp:Label ID="lblTotalCollateral" runat="server"></asp:Label>
                                         </td>
                                         <td>
                                             <asp:Button ID="btnExit0" runat="server" CssClass="btn btn-danger btn-block btn-flats" Height="40px" Text="Exit" Width="150px" />
                                             <asp:TextBox ID="txtselectedsec" runat="server" Enabled="False" Visible="False"></asp:TextBox>
                                             <asp:TextBox ID="txtamount" runat="server" Enabled="False" Visible="False"></asp:TextBox>
                                         </td>
                                     </tr>
                                    
                                 </table>
                             </td>
                         </tr>
                     </table>
                </div><!-- /.box-body -->
              </div><!-- /.box -->

             <!-- DONUT CHART -->
              <div class="box box-danger">
                <div class="box-header with-border">
                  <h3 class="box-title"></h3>
                  <div class="box-tools pull-right">
                    <button class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i></button>
                   <%-- <button class="btn btn-box-tool" data-widget="remove"><i class="fa fa-times"></i></button>--%>
                  </div>
                </div>
                <div class="box-body chart-responsive">
                  <%--<div class="chart" id="fx" style="height: 300px; position: relative;"></div>--%>
                </div><!-- /.box-body -->
              </div><!-- /.box -->

            </div><!-- /.col (LEFT) -->
            <div class="col-md-6">
             <div class="box box-success">
                <div class="box-header with-border">
                  <h3 class="box-title">Using Deals</h3>
                  <div class="box-tools pull-right">
                    <button class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i></button>
                    <button class="btn btn-box-tool" data-widget="remove"><i class="fa fa-times"></i></button>
                  </div>
                </div>
                <div class="box-body chart-responsive">
                  
                      <table style="width:50%;">
                          <tr>
                              <td style="width: 20%">
                                  <asp:Label ID="Label11" runat="server" Font-Bold="True" Font-Size="Medium" Text="Secured Deals"></asp:Label>
                                  <asp:TextBox ID="txtSecured" runat="server" Enabled="False"></asp:TextBox>
                                  <br />
                              </td>
                              <td>
                                  <asp:Label ID="Label13" runat="server" Font-Bold="True" Font-Size="Small" Text="Current Security"></asp:Label>
                                  <br />
                                  <br />
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
                                              <eo:MenuItem Text-Html="Remove">
                                              </eo:MenuItem>
                                          </Items>
                                      </topgroup>
                                  </eo:ContextMenu>
                                  <eo:Grid ID="GrdSecured" runat="server" OnItemCommand="GrdSecured_ItemCommand" ClientSideOnContextMenu="ShowContextMenu" ClientSideOnItemSelected="OnItemSelected3" BorderColor="#C7D1DF" BorderWidth="1px" ColumnHeaderAscImage="00050303" ColumnHeaderDescImage="00050304" ColumnHeaderDividerImage="00050302" FixedColumnCount="1" Font-Bold="False" Font-Italic="False" Font-Names="Verdana" Font-Overline="False" Font-Size="9pt" Font-Strikeout="False" Font-Underline="False" GridLineColor="199, 209, 223" GridLines="Both" Height="200px" ItemHeight="19" Width="608px">
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
                                          <eo:StaticColumn DataField="tb_id" HeaderText="Sec Ref" Width="150">
                                          </eo:StaticColumn>
                                          <eo:StaticColumn DataField="securityamount" HeaderText="Sec Amount" Width="150">
                                          </eo:StaticColumn>
                                          <eo:StaticColumn DataField="dealreferencesecurity" HeaderText="Sec Description" Width="150">
                                          </eo:StaticColumn>
                                      </Columns>
                                      <columntemplates>
                                          <eo:RowNumberColumn>
                                          </eo:RowNumberColumn>
                                          <eo:StaticColumn>
                                          </eo:StaticColumn>
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
                                      </columntemplates>
                                      <FooterStyle CssText="padding-bottom:4px;padding-left:4px;padding-right:4px;padding-top:4px;" />
                                  </eo:Grid>
                                  <br />
                              </td>
                              <%--  <td>&nbsp;</td>--%>
                          </tr>
                          <tr>
                              <td style="width: 20%">
                                  <asp:Label ID="Label12" runat="server" Font-Bold="True" Font-Size="Medium" Text="Available Securities"></asp:Label>
                                  <br />
                                  <asp:DropDownList ID="cmbUnsecured" runat="server" AutoPostBack="True" class="form-control select2" Font-Size="Small" style="margin-left: 0px" Width="100%">
                                      <asp:ListItem Text="Select Security" Value="0"></asp:ListItem>
                                  </asp:DropDownList>
                                  <br />
                              </td>
                              <td>
                                  <table style="width:100%;">
                                      <tr>
                                          <td style="width: 180px">
                                              <asp:Label ID="Label14" runat="server" Text="Purchase Value"></asp:Label>
                                          </td>
                                          <td>
                                              <asp:Label ID="PurValue" runat="server"></asp:Label>
                                              <br />
                                          </td>
                                          <td>
                                              <asp:TextBox ID="lblcurr" runat="server" Enabled="False" Visible="False"></asp:TextBox>
                                          </td>
                                      </tr>
                                      <tr>
                                          <td style="width: 180px">
                                              <asp:Label ID="Label15" runat="server" Text="Inerest Rate"></asp:Label>
                                          </td>
                                          <td>
                                              <asp:Label ID="IntRate" runat="server"></asp:Label>
                                              <br />
                                          </td>
                                          <td>
                                              <asp:TextBox ID="txtDealsRef" runat="server" Enabled="False" Visible="False"></asp:TextBox>
                                          </td>
                                      </tr>
                                      <tr>
                                          <td style="width: 180px">
                                              <asp:Label ID="Label16" runat="server" Text="Maturity Date"></asp:Label>
                                          </td>
                                          <td>
                                              <asp:Label ID="maturitySecurity" runat="server"></asp:Label>
                                              <br />
                                          </td>
                                          <td>&nbsp;</td>
                                      </tr>
                                      <tr>
                                          <td style="width: 180px">
                                              <asp:Label ID="Label17" runat="server" Text="Days To Maturity"></asp:Label>
                                          </td>
                                          <td>
                                              <asp:Label ID="DaysMaturity" runat="server"></asp:Label>
                                              <br />
                                          </td>
                                          <td>&nbsp;</td>
                                      </tr>
                                      <tr>
                                          <td style="width: 180px">
                                              <asp:Label ID="Label18" runat="server" Text="Amount Available To Secure"></asp:Label>
                                          </td>
                                          <td>
                                              <asp:Label ID="AvalableForSale" runat="server"></asp:Label>
                                          </td>
                                          <td>
                                              <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary btn-block btn-flats" Height="40px" Text="OK" Width="150px" />
                                              <br />
                                          </td>
                                      </tr>
                                      <tr>
                                          <td style="width: 180px">
                                              <asp:Label ID="Label19" runat="server" Font-Bold="True" Text="Security Amount"></asp:Label>
                                          </td>
                                          <td>
                                              <asp:TextBox ID="txtSale" runat="server"></asp:TextBox>
                                          </td>
                                          <td>
                                              &nbsp;</td>
                                      </tr>
                                  </table>
                              </td>
                              <%--<td>&nbsp;</td>--%>
                          </tr>
                          <tr>
                              <td style="width: 22%">&nbsp;</td>
                              <td>
                                  <br />
                                  <asp:TextBox ID="txtmatured" runat="server" Enabled="False" Visible="False"></asp:TextBox>
                                  <asp:TextBox ID="lblTB" runat="server" Enabled="False" Visible="False"></asp:TextBox>
                                  <asp:TextBox ID="lblREmeveAmt" runat="server" Enabled="False" Visible="False"></asp:TextBox>
                                  <br />
                                  <br />
                              </td>
                              <%-- <td>&nbsp;</td>--%>
                          </tr>
                          <caption>
                              <hr />
                          </caption>
                      </table>
                    <%--</div>--%>
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

  
    <script>
        function OnItemSelected(grid) {
            //Get the selected item
            var item = grid.getSelectedItem();

            //Raises server side ItemCommand event.
            //The first parameter is the item index.
            //The second parameter is an additional
            //value that you pass to the server side.
            //This value is not used by the Grid
            grid.raiseItemCommandEvent(item.getIndex(), "select");
        }
        function OnItemSelected1(grid) {
            //Get the selected item
            var item = grid.getSelectedItem();

            //Raises server side ItemCommand event.
            //The first parameter is the item index.
            //The second parameter is an additional
            //value that you pass to the server side.
            //This value is not used by the Grid
            grid.raiseItemCommandEvent(item.getIndex(), "select");
        }
        //<!--JS_SAMPLE_END-->
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
            var grid = eo_GetObject("<%=GrdSecured.ClientID%>");

         var item = eventInfo.getItem();
         switch (item.getText()) {

             case "Remove":
                 var x;
                 //Get the selected item
                 if (confirm("Remove Deal Security?") == true) {
                     x = 1;
                     var item = grid.getSelectedItem();


                     grid.raiseItemCommandEvent(item.getIndex(), "Remove");
                 };
                 document.getElementById("demo").innerHTML = x;


                 break;
           

             case "Save":
                 //Save menu item's RaisesServerEvent is set to true,
                 //so the event is handled on the server side
         }
     }

     //<!--JS_SAMPLE_END-->
    </script>
    <script>
        function OnItemSelected3(grid) {
            //Get the selected item
            var item = grid.getSelectedItem();

            //Raises server side ItemCommand event.
            //The first parameter is the item index.
            //The second parameter is an additional
            //value that you pass to the server side.
            //This value is not used by the Grid
            grid.raiseItemCommandEvent(item.getIndex(), "select");
        }
        //<!--JS_SAMPLE_END-->
    </script>
<script>
    function myFunction() {
        var x;
        if (confirm("Press a button!") == true) {
            x = 1;
        } else {
            x = 0;
        }
        //document.getElementById("demo").innerHTML = x;



    }
</script>
   </asp:Content>


     


