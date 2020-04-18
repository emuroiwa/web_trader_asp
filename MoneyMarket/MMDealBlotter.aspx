
<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Main.Master" CodeBehind="MMDealBlotter.aspx.vb" Inherits="WEBTDS.MMDealBlotter" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="ajaxToolkit" %>
<%@ Register assembly="EO.Web" namespace="EO.Web" tagprefix="eo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     
    <head />
    
 
 
   
  <form id="form1" runat="server">
      
       <!-- Horizontal Form -->
              <div class="box box-info">
                <div class="box-header with-border">
                    <h3 class="box-title">MM Deal Blotter</h3> 
                    <%--<td 
                                   <asp:Panel ID="Panel1" runat="server" style="width: 292px" >--%>
                </div><!-- /.box-header -->
                <!-- form start -->
            

                  <div class="box-body">
                      <asp:ScriptManager ID="ScriptManager2" runat="server"></asp:ScriptManager>
                      <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                      <%--<button type="button" class="btn btn-success">Avg Rate <asp:Label ID="avgRate" runat="server" Text="Label"></asp:Label></button>     
                            <button type="button" class="btn btn-success">Avg Size <asp:Label ID="avgSize" runat="server"></asp:Label></button>      
                            <button type="button" class="btn btn-success">Avg Tenor <asp:Label ID="avgTenor" runat="server"></asp:Label></button>--%>
                     <ContentTemplate>
                     
                      <table style="width: ;">
                          <%--<button type="button" class="btn btn-success">Avg Rate <asp:Label ID="avgRate" runat="server" Text="Label"></asp:Label></button>     
                            <button type="button" class="btn btn-success">Avg Size <asp:Label ID="avgSize" runat="server"></asp:Label></button>      
                            <button type="button" class="btn btn-success">Avg Tenor <asp:Label ID="avgTenor" runat="server"></asp:Label></button>--%>
                          <tr>
                              <td> <%--<td 
                                   <asp:Panel ID="Panel1" runat="server" style="width: 292px" >--%>

                           <table style="width:50%;">
                                             <%-- <Triggers>
                <asp:AsyncPostBackTrigger ControlID="cmbCustomerLoan" EventName="SelectedIndexChanged" />
            </Triggers>--%>
                                              <tr>
                                                  <td style="width: 20%; height: 37px;"></td>
                                                  <td style="width: 13%; height: 37px;"> 
                                                      <asp:CheckBox ID="chkBackoffice" runat="server" Text="Back Ofice Users" AutoPostBack="true" />
                                                 
                                                  </td>
                                                 
                                              </tr>
                                              <tr>
                                                  <td style="width: 20%; height: 37px;"> </td>
                                                  <td style="width: 13%; height: 37px;"> 
                                                 <asp:CheckBox ID="CheckFrontAuthoriser" runat="server" AutoPostBack="true" Text="Front Office users" />
                                                  </td>
                                                 
                                              </tr>
                                             <%--<button type="button" class="btn btn-success">Avg Rate <asp:Label ID="avgRate" runat="server" Text="Label"></asp:Label></button>     
                            <button type="button" class="btn btn-success">Avg Size <asp:Label ID="avgSize" runat="server"></asp:Label></button>      
                            <button type="button" class="btn btn-success">Avg Tenor <asp:Label ID="avgTenor" runat="server"></asp:Label></button>--%>
                                               
                                               <tr>
                                                  <td style="width: 20%; height: 22px;"></td>
                                                  <td style="width: 13%; height: 22px;">Filter Options</td>
                                                 
                                              </tr>
                                               <tr>
                                                  <td style="width: 20%; padding-bottom:20px" >
                                <asp:Label ID="Label14" runat="server" Font-Size="Medium" Text="Status" Font-Bold="True"></asp:Label></td>
                                                  <td style="width:13%">
                  <asp:DropDownList ID="cmbDealStatus" runat="server" AutoPostBack="true" class="form-control select2" style="width: 100%;">
                      <asp:ListItem>Live</asp:ListItem>
                      <asp:ListItem>Matured</asp:ListItem>
                      <asp:ListItem>Cancelled</asp:ListItem>
                      <asp:ListItem>Authorisation Pending A</asp:ListItem>
                      <asp:ListItem>Authorisation Pending B</asp:ListItem>
                      <asp:ListItem>Verification Pending</asp:ListItem>
                      <asp:ListItem>Authorisation Declined</asp:ListItem>
                      <asp:ListItem>Verification Declined</asp:ListItem>
                                                      </asp:DropDownList></td>
                                                  
                                              </tr>
                                               <tr>
                                                  <td style="width: 20%; padding-bottom:20px" >
                                <asp:Label ID="Label1" runat="server" Font-Size="Medium" Text="Dealer " Font-Bold="True"></asp:Label></td>
                                                  <td style="width:13%">
                  <asp:DropDownList ID="cmbDealer" runat="server" AutoPostBack="true" class="form-control select2" style="width: 100%;"></asp:DropDownList></td>
                                                  <td style="width: 10px">&nbsp;</td>
                                              </tr>
                                               <tr>
                                                  <td style="width: 20%; padding-bottom:20px" >
                                <asp:Label ID="Label2" runat="server" Font-Size="Medium" Text="Currency" Font-Bold="True"></asp:Label></td>
                                                  <td style="width:13%">
                  <asp:DropDownList ID="cmbcurrency" runat="server" AutoPostBack="true" class="form-control select2" style="width: 100%;"></asp:DropDownList></td>
                                                  <td style="width: 10px">&nbsp;</td>
                                              </tr>
                                             <%--<button type="button" class="btn btn-success">Avg Rate <asp:Label ID="avgRate" runat="server" Text="Label"></asp:Label></button>     
                            <button type="button" class="btn btn-success">Avg Size <asp:Label ID="avgSize" runat="server"></asp:Label></button>      
                            <button type="button" class="btn btn-success">Avg Tenor <asp:Label ID="avgTenor" runat="server"></asp:Label></button>--%>
                                                <tr>
                                                  <td style="width: 20%">&nbsp;</td>
                                                  <td  style="align-content:center;padding-bottom:10px; width: 13%;">
                     <asp:Button ID="btnClear" runat="server" CssClass="btn btn-warning btn-block btn-flats" Height="40px" Text="Clear Filters" Width="150px" /></td>
                                                  
                                              </tr>
                                                <tr>
                                                  <td style="width: 20%"> </td>
                                                  <td style="align-content:center;padding-bottom:10px; width: 13%;">
                     <asp:Button ID="btnExit" runat="server" CssClass="btn btn-danger btn-block btn-flats" Height="40px" Text="Exit" Width="150px" /></td>
                                                  
                                              </tr>
                                          </table>
                    
                      <%--</asp:Panel>--%>

                    </td>
                              <td>&nbsp;</td>
                              <td>
                                  <eo:ContextMenu ID="Menu1" runat="server" ClientSideOnItemClick="OnContextMenuItemClicked" ControlSkinID="None"  Width="144px">
                                      <TopGroup Style-CssText="cursor:hand;font-family:Verdana;font-size:11px;">
                                          <Items>
                                              <eo:MenuItem Text-Html="Deal Operations">
                                                  <SubMenu>
                                                      <Items>
                                                          <eo:MenuItem Text-Html="Amend Deal Tax">
                                                          </eo:MenuItem>
                                                          <eo:MenuItem Text-Html="Amend Deal Interest Rate">
                                                          </eo:MenuItem>
                                                          <eo:MenuItem Text-Html="Change Maturity Date">
                                                          </eo:MenuItem>
                                                          <eo:MenuItem Text-Html="Change Principle">
                                                          </eo:MenuItem>
                                                          <eo:MenuItem Text-Html="Amend Settlement Details">
                                                          </eo:MenuItem>
                                                          <eo:MenuItem Text-Html="Change Deal Instructions">
                                                          </eo:MenuItem>
                                                          <eo:MenuItem Text-Html="Amend Security Details">
                                                          </eo:MenuItem>
                                                          <eo:MenuItem Text-Html="Cancel Deal">
                                                          </eo:MenuItem>
                                                      </Items>
                                                  </SubMenu>
                                              </eo:MenuItem>
                                              <eo:MenuItem Text-Html="Filtering options">
                                              </eo:MenuItem>
                                              <eo:MenuItem RaisesServerEvent="True" Text-Html="Cancel Reason/Comment">
                                              </eo:MenuItem>
                                              <eo:MenuItem Text-Html="Deal Verification">
                                                  <SubMenu>
                                                      <Items>
                                                          <eo:MenuItem Text-Html="Accept">
                                                          </eo:MenuItem>
                                                          <eo:MenuItem Text-Html="Decline">
                                                          </eo:MenuItem>
                                                      </Items>
                                                  </SubMenu>
                                              </eo:MenuItem>
                                              <eo:MenuItem Text-Html="Export To Excel">
                                              </eo:MenuItem>
                                              <eo:MenuItem Text-Html="Deal Postings Enquiry">
                                              </eo:MenuItem>
                                              <eo:MenuItem Text-Html="Re-Print Deal Slip">
                                              </eo:MenuItem>
                                          </Items>
                                      </TopGroup>
                                      <LookItems>
                                          <eo:MenuItem IsSeparator="True" ItemID="_Separator" NormalStyle-CssText="background-color:#E0E0E0;height:1px;width:1px;">
                                          </eo:MenuItem>
                                          <eo:MenuItem HoverStyle-CssText="color:#F7B00A;padding-left:5px;padding-right:5px;" ItemID="_Default" NormalStyle-CssText="padding-left:5px;padding-right:5px;">
                                              <SubMenu CollapseEffect-Type="GlideTopToBottom" ExpandEffect-Type="GlideTopToBottom" ItemSpacing="5" OffsetX="3" OffsetY="-4" ShadowDepth="0" Style-CssText="border-right: #e0e0e0 1px solid; padding-right: 3px; border-top: #e0e0e0 1px solid; padding-left: 3px; font-size: 12px; padding-bottom: 3px; border-left: #e0e0e0 1px solid; cursor: hand; color: #606060; padding-top: 3px; border-bottom: #e0e0e0 1px solid; font-family: arial; background-color: #f7f8f9">
                                              </SubMenu>
                                          </eo:MenuItem>
                                      </LookItems>
                                  </eo:ContextMenu>
                                  <eo:Grid ID="GrdDealsMM" runat="server" BorderColor="#C7D1DF" BorderWidth="1px" ColumnHeaderAscImage="00050303" ColumnHeaderDescImage="00050304" ColumnHeaderDividerImage="00050302" FixedColumnCount="1" Font-Bold="False" Font-Italic="False" Font-Names="Verdana" Font-Overline="False" Font-Size="9pt" Font-Strikeout="False" Font-Underline="False" GridLineColor="199, 209, 223" GridLines="Both" Height="252px" ItemHeight="19" Width="993px" ClientSideOnContextMenu="ShowContextMenu" ClientSideOnItemSelected="OnItemSelected"
        OnItemCommand="GrdDealsMM_ItemCommand">
                                      <ItemStyles>
                                          <eo:GridItemStyleSet>
                                              <ItemStyle CssText="background-color: white" />
                                              <ItemHoverStyle CssText="background-image: url(00050206); background-repeat: repeat-x" />
                                              <SelectedStyle CssText="background-image: url(00050207); background-repeat: repeat-x" />
                                              <CellStyle CssText="padding-left:8px;padding-top:2px; color:#336699;white-space:nowrap;" />
                                          </eo:GridItemStyleSet>
                                      </ItemStyles>
                                      <ColumnHeaderStyle CssText="background-image:url('00050301');padding-left:8px;padding-top:2px;font-weight: bold;color:white;" />
                                      <Columns>
                                          <eo:RowNumberColumn HeaderText="RowNum" Name="rownum">
                                          </eo:RowNumberColumn>
                                          <eo:StaticColumn DataField="DealReference" HeaderText="DealRef">
                                          </eo:StaticColumn>
                                          <eo:StaticColumn HeaderText="Customer" DataField="FullName">
                                          </eo:StaticColumn>
                                          <eo:StaticColumn DataField="CustomerNumber" HeaderText="CustNumber">
                                          </eo:StaticColumn>
                                          <eo:StaticColumn DataField="DealAmount" HeaderText="Deal Amount">
                                          </eo:StaticColumn>
                                          <eo:StaticColumn DataField="InterestRate" HeaderText="InterestRate">
                                          </eo:StaticColumn>
                                          <eo:StaticColumn DataField="Tenor" HeaderText="Tenor">
                                          </eo:StaticColumn>
                                          <eo:StaticColumn DataField="StartDate" HeaderText="Starts">
                                          </eo:StaticColumn>
                                          <eo:StaticColumn DataField="MaturityDate" HeaderText="Matures">
                                          </eo:StaticColumn>
                                          <eo:StaticColumn DataField="DealCapturer" HeaderText="Dealer">
                                          </eo:StaticColumn>
                                      </Columns>
                                      <ColumnTemplates>
                                          <eo:TextBoxColumn>
                                              <TextBoxStyle CssText="BORDER-RIGHT: #7f9db9 1px solid; PADDING-RIGHT: 2px; BORDER-TOP: #7f9db9 1px solid; PADDING-LEFT: 2px; FONT-SIZE: 8.75pt; PADDING-BOTTOM: 1px; MARGIN: 0px; BORDER-LEFT: #7f9db9 1px solid; PADDING-TOP: 2px; BORDER-BOTTOM: #7f9db9 1px solid; FONT-FAMILY: Tahoma" />
                                          </eo:TextBoxColumn>
                                          <eo:DateTimeColumn>
                                              <DatePicker ControlSkinID="None" DayCellHeight="16" DayCellWidth="19" DayHeaderFormat="FirstLetter" DisabledDates="" OtherMonthDayVisible="True" SelectedDates="" TitleLeftArrowImageUrl="DefaultSubMenuIconRTL" TitleRightArrowImageUrl="DefaultSubMenuIcon">
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
                                              </DatePicker>
                                          </eo:DateTimeColumn>
                                          <eo:MaskedEditColumn>
                                              <MaskedEdit ControlSkinID="None" TextBoxStyle-CssText="BORDER-RIGHT: #7f9db9 1px solid; PADDING-RIGHT: 2px; BORDER-TOP: #7f9db9 1px solid; PADDING-LEFT: 2px; PADDING-BOTTOM: 1px; MARGIN: 0px; BORDER-LEFT: #7f9db9 1px solid; PADDING-TOP: 2px; BORDER-BOTTOM: #7f9db9 1px solid; font-family:Courier New;font-size:8pt;">
                                              </MaskedEdit>
                                          </eo:MaskedEditColumn>
                                      </ColumnTemplates>
                                      <FooterStyle CssText="padding-bottom:4px;padding-left:4px;padding-right:4px;padding-top:4px;" />
                                  </eo:Grid>
                                   <p>
                                       &nbsp;</p>
                              </td>
                             
                          </tr>
                          <%-- <Triggers>
                <asp:AsyncPostBackTrigger Contr olID="cmbCustomerLoan" EventName="SelectedIndexChanged" />
            </Triggers>--%>
                      </table>
        <eo:CallbackPanel ID="CallbackPanel2" runat="server" Height="150px" Width="100%" Triggers="{ControlID:txtdealref;Parameter:}">
                  
            <ContentTemplate>
                <fieldset title="Security">
                   <div class="row">
            <div class="col-md-6">
                       <div class="panel box box-success">
                      <div class="box-header with-border">
                        <h4 class="box-title">
                          <a data-toggle="collapse" data-parent="#accordion" href="#history1">
                             Details
                          </a>
                        </h4>
                      </div>
                      <div id="history1" class="panel-collapse collapse-in">
                        <div class="box-body">
                            
<table style="width: 100%;">
                                <tr>
                                    <td style="width: 135px; padding-bottom:20px" >
                                <asp:Label ID="Label3" runat="server" Font-Size="Small" Text="Deal Reference"></asp:Label></td>
                                    <td style="width: 172px; padding-bottom:20px" >
                                        <asp:TextBox ID="txtdealref" runat="server" AutoPostBack="true" Enabled="False" BackColor="White" ForeColor="White" BorderColor="White"></asp:TextBox>
                                    </td></td>
                                    <td style="width: 140px; padding-bottom:20px" >
                                        <asp:Label ID="Label60" runat="server" Font-Size="Small" Text="Yield Rate"></asp:Label>
                                    </td></td>
                                    <td style="width: 190px; padding-bottom:20px" >
                                        <asp:Label ID="lblYieldRate" runat="server" Font-Size="Small" BackColor="#66FFFF"></asp:Label>
                                    </td></td>
                                    <td style="width: 196px; padding-bottom:20px" >
                                <asp:Label ID="Label7" runat="server" Font-Size="Small" Text="Currency"></asp:Label></td></td>
                                    <td style="width: 82px; padding-bottom:20px" >
                                <asp:Label ID="lblcurrency" runat="server" Font-Size="Small" BackColor="#66FFFF"></asp:Label></td></td>
                                   <%-- <td style="width: 437px; padding-bottom:20px" >
                                        &nbsp;</td></td>
                                    <td style="width: 291px; padding-bottom:20px" >
                                        &nbsp;</td></td>--%>
                                    
                                </tr>
                               <tr>
                                    <td style="width: 135px; padding-bottom:20px" >
                                <asp:Label ID="Label11" runat="server" Font-Size="Small" Text="External Ref"></asp:Label></td>
                                    <td style="width: 172px; padding-bottom:20px" >
                                <asp:Label ID="fccRef" runat="server" Font-Size="Small" BackColor="#66FFFF"></asp:Label></td></td>
                                    <td style="width: 140px; padding-bottom:20px" >
                                        <asp:Label ID="Label68" runat="server" Font-Size="Small" Text="Discount Rate"></asp:Label>
                                    </td></td>
                                    <td style="width: 190px; padding-bottom:20px" >
                                        <asp:Label ID="lblDiscountRate" runat="server" Font-Size="Small" BackColor="#66FFFF"></asp:Label>
                                    </td></td>
                                    <td style="width: 196px; padding-bottom:20px" >
                                <asp:Label ID="Label16" runat="server" Font-Size="Small" Text="Client Full Name"></asp:Label></td></td>
                                    <td style="width: 82px; padding-bottom:20px" >
                                <asp:Label ID="lblcustomer" runat="server" Font-Size="Small" BackColor="#66FFFF"></asp:Label></td></td>
                                   <%-- <td style="width: 437px; padding-bottom:20px" >
                                        &nbsp;</td></td>
                                    <td style="width: 291px; padding-bottom:20px" >
                                        &nbsp;</td></td>--%>
                                    
                                </tr>
                                <tr>
                                    <td style="width: 135px; padding-bottom:20px" >
                                        <asp:Label ID="Label36" runat="server" Font-Size="Small" Text="Deal Amount"></asp:Label>
                                    </td>
                                    <td style="width: 172px; padding-bottom:20px" >
                                        <asp:Label ID="lblAmount" runat="server" Font-Size="Small" BackColor="#66FFFF"></asp:Label>
                                    </td></td>
                                    <td style="width: 140px; padding-bottom:20px" >
                                        <asp:Label ID="Label62" runat="server" Font-Size="Small" Text="Acrued To Date"></asp:Label>
                                    </td></td>
                                    <td style="width: 190px; padding-bottom:20px" >
                                        <asp:Label ID="lblAccrued" runat="server" Font-Size="Small" BackColor="#66FFFF"></asp:Label>
                                    </td></td>
                                    <td style="width: 196px; padding-bottom:20px" >
                                <asp:Label ID="lblstartdate" runat="server" Font-Size="Small" Text="Start Date"></asp:Label></td></td>
                                    <td style="width: 82px; padding-bottom:20px" >
                                <asp:Label ID="lblStart" runat="server" Font-Size="Small" BackColor="#66FFFF"></asp:Label></td></td>
                                   <%-- <td style="width: 437px; padding-bottom:20px" >
                                        &nbsp;</td></td>
                                    <td style="width: 291px; padding-bottom:20px" >
                                        &nbsp;</td></td>--%>
                                    
                                </tr>
                               <tr>
                                    <td style="width: 135px; padding-bottom:20px" >
                                        <asp:Label ID="Label44" runat="server" Font-Size="Small" Text="Maturity Amount"></asp:Label>
                                    </td>
                                    <td style="width: 172px; padding-bottom:20px" >
                                        <asp:Label ID="lblMaturityAm" runat="server" Font-Size="Small" BackColor="#66FFFF"></asp:Label>
                                    </td></td>
                                    <td style="width: 140px; padding-bottom:20px" >
                                        <asp:Label ID="Label48" runat="server" Font-Size="Small" Text="Tenor"></asp:Label>
                                    </td></td>
                                    <td style="width: 190px; padding-bottom:20px" >
                                        <asp:Label ID="lbltenor" runat="server" Font-Size="Small" BackColor="#66FFFF"></asp:Label>
                                    </td></td>
                                    <td style="width: 196px; padding-bottom:20px" >
                                <asp:Label ID="Label32" runat="server" Font-Size="Small" Text="Maturity Date"></asp:Label></td></td>
                                    <td style="width: 82px; padding-bottom:20px" >
                                <asp:Label ID="lblMaturityDate" runat="server" Font-Size="Small" BackColor="#66FFFF"></asp:Label></td></td>
                                    <%--<td style="width: 437px; padding-bottom:20px" >
                                        &nbsp;</td></td>
                                    <td style="width: 291px; padding-bottom:20px" >
                                        &nbsp;</td></td>--%>
                                    
                                </tr>
                                <tr>
                                    <td style="width: 135px; padding-bottom:20px" >
                                        <asp:Label ID="Label52" runat="server" Font-Size="Small" Text="Intrest Days Basis"></asp:Label>
                                    </td>
                                    <td style="width: 172px; padding-bottom:20px" >
                                        <asp:Label ID="lblintDaysbasis" runat="server" Font-Size="Small" BackColor="#66FFFF"></asp:Label>
                                    </td></td>
                                    <td style="width: 140px; padding-bottom:20px" >
                                        <asp:Label ID="Label56" runat="server" Font-Size="Small" Text="Tax Rate"></asp:Label>
                                    </td></td>
                                    <td style="width: 190px; padding-bottom:20px" >
                                        <asp:Label ID="lblTaxRate" runat="server" Font-Size="Small" BackColor="#66FFFF"></asp:Label>
                                    </td></td>
                                    <td style="width: 196px; padding-bottom:20px" >
                                        <asp:Label ID="Label50" runat="server" Font-Size="Small" Text="Days to Maturity"></asp:Label>
                                    </td></td>
                                    <td style="width: 82px; padding-bottom:20px" >
                                        <asp:Label ID="lblRemain" runat="server" Font-Size="Small" BackColor="#66FFFF"></asp:Label>
                                   <%-- </td></td>
                                    <td style="width: 437px; padding-bottom:20px" >
                                        &nbsp;</td></td>
                                    <td style="width: 291px; padding-bottom:20px" >
                                        &nbsp;</td></td>--%>
                                    
                                </tr>
                                <tr>
                                    <td style="width: 135px; padding-bottom:20px" >
                                        <asp:Label ID="Label76" runat="server" Font-Size="Small" Text="Deal Status"></asp:Label>
                                    </td>
                                    <td style="width: 172px; padding-bottom:20px" >
                                        <asp:Label ID="lblStatus" runat="server" Font-Size="Small" BackColor="#66FFFF"></asp:Label>
                                    </td></td>
                                    <td style="width: 140px; padding-bottom:20px" >
                                        <asp:Label ID="Label58" runat="server" Font-Size="Small" Text="Tax Amount"></asp:Label>
                                    </td></td>
                                    <td style="width: 190px; padding-bottom:20px" >
                                        <asp:Label ID="lblTaxAmount" runat="server" Font-Size="Small" BackColor="#66FFFF"></asp:Label>
                                    </td></td>
                                    <td style="width: 196px; padding-bottom:20px" >
                                        &nbsp;</td></td>
                                    <td style="width: 82px; padding-bottom:20px" >
                                        &nbsp;</td></td>
                                   <%-- <td style="width: 437px; padding-bottom:20px" >
                                        &nbsp;</td></td>
                                    <td style="width: 291px; padding-bottom:20px" >
                                        &nbsp;</td></td>--%>
                                    
                                </tr>
                              <tr>
                                    <td style="width: 135px; padding-bottom:20px" >
                                        <asp:Label ID="Label84" runat="server" Font-Size="Small" Text="Deal Capturer"></asp:Label>
                                    </td>
                                    <td style="width: 172px; padding-bottom:20px" >
                                        <asp:Label ID="lblDealCapturer" runat="server" Font-Size="Small" BackColor="#66FFFF"></asp:Label>
                                    </td></td>
                                    <td style="width: 140px; padding-bottom:20px" >
                                        <asp:Label ID="lblreason" runat="server" Font-Size="Small"></asp:Label>
                                    </td></td>
                                    <td style="width: 190px; padding-bottom:20px" >
                                        <asp:Label ID="lblOtherStatus" runat="server" Font-Size="Small" BackColor="#66FFFF"></asp:Label>
                                    </td></td>
                                    <td style="width: 196px; padding-bottom:20px" >
                                        &nbsp;</td></td>
                                    <td style="width: 82px; padding-bottom:20px" >
                                        &nbsp;</td></td>
                                    <%--<td style="width: 437px; padding-bottom:20px" >
                                        &nbsp;</td></td>
                                    <td style="width: 291px; padding-bottom:20px" >
                                        &nbsp;</td></td>--%>
                                    
                                </tr>
                                <tr>
                                    <td style="width: 135px; padding-bottom:20px" >
                                        &nbsp;</td>
                                    <td style="width: 172px; padding-bottom:20px" >
                                        &nbsp;</td></td>
                                    <td style="width: 140px; padding-bottom:20px" >
                                        &nbsp;</td></td>
                                    <td style="width: 190px; padding-bottom:20px" >
                                        <asp:Label ID="lblDealCancelled" runat="server" Font-Size="Medium" BackColor="Yellow"></asp:Label>
                                    </td></td>
                                    <td style="width: 196px; padding-bottom:20px" >
                                        &nbsp;</td></td>
                                    <td style="width: 82px; padding-bottom:20px" >
                                        &nbsp;</td></td>
                                    <%--<td style="width: 437px; padding-bottom:20px" >
                                        &nbsp;</td></td>
                                    <td style="width: 291px; padding-bottom:20px" >
                                        &nbsp;</td></td>--%>
                                    
                                </tr>
                              
                            </table>
                        </div>
                      </div>
                    </div>
                </div><!-- /.col (LEFT) -->
                         <div class="col-md-6">
                        <div class="panel box box-success">
                      <div class="box-header with-border">
                        <h4 class="box-title">
                          <a data-toggle="collapse" data-parent="#accordion" href="#limitsTrans">
                             Limits Transaction
                          </a>
                        </h4>
                      </div>
                      <div id="limitsTrans" class="panel-collapse collapse">
                        <div class="box-body">

                             <table style="width:100%;">
                                 <tr>
                                     <td>
                                         <asp:Label ID="Label85" runat="server" Text="Limit Authoriser"></asp:Label>
                                     </td>
                                     <td>
                                         <asp:Label ID="lblLimitAuthoriser" runat="server" BackColor="#66FFFF"></asp:Label>
                                     </td>
                                     <td>
                                         <asp:Label ID="Label91" runat="server" Text="Dealer"></asp:Label>
                                     </td>
                                      <td>
                                          <asp:Label ID="lblDealerNameLimit" runat="server" BackColor="#66FFFF"></asp:Label>
                                     </td>
                                 </tr>
                                 <tr>
                                     <td>
                                         <asp:Label ID="Label86" runat="server" Text="Limit Exceeded"></asp:Label>
                                     </td>
                                     <td>
                                         <asp:Label ID="lblLimitStatus" runat="server" BackColor="#66FFFF"></asp:Label>
                                     </td>
                                     <td>
                                         <asp:Label ID="Label92" runat="server" Text="Dealer Limit"></asp:Label>
                                     </td>
                                      <td>
                                          <asp:Label ID="lblDealerLimit" runat="server" BackColor="#66FFFF"></asp:Label>
                                     </td>
                                 </tr>
                                 <tr>
                                     <td>
                                         <asp:Label ID="Label87" runat="server" Text="Limit Period"></asp:Label>
                                     </td>
                                     <td>
                                         <asp:Label ID="lblLimitPeriod" runat="server" BackColor="#66FFFF"></asp:Label>
                                     </td>
                                     <td>&nbsp;</td>
                                      <td>&nbsp;</td>
                                 </tr>
                             </table>

                      
                        </div>
                      </div>
                    </div> 
                 <%--  --%>
                
                       
                     <div class="panel box box-success">
                      <div class="box-header with-border">
                        <h4 class="box-title">
                          <a data-toggle="collapse" data-parent="#accordion" href="#limitsCounterparty">
                            Limits Counterparty
                          </a>
                        </h4>
                      </div>
                      <div id="limitsCounterparty" class="panel-collapse collapse">
                        <div class="box-body">
                        <table style="width:100%;">
                                 <tr>
                                     <td>
                                         <asp:Label ID="Label4" runat="server" Text="Limit Authoriser"></asp:Label>
                                     </td>
                                     <td>
                                         <asp:Label ID="lmtAuthCounterparty" runat="server" BackColor="#66FFFF"></asp:Label>
                                     </td>
                                     <td>
                                         <asp:Label ID="Label6" runat="server" Text="Dealer"></asp:Label>
                                     </td>
                                      <td>
                                          <asp:Label ID="lmtDealerCounterparty" runat="server" BackColor="#66FFFF"></asp:Label>
                                     </td>
                                 </tr>
                                 <tr>
                                     <td>
                                         <asp:Label ID="Label10" runat="server" Text="Limit Exceeded"></asp:Label>
                                     </td>
                                     <td>
                                         <asp:Label ID="lmtExceededCounterparty" runat="server" BackColor="#66FFFF"></asp:Label>
                                     </td>
                                     <td>
                                         <asp:Label ID="Label13" runat="server" Text="Counterparty Limit"></asp:Label>
                                     </td>
                                      <td>
                                          <asp:Label ID="lmtDealerLimitCounterparty" runat="server" BackColor="#66FFFF"></asp:Label>
                                     </td>
                                 </tr>
                                 <tr>
                                     <td>
                                         <asp:Label ID="Label17" runat="server" Text="Limit "></asp:Label>
                                     </td>
                                     <td>
                                         <asp:Label ID="lmtCounterparty" runat="server" BackColor="#66FFFF"></asp:Label>
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
                        <h4 class="box-title">
                            
                          <a data-toggle="collapse" data-parent="#accordion" href="#Authorisation">
                            Authorisation
                          </a>
                        </h4>
                          
                      </div>
                      <div id="Authorisation" class="panel-collapse collapse">
                        <div class="box-body">
                        <table style="width:100%;">
                            <tr>
                                     <td><b>
                                        
                                         Front Office</b></td>
                                     <td>
                                         
                                     </td>
                                     <td>
                                         
                                        <b> Back Office</b></td>
                                      <td>
                                          
                                     </td>
                                 </tr>
                                 <tr>
                                     <td>
                                         <asp:Label ID="Label5" runat="server" Text="First authorisation"></asp:Label>
                                     </td>
                                     <td>
                                         <asp:Label ID="txtFront1" runat="server" BackColor="#66FFFF"></asp:Label>
                                     </td>
                                     <td>
                                         <asp:Label ID="Label12" runat="server" Text="First Authorisation"></asp:Label>
                                     </td>
                                      <td>
                                          <asp:Label ID="txtBack1" runat="server" BackColor="#66FFFF"></asp:Label>
                                     </td>
                                 </tr>
                                 <tr>
                                     <td>
                                         <asp:Label ID="Label18" runat="server" Text="Comment"></asp:Label>
                                     </td>
                                     <td>
                                         <asp:TextBox ID="txtFrontComment" runat="server" TextMode="MultiLine" ></asp:TextBox>
                                     </td>
                                     <td>
                                         <asp:Label ID="Label20" runat="server" Text="Second Authorisation"></asp:Label>
                                     </td>
                                      <td>
                                          <asp:Label ID="txtBack2" runat="server" BackColor="#66FFFF"></asp:Label>
                                     </td>
                                 </tr>
                                 <tr>
                                     <td>
                                         <asp:Label ID="Label22" runat="server" Text="Second Authorisation "></asp:Label>
                                     </td>
                                     <td>
                                         <asp:Label ID="txtFront2" runat="server" BackColor="#66FFFF"></asp:Label>
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
                        <h4 class="box-title">
                          <a data-toggle="collapse" data-parent="#accordion" href="#dealEvents">
                            Deal Events
                          </a>
                        </h4>
                      </div>
                      <div id="dealEvents" class="panel-collapse collapse">
                        <div class="box-body">
                            <eo:Grid ID="GrdEvents" runat="server" BorderColor="#C7D1DF" BorderWidth="1px" ColumnHeaderAscImage="00050303" ColumnHeaderDescImage="00050304" ColumnHeaderDividerImage="00050302" FixedColumnCount="1" Font-Bold="False" Font-Italic="False" Font-Names="Verdana" Font-Overline="False" Font-Size="9pt" Font-Strikeout="False" Font-Underline="False" GridLineColor="199, 209, 223" GridLines="Both" Height="200px" ItemHeight="19" Width="1614px">
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
                                    <eo:StaticColumn HeaderText="Amentment Date" DataField="DateRevised">
                                    </eo:StaticColumn>
                                    <eo:StaticColumn HeaderText="Status Changed" DataField="changed">
                                    </eo:StaticColumn>
                                    <eo:StaticColumn HeaderText="Deal Ref" DataField="dealreference">
                                    </eo:StaticColumn>
                                    <eo:StaticColumn HeaderText="Before - Deal Amount" DataField="OlddealAmount">
                                    </eo:StaticColumn>
                                    <eo:StaticColumn HeaderText="After - Deal Amount" DataField="newdealAmount">
                                    </eo:StaticColumn>
                                    <eo:StaticColumn HeaderText="Before- Maturity Amt" DataField="OldMaturityAmount">
                                    </eo:StaticColumn>
                                    <eo:StaticColumn HeaderText="After- Maturity Amt" DataField="newMaturityAmount">
                                    </eo:StaticColumn>
                                    <eo:StaticColumn HeaderText="Before- MaturityDate" DataField="OldMaturityDate">
                                    </eo:StaticColumn>
                                    <eo:StaticColumn HeaderText="After- MaturityDate" DataField="newMaturityDate">
                                    </eo:StaticColumn>
                                    <eo:StaticColumn HeaderText="Before-InterestRate" DataField="OldInterestrate">
                                    </eo:StaticColumn>
                                    <eo:StaticColumn HeaderText="After-InterestRate" DataField="newInterestrate">
                                    </eo:StaticColumn>
                                    <eo:StaticColumn HeaderText="Before-Tenor" DataField="OldTenor">
                                    </eo:StaticColumn>
                                    <eo:StaticColumn HeaderText="After-Tenor" DataField="newTenor">
                                    </eo:StaticColumn>
                                    <eo:StaticColumn HeaderText="Interest Value Date" DataField="InterestValueDate">
                                    </eo:StaticColumn>
                                    <eo:StaticColumn HeaderText="Before-NetInterest" DataField="OldNetInterest">
                                    </eo:StaticColumn>
                                    <eo:StaticColumn HeaderText="After-NetInterest" DataField="newNetInterest">
                                    </eo:StaticColumn>
                                    <eo:StaticColumn HeaderText="Before-GrossInterest" DataField="OldGrossInterest">
                                    </eo:StaticColumn>
                                    <eo:StaticColumn HeaderText="After-GrossInterest" DataField="newGrossInterest">
                                    </eo:StaticColumn>
                                    <eo:StaticColumn DataField="OldTaxAmount" HeaderText="Before-Tax">
                                    </eo:StaticColumn>
                                    <eo:StaticColumn HeaderText="After-Tax" DataField="newTaxAmount">
                                    </eo:StaticColumn>
                                    <eo:StaticColumn HeaderText="Before-Tax">
                                    </eo:StaticColumn>
                                    <eo:StaticColumn HeaderText="UserID" DataField="UserID">
                                    </eo:StaticColumn>
                                    <eo:StaticColumn HeaderText="Change Seq" DataField="recnumber">
                                    </eo:StaticColumn>
                                    <eo:StaticColumn HeaderText="Days Effective" DataField="DaysOnChangeActual">
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
                            
                        </div>
                      </div>
                    </div>
                     <div class="panel box box-success">
                      <div class="box-header with-border">
                        <h4 class="box-title">
                          <a data-toggle="collapse" data-parent="#accordion" href="#More">
                           More Details
                          </a>
                        </h4>
                      </div>
                      <div id="More" class="panel-collapse collapse">
                        <div class="box-body">
                           <table style="width:100%;">
                            <tr>
                                     <td><b>
                                        
                                         Front Office</b></td>
                                     <td>
                                         
                                     </td>
                                     <td>
                                         
                                         <b>Security Details</b></td>
                                      <td>
                                          
                                         <b> Settlement Details</b></td>
                                <td></td>
                                 </tr>
                                 <tr>
                                     <td>
                                         <asp:Label ID="Label8" runat="server" Text="Deal Start :"></asp:Label>
                                     </td>
                                     <td>
                                         <asp:TextBox ID="txtStartInstr" runat="server" TextMode="MultiLine"></asp:TextBox>
                                     </td>
                                     <td>
                                         &nbsp;</td>
                                      <td>
                                          <asp:Label ID="Label19" runat="server">Inception Debit Account</asp:Label>
                                     </td>
                                     <td>
                                         <asp:Label ID="lblInceptionDebitAcc" runat="server" BackColor="#66FFFF"></asp:Label>
                                     </td>
                                 </tr>
                                 <tr>
                                     <td>
                                         <asp:Label ID="Label21" runat="server" Text="Deal End :"></asp:Label>
                                     </td>
                                     <td>
                                         <asp:TextBox ID="txtEnd" runat="server" TextMode="MultiLine" ></asp:TextBox>
                                     </td>
                                     <td>
                                         <asp:Label ID="txtSecurityDetails" runat="server" BackColor="#66FFFF"></asp:Label>
                                     </td>
                                      <td>
                                          <asp:Label ID="Label24" runat="server">Inception Credit Account</asp:Label><br /><br />
                                          <asp:Label ID="Label9" runat="server">Maturity Interest Account</asp:Label><br /><br />
                                          <asp:Label ID="Label15" runat="server">Interest Accrual (Debit)</asp:Label>
                                     </td>
                                     <td>
                                         <asp:Label ID="lblInceptionCreditAcc" runat="server" BackColor="#66FFFF"></asp:Label><br /><br />
                                         <asp:Label ID="lbllMaturityInterest" runat="server" BackColor="#66FFFF"></asp:Label><br /><br />
                                         <asp:Label ID="lblIAccrualDebit" runat="server" BackColor="#66FFFF"></asp:Label>
                                     </td>
                                 </tr>
                                 <tr>
                                     <td>
                                         <asp:Label ID="Label25" runat="server" Text="On Maturity :"></asp:Label>
                                     </td>
                                     <td>
                                         <asp:TextBox ID="txtFinal" runat="server" TextMode="MultiLine"></asp:TextBox>
                                     </td>
                                     <td>&nbsp;</td>
                                      <td>
                                          <asp:Label ID="Label93" runat="server" Text="Interest Accrual (Credit)"></asp:Label>
                                     </td>
                                     <td>
                                         <asp:Label ID="lblIAccrualCredit" runat="server" BackColor="#66FFFF"></asp:Label>
                                     </td>
                                 </tr>

                             </table>
                        </div>
                      </div>
                    
                 </div>
                  </div><!-- /.col (RIGHT) -->
          </div><!-- /.row -->
                    </ContentTemplate>
            </eo:CallbackPanel>

           <%-- <Triggers>
                <asp:AsyncPostBackTrigger ControlID="GrdDealsMM" EventName=""/>
            </Triggers>
        </asp:UpdatePanel>--%>
                        
                      

                
    </ContentTemplate>
                           </asp:UpdatePanel>

                        <%--</div><!-- /.box --> 
 
       
         

 
                     
                     </form>

                      
                            

                             
                              </head>
                         <%-- </ContentTemplate>
                      </asp:UpdatePanel>--%>
                  <!-- <button type="button" class="btn btn-block btn-success btn-xs" data-toggle="modal" data-target="#DealCodeDetail">
  Launch demo modal
</button>-->
                  </div><!-- /.box-body -->
                 <%-- <div class="box-footer"> 
                     <asp:Button ID="btncancel" runat="server" Text="Cancel" CssClass="btn btn-default"  />
                       <asp:Button ID="btnRemove" runat="server" Text="Remove" CssClass="btn btn-default"  />
                      
                      <asp:Button ID="btnSave" runat="server" Text="Secure   " CssClass="btn btn-info pull-right" />
                    <%--<button type="submit" class="btn btn-default">Cancel</button>
                    <button type="submit" class="btn btn-info pull-right">Sign in</button>--%>
                  </div><!-- /.box-footer -->
             
             </div><!-- /.box --> 
 
       
         

  </form>
 <script type="text/javascript">
     //<!--JS_SAMPLE_BEGIN-->

    var g_itemIndex = -1;
     var g_cellIndex = -1;

     function ShowContextMenu(e, grid, item, cell) {
         //Save the target cell index
         g_itemIndex = item.getIndex();
         g_cellIndex = cell.getColIndex();

         //Show the context menu
         var menu = eo_GetObject("<%=Menu1.ClientID%>");
    eo_ShowContextMenu(e, "<%=Menu1.ClientID%>");

    //Return true to indicate that we have
    //displayed a context menu
    return true;
}

function OnContextMenuItemClicked(e, eventInfo) {
    var grid = eo_GetObject("<%=GrdDealsMM.ClientID%>");

    var item = eventInfo.getItem();
    switch (item.getText()) {
        case "Amend Deal Tax":
            //Show the item details
            var gridItem = grid.getItem(g_itemIndex);
            <%--alert(
                "Details about this grid item:\r\n" +
                "deal: " + gridItem.getCell(1).getValue() + "\r\n" +
                "Posted By: " + gridItem.getCell(2).getValue() + "\r\n" +
                "Topic: " + gridItem.getCell(3).getValue());
            document.getElementById("<%= txtdealref.ClientID%>").value = gridItem.getCell(1).getValue()
            //window.location.reload();--%>
            var dealcode = gridItem.getCell(1).getValue()
         
            window.open('amendtax.aspx?dealref=' + dealcode, '_self', false)

            break;

        case "Amend Deal Interest Rate":
          
            var gridItem = grid.getItem(g_itemIndex);
            var dealcode = gridItem.getCell(1).getValue()
         
            window.open('amendInterest.aspx?dealref=' + dealcode, '_self', false)
            break;

        case "Change Maturity Date":
            var gridItem = grid.getItem(g_itemIndex);
            var dealcode = gridItem.getCell(1).getValue()

            window.open('ChangeMaturity.aspx?dealref=' + dealcode, '_self', false)
            break;

        case "Change Principle":
            var gridItem = grid.getItem(g_itemIndex);
            var dealcode = gridItem.getCell(1).getValue()

            window.open('ChangePrinciple.aspx?dealref=' + dealcode, '_self', false)
            break;
        case "Change Deal Instructions":
            var gridItem = grid.getItem(g_itemIndex);
            var dealcode = gridItem.getCell(1).getValue()

            window.open('changeInstructions.aspx?dealref=' + dealcode, '_self', false)
            break;
        case "Amend Security Details":
            var gridItem = grid.getItem(g_itemIndex);
            var dealcode = gridItem.getCell(1).getValue()

            window.open('SecuritySwap.aspx?dealref=' + dealcode, '_self', false)
            break;
        case "Deal Postings Enquiry":
            var gridItem = grid.getItem(g_itemIndex);
            var dealcode = gridItem.getCell(1).getValue()

            window.open('PostingEnquiry.aspx?dealref=' + dealcode, '_self', false)
            break;
        case "Accept":
            //function OnItemSelected(grid) {
                //Get the selected item
                var item = grid.getSelectedItem();

             
                grid.raiseItemCommandEvent(item.getIndex(), "Accept");
            //}
            break;
        case "Decline":
            
            var gridItem = grid.getItem(g_itemIndex);
            var dealcode = gridItem.getCell(1).getValue()

            window.open('Decline.aspx?dealref=' + dealcode, '_self', false)
            
            break;
           
        case "Cancel Deal":
           
                //Get the selected item
            var gridItem = grid.getItem(g_itemIndex);
            var dealcode = gridItem.getCell(1).getValue()

            window.open('CancelDeal.aspx?dealref=' + dealcode, '_self', false)
           
            break;
        case "Cancel Reason/Comment":
           
                //Get the selected item
                var item = grid.getSelectedItem();


                grid.raiseItemCommandEvent(item.getIndex(), "Comment");
            
            break;
        case "Export To Excel":
            
                //Get the selected item
                var item = grid.getSelectedItem();


                grid.raiseItemCommandEvent(item.getIndex(), "Export");
           
            break;
        case "Re-Print Deal Slip":
            
                //Get the selected item
                var item = grid.getSelectedItem();


                grid.raiseItemCommandEvent(item.getIndex(), "Print");
           
            break;
        case "Cancel Deal":
           
                //Get the selected item
                var item = grid.getSelectedItem();


                grid.raiseItemCommandEvent(item.getIndex(), "Cancel Deal");
          
            break;
           

        case "Save":
            //Save menu item's RaisesServerEvent is set to true,
            //so the event is handled on the server side
    }
}

//<!--JS_SAMPLE_END-->
    </script>
    <script>
        function OnItemSelected(grid)
{
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

    
</asp:Content>
