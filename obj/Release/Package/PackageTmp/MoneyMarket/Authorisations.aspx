<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Main.Master" CodeBehind="Authorisations.aspx.vb" Inherits="WEBTDS.Authorisations" %>

<%@ Register Assembly="EO.Web" Namespace="EO.Web" TagPrefix="eo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
      <%--  function openModal(dealcanceled, ref, dealer) {
            document.getElementById("<%= lblDealCancelled.ClientID%>").value = dealcanceled;
            document.getElementById("<%= lblRef.ClientID%>").value = ref;
            document.getElementById("<%= lblDealer.ClientID%>").innerText = dealer;
            $('#decline').modal('show');
        }--%>
</script>   <script type="text/javascript">
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
        case "Detail":
            //Show the item details
            var gridItem = grid.getItem(g_itemIndex);
            alert(
                "Details about this grid item:\r\n" +
                "deal: " + gridItem.getCell(1).getValue() + "\r\n" +
                "Posted By: " + gridItem.getCell(2).getValue() + "\r\n" +
                "Topic: " + gridItem.getCell(3).getValue());
            document.getElementById("<%= txtdealref.ClientID%>").value = gridItem.getCell(1).getValue()
            //window.location.reload();
            break;

        case "Delete":
            //Stop editing
            grid.editCell(-1);

            //Delete the item
            grid.deleteItem(g_itemIndex);
            break;

        case "Add New":
            //This Grid's AllowNewItem is set to true. In this case
            //the Grid displays a temporary new item as the last item
            //The following code does not actually add a new item,
            //but rather put the temporary new item into edit mode
            var itemIndex = grid.getItemCount();

            //Put the item into edit mode
            grid.editCell(itemIndex, 1);

            //Scroll the item into view
            grid.getItem(itemIndex).ensureVisible();
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
            grid.raiseItemCommandEvent(item.getIndex(), "select");
        }
        //<!--JS_SAMPLE_END-->
    </script>
  <form id="form1" runat="server">
       <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
       <!-- Horizontal Form -->
      <asp:UpdatePanel ID="UpdatePanelClient" UpdateMode="Always" runat="server">
                        <ContentTemplate>
      <asp:Label ID="lblError" runat="server" Text=""></asp:Label><asp:Label ID="lblWarning" runat="server" Text=""></asp:Label><asp:Label ID="lblSuccess" runat="server" Text=""></asp:Label>
              <div class="box box-info">
                        <div class="box-header with-border">
                  <h3 class="box-title">Authorisations</h3> </div>
                  <!-- /.box-header -->
                <!-- form start -->
             
                  <div class="box-body">
                      <table style="width:100%;height:100px">
                          <tr><td colspan="2">
                        
                             
                                   <eo:ContextMenu ID="Menu1" runat="server" ClientSideOnItemClick="OnContextMenuItemClicked" ControlSkinID="None"  Width="100%">
                                      <TopGroup Style-CssText="cursor:hand;font-family:Verdana;font-size:11px;">
                                          <Items>
                                              <eo:MenuItem Text-Html="View Authorisation Info">
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
                                  <eo:Grid ID="GrdDealsMM" runat="server" BorderColor="#C7D1DF" BorderWidth="1px" ColumnHeaderAscImage="00050303" ColumnHeaderDescImage="00050304" ColumnHeaderDividerImage="00050302" FixedColumnCount="1" Font-Bold="False" Font-Italic="False" Font-Names="Verdana" Font-Overline="False" Font-Size="9pt" Font-Strikeout="False" Font-Underline="False" GridLineColor="199, 209, 223" GridLines="Both" Height="252px" ItemHeight="19" Width="100%" ClientSideOnContextMenu="ShowContextMenu" ClientSideOnItemSelected="OnItemSelected" OnItemCommand="GrdDealsMM_ItemCommand"  CssClass="table table-bordered table-striped" >
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
                                          <eo:StaticColumn DataField="InterestRate" HeaderText="InterestRate"  Width="100" AllowSort="True">
                                          </eo:StaticColumn>
                                          <eo:StaticColumn DataField="Tenor" HeaderText="Tenor"  Width="100" AllowSort="True">
                                          </eo:StaticColumn>
                                          <eo:StaticColumn DataField="StartDate" HeaderText="Starts"  Width="100" AllowSort="True">
                                          </eo:StaticColumn>
                                          <eo:StaticColumn DataField="MaturityDate" HeaderText="Matures"  Width="200" AllowSort="True">
                                          </eo:StaticColumn>
                                          <eo:StaticColumn DataField="DealCapturer" HeaderText="Dealer"  Width="200" AllowSort="True">
                                          </eo:StaticColumn>
                                      </Columns>
                                  
                                      <FooterStyle CssText="padding-bottom:4px;padding-left:4px;padding-right:4px;padding-top:4px;" />
                                  </eo:Grid>
                            
                              </td></tr>


                          </table></div>   </div>  <eo:CallbackPanel ID="CallbackPanel2" runat="server" Height="150px" Width="100%" Triggers="{ControlID:txtdealref;Parameter:}">
                                 
                             
                                 <div class="row">
            <div class="col-md-6">
                                       <div class="panel box box-success">
                      <div class="box-header with-border">
                        <h4 class="box-title">
                            <a href="#security"><asp:Label ID="lblCurrency1" runat="server" Text=""></asp:Label></a><a data-toggle="collapse" data-parent="#accordion" href="#currency">
                           </a>
                        </h4>
                      </div>
                      <div id="currency" class="panel-collapse collapse in">
                        <div class="box-body">

                          <table style="width:100%">
                              <tr style="padding-bottom:10px">
                                            <td style="width:20%;padding-bottom:10px">Customer Name</td>
                                  <td colspan="2">
                                      <asp:Label ID="lblCustName" runat="server" Text="" BackColor="#66FFFF"></asp:Label></td>
                                
                                  <td>
                                      <asp:Label ID="lblCustNumber" runat="server" Text="" BackColor="#66FFFF"></asp:Label></td>
                              </tr>
                              <tr style="padding-bottom:10px">
                                              <td style="width:20%;padding-bottom:10px">Deal Reference</td>
                                  <td style="width:30%">
                                      <asp:Label ID="lblReference" runat="server" Text="" BackColor="#66FFFF"></asp:Label>
                                                                              <asp:TextBox ID="txtdealref" runat="server" AutoPostBack="true"  BorderStyle="None"  Enabled="False" BackColor="White" ForeColor="White" BorderColor="White"></asp:TextBox>

                                  </td>
                                
                                  <td style="width:20%">
                                    Tenor</td>
                                          <td style="width:30%">
                                      <asp:Label ID="lblTenor" runat="server" Text="" BackColor="#66FFFF"></asp:Label></td>
                              </tr>
                               <tr style="padding-bottom:10px">
                                              <td style="width:20%;padding-bottom:10px">
                                                  <asp:Label ID="DealAmount" runat="server" Text=""></asp:Label></td>
                                  <td colspan="1">
                                      <asp:Label ID="lblAmount" runat="server" BackColor="#66FFFF"></asp:Label></td>
                                
                                  <td>
                                      Yield Rate</td>
                                          <td>
                                      <asp:Label ID="lblRate" runat="server" Text="" BackColor="#66FFFF"></asp:Label></td>
                              </tr>
                                <tr style="padding-bottom:10px">
                                              <td style="width:20%;padding-bottom:10px">Start Date</td>
                                  <td colspan="1">
                                      <asp:Label ID="lblDateEntered" runat="server" Text="" BackColor="#66FFFF"></asp:Label></td>
                                
                                  <td>
                                      Discount&nbsp; Rate</td>
                                          <td>
                                      <asp:Label ID="lblDiscountRate" runat="server" Text="" BackColor="#66FFFF"></asp:Label></td>
                              </tr>

                                <tr style="padding-bottom:10px">
                                              <td style="width:20%;padding-bottom:10px">Maturity Date</td>
                                  <td colspan="1">
                                      <asp:Label ID="lblMaturityDate" runat="server" Text="" BackColor="#66FFFF"></asp:Label></td>
                                
                                  <td>
                                   
                                      <asp:Label ID="MaturityAmount" runat="server" Text=""></asp:Label>
                                  </td>
                                          <td>
                                      <asp:Label ID="lblMaturityAmount" runat="server" Text="" BackColor="#66FFFF"></asp:Label></td>
                              </tr>
                                          <tr style="padding-bottom:10px">
                                              <td style="width:20%;padding-bottom:10px">Tax Rate</td>
                                  <td colspan="1">
                                      <asp:Label ID="lblTaxRate" runat="server" Text="" BackColor="#66FFFF"></asp:Label></td>
                                
                                  <td>
                                    
                                      <asp:Label ID="Netinterest" runat="server" Text=""></asp:Label>
                                  </td>
                                          <td>
                                      <asp:Label ID="lblNetInt" runat="server" Text="" BackColor="#66FFFF"></asp:Label></td>
                              </tr>
                                 <tr style="padding-bottom:10px">
                                              <td style="width:20%;padding-bottom:10px">Tax Amount</td>
                                  <td colspan="1">
                                      <asp:Label ID="lblTaxamt" runat="server" Text="" BackColor="#66FFFF"></asp:Label></td>
                                
                                  <td>
                                      &nbsp;</td>
                                          <td>
                                              &nbsp;</td>
                              </tr>
                                    <tr style="padding-bottom:10px">
                                              <td style="width:20%;padding-bottom:10px">&nbsp;</td>
                                  <td colspan="1">
                                      &nbsp;</td>
                                
                                  <td>
                                      Dealer</td>
                                          <td>
                                              <asp:Label ID="lblDealer" runat="server" Text="" BackColor="#66FFFF"></asp:Label>&nbsp;</td>
                              </tr>
                                    <tr style="padding-bottom:10px">
                                              <td style="width:20%;padding-bottom:10px">
                                                  &nbsp;</td>
                                  <td colspan="1">
                                      &nbsp;</td>
                                
                                  <td>
                                      Verified By</td>
                                          <td>
                                                  <asp:Label ID="Verifier" runat="server" Text="" BackColor="#66FFFF"></asp:Label></td>
                              </tr>
                                    <tr style="padding-bottom:10px">
                                              <td style="width:20%;padding-bottom:10px">&nbsp;</td>
                                  <td colspan="1">
                                      &nbsp;</td>
                                
                                  <td>
                                      Email</td>
                                          <td>
                                              <asp:Label ID="linkEmail" runat="server" Text="" BackColor="#66FFFF"></asp:Label>
                                              &nbsp;</td>
                              </tr>
                              </tr>
                                    <tr style="padding-bottom:10px">
                                              <td style="width:20%;padding-bottom:10px">
                                                  <asp:Label ID="dealtype" runat="server" Text=""></asp:Label>
                                              </td>
                                  <td colspan="1">
                                      &nbsp;</td>
                                
                                  <td><asp:Label ID="lblCurrency" runat="server" Text="" BackColor="Yellow"></asp:Label>
                                     </td>
                                          <td>
                                               <asp:Label ID="lblCom" runat="server" Text="" BackColor="#66FFFF"></asp:Label>
                                              </td>
                              </tr>
                                    <tr style="padding-bottom:10px">
                                            <td colspan="2"><asp:Label ID="lblReason" runat="server" Text="Reason Cancelled" Visible="false"></asp:Label><br />
                                                <asp:TextBox ID="txtReason" runat="server" TextMode="MultiLine" Visible="False" Width="100%"></asp:TextBox>
                                            </td>
                                  <td colspan="1">
                                      Status</td>
                                
                                  <td>
                                      <asp:Label ID="lblDealStatus" runat="server" Text="" BackColor="#66FFFF"></asp:Label>
                                            </td>
                                          <td>
                                              &nbsp;</td>
                              </tr>
                          </table>
                        </div>
                      </div>
                    </div>
                           </div>
            <div class="col-md-6">
                              
                                        <!-- panel 2-->
                    <div class="panel box box-success">
                      <div class="box-header with-border">
                        <h4 class="box-title">
                         
                              <a data-toggle="collapse" data-parent="#accordion" href="#instructions">
                            Instructions
                          </a>
                        </h4>
                      </div>
                      <div id="instructions" class="panel-collapse collapse">
                        <div class="box-body">
                         <table style="width:100%">
                             <tr><td>Deal Starts</td>
                                 <td>
                                     <asp:TextBox ID="lblStartInstr" runat="server" TextMode="MultiLine" Width="100%" Enabled="false"></asp:TextBox></td>

                             </tr>
                             <tr><td>Deal End</td>
                                 <td>
                                     <asp:TextBox ID="lblEnd" runat="server" TextMode="MultiLine" Width="100%" Enabled="false"></asp:TextBox></td>

                             </tr>
                         </table>
                        </div>
                      </div>
                    </div>
                                             <!-- panel 2-->
                    <div class="panel box box-success">
                      <div class="box-header with-border">
                        <h4 class="box-title">
                          <a data-toggle="collapse" data-parent="#accordion" href="#securitydetails">
                            Security&nbsp; Details 
                          </a>
                        </h4>
                      </div>
                      <div id="securitydetails" class="panel-collapse collapse">
                        <div class="box-body">
                            <asp:TextBox ID="txtSecurityDetails" runat="server" TextMode="MultiLine" Width="100%" Enabled="false"></asp:TextBox>
                        </div>
                      </div>
                    </div>            <!-- panel 2-->
                    <div class="panel box box-success">
                      <div class="box-header with-border">
                        <h4 class="box-title">
                          <a data-toggle="collapse" data-parent="#accordion" href="#settlement">
                            Settlement 
                          </a>
                        </h4>
                      </div>
                      <div id="settlement" class="panel-collapse collapse">
                        <div class="box-body">
                            <asp:Label ID="Label2" runat="server" Text=""></asp:Label>
                        </div>
                      </div>
                    </div>
                            <center>                     <div class="panel box box-success">
                      <div class="box-header with-border">
                        <h4 class="box-title">
                          <a data-toggle="collapse" data-parent="#accordion" href="#buttons">
                           Authorise 
                          </a>
                        </h4>
                      </div>
                      <div id="buttons" class="panel-collapse collapse-in">
                        <div class="box-body"><table style="width:100%">
    <tr>
         
        <td>
            <asp:Button ID="cmdAuthorize" runat="server" Text="Authorise"  CssClass="btn btn-success btn-block btn-flats" Height="40px" Width="100px" /></td>
        <td><a data-target="#decline" data-toggle="modal">
                                <button class="btn btn-danger btn-block btn-flats" style="width:100px;height:40px">
                                   Decline
                                </button>
                                </a></td>
        <td><asp:Button ID="CmdRefresh" runat="server" Text="Refresh"  CssClass="btn btn-info btn-block btn-flats" Height="40px" Width="100px"  /></td>
       </tr>
  <tr>
       
        <td colspan="4">
            <asp:CheckBox ID="PrintConfirmNow" runat="server" ForeColor="Red" Text="Print confirmation immediately" />
        </td>
    </tr>

</table>   </div>
                      </div>
                    </div> </center> 


                                         </eo:CallbackPanel> 
                            

                      </div>
                                    </div>
                           
      
          <div class="example-modal1 modal" id="decline" >
                        <div class="modal-dialog" >
                            <div class="modal-content"><div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                <h4 class="modal-title">Authorisations Decline </h4></div>
                
                             
                                <div class="modal-body">
                                     <table style="width:100%">
                                         <tr><td style="width:20%;padding-bottom:15px">Deal Cancelled</td><td>
                                             <asp:Label ID="lblDealCancelled"  BackColor="#66FFFF" runat="server" Text=""></asp:Label></td></tr>
                                         <tr><td style="width:20%;padding-bottom:15px">Deal Reference</td><td>
                                             <asp:Label ID="lblRef"  BackColor="#66FFFF" runat="server" Text=""></asp:Label></td></tr>
                                         <tr><td style="width:20%;padding-bottom:15px">Dealer Name</td><td>
                                             <asp:Label ID="lblDealerName"  BackColor="#66FFFF" runat="server" Text=""></asp:Label></td></tr>
                                         <tr><td style="width:20%;padding-bottom:15px">Comment</td><td>
                                             <asp:TextBox ID="txtComment" runat="server" TextMode="MultiLine" Rows="3" Width="80%"></asp:TextBox></td></tr>
                                          <tr><td></td><td>
                                              <asp:Button ID="btnDecline" runat="server" CssClass="btn btn-danger btn-block btn-flats" Height="40px" Text="Decline Authorisation" Width="150px"  />
                                            </td></tr>
                                     </table>
                        </div><div class="modal-footer">
                            <button type="button" class="btn btn-default pull-left" data-dismiss="modal">Close</button></div></div></div><!-- /.modal-dialog -->

          </div>
       </ContentTemplate></asp:UpdatePanel>
                            </form>
</asp:Content>
