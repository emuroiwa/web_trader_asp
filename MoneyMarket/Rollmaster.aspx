<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Main.Master" CodeBehind="Rollmaster.aspx.vb" Inherits="WEBTDS.Rollmaster" %>
<%@ Register assembly="EO.Web" namespace="EO.Web" tagprefix="eo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 
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

         
        <asp:Label ID="lblError" runat="server" Text=""></asp:Label>
        <asp:Label ID="lblWarning" runat="server" Text=""></asp:Label>
        <asp:Label ID="lblSuccess" runat="server" Text=""></asp:Label>
         <asp:UpdatePanel ID="UpdatePanelAll" UpdateMode="Always" runat="server"><ContentTemplate>
       
        <style>
            #leftcol{
   
    margin:0 auto;
    width:49%;
    float:left;
	    height:100%;
}

#rightcol{
    float:left;
    width:49%;
    height:100%;
    margin:0 auto;
    /*border:1px solid black;
*/
}
        </style>
<body>

    
              <div class="box box-info">
                <div class="box-header with-border">
                  <h3 class="box-title">Rollover Master</h3> 
                     
                </div><!-- /.box-header -->
                <!-- form start -->
             
                  <div class="box-body">
    
        <div id="leftcol">
          
              <div class="box box-info">
                <div class="box-header with-border">
                  <h3 class="box-title">Deals</h3> 
                     
                </div><!-- /.box-header -->
                <!-- form start -->
             
                  <div class="box-body">
                 
                      <eo:grid ID="GridDeals" runat="server" BorderColor="#C7D1DF" BorderWidth="1px" ColumnHeaderAscImage="00050303" ColumnHeaderDescImage="00050304" ColumnHeaderDividerImage="00050302" FixedColumnCount="1" Font-Bold="False" Font-Italic="False" Font-Names="Verdana" Font-Overline="False" Font-Size="9pt" Font-Strikeout="False" Font-Underline="False" GridLineColor="199, 209, 223" GridLines="Both" Height="252px" ItemHeight="19" Width="100%" ClientSideOnContextMenu="ShowContextMenu" ClientSideOnItemSelected="OnItemSelected" OnItemCommand="GridDeals_ItemCommand"  CssClass="table table-bordered table-striped" >
                                      <ItemStyles>
                                          <eo:griditemstyleset>
                                              <ItemStyle CssText="background-color: white" />
                                              <ItemHoverStyle CssText="background-image: url(00050206); background-repeat: repeat-x" />
                                              <SelectedStyle CssText="background-image: url(00050207); background-repeat: repeat-x" />
                                              <CellStyle CssText="padding-left:8px;padding-top:2px; color:#336699;white-space:nowrap;" />
                                          </eo:GridItemStyleSet>
                                      </ItemStyles>
                                      <ColumnHeaderStyle CssText="background-image:url('00050301');padding-left:8px;padding-top:2px;font-weight: bold;color:white;" />
                                      <Columns>
                                        
                                          <eo:staticcolumn DataField="dealref" HeaderText="Deal Reference" Width="200" AllowSort="True">
                                          </eo:StaticColumn>
                                          <eo:staticcolumn HeaderText="Interest Options" DataField="interestopt" Width="250" AllowSort="True">
                                          </eo:StaticColumn>
                                          <eo:staticcolumn DataField="freqopt" HeaderText="Frequency Options"  Width="200" AllowSort="True">
                                          </eo:StaticColumn>
                                          <eo:staticcolumn DataField="freqdays" HeaderText="Frequency Days"  Width="200" AllowSort="True">
                                          </eo:StaticColumn>
                                         
                                      </Columns>
                                  
                                      <FooterStyle CssText="padding-bottom:4px;padding-left:4px;padding-right:4px;padding-top:4px;" />
                                  </eo:Grid>
                  </div></div>
                      <%-- end of leftcol div --%>
                             </div>
                     <%-- end of leftcol div --%>
        <div id="rightcol">
        
              <div class="box box-info">
                <div class="box-header with-border">
                  <h3 class="box-title">Rollover Status</h3> 
                     
                </div><!-- /.box-header -->
                <!-- form start -->
             
                  <div class="box-body">
                                        
               <eo:grid ID="GrdRollOptions" runat="server" BorderColor="#C7D1DF" BorderWidth="1px" ColumnHeaderAscImage="00050303" ColumnHeaderDescImage="00050304" ColumnHeaderDividerImage="00050302" FixedColumnCount="1" Font-Bold="False" Font-Italic="False" Font-Names="Verdana" Font-Overline="False" Font-Size="9pt" Font-Strikeout="False" Font-Underline="False" GridLineColor="199, 209, 223" GridLines="Both" Height="252px" ItemHeight="19" Width="100%" ClientSideOnContextMenu="ShowContextMenu" ClientSideOnItemSelected="OnItemSelected" CssClass="table table-bordered table-striped" >
                                      <ItemStyles>
                                          <eo:griditemstyleset>
                                              <ItemStyle CssText="background-color: white" />
                                              <ItemHoverStyle CssText="background-image: url(00050206); background-repeat: repeat-x" />
                                              <SelectedStyle CssText="background-image: url(00050207); background-repeat: repeat-x" />
                                              <CellStyle CssText="padding-left:8px;padding-top:2px; color:#336699;white-space:nowrap;" />
                                          </eo:GridItemStyleSet>
                                      </ItemStyles>
                                      <ColumnHeaderStyle CssText="background-image:url('00050301');padding-left:8px;padding-top:2px;font-weight: bold;color:white;" />
                                      <Columns>
                                        
                                          <eo:staticcolumn DataField="rolloverdate" HeaderText="Roll Overdate" Width="200" AllowSort="True">
                                          </eo:StaticColumn>
                                          <eo:staticcolumn HeaderText="Deal Amount" DataField="dealamount" Width="250" AllowSort="True">
                                          </eo:StaticColumn>
                                          <eo:staticcolumn DataField="interestrate" HeaderText="Interest Rate"  Width="200" AllowSort="True">
                                          </eo:StaticColumn>
                                          <eo:staticcolumn DataField="maturityamount" HeaderText="Maturity Amount"  Width="200" AllowSort="True">
                                          </eo:StaticColumn>
                                          <eo:staticcolumn DataField="maturityamount" HeaderText="Maturity Amount"  Width="200" AllowSort="True">
                                          </eo:StaticColumn>
                                          <eo:staticcolumn DataField="actioned" HeaderText="actioned"  Width="75" AllowSort="True">
                                          </eo:StaticColumn>
                                         
                                      </Columns>
                                  
                                      <FooterStyle CssText="padding-bottom:4px;padding-left:4px;padding-right:4px;padding-top:4px;" />
                                  </eo:Grid>

                  </div></div>
             <%-- end of grid--%>
           

         <%-- end of rightcol div --%>
                             </div>
                      <table style="width:100%">
                              <tr style="padding-bottom:10px">
                                            <td style="width:20%;padding-bottom:10px"><asp:Label ID="Label14" runat="server" Font-Size="Small" Text="Customer Name"></asp:Label>
                                            </td>
                                  <td colspan="2">
                                      <asp:Label ID="lblCustName" runat="server" Text="" BackColor="#66FFFF"></asp:Label></td>
                                
                                  <td>
                                      <asp:Label ID="lblCustNumber" runat="server" Text="" BackColor="#66FFFF"></asp:Label></td>
                              </tr>
                              <tr style="padding-bottom:10px">
                                              <td style="width:20%;padding-bottom:10px"><asp:Label ID="Label1" runat="server" Font-Size="Small" Text="Deal Reference"></asp:Label></td>
                                  <td style="width:30%">
                                      <asp:Label ID="lblReference" runat="server" Text="" BackColor="#66FFFF"></asp:Label>
                                                                              <asp:TextBox ID="txtdealref" runat="server" BorderStyle="None" AutoPostBack="true" Enabled="False" BackColor="White" ForeColor="White" BorderColor="White"></asp:TextBox>

                                  </td>
                                
                                  <td style="width:20%">
                                    <asp:Label ID="Label27" runat="server" Font-Size="Small" Text="Tenor"></asp:Label>
                                              </td>
                                          <td style="width:30%">
                                      <asp:Label ID="lblTenor" runat="server" Text="" BackColor="#66FFFF"></asp:Label></td>
                              </tr>
                               <tr style="padding-bottom:10px">
                                              <td style="width:20%;padding-bottom:10px">
                                                  <asp:Label ID="DealAmount" runat="server" Font-Size="Small" Text=""></asp:Label>
                                              </td>
                                  <td colspan="1">
                                      <asp:Label ID="lblAmount" runat="server" BackColor="#66FFFF"></asp:Label></td>
                                
                                  <td>
                                      <asp:Label ID="Label26" runat="server" Font-Size="Small" Text="Yield Rate"></asp:Label>
                                              </td>
                                          <td>
                                      <asp:Label ID="lblRate" runat="server" Text="" BackColor="#66FFFF"></asp:Label></td>
                              </tr>
                                <tr style="padding-bottom:10px">
                                              <td style="width:20%;padding-bottom:10px"><asp:Label ID="Label16" runat="server" Font-Size="Small" Text="Start Date"></asp:Label>
                                              </td>
                                  <td colspan="1">
                                      <asp:Label ID="lblDateEntered" runat="server" Text="" BackColor="#66FFFF"></asp:Label></td>
                                
                                  <td>
                                      <asp:Label ID="Label25" runat="server" Font-Size="Small" Text="Discount&nbsp; Rate"></asp:Label>
                                              </td>
                                          <td>
                                      <asp:Label ID="lblDiscountRate" runat="server" Text="" BackColor="#66FFFF"></asp:Label></td>
                              </tr>

                                <tr style="padding-bottom:10px">
                                              <td style="width:20%;padding-bottom:10px"><asp:Label ID="Label17" runat="server" Font-Size="Small" Text="Maturity Date"></asp:Label>
                                              </td>
                                  <td colspan="1">
                                      <asp:Label ID="lblMaturityDate" runat="server" Text="" BackColor="#66FFFF"></asp:Label></td>
                                
                                  <td>
                                   
                                     <%-- <asp:Label ID="MaturityAmount" runat="server" Text=""></asp:Label>--%>
                                      <asp:Label ID="Label24" runat="server" Font-Size="Small" Text="Maturity Amount"></asp:Label>
                                  </td>
                                          <td>
                                      <asp:Label ID="lblMaturityAmount" runat="server" Text="" BackColor="#66FFFF"></asp:Label></td>
                              </tr>
                                          <tr style="padding-bottom:10px">
                                              <td style="width:20%;padding-bottom:10px"><asp:Label ID="Label18" runat="server" Font-Size="Small" Text="Tax Rate"></asp:Label>
                                              </td>
                                  <td colspan="1">
                                      <asp:Label ID="lblTaxRate" runat="server" Text="" BackColor="#66FFFF"></asp:Label></td>
                                
                                  <td>
                                      &nbsp;</td>
                                          <td>
                                     <%-- <asp:Label ID="lblNetInt" runat="server" Text="" BackColor="#66FFFF"></asp:Label>--%></td>
                              </tr>
                                 <tr style="padding-bottom:10px">
                                              <td style="width:20%;padding-bottom:10px"><asp:Label ID="Label19" runat="server" Font-Size="Small" Text="Tax Amount"></asp:Label>
                                              </td>
                                  <td colspan="1">
                                      <asp:Label ID="lblTaxamt" runat="server" Text="" BackColor="#66FFFF"></asp:Label></td>
                                
                                  <td>
                                      <asp:Label ID="Label23" runat="server" Font-Size="Small" Text="Status"></asp:Label>
                                              </td>
                                          <td>
                                              <asp:Label ID="lblDealStatus" runat="server" BackColor="#66FFFF" Text=""></asp:Label>
                                              </td>
                              </tr>
                                    <tr style="padding-bottom:10px">
                                              <td style="width:20%;padding-bottom:10px"><asp:Label ID="Label20" runat="server" Font-Size="Small" Text="Days To Maturity"></asp:Label>
                                              </td>
                                  <td colspan="1">
                                      <asp:Label ID="lblRemain" runat="server" BackColor="#66FFFF" Text=""></asp:Label>
                                              </td>
                                
                                  <td>
                                      <asp:Label ID="Label22" runat="server" Font-Size="Small" Text="Dealer"></asp:Label>
                                              </td>
                                          <td>
                                              <asp:Label ID="lblDealer" runat="server" Text="" BackColor="#66FFFF"></asp:Label>&nbsp;</td>
                              </tr>
                                    <tr style="padding-bottom:10px">
                                              <td style="width:20%;padding-bottom:10px">
                                                  <asp:Label ID="Label21" runat="server" Font-Size="Small" Text="Accrued To Date"></asp:Label>
                                              </td>
                                  <td colspan="1">
                                      <asp:Label ID="lblAccrued" runat="server" BackColor="#66FFFF" Text=""></asp:Label>
                                              </td>
                                
                                  <td>
                                      &nbsp;</td>
                                          <td>
                                                <%--  <asp:Label ID="Verifier" runat="server" Text="" BackColor="#66FFFF"></asp:Label>--%></td>
                              </tr>
                                    <tr style="padding-bottom:10px">
                                              <td style="width:20%;padding-bottom:10px">&nbsp;</td>
                                  <td colspan="1">
                                      &nbsp;</td>
                                
                                  <td>
                                      &nbsp;</td>
                                          <td>
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
                                            <td colspan="2"><br />
                                            </td>
                                  <td colspan="1">
                                      &nbsp;</td>
                                
                                  <td>
                                      &nbsp;</td>
                                          <td>
                                              &nbsp;</td>
                              </tr>
                          </table>
    
                              <%-- end of rightcol div --%>                

</body>







        </ContentTemplate></asp:UpdatePanel>
    </form>
</asp:Content>
