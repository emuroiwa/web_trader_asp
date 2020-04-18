<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Main.Master" CodeBehind="MaturitiesMaintanance.aspx.vb" Inherits="WEBTDS.MaturitiesMaintanance" %>

<%@ Register Assembly="EO.Web" Namespace="EO.Web" TagPrefix="eo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <!-- DataTables -->
     <!-- jQuery 2.1.4 -->
    <script src="../../plugins/jQuery/jQuery-2.1.4.min.js"></script>
    <script src="../../plugins/datatables/jquery.dataTables.min.js"></script>
    <script src="../../plugins/datatables/dataTables.bootstrap.min.js"></script>
    <script>
        $(function () {
            $("#example1").DataTable();
            $('#example2').DataTable({
                "paging": true,
                "lengthChange": false,
                "searching": false,
                "ordering": true,
                "info": true,
                "autoWidth": false
            });
        });
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
             var menu = eo_GetObject("<%=Menu1.ClientID%>");
               eo_ShowContextMenu(e, "<%=Menu1.ClientID%>");

               //Return true to indicate that we have
               //displayed a context menu
               return true;
           }

           function OnContextMenuItemClicked(e, eventInfo) {
               var grid = eo_GetObject("<%=GrdDealsMaturity.ClientID%>");

               var item = eventInfo.getItem();
               switch (item.getText()) {
                   case "Load Deal":
                       //Show the item details
                       var gridItem = grid.getItem(g_itemIndex);
                       alert(
                           "Details about this Deal:\r\n" +
                           "Deal Reference: " + gridItem.getCell(0).getValue() + "\r\n" +
                           "Client: " + gridItem.getCell(1).getValue() + "\r\n" +
                           "Deal Type: " + gridItem.getCell(9).getValue());
                       if (gridItem.getCell(9).getValue() == "Basic Loan") {
                           window.location = "MMLoan.aspx?dealreference=" + gridItem.getCell(0).getValue()
                       }
                       else {
                           window.location = "MMDeposit.aspx?dealreference=" + gridItem.getCell(0).getValue()
                       }
            //window.location.reload();
            break;

        // if(item.getText()=="Load Deal") {
       
              //   alert(gridItem.getCell(9).getValue())
                 //if (gridItem.getCell(9).getValue() = "Basic Loan") {
                 //    window.location = "MMLoan.aspx"
                 //}
                 //else {
                 //    window.location = "MMDeposit.aspx?dealreference="+ gridItem.getCell(0).getValue()
                 //}
            //window.location.reload();
        

       
    }
}

//<!--JS_SAMPLE_END-->
    </script>
       <!-- DataTables -->
    <link rel="stylesheet" href="../../plugins/datatables/dataTables.bootstrap.css">
     <form runat="server">
           <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager> 
         <asp:UpdatePanel ID="UpdatePanelCalender" UpdateMode="Conditional" runat="server"><ContentTemplate>
                                    <fieldset title="Calender">
  <div class="box box-info">
                <div class="box-header with-border">
                  <h3 class="box-title">Maturities Maintanance</h3> 
                </div>
      <div style=" width:100%; overflow: auto">
    <table style="width:100%;height:100%" >

        <tr style="height:25%;width:100%"><td>
    <asp:Calendar ID="CalendarMaturities" runat="server" BackColor="White" BorderColor="Black" BorderStyle="Solid" CellSpacing="1" Font-Names="Verdana" Font-Size="9pt" ForeColor="Black" Height="200px" NextPrevFormat="ShortMonth" Width="100%" SelectedDate="2014-10-02">
        <DayHeaderStyle Font-Bold="True" Font-Size="8pt" ForeColor="#333333" Height="8pt" />
        <NextPrevStyle Font-Bold="True" Font-Size="8pt" ForeColor="White" />
        <OtherMonthDayStyle ForeColor="#999999" />
        <SelectedDayStyle BackColor="#3c8cbc" ForeColor="White" />
        <TitleStyle BackColor="#3c8cbc" BorderStyle="Solid" Font-Bold="True" Font-Size="12pt" ForeColor="White" Height="12pt" />
        <TodayDayStyle BackColor="#999999" ForeColor="White" />
    </asp:Calendar>
            </td></tr>
        <tr style="height:75%"><td>
            <hr />
          
                                   <eo:ContextMenu ID="Menu1" runat="server" ClientSideOnItemClick="OnContextMenuItemClicked" ControlSkinID="None"  Width="100%">
                                      <TopGroup Style-CssText="cursor:hand;font-family:Verdana;font-size:11px;">
                                          <Items>
                                              <eo:MenuItem Text-Html="Load Deal">
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
               <eo:Grid ID="GrdDealsMaturity" runat="server" BorderColor="#C7D1DF" BorderWidth="1px" ColumnHeaderAscImage="00050303" ColumnHeaderDescImage="00050304" ColumnHeaderDividerImage="00050302" FixedColumnCount="1" Font-Bold="False" Font-Italic="False" Font-Names="Verdana" Font-Overline="False" Font-Size="9pt" Font-Strikeout="False" Font-Underline="False" GridLineColor="199, 209, 223" GridLines="Both" Height="252px" ItemHeight="19" Width="100%" ClientSideOnContextMenu="ShowContextMenu" ClientSideOnItemSelected="OnItemSelected" CssClass="table table-bordered table-striped" >
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
                                        
                                          <eo:StaticColumn DataField="DealReference" HeaderText="Deal Reference" Width="200" AllowSort="True">
                                          </eo:StaticColumn>
                                          <eo:StaticColumn HeaderText="Customer Name" DataField="FullName" Width="250" AllowSort="True">
                                          </eo:StaticColumn>
                                          <eo:StaticColumn DataField="CustomerNumber" HeaderText="Customer Number"  Width="200" AllowSort="True">
                                          </eo:StaticColumn>
                                          <eo:StaticColumn DataField="DealAmount" HeaderText="Deal Amount"  Width="200" AllowSort="True">
                                          </eo:StaticColumn>
                                          <eo:StaticColumn DataField="maturityamount" HeaderText="Maturity Amount"  Width="200" AllowSort="True">
                                          </eo:StaticColumn>
                                          <eo:StaticColumn DataField="InterestRate" HeaderText="InterestRate"  Width="75" AllowSort="True">
                                          </eo:StaticColumn>
                                          <eo:StaticColumn DataField="Instruction" HeaderText="Instruction1"  Width="250" AllowSort="True">
                                          </eo:StaticColumn>
                                          <eo:StaticColumn DataField="Instructionmat" HeaderText="Instruction2"  Width="250" AllowSort="True">
                                          </eo:StaticColumn>
                                          <eo:StaticColumn DataField="dealCancelled" HeaderText="Status"  Width="50" AllowSort="True">
                                          </eo:StaticColumn>
                                          <eo:StaticColumn DataField="othercharacteristics" HeaderText="Deal Structure"  Width="200" AllowSort="True">
                                          </eo:StaticColumn>
                                      </Columns>
                                  
                                      <FooterStyle CssText="padding-bottom:4px;padding-left:4px;padding-right:4px;padding-top:4px;" />
                                  </eo:Grid>
           
                
            </td></tr>
        </table>
      
   </div>
      </div>
                                        </fieldset></ContentTemplate>
                                    <Triggers><asp:AsyncPostBackTrigger ControlID="CalendarMaturities" EventName="SelectionChanged" />
                                                                          </Triggers></asp:UpdatePanel>
    </form>
     <!-- page script -->
    <script>
        $(function () {
            $("#example1").DataTable();
            $('#example2').DataTable({
                "paging": true,
                "lengthChange": false,
                "searching": false,
                "ordering": true,
                "info": true,
                "autoWidth": false
            });
        });
    </script> 
</asp:Content>
