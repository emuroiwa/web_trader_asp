
<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Main.Master" CodeBehind="SecurityListings.aspx.vb" Inherits="WEBTDS.SecurityListings"   %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="ajaxToolkit" %>
<%@ Register assembly="EO.Web" namespace="EO.Web" tagprefix="eo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  

  
  <head runat="server" />
  <form id="form1" runat="server">
       <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
      <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <p id="demo"></p>
                 <asp:Label ID="lblError" runat="server"></asp:Label>
        <asp:Label ID="lblsuccess" runat="server"></asp:Label>
                  <asp:Label ID="lblwarning" runat="server"> </asp:Label>
       <!-- Horizontal Form -->
              <div class="box box-info">
                <div class="box-header with-border">
                  <h3 class="box-title">Security</h3>  
   
                </div><!-- /.box-header -->
                <!-- form start -->
             
                  <div class="box-body">
                      
                     </HeaderTemplate>
                      <eo:CallbackPanel ID="CallbackPanel1" runat="server" Height="150px" Triggers="{ControlID:lblsecref;Parameter:}" Width="200px">
                          <table style="width:100%;">
                              <tr>
                                  <td style="width: 567px">
                                      <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="Medium" Text="Filter"></asp:Label>
                                      <br />
                                      <asp:DropDownList ID="cmbOptions" runat="server" AutoPostBack="True" class="form-control select2" Font-Size="Small" style="margin-left: 0px" Width="80%">
                                          <asp:ListItem Text="Select Option" Value="0"></asp:ListItem>
                                          <asp:ListItem>Pending Approval</asp:ListItem>
                                          <asp:ListItem>Expired</asp:ListItem>
                                          <asp:ListItem>Cancelled</asp:ListItem>
                                          <asp:ListItem>Approved</asp:ListItem>
                                      </asp:DropDownList>
                                  </td>
                                  <td>
                                      <asp:Button ID="btnExit" runat="server" CssClass="btn btn-danger btn-block btn-flats" Height="40px" Text="Exit" Width="150px" />
                                  </td>
                              </tr>
                              <tr>
                                  
                                  <td>
                                      <asp:TextBox ID="txtstatus" runat="server" Enabled="False" Visible="False"></asp:TextBox>
                                  </td>
                              </tr>
                              <tr>
                                  <td style="width: 567px">
                                      <asp:Label ID="Label2" runat="server" Font-Bold="True" Font-Size="Medium" Text="Listing"></asp:Label>
                                      <eo:ContextMenu ID="Menu1" runat="server" CheckIconUrl="OfficeCheckIcon" ControlSkinID="None" Width="200px" ClientSideOnItemClick="OnContextMenuItemClicked"  >
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
                                                  <eo:MenuItem Text-Html="Security Item Detail">
                                                  </eo:MenuItem>
                                                  <eo:MenuItem Text-Html="Security Report">
                                                  </eo:MenuItem>
                                                  <eo:MenuItem Text-Html="Cancel">
                                                  </eo:MenuItem>
                                                  <eo:MenuItem Text-Html="Approve">
                                                  </eo:MenuItem>
                                              </Items>
                                          </TopGroup>
                                      </eo:ContextMenu>
                                      <eo:Grid ID="GrdListings" runat="server" BorderColor="#C7D1DF" BorderWidth="1px" ClientSideOnContextMenu="ShowContextMenu" ClientSideOnItemSelected="OnItemSelected" ColumnHeaderAscImage="00050303" ColumnHeaderDescImage="00050304" ColumnHeaderDividerImage="00050302" FixedColumnCount="1" Font-Bold="False" Font-Italic="False" Font-Names="Verdana" Font-Overline="False" Font-Size="9pt" Font-Strikeout="False" Font-Underline="False" GridLineColor="199, 209, 223" GridLines="Both" Height="200px" ItemHeight="19" OnItemCommand="GrdListings_ItemCommand" style="margin-left: 5px" Width="957px">
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
                                              <eo:RowNumberColumn>
                                              </eo:RowNumberColumn>
                                              <eo:StaticColumn DataField="fullname" HeaderText="Customer" Width="200">
                                              </eo:StaticColumn>
                                              <eo:StaticColumn DataField="collateralreference" HeaderText="Sec Ref" Width="150">
                                              </eo:StaticColumn>
                                              <eo:StaticColumn DataField="collateraldescription" HeaderText="Sec Ref Ext" Width="150">
                                              </eo:StaticColumn>
                                              <eo:StaticColumn DataField="collateralextreference" HeaderText="Description" Width="150">
                                              </eo:StaticColumn>
                                              <eo:StaticColumn DataField="collateralbankvalue" HeaderText="Bank Valuation" Width="150">
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
                                              <eo:StaticColumn>
                                              </eo:StaticColumn>
                                              <eo:RowNumberColumn>
                                              </eo:RowNumberColumn>
                                          </ColumnTemplates>
                                          <FooterStyle CssText="padding-bottom:4px;padding-left:4px;padding-right:4px;padding-top:4px;" />
                                      </eo:Grid>
                                      <br />
                                  </td>
                                  <td>
                                      <asp:TextBox ID="txtcustomer" runat="server" Enabled="False" Visible="False"></asp:TextBox>
                                      <asp:TextBox ID="txtcreatedby" runat="server" Enabled="False" Visible="False"></asp:TextBox>
                                      <br />
                                      <asp:TextBox ID="lblsecref" runat="server" Enabled="False" Visible="False"></asp:TextBox>
                                      <br />
                                      <br />
                                      <asp:TextBox ID="txtexpired" runat="server" Enabled="False" Visible="False"></asp:TextBox>
                                      <br />
                                      
                                      
                                  </td>
                              </tr>
                          </table>
                      </eo:CallbackPanel>
                      
                  </div><!-- /.box-body -->
                 <%-- <div class="box-footer"> 
                     <asp:Button ID="btncancel" runat="server" Text="Cancel" CssClass="btn btn-default"  /><asp:Button ID="btnnew" runat="server" Text="New   " CssClass="btn btn-info pull-right" />
                    <%--<button type="submit" class="btn btn-default">Cancel</button>
                    <button type="submit" class="btn btn-info pull-right">Sign in</button>--%>
                  </div><!-- /.box-footer --> 
          <%-- <div class="code">
<pre>
    &lt;asp:UpdatePanel runat="server" ID="UpdatePanel1"&gt;
        &lt;ContentTemplate&gt;
            Result:
            &lt;asp:Label runat="server" ID="Label1" ForeColor="Red"&gt;&lt;/asp:Label&gt;&lt;br /&gt;
            &lt;dns:ClassicButton runat="server" ID="ClassicButton1" OnClick="OnContextMenuItemClicked" Text="Click on me"
                ConfirmText="Are you sure?" /&gt;
        &lt;/ContentTemplate&gt;
    &lt;/asp:UpdatePanel&gt;
</pre>
    </div>--%>
         
          </ContentTemplate>
    </asp:UpdatePanel>
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
         var grid = eo_GetObject("<%=GrdListings.ClientID%>");

    var item = eventInfo.getItem();
    switch (item.getText()) {
       
        case "Approve":
            //function OnItemSelected(grid) {
            //Get the selected item
            var item = grid.getSelectedItem();
            

           

            grid.raiseItemCommandEvent(item.getIndex(), "Approve");
            //}
            break;
        
        case "Cancel":
            var x;
                //Get the selected item
            if (confirm("Press a button!") == true) {
                x = 1;
                var item = grid.getSelectedItem();


                grid.raiseItemCommandEvent(item.getIndex(), "Cancel");
            };
            document.getElementById("demo").innerHTML = x;
            break;
        case "Security Item Detail":

            //Get the selected item
            var item = grid.getSelectedItem();


            grid.raiseItemCommandEvent(item.getIndex(), "Detail");

            break;
        case "Security Report":

            //Get the selected item
            var item = grid.getSelectedItem();


            grid.raiseItemCommandEvent(item.getIndex(), "Report");

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

