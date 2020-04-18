<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Main.Master" CodeBehind="ActionList.aspx.vb" Inherits="WEBTDS.ActionList" %>

<%@ Register Assembly="EO.Web" Namespace="EO.Web" TagPrefix="eo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <script type="text/javascript">
         //<!--JS_SAMPLE_BEGIN-->

      

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

    
        
<body>

         <asp:UpdatePanel ID="UpdatePanelAll" UpdateMode="Always" runat="server"><ContentTemplate>
       
          <div class="row">
            <div class="col-md-3">
          
              <div class="box box-info">
                <div class="box-header with-border">
                  <h3 class="box-title">Filter Options</h3> 
                     
                </div><!-- /.box-header -->
                <!-- form start -->
             
                  <div class="box-body">
                       <br />
                      <br />

                      <asp:CheckBox ID="CheckShowAll"  AutoPostBack="true" runat="server" Text="Show All Pending" />

                      <br />
                      <br />

                      <br />

                     <table style="width:100%;padding-bottom:20px">
                         <tr>
                             <td style="width:30%;padding-bottom:20px" >Currency</td>
                             <td style="width:70%;padding-bottom:20px"> <asp:DropDownList ID="cmbCurrency" runat="server" AutoPostBack="true" class="form-control select2" style="width: 80%;"></asp:DropDownList></td>

                         </tr>
                           <tr>
                                 <td style="width:30%">Status</td>
                             <td style="width:70%"><asp:DropDownList ID="cmbActionStatus" runat="server" AutoPostBack="true" class="form-control select2" Width="80%"><asp:ListItem></asp:ListItem><asp:ListItem>Actioned</asp:ListItem><asp:ListItem>Pending</asp:ListItem></asp:DropDownList></td>

                           </tr>
                         <tr>
                             <td colspan="2">
                               
                             </td>
                         </tr> 
                         <tr>
                             <td colspan="2"><br /><br /><br />Maturity Date<center>
                                 <asp:Calendar ID="CalendarMaturities2" runat="server" Width="80%" BackColor="#FFFFCC" BorderColor="#FFCC66" BorderWidth="1px" DayNameFormat="Shortest" Font-Names="Verdana" Font-Size="8pt" ForeColor="#663399" Height="220px" ShowGridLines="True">
                                     <DayHeaderStyle BackColor="#FFCC66" Font-Bold="True" Height="1px" />
                                     <NextPrevStyle Font-Size="9pt" ForeColor="#FFFFCC" />
                                     <OtherMonthDayStyle ForeColor="#CC9966" />
                                     <SelectedDayStyle BackColor="#CCCCFF" Font-Bold="True" />
                                     <SelectorStyle BackColor="#FFCC66" />
                                     <TitleStyle BackColor="#990000" Font-Bold="True" Font-Size="9pt" ForeColor="#FFFFCC" />
                                     <TodayDayStyle BackColor="#FFCC66" ForeColor="White" />
                                 </asp:Calendar></center>
                             </td>
                         </tr>
                     </table>
                      <br /> <br /> <br /> <br /> <br /> <br /> <br /> <br /> <br />
                  </div></div>
                      <%-- end of leftcol div --%>
                             </div>
                     <%-- end of leftcol div --%>
   
            <div class="col-md-9">
              <div class="box box-info">
                <div class="box-header with-border">
                  <h3 class="box-title">Filter Options</h3> 
                     
                </div><!-- /.box-header -->
                <!-- form start -->
             
                  <div class="box-body">
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
               <eo:Grid ID="GrdDealsMaturity" runat="server" BorderColor="#C7D1DF" BorderWidth="1px" ColumnHeaderAscImage="00050303" ColumnHeaderDescImage="00050304" ColumnHeaderDividerImage="00050302" FixedColumnCount="1" Font-Bold="False" Font-Italic="False" Font-Names="Verdana" Font-Overline="False" Font-Size="9pt" Font-Strikeout="False" Font-Underline="False" GridLineColor="199, 209, 223" GridLines="Both" Height="252px" ItemHeight="19" Width="100%" ClientSideOnContextMenu="ShowContextMenu" ClientSideOnItemSelected="OnItemSelected" OnItemCommand="GrdDealsMaturity_ItemCommand"  CssClass="table table-bordered table-striped" >
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

                  </div></div>
             <%-- end of grid--%>
              <div class="example-modal1 modal" id="AttachSecurity">
                        <div class="modal-dialog" >
                            <div class="modal-content"><div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                <h4 class="modal-title">Attach Deals As Security </h4></div>
                                <div class="modal-body"><p><asp:Label ID="Label1" runat="server"></asp:Label><asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Conditional" runat="server"><ContentTemplate>
                                    <fieldset title="Security">
                                    <table style="width: 100%; height: 178px; table-layout: auto;">
                                    <tr><td colspan="2">
                                    <asp:DropDownList ID="cmbTB" runat="server" class="form-control select2" Width="100%" AutoPostBack="True"><asp:ListItem Text="Please Select Security" Value="0"></asp:ListItem></asp:DropDownList><hr /></td></tr>
                                        <tr><td style="width:30%">Currency</td>
                                        <td style="width:70%; padding-bottom:10px "><asp:TextBox ID="lblcurrency1" runat="server" Width="70%" Enabled="false"></asp:TextBox></td></tr>
                                        <tr><td style="width:30%">Purchase Value</td>
                                            <td style ="width:70%; padding-bottom:10px "><asp:TextBox ID="PurValue" runat="server"  Width="70%" Enabled="false"></asp:TextBox></td></tr>
                                        <tr><td style="width:30%">Interest Rate</td>
                                            <td style ="width:70%; padding-bottom:10px "><asp:TextBox ID="IntRate" runat="server"  Width="70%" Enabled="false"></asp:TextBox></td></tr>
                                         <tr><td style="width:30%">Days To Maturity</td><td style="width:70%; padding-bottom:10px ">
                           <asp:TextBox ID="DaysMaturity" runat="server"  Width="70%"></asp:TextBox></td></tr>
                                        <tr><td style="width:30%">Avaliable To Sale</td><td style="width:70%; padding-bottom:10px ">
                           <asp:TextBox ID="AvaliableForSale" runat="server"  Width="70%"></asp:TextBox></td></tr>

                                        <tr><td style="width:30%">Maturity Date</td><td style="width:70%; padding-bottom:10px ">
                                            <asp:TextBox ID="txtMaturityDate" runat="server"  Enabled="false" Width="70%" ></asp:TextBox></td></tr>
                                        <tr><td style="width:30%">Security Amount</td><td style="width:70%; padding-bottom:10px "><asp:TextBox ID="txtSale" runat="server"   Width="70%"></asp:TextBox></td><asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Error!! Enter Security Amount " ControlToValidate="txtSale"></asp:RequiredFieldValidator></tr>
                                        <tr><td colspan="2"><asp:Button ID="cmdSale" runat="server" Text="Add" class="btn btn-block btn-success "></asp:Button></td>

                                        </tr></table><hr /></fieldset></ContentTemplate>
                                    <Triggers><asp:AsyncPostBackTrigger ControlID="cmbTB" EventName="SelectedIndexChanged" />

                                    </Triggers></asp:UpdatePanel>
                        </div><div class="modal-footer">
                            <button type="button" class="btn btn-default pull-left" data-dismiss="modal">Close</button></div></div></div><!-- /.modal-dialog --></div>
             <div class="box box-info">
                <div class="box-header with-border">
                  <h3 class="box-title">
                      <asp:Label ID="lbldealdescription" runat="server" Text=""></asp:Label>
                                  <asp:Label ID="lblCurrency" runat="server" Text="" BackColor="Yellow"></asp:Label>
                              </h3> 
                     
                </div><!-- /.box-header -->
                <!-- form start -->
             
                  <div class="box-body">
                                                                                                    <asp:TextBox ID="txtdealref" runat="server" BorderStyle="None" AutoPostBack="true" Enabled="False" BackColor="White" ForeColor="White" BorderColor="White"></asp:TextBox>

                                  
                      <table style="width:100%" border="1">
                          <tr>
                              <td style="width:13%">
                                  Customer Name</td><td style="width:20%;padding-bottom:15px">
                                      <asp:Label ID="lblCustName" runat="server" Text="" BackColor="#66FFFF"></asp:Label>
                                      <asp:Label ID="lblCustNumber" runat="server" Text="" BackColor="#66FFFF"></asp:Label></td><td style="width:13%">
                              Tax Rate</td><td style="width:20%;padding-bottom:15px">
                                      <asp:Label ID="lblTaxRate" runat="server" Text="" BackColor="#66FFFF"></asp:Label></td><td style="width:13%">
                                      Yield Rate</td><td style="width:20%;padding-bottom:15px">
                                      <asp:Label ID="lblRate" runat="server" Text="" BackColor="#66FFFF"></asp:Label></td>

                          </tr>
                                                    <tr>
                              <td style="width:13%; height: 23px;">
                                                  Deal Reference</td><td style="width:20%; padding-bottom:15px">
                                      <asp:Label ID="lblReference" runat="server" Text="" BackColor="#66FFFF"></asp:Label>
                                                                              </td><td style="width:13%; height: 23px;">
                                                        Tax Amount</td><td style="width:20%;padding-bottom:15px">
                                      <asp:Label ID="lblTaxamt" runat="server" Text="" BackColor="#66FFFF"></asp:Label></td><td style="width:13%; height: 23px;">
                                   Maturity Amount
                                     <%-- <asp:Label ID="MaturityAmount" runat="server" Text=""></asp:Label>--%>
                                                        </td><td style="width:20%;padding-bottom:15px">
                                      <asp:Label ID="lblMaturityAmount" runat="server" Text="" BackColor="#66FFFF"></asp:Label></td>

                          </tr>
                          <tr>
                              <td style="width:13%">
                                                  <asp:Label ID="DealAmount0" runat="server" Text="Deal Amount"></asp:Label>
                                                        </td><td style="width:20%;padding-bottom:15px">
                                      <asp:Label ID="lblAmount" runat="server" BackColor="#66FFFF"></asp:Label></td><td style="width:13%">
                              Days To Maturity</td><td style="width:20%;padding-bottom:15px">
                                      <asp:Label ID="lblRemain" runat="server" BackColor="#66FFFF" Text=""></asp:Label>
                                              </td><td style="width:13%">
                                    Net Interest
                                     <%-- <asp:Label ID="Netinterest" runat="server" Text=""></asp:Label>--%>
                              </td><td style="width:20%;padding-bottom:15px">
                                      <asp:Label ID="lblNetInt" runat="server" Text="" BackColor="#66FFFF"></asp:Label></td>

                          </tr>
                          <tr>
                              <td style="width:13%">
                                  Start Date</td><td style="width:20%;padding-bottom:15px">
                                      <asp:Label ID="lblDateEntered" runat="server" Text="" BackColor="#66FFFF"></asp:Label></td><td style="width:13%">
                                                  Accrued To Date</td><td style="width:20%;padding-bottom:15px">
                                      <asp:Label ID="lblAccrued" runat="server" BackColor="#66FFFF" Text=""></asp:Label>
                                              </td><td style="width:13%">
                                      Interest Day Basis</td><td style="width:20%;padding-bottom:15px">
                                              <asp:Label ID="lblintDaysbasis" runat="server" BackColor="#66FFFF" Text=""></asp:Label>
                                              </td>

                          </tr>
                          <tr>
                              <td style="width:13%">
                                  Maturity Date</td><td style="width:20%;padding-bottom:15px">
                                      <asp:Label ID="lblMaturityDate" runat="server" Text="" BackColor="#66FFFF"></asp:Label></td><td style="width:13%">
                                    Tenor</td><td style="width:20%;padding-bottom:15px">
                                      <asp:Label ID="lblTenor" runat="server" Text="" BackColor="#66FFFF"></asp:Label></td><td style="width:13%">
                                      Dealer</td><td style="width:20%;padding-bottom:15px">
                                              <asp:Label ID="lblDealer" runat="server" Text="" BackColor="#66FFFF"></asp:Label></td>

                          </tr><tr>
                              <td style="width:13%">
                                      Status</td><td style="width:20%;padding-bottom:15px">
                                      <asp:Label ID="lblDealStatus" runat="server" Text="" BackColor="#66FFFF"></asp:Label>
                                            </td><td style="width:13%">
                                      Discount&nbsp; Rate</td><td style="width:20%;padding-bottom:15px">
                                      <asp:Label ID="lblDiscountRate" runat="server" Text="" BackColor="#66FFFF"></asp:Label></td><td style="width:13%">
                                      Verified By</td><td style="width:20%;padding-bottom:15px">
                                                  <asp:Label ID="Verifier" runat="server" Text="" BackColor="#66FFFF"></asp:Label></td>

                          </tr>
<tr>
                              <td colspan="6">
                                  <center>


                                                <asp:TextBox ID="txtInstructions" runat="server" TextMode="MultiLine"  Width="100%" Height="100px" Enabled="false"></asp:TextBox>


                                  </center>
                                      </td>

                          </tr>

                      </table>
                  </div></div>

         <%-- end of rightcol div --%>
                             </div>
    </div>
                              <%-- end of rightcol div --%>                
           </ContentTemplate></asp:UpdatePanel>
</body>







 
    </form>
</asp:Content>
