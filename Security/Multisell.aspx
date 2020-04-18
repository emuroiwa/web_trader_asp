
<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Main.Master" CodeBehind="Multisell.aspx.vb" Inherits="WEBTDS.Multisell" %>

<%@ Register Assembly="EO.Web" Namespace="EO.Web" TagPrefix="eo" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   
 
   
 <head />
  <form id="form1" runat="server">
       <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
     <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
 <asp:Label ID="lblError" runat="server"></asp:Label>
        <asp:Label ID="lblsuccess" runat="server"></asp:Label>
                  <asp:Label ID="lblwarning" runat="server"> </asp:Label>
      <%-- <!-- Horizontal Form -->
              <div class="box box-info">
                <div class="box-header with-border">
                  <h3 class="box-title">Securities Master </h3>  
   
                </div><!-- /.box-header -->
                <!-- form start -->
             
                  <div class="box-body">--%>

                <div class="box box-success">
           <div class="box-header with-border">
                  <h3 class="box-title">Multi Sell </h3>
                  <div class="box-tools pull-right"> 
                    <%--<button class="btn btn-box-tool" data-widget="remove"><i class="fa fa-times"></i></button>--%>
                      <%--<button class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i></button>--%>
                  </div><!-- /.box-tools -->
                </div><!-- /.box-header -->
                <div class="box-body">

                
                     </div></div> 
                              <ContentTemplate>
                               <center>  
                                   

                                   <table style="width:80%;">
                                     <tr>
                                           <td style="width: 13%">
                                               <asp:Label ID="lbltb" runat="server" Font-Bold="True" Font-Size="Medium" Text="Purchases"></asp:Label>
                                           </td>
                                           <td style="width: 10%">
                                               <asp:Label ID="Label16" runat="server" Text="Currency"></asp:Label>
                                           </td>
                                           <td style="width: 10%">
                                               <asp:TextBox ID="lblCurrency" runat="server" Enabled="False" Font-Size="Small" required="required" style="margin-left: 0px" title="Please Enter Present value" Width="75%"></asp:TextBox>
                                           </td>
                                           <td style="width: 10%">
                                               <asp:Label ID="Label19" runat="server" Text="Days To Maturity"></asp:Label>
                                           </td>
                                           <td style="width: 8%">
                                               <asp:TextBox ID="lblDaysToMaturity" runat="server" Enabled="False" Font-Size="Small" required="required" style="margin-left: 0px" title="Please Enter Present value" Width="75%"></asp:TextBox>
                                           </td>
                                           <td style="width: 10%">
                                               <asp:Label ID="lblreff" runat="server" Visible="False"></asp:Label>
                                           </td>
                                          
                                       </tr>
                                           <tr>
                                               <td style="width: 8%; ">
                                                   <asp:DropDownList ID="cmbTB" runat="server" AutoPostBack="True" class="form-control select2" Font-Size="Small" style="margin-left: 0px" Width="98%">
                                                       <asp:ListItem Text="Select TB" Value="0"></asp:ListItem>
                                                   </asp:DropDownList>
                                                   <br />
                                               </td>
                                               <td class="modal-sm" style="width: 15%; ">
                                                   <asp:Label ID="lblDesc" runat="server" Text="Amount Available"></asp:Label>
                                               </td>
                                               <td style="width: 3%; ">
                                                   <asp:TextBox ID="lblAvail" runat="server" Enabled="False" Font-Size="Small" required="required" style="margin-left: 0px" title="Please Enter Present value" Width="75%"></asp:TextBox>
                                                   <br />
                                               </td>
                                               <td style="width: 15%; ">
                                                   <asp:Label ID="Label20" runat="server" Text="Interest days basis"></asp:Label>
                                               </td>
                                               <td style="width: 5%; ">
                                                   <asp:TextBox ID="lblBasisMulti" runat="server" Enabled="False" Font-Size="Small" required="required" style="margin-left: 0px" title="Please Enter Present value" Width="75%"></asp:TextBox>
                                               </td>
                                               <td style="width: 10%; ">
                                                   
                                                   <asp:Label ID="lblpuref" runat="server" Visible="False"></asp:Label>
                                                   
                                                   <br />
                                                   
                                                   </td>
                                               
                                           </tr>
                                           <tr>
                                               <td style="width: 5%">
                                                   <asp:TextBox ID="selloptionValue" runat="server" Visible="False"></asp:TextBox>
                                               </td>
                                               <td class="modal-sm" style="width: 15%">
                                                   <asp:Label ID="rate" runat="server" Text="Discount Rate"></asp:Label>
                                               </td>
                                               <td style="width: 3%">
                                                   <asp:TextBox ID="lblPurchaseRate" runat="server" Enabled="False" Font-Size="Small" required="required" style="margin-left: 0px" title="Please Enter Present value" Width="75%"></asp:TextBox>
                                               </td>
                                               <td style="width: 10%">
                                                   <asp:Label ID="Label21" runat="server" Text="Tenor"></asp:Label>
                                               </td>
                                               <td style="width: 8%">
                                                   <asp:TextBox ID="lblTenorMulti" runat="server" Enabled="False" Font-Size="Small" required="required" style="margin-left: 0px" title="Please Enter Present value" Width="75%"></asp:TextBox>
                                               </td>
                                               <td style="width: 10%">
                                                   <asp:Label ID="portdesc" runat="server"></asp:Label>
                                                   <asp:Label ID="portidd" runat="server"></asp:Label>
                                               </td>
                                              
                                           </tr>
                                           
                                    
                                   </table>
                                    </center>
                                  <hr />
                                   
                <fieldset title="Security">
                       <div class="panel box box-success">
                      <div class="box-header with-border">
                        <h4 class="box-title">
                          <a data-toggle="collapse" data-parent="#accordion" href="#history1">
                       Action Sale
                          </a>
                        </h4>
                      </div>
                      <div id="history1" class="panel-collapse collapse-in">
                        <div class="box-body">
<eo:Grid ID="Grid1" runat="server" BorderColor="#C7D1DF" BorderWidth="1px" ColumnHeaderAscImage="00050303" ColumnHeaderDescImage="00050304" ColumnHeaderDividerImage="00050302" FixedColumnCount="1" Font-Bold="False" Font-Italic="False" Font-Names="Verdana" Font-Overline="False" Font-Size="9pt" Font-Strikeout="False" Font-Underline="False" GridLineColor="199, 209, 223" GridLines="Both" Height="85px" ItemHeight="19" Width="1115px">
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
        <eo:RowNumberColumn HeaderText="RowNum" Width="50">
        </eo:RowNumberColumn>
        <eo:StaticColumn HeaderText="SecRef" Width="150" DataField="SecRef">
        </eo:StaticColumn>
        <eo:StaticColumn HeaderText="DealRef" Width="150" DataField="dealref">
        </eo:StaticColumn>
        <eo:StaticColumn HeaderText="SaleAmt" Width="150" DataField="SelAmt">
        </eo:StaticColumn>
        <eo:StaticColumn HeaderText="Cost" Width="150" DataField="cost">
        </eo:StaticColumn>
        <eo:StaticColumn HeaderText="Profit" Width="150" DataField="profit">
        </eo:StaticColumn>
        <eo:StaticColumn HeaderText="PresentValue" Width="150" DataField="presentV">
        </eo:StaticColumn>
        <eo:StaticColumn HeaderText="MaturityAmt" Width="150" DataField="matamt">
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
        <eo:StaticColumn>
        </eo:StaticColumn>
        <eo:RowNumberColumn>
        </eo:RowNumberColumn>
    </columntemplates>
    <FooterStyle CssText="padding-bottom:4px;padding-left:4px;padding-right:4px;padding-top:4px;" />
                            </eo:Grid>
                            <br />
                            <table style="width:80%;">
                                <tr>
                                    <td style="width: 15%">
                                        <asp:Label ID="Label22" runat="server" Font-Bold="True" Font-Size="Medium" Text="Selling"></asp:Label>
                                    </td>
                                    <td style="width: 30%">
                                        &nbsp;</td>
                                    <td>
                                        <asp:Button ID="btnAdd" runat="server" CssClass="btn btn-primary btn-block btn-flats" Height="40px" Text="Add" Width="150px" />
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 15%">
                                        <asp:Label ID="Label23" runat="server" Text="Tenor"></asp:Label>
                                    </td>
                                    <td style="width: 30%">
                                        <asp:TextBox ID="txtSellTenorMulti" runat="server" Font-Size="Small" required="required" style="margin-left: 0px" title="Please Enter Present value" Width="75%"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnRemove" runat="server" CssClass="btn btn-primary btn-block btn-flats" Height="40px" Text="Remove" Width="150px" />
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 15%">
                                        <asp:Label ID="Label24" runat="server" Text="Rate"></asp:Label>
                                    </td>
                                    <td style="width: 30%">
                                        <asp:TextBox ID="txtRate" runat="server" Font-Size="Small" required="required" style="margin-left: 0px" title="Please Enter Present value" Width="75%"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnOk" runat="server" CssClass="btn btn-primary btn-block btn-flats" Height="40px" Text="Ok" Width="150px" />
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 15%">
                                        <asp:Label ID="Label25" runat="server" Text="Sell Amount"></asp:Label>
                                    </td>
                                    <td style="width: 30%">
                                        <asp:TextBox ID="txtSellAmt" runat="server" Font-Size="Small" style="margin-left: 0px" title="Please Enter Present value" Width="75%"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnReset" runat="server" CssClass="btn btn-primary btn-block btn-flats" Height="40px" Text="Reset" Width="150px" />
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 15%">
                                        <asp:Label ID="Label26" runat="server" Text="Total Sell" Font-Bold="True"></asp:Label>
                                    </td>
                                    <td style="width: 30%">
                                        <asp:Label ID="lblTotal" runat="server" Font-Bold="True" ForeColor="Blue"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnExit" runat="server" CssClass="btn btn-danger btn-block btn-flats" Height="40px" Text="Exit" Width="150px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 15%">
                                        
                                    </td>
                                    <td style="width: 30%">
                                        
                                        <asp:Label ID="cost" runat="server" Visible="False"></asp:Label>
                                        <asp:Label ID="profit" runat="server" Visible="False"></asp:Label>
                                        <asp:Label ID="sellpv" runat="server" Visible="False"></asp:Label>
                                        
                                    </td>
                                    <td>
                                    
                                    </td>
                                </tr>
                            </table>
                        </div>
                      </div>
                    </div>
                      <%--  <div class="panel box box-success">
                      <div class="box-header with-border">
                        <h4 class="box-title">
                          <a data-toggle="collapse" data-parent="#accordion" href="#history">
                            Client History
                          </a>
                        </h4>
                      </div>
                      <div id="history" class="panel-collapse collapse-in">
                        <div class="box-body">
                        <button type="button" class="btn btn-success">Avg Rate <asp:Label ID="avgRate" runat="server" Text="Label"></asp:Label></button>     
                            <button type="button" class="btn btn-success">Avg Size <asp:Label ID="avgSize" runat="server"></asp:Label></button>      
                            <button type="button" class="btn btn-success">Avg Tenor <asp:Label ID="avgTenor" runat="server"></asp:Label></button>
                             <hr />
                                 
                                     
  
                                     
                        </div>
                      </div>
                    </div> --%>

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

            Session("removedItem") = item;
        
        }
        //<!--JS_SAMPLE_END-->
    </script>
    
         
        

  

   
     
   </asp:Content>

