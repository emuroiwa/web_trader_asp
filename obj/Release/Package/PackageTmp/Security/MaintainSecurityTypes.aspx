
<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Main.Master" CodeBehind="MaintainSecurityTypes.aspx.vb" Inherits="WEBTDS.MaintainSecurityTypes" %>

<%@ Register Assembly="EO.Web" Namespace="EO.Web" TagPrefix="eo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  
   

  <head />
  <form id="form1" runat="server">
       <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
       <!-- Horizontal Form -->
         <asp:UpdatePanel ID="UpdatePanelAll" UpdateMode="Always" runat="server"><ContentTemplate>
              <div class="box box-info">
                <div class="box-header with-border">
                  <h3 class="box-title">Security Types</h3> 
                    
                </div><!-- /.box-header -->
                <!-- form start -->
             
                  <div class="box-body">
                      <%--<center>--%>
                        
                          <table style="width:100%;">
                              <tr>
                                  <td style="height: 45px; width: 224px;">
                                      <asp:Label ID="Label1" runat="server" Text="Collateral ID"></asp:Label>
                                  </td>
                                  <td style="width: 454px; height: 45px">
                                      <asp:TextBox ID="txtID" runat="server" style="margin-left: 0px" title="Please Enter Present value" Width="75%" Font-Size="Small"></asp:TextBox>
                                      <br />
                                  </td>
                                  <td style="height: 45px">
                                      <asp:Button ID="btnNew" runat="server" CssClass="btn btn-primary btn-block btn-flats" Height="40px" Text="New" Width="150px" />
                                      <br />
                                  </td>
                              </tr>
                              <tr>
                                  <td style="width: 224px">
                                      <asp:Label ID="Label2" runat="server" Text="Collateral Description"></asp:Label>
                                  </td>
                                  <td style="width: 454px">
                                      <asp:TextBox ID="txtDescription" runat="server" style="margin-left: 0px" title="Please Enter Present value" Width="75%" Font-Size="Small"></asp:TextBox>
                                      <br />
                                  </td>
                                  <td>
                                      <asp:Button ID="btnSave0" runat="server" CssClass="btn btn-primary btn-block btn-flats" Height="40px" Text="Save" Width="150px" />
                                      <br />
                                  </td>
                              </tr>
                              <tr>
                                  <td style="width: 224px">
                                      <asp:Label ID="Label3" runat="server" Text="Bank Valuation Margin"></asp:Label>
                                  </td>
                                  <td style="width: 454px">
                                      <asp:TextBox ID="txtBankValuation" runat="server" style="margin-left: 0px" title="Please Enter Present value" Width="75%" Font-Size="Small"></asp:TextBox>
                                  </td>
                                  <td>
                                      <asp:Button ID="btnExit0" runat="server" CssClass="btn btn-danger btn-block btn-flats" Height="40px" Text="Exit" Width="150px" />
                                  </td>
                              </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <asp:CheckBox ID="CheckBoxLoans" runat="server" Text="Secure Loans" />
                                    <br />
                                    <asp:CheckBox ID="CheckBoxDeposit" runat="server" Text="Secure Deposits" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txtloan" runat="server" Enabled="False" Visible="False"></asp:TextBox>
                                    <asp:TextBox ID="txtdeposit" runat="server" Enabled="False" Visible="False"></asp:TextBox>
                                </td>
                            </tr>
                          </table>
                      <%--</center> --%>
                      <eo:ContextMenu ID="Menu1" runat="server" CheckIconUrl="OfficeCheckIcon" ControlSkinID="None" Width="200px" ClientSideOnItemClick="OnContextMenuItemClicked">
                          <LookItems>
                              <eo:MenuItem DisabledStyle-CssText="background-color:transparent;border-bottom-style:none;border-left-style:none;border-right-style:none;border-top-style:none;padding-bottom:1px;padding-left:5px;padding-right:5px;padding-top:1px;color:lightgrey" Height="24" HoverStyle-CssText="background-color:#c1d2ee;border-bottom-color:#316ac5;border-bottom-style:solid;border-bottom-width:1px;border-left-color:#316ac5;border-left-style:solid;border-left-width:1px;border-right-color:#316ac5;border-right-style:solid;border-right-width:1px;border-top-color:#316ac5;border-top-style:solid;border-top-width:1px;padding-left:4px;padding-right:4px;padding-top:0px;padding-bottom:0px;" ItemID="_TopLevelItem" NormalStyle-CssText="background-color:transparent;border-bottom-style:none;border-left-style:none;border-right-style:none;border-top-style:none;padding-bottom:1px;padding-left:5px;padding-right:5px;padding-top:1px;" SelectedStyle-CssText="background-color:white;border-bottom-color:#316ac5;border-bottom-style:solid;border-bottom-width:1px;border-left-color:#316ac5;border-left-style:solid;border-left-width:1px;border-right-color:#316ac5;border-right-style:solid;border-right-width:1px;border-top-color:#316ac5;border-top-style:solid;border-top-width:1px;padding-left:4px;padding-right:4px;padding-top:0px;padding-bottom:0px;">
                                  <SubMenu CollapseEffect-Duration="150" CollapseEffect-Type="Fade" ExpandEffect-Duration="150" ExpandEffect-Type="Fade" ItemSpacing="3" LeftIconCellWidth="25" SideImage="OfficeXPSideBar" Style-CssText="background-color:#fcfcf9;border-bottom-color:#999999;border-bottom-style:solid;border-bottom-width:1px;border-left-color:#999999;border-left-style:solid;border-left-width:1px;border-right-color:#999999;border-right-style:solid;border-right-width:1px;border-top-color:#999999;border-top-style:solid;border-top-width:1px;color:black;cursor:hand;font-family:Tahoma;font-size:8pt;padding-bottom:1px;padding-left:1px;padding-right:1px;padding-top:1px;">
                                  </SubMenu>
                              </eo:MenuItem>
                              <eo:MenuItem IsSeparator="True" ItemID="_Separator" NormalStyle-CssText="background-color:#c5c2b8;height:1px;margin-left:30px;width:1px;">
                              </eo:MenuItem>
                              <eo:MenuItem DisabledStyle-CssText="background-color:transparent;border-bottom-style:none;border-left-style:none;border-right-style:none;border-top-style:none;padding-bottom:1px;padding-left:2px;padding-right:5px;padding-top:1px;color:lightgrey" Height="24" HoverStyle-CssText="background-color:#c1d2ee;border-bottom-color:#316ac5;border-bottom-style:solid;border-bottom-width:1px;border-left-color:#316ac5;border-left-style:solid;border-left-width:1px;border-right-color:#316ac5;border-right-style:solid;border-right-width:1px;border-top-color:#316ac5;border-top-style:solid;border-top-width:1px;padding-left:1px;padding-right:4px;padding-top:0px;" ItemID="_Default" NormalStyle-CssText="background-color:transparent;border-bottom-style:none;border-left-style:none;border-right-style:none;border-top-style:none;padding-bottom:1px;padding-left:2px;padding-right:5px;padding-top:1px;" SelectedStyle-CssText="background-color:white;border-bottom-color:#316ac5;border-bottom-style:solid;border-bottom-width:1px;border-left-color:#316ac5;border-left-style:solid;border-left-width:1px;border-right-color:#316ac5;border-right-style:solid;border-right-width:1px;border-top-color:#316ac5;border-top-style:solid;border-top-width:1px;padding-left:1px;padding-right:4px;padding-top:0px;" Text-Padding-Right="30">
                                  <SubMenu CollapseEffect-Duration="150" CollapseEffect-Type="Fade" ExpandEffect-Duration="150" ExpandEffect-Type="Fade" ItemSpacing="3" LeftIconCellWidth="25" SideImage="OfficeXPSideBar" Style-CssText="background-color:#fcfcf9;border-bottom-color:#999999;border-bottom-style:solid;border-bottom-width:1px;border-left-color:#999999;border-left-style:solid;border-left-width:1px;border-right-color:#999999;border-right-style:solid;border-right-width:1px;border-top-color:#999999;border-top-style:solid;border-top-width:1px;color:black;cursor:hand;font-family:Tahoma;font-size:8pt;padding-bottom:1px;padding-left:1px;padding-right:1px;padding-top:1px;">
                                  </SubMenu>
                              </eo:MenuItem>
                          </LookItems>
                          <TopGroup>
                              <Items>
                                  <eo:MenuItem Text-Html="Delete">
                                  </eo:MenuItem>
                                  <eo:MenuItem Text-Html="Edit">
                                  </eo:MenuItem>
                              </Items>
                          </TopGroup>
                      </eo:ContextMenu>
                      <eo:Grid ID="GrdTypes" runat="server" BorderColor="#C7D1DF" BorderWidth="1px" ColumnHeaderAscImage="00050303" ColumnHeaderDescImage="00050304" ColumnHeaderDividerImage="00050302" FixedColumnCount="1" Font-Bold="False" Font-Italic="False" Font-Names="Verdana" Font-Overline="False" Font-Size="9pt" Font-Strikeout="False" Font-Underline="False" GridLineColor="199, 209, 223" GridLines="Both" Height="200px" ItemHeight="19" Width="834px" ClientSideOnItemSelected="OnItemSelected"
        OnItemCommand="GrdTypes_ItemCommand" ClientSideOnContextMenu="ShowContextMenu">
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
                              <eo:RowNumberColumn HeaderText="RowNum">
                              </eo:RowNumberColumn>
                              <eo:StaticColumn HeaderText="ID" Width="150" DataField="collateralId">
                              </eo:StaticColumn>
                              <eo:StaticColumn HeaderText="Description" Width="150" DataField="collateraldescription">
                              </eo:StaticColumn>
                              <eo:StaticColumn HeaderText="Bank Valuation%" Width="150" DataField="collateralbankvaluation">
                              </eo:StaticColumn>
                              <eo:StaticColumn HeaderText="Secure Loans" Width="150" DataField="SecureLoan">
                              </eo:StaticColumn>
                              <eo:StaticColumn HeaderText="Secure Deposits" Width="150" DataField="SecureDeposit">
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

                            <!-- <button type="button" class="btn btn-block btn-success btn-xs" data-toggle="modal" data-target="#DealCodeDetail">
  Launch demo modal
</button>-->
                  </div><!-- /.box-body -->
                
                 
              
              </div><!-- /.box --> 

     
                
         </ContentTemplate></asp:UpdatePanel>

  </form>
    </head>
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
             var grid = eo_GetObject("<%=GrdTypes.ClientID%>");

            var item = eventInfo.getItem();
            switch (item.getText()) {

                case "Delete":
                    //function OnItemSelected(grid) {
                    //Get the selected item
                    var item = grid.getSelectedItem();




                    grid.raiseItemCommandEvent(item.getIndex(), "Delete");
                    //}
                    break;

                case "Edit":

                    //Get the selected item
                    var item = grid.getSelectedItem();


                    grid.raiseItemCommandEvent(item.getIndex(), "Edit");

                    break;


                case "Save":
                    //Save menu item's RaisesServerEvent is set to true,
                    //so the event is handled on the server side
            }
        }

        //<!--JS_SAMPLE_END-->
    </script>
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
        //<!--JS_SAMPLE_END-->
    </script>
</asp:Content>
