<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Main.Master" CodeBehind="eo.aspx.vb" Inherits="WEBTDS.eo" %>
<%@ Register assembly="EO.Web" namespace="EO.Web" tagprefix="eo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <form id="form1" runat="server">
         Deals On Dash Board<eo:Grid ID="GridDashBoard" runat="server" 
                                        BorderColor="#C7D1DF" BorderWidth="1px" ColumnHeaderAscImage="00050303" 
                                        ColumnHeaderDescImage="00050304" ColumnHeaderDividerImage="00050302" 
                                        DataTable="dealsdashboard" EnableKeyboardNavigation="True" FixedColumnCount="1" 
                                        Font-Bold="False" Font-Italic="False" Font-Names="Verdana" 
                                        Font-Overline="False" Font-Size="9pt" Font-Strikeout="False" 
                                        Font-Underline="False" ForceSSL="True" GridLineColor="199, 209, 223" 
                                        GridLines="Both" Height="84px" ItemHeight="19" RunningMode="Server" 
                                        Width="924px">
                                        <FooterStyle CssText="padding-bottom:4px;padding-left:4px;padding-right:4px;padding-top:4px;" />
                                        <ColumnTemplates>
                                            <eo:TextBoxColumn>
                                                <TextBoxStyle CssText="BORDER-RIGHT: #7f9db9 1px solid; PADDING-RIGHT: 2px; BORDER-TOP: #7f9db9 1px solid; PADDING-LEFT: 2px; FONT-SIZE: 8.75pt; PADDING-BOTTOM: 1px; MARGIN: 0px; BORDER-LEFT: #7f9db9 1px solid; PADDING-TOP: 2px; BORDER-BOTTOM: #7f9db9 1px solid; FONT-FAMILY: Tahoma" />
                                            </eo:TextBoxColumn>
                                            <eo:DateTimeColumn>
                                                <DatePicker ControlSkinID="None" DayCellHeight="16" DayCellWidth="19" 
                                                    DayHeaderFormat="FirstLetter" DisabledDates="" OtherMonthDayVisible="True" 
                                                    SelectedDates="" TitleLeftArrowImageUrl="DefaultSubMenuIconRTL" 
                                                    TitleRightArrowImageUrl="DefaultSubMenuIcon">
                                                    <TodayStyle CssText="font-family: tahoma; font-size: 12px; border-right: #bb5503 1px solid; border-top: #bb5503 1px solid; border-left: #bb5503 1px solid; border-bottom: #bb5503 1px solid" />
                                                    <SelectedDayStyle CssText="font-family: tahoma; font-size: 12px; background-color: #fbe694; border-right: white 1px solid; border-top: white 1px solid; border-left: white 1px solid; border-bottom: white 1px solid" />
                                                    <DisabledDayStyle CssText="font-family: tahoma; font-size: 12px; color: gray; border-right: white 1px solid; border-top: white 1px solid; border-left: white 1px solid; border-bottom: white 1px solid" />
                                                    <PickerStyle CssText="border-bottom-color:#7f9db9;border-bottom-style:solid;border-bottom-width:1px;border-left-color:#7f9db9;border-left-style:solid;border-left-width:1px;border-right-color:#7f9db9;border-right-style:solid;border-right-width:1px;border-top-color:#7f9db9;border-top-style:solid;border-top-width:1px;font-family:Courier New;font-size:8pt;margin-bottom:0px;margin-left:0px;margin-right:0px;margin-top:0px;padding-bottom:1px;padding-left:2px;padding-right:2px;padding-top:2px;" />
                                                    <CalendarStyle CssText="background-color: white; border-right: #7f9db9 1px solid; padding-right: 4px; border-top: #7f9db9 1px solid; padding-left: 4px; font-size: 9px; padding-bottom: 4px; border-left: #7f9db9 1px solid; padding-top: 4px; border-bottom: #7f9db9 1px solid; font-family: tahoma" />
                                                    <TitleArrowStyle CssText="cursor:hand" />
                                                    <DayHoverStyle CssText="font-family: tahoma; font-size: 12px; border-right: #fbe694 1px solid; border-top: #fbe694 1px solid; border-left: #fbe694 1px solid; border-bottom: #fbe694 1px solid" />
                                                    <MonthStyle CssText="font-family: tahoma; font-size: 12px; margin-left: 14px; cursor: hand; margin-right: 14px" />
                                                    <TitleStyle CssText="background-color:#9ebef5;font-family:Tahoma;font-size:12px;padding-bottom:2px;padding-left:6px;padding-right:6px;padding-top:2px;" />
                                                    <OtherMonthDayStyle CssText="font-family: tahoma; font-size: 12px; color: gray; border-right: white 1px solid; border-top: white 1px solid; border-left: white 1px solid; border-bottom: white 1px solid" />
                                                    <DayHeaderStyle CssText="font-family: tahoma; font-size: 12px; border-bottom: #aca899 1px solid" />
                                                    <DayStyle CssText="font-family: tahoma; font-size: 12px; border-right: white 1px solid; border-top: white 1px solid; border-left: white 1px solid; border-bottom: white 1px solid" />
                                                </DatePicker>
                                            </eo:DateTimeColumn>
                                            <eo:MaskedEditColumn>
                                                <MaskedEdit ControlSkinID="None" 
                                                    TextBoxStyle-CssText="BORDER-RIGHT: #7f9db9 1px solid; PADDING-RIGHT: 2px; BORDER-TOP: #7f9db9 1px solid; PADDING-LEFT: 2px; PADDING-BOTTOM: 1px; MARGIN: 0px; BORDER-LEFT: #7f9db9 1px solid; PADDING-TOP: 2px; BORDER-BOTTOM: #7f9db9 1px solid; font-family:Courier New;font-size:8pt;">
                                                </MaskedEdit>
                                            </eo:MaskedEditColumn>
                                        </ColumnTemplates>
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
                                                MinWidth="20" ReadOnly="True" SortOrder="Ascending" Width="150">
                                                <CellStyle CssText="font-size:11px; color: #000000; font-family: Calibri;" />
                                            </eo:CustomColumn>
                                            <eo:CustomColumn DataField="dealamount" DataFormat="{0:N2}" DataType="Float" 
                                                HeaderText="Deal Amount" MinWidth="10" ReadOnly="True" SortOrder="Ascending" 
                                                Width="150">
                                                <CellStyle CssClass="" 
                                                    CssText="font-size:11px; color: #000000; font-family: Calibri;" />
                                            </eo:CustomColumn>
                                            <eo:CustomColumn DataField="dateentered" HeaderText="Date Entered" 
                                                MinWidth="10" ReadOnly="True" SortOrder="Ascending">
                                                <CellStyle CssClass="" 
                                                    CssText="font-size:11px; color: #000000; font-family: Calibri;" />
                                            </eo:CustomColumn>
                                            <eo:CustomColumn DataField="startdate" HeaderText="Start Date" MinWidth="10" 
                                                ReadOnly="True" SortOrder="Ascending">
                                                <CellStyle CssClass="" 
                                                    CssText="font-size:11px; color: #000000; font-family: Calibri;" />
                                            </eo:CustomColumn>
                                            <eo:CustomColumn DataField="maturitydate" HeaderText="Maturity Date" 
                                                MinWidth="10" ReadOnly="True" SortOrder="Ascending">
                                                <CellStyle CssClass="" 
                                                    CssText="font-size:11px; color: #000000; font-family: Calibri;" />
                                            </eo:CustomColumn>
                                            <eo:CustomColumn DataField="currency" HeaderText="Currency" MinWidth="10" 
                                                ReadOnly="True" SortOrder="Ascending">
                                                <CellStyle CssClass="" 
                                                    CssText="font-size:11px; color: #000000; font-family: Calibri;" />
                                            </eo:CustomColumn>
                                            <eo:CustomColumn DataField="expired" HeaderText="Expired" MinWidth="10" 
                                                ReadOnly="True" SortOrder="Ascending">
                                                <CellStyle CssClass="" 
                                                    CssText="font-size:11px; color: #000000; font-family: Calibri;" />
                                            </eo:CustomColumn>
                                            <eo:DeleteCommandColumn Name="delete">
                                            </eo:DeleteCommandColumn>
                                        </Columns>
                                    </eo:Grid>
    </form>

</asp:Content>
