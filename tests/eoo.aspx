<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Main.Master" CodeBehind="eoo.aspx.vb" Inherits="WEBTDS.eoo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="ajaxToolkit" %>
<%@ Register assembly="EO.Web" namespace="EO.Web" tagprefix="eo" %>
  
    <script>
        function OnItemDelete(grid, itemIndex, colIndex, commandName)
{
    //Ask user to confirm the delete
    if (window.confirm("Are you sure you want to delete this row?"))
        grid.deleteItem(itemIndex);
}
//<!--JS_SAMPLE_END-->
</script>
    <form runat="server">
         <asp:ScriptManager ID="ScriptManager2" runat="server"></asp:ScriptManager>
        <table style="width: auto;">
            <tr>
                <td style="width: 608px">
                    <eo:ContextMenu ID="Menu1" runat="server" ClientSideOnItemClick="OnContextMenuItemClicked" ControlSkinID="None"  Width="144px">
                                      <TopGroup Style-CssText="cursor:hand;font-family:Verdana;font-size:11px;">
                                          <Items>
                                              <eo:MenuItem Text-Html="Detail">
                                              </eo:MenuItem>
                                              <eo:MenuItem Text-Html="Delete">
                                              </eo:MenuItem>
                                              <eo:MenuItem Text-Html="Add New">
                                              </eo:MenuItem>
                                              <eo:MenuItem IsSeparator="True">
                                              </eo:MenuItem>
                                              <eo:MenuItem RaisesServerEvent="True" Text-Html="Save">
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
                                  <eo:Grid ID="GrdDealsMM" runat="server" BorderColor="#C7D1DF" BorderWidth="1px" ColumnHeaderAscImage="00050303" ColumnHeaderDescImage="00050304" ColumnHeaderDividerImage="00050302" FixedColumnCount="1" Font-Bold="False" Font-Italic="False" Font-Names="Verdana" Font-Overline="False" Font-Size="9pt" Font-Strikeout="False" Font-Underline="False" GridLineColor="199, 209, 223" GridLines="Both" Height="252px" ItemHeight="19" Width="1013px" ClientSideOnContextMenu="ShowContextMenu" ClientSideOnItemCommand="OnItemDelete">
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
                                          <eo:DeleteCommandColumn Name="Delete">
                                          </eo:DeleteCommandColumn>
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
        <eo:CallbackPanel runat="server" ID="CallbackPanel1" Triggers="{ControlID:GrdDealsMM;Parameter:}">
            <asp:Label ID="lblInfo" runat="server"></asp:Label>
        </eo:CallbackPanel>
    </p>
                </td>
            </tr>
        </table>
    </form>
</asp:Content>
