<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Main.Master" CodeBehind="Deals.aspx.vb" Inherits="WEBTDS.Deals" %>
<%@ Register Assembly="EO.Web" Namespace="EO.Web" TagPrefix="eo" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<head runat="server" />
    <form id="form1" runat="server">
    
    <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
                                  <asp:UpdatePanel ID="UpdatePanelAll" UpdateMode="Always" runat="server"><ContentTemplate>
               <div class="box box-info">
                <div class="box-header with-border">
                  <h3 class="box-title">Filter Options</h3> 
                     
                </div><!-- /.box-header -->
                <!-- form start -->
             <%-- filter options table contents --%>
                  <div class="box-body">

                      <table style="width:100%"><tr><td  style="width:33%">    <asp:Label ID="Label4" runat="server" Text="Deal Status" BorderColor="#66FFFF" BackColor="#FFFFCC"></asp:Label> <br />
  
                          <asp:DropDownList ID="cmbStatus" runat="server" AutoPostBack="True" class="form-control select2" style="width: 80%;">
                                  <asp:ListItem Value=""></asp:ListItem>
                              <asp:ListItem Value="0">Live Deals</asp:ListItem>
                              <asp:ListItem Value="1">Matured Deals</asp:ListItem>
                              <asp:ListItem Value="2">All Deals</asp:ListItem>

                                      </asp:DropDownList>
                                                    </td>
                          <td  style="width:33%">  <asp:Label ID="Label1" runat="server" Text="Deal Status" BorderColor="#66FFFF" BackColor="#FFFFCC"></asp:Label> <br />
                                
                          <asp:DropDownList ID="cmbReport" runat="server" AutoPostBack="True" class="form-control select2" style="width: 80%;" Enabled="false">
                             
                                      </asp:DropDownList> </td>
                          <td  style="width:33%">  <div class="info-box">
                <span class="info-box-icon bg-yellow"><i class="fa fa-files-o"></i></span>
                <div class="info-box-content">
                  <span class="info-box-number">Parameters</span>
                  <span class="info-box-text"><asp:Label ID="lblRtpSpec" runat="server" Text="" ></asp:Label></span>
                </div><!-- /.info-box-content -->
              </div><!-- /.info-box -->
                              </td>
                             </tr></table>
                      </div></div>
        <div class="row">
            <div class="col-md-4">
                  
              <div class="box box-default">
                <div class="box-header with-border">
                  <h3 class="box-title">By Date</h3>
                  <div class="box-tools pull-right">
                    <button class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i></button>
                    <button class="btn btn-box-tool" data-widget="remove"><i class="fa fa-times"></i></button>
                  </div>
                </div><!-- /.box-header -->
                <div class="box-body">
                          <table style="width:100%">
                                  <asp:Panel ID="grpDateRange" runat="server">
                                  <tr><td> <asp:Label ID="Label2" runat="server" Text="Start Date" BorderColor="#66FFFF" BackColor="#FFFFCC"></asp:Label> <br />
                                  <asp:TextBox ID="dt1" runat="server" Width="80%"></asp:TextBox>
                                         <ajaxToolkit:CalendarExtender ID="dt1_CalendarExtender" runat="server" BehaviorID="dt1_CalendarExtender" TargetControlID="dt1" />
                                         </td></tr>
                                  <tr><td>
                                      <asp:Label ID="Label3" runat="server" Text="End Date" BorderColor="#66FFFF" BackColor="#FFFFCC"></asp:Label> <br />
                                      <asp:TextBox ID="dt2" runat="server"  Width="80%"></asp:TextBox>
                                      <ajaxToolkit:CalendarExtender ID="dt2_CalendarExtender" runat="server" BehaviorID="dt2_CalendarExtender" TargetControlID="dt2" />
                                      </td></tr>
                      </asp:Panel>
                                  <asp:Panel ID="grpNum" runat="server">
                                  <tr><td>
                                        <asp:Label ID="Label5" runat="server" Text="Number Of Days" type="number" min="1" BorderColor="#66FFFF" BackColor="#FFFFCC"></asp:Label> <br />
                                      <asp:TextBox ID="Tnum" runat="server"></asp:TextBox>
                                      </td></tr>
                                   </asp:Panel>
                              </table>
                    </div>
               
               
  <div class="overlay" id="divdate" runat="server" visible="true">
                  <i class="fa fa-refresh fa-spin"></i>
                </div>
              </div>

              
                </div>  
              <div class="col-md-4">
                    
              <div class="box box-default">
                <div class="box-header with-border">
                  <h3 class="box-title">Deal Codes</h3>
                  <div class="box-tools pull-right">
                    <button class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i></button>
                    <button class="btn btn-box-tool" data-widget="remove"><i class="fa fa-times"></i></button>
                  </div>
                </div><!-- /.box-header -->
                <div class="box-body">    <div class="overlay">
                  <i class="fa fa-refresh fa-spin"></i>
                </div>
                     <asp:Panel ID="grpDealtypes" runat="server"> <br />
                                 <eo:grid ID="GridDeals" runat="server" BorderColor="#C7D1DF" 
                                        BorderWidth="1px" ColumnHeaderAscImage="00050303" 
                                        ColumnHeaderDescImage="00050304" ColumnHeaderDividerImage="00050302" 
                                        FixedColumnCount="1" Font-Bold="False" Font-Italic="False" Font-Names="Verdana" 
                                        Font-Overline="False" Font-Size="9pt" Font-Strikeout="False" 
                                        Font-Underline="False" GridLineColor="199, 209, 223" GridLines="Both" 
                                        Height="130px" ItemHeight="19" Width="100%" >
                                        <FooterStyle CssText="padding-bottom:4px;padding-left:4px;padding-right:4px;padding-top:4px;" />
                                      
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
                                            <eo:CheckBoxColumn Width="40">
                                            </eo:CheckBoxColumn>
                                              <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="Deal_code" 
                                                HeaderText="Deal Code" MinWidth="20" ReadOnly="True" Width="150">
                                               
                                            </eo:CustomColumn>
                                            <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="dealBasictype" 
                                                HeaderText="Structure" MinWidth="20" ReadOnly="True" Width="300">
                                               
                                            </eo:CustomColumn>
                                           
                                        </Columns>
                                    </eo:grid></asp:Panel>
                    </div>

                    <div class="overlay" id="divdealtypes" runat="server" visible="true">
                  <i class="fa fa-refresh fa-spin"></i>
                </div>
              </div>

                </div>
                <div class="col-md-4">
  
              <div class="box box-default">
                <div class="box-header with-border">
                  <h3 class="box-title">Dealers</h3>
                  <div class="box-tools pull-right">
                    <button class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i></button>
                    <button class="btn btn-box-tool" data-widget="remove"><i class="fa fa-times"></i></button>
                  </div>
                </div><!-- /.box-header -->
                <div class="box-body">
                       <asp:Panel ID="grpDealer" runat="server">  <br />
                                <eo:grid ID="GridDealer" runat="server" BorderColor="#C7D1DF" 
                                        BorderWidth="1px" ColumnHeaderAscImage="00050303" 
                                        ColumnHeaderDescImage="00050304" ColumnHeaderDividerImage="00050302" 
                                        FixedColumnCount="1" Font-Bold="False" Font-Italic="False" Font-Names="Verdana" 
                                        Font-Overline="False" Font-Size="9pt" Font-Strikeout="False" 
                                        Font-Underline="False" GridLineColor="199, 209, 223" GridLines="Both" 
                                        Height="130px" ItemHeight="19" Width="100%" >
                                        <FooterStyle CssText="padding-bottom:4px;padding-left:4px;padding-right:4px;padding-top:4px;" />
                                      
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
                                            <eo:CheckBoxColumn Width="40">
                                            </eo:CheckBoxColumn>
                                           
                                             <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="user_id" 
                                                HeaderText="Dealer Name" MinWidth="20" ReadOnly="True" Width="450">
                                               
                                            </eo:CustomColumn>

                                           
                                        </Columns>
                                    </eo:grid>
                                    </asp:Panel>
                    </div>  <div class="overlay" id="divdealer" runat="server" visible="true">
                  <i class="fa fa-refresh fa-spin"></i>
                </div></div>

                </div>
              </div>

        <div class="row">
            <div class="col-md-4">
                  
              <div class="box box-default">
                <div class="box-header with-border">
                  <h3 class="box-title">Deal Codes</h3>
                  <div class="box-tools pull-right">
                    <button class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i></button>
                    <button class="btn btn-box-tool" data-widget="remove"><i class="fa fa-times"></i></button>
                  </div>
                </div><!-- /.box-header -->
                <div class="box-body">
                        <asp:Panel ID="grpDealref" runat="server"> <br />
                                 <eo:grid ID="GridDealRef" runat="server" BorderColor="#C7D1DF" 
                                        BorderWidth="1px" ColumnHeaderAscImage="00050303" 
                                        ColumnHeaderDescImage="00050304" ColumnHeaderDividerImage="00050302" 
                                        FixedColumnCount="1" Font-Bold="False" Font-Italic="False" Font-Names="Verdana" 
                                        Font-Overline="False" Font-Size="9pt" Font-Strikeout="False" 
                                        Font-Underline="False" GridLineColor="199, 209, 223" GridLines="Both" 
                                        Height="130px" ItemHeight="19" Width="100%" >
                                        <FooterStyle CssText="padding-bottom:4px;padding-left:4px;padding-right:4px;padding-top:4px;" />
                                      
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
                                            <eo:CheckBoxColumn Width="40">
                                            </eo:CheckBoxColumn>
                                              <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="dealreference" 
                                                HeaderText="Deal Reference" MinWidth="20" ReadOnly="True" Width="450">
                                               
                                            </eo:CustomColumn>
                                           
                                        </Columns>
                                    </eo:grid>
                                      </asp:Panel>
                    </div>  <div class="overlay" id="divdealref" runat="server" visible="true">
                  <i class="fa fa-refresh fa-spin"></i>
                </div></div>

              
                </div>  
              <div class="col-md-4">
                    
              <div class="box box-default">
                <div class="box-header with-border">
                  <h3 class="box-title">Customers</h3>
                  <div class="box-tools pull-right">
                    <button class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i></button>
                    <button class="btn btn-box-tool" data-widget="remove"><i class="fa fa-times"></i></button>
                  </div>
                </div><!-- /.box-header -->
                <div class="box-body">
                     <asp:Panel ID="grpCustomer" runat="server">   <%-- grid for dealscontents --%>
                         <br />
                                 <eo:grid ID="GridCustomer" runat="server" BorderColor="#C7D1DF" 
                                        BorderWidth="1px" ColumnHeaderAscImage="00050303" 
                                        ColumnHeaderDescImage="00050304" ColumnHeaderDividerImage="00050302" 
                                        FixedColumnCount="1" Font-Bold="False" Font-Italic="False" Font-Names="Verdana" 
                                        Font-Overline="False" Font-Size="9pt" Font-Strikeout="False" 
                                        Font-Underline="False" GridLineColor="199, 209, 223" GridLines="Both" 
                                        Height="130px" ItemHeight="19" Width="100%" >
                                        <FooterStyle CssText="padding-bottom:4px;padding-left:4px;padding-right:4px;padding-top:4px;" />
                                      
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
                                            <eo:CheckBoxColumn Width="40">
                                            </eo:CheckBoxColumn>
                                              <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="customer_Number" 
                                                HeaderText="Customer Number" MinWidth="20" ReadOnly="True" Width="70">
                                               
                                            </eo:CustomColumn>
                                            <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="fullName" 
                                                HeaderText="Full Name" MinWidth="20" ReadOnly="True" Width="450">
                                               
                                            </eo:CustomColumn>
                                           
                                        </Columns>
                                    </eo:grid>
                                     <table>

                                         <tr><td style="padding-bottom:15px">   Range Start</td><td><asp:TextBox ID="txtStartRange" runat="server"></asp:TextBox></td></tr>
                                         <tr><td>   Range End</td><td><asp:TextBox ID="txtEndCust" runat="server"></asp:TextBox></td></tr>

                                     </table>
                              
                                         
                                        </asp:Panel>
                    </div>  <div class="overlay" id="divcustomer" runat="server" visible="true">
                  <i class="fa fa-refresh fa-spin"></i>
                </div></div>

                </div>
                <div class="col-md-4">
  
              <div class="box box-default">
                <div class="box-header with-border">
                  <h3 class="box-title">Deal Types</h3>
                  <div class="box-tools pull-right">
                    <button class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i></button>
                    <button class="btn btn-box-tool" data-widget="remove"><i class="fa fa-times"></i></button>
                  </div>
                </div><!-- /.box-header -->
                <div class="box-body">
                    <asp:Panel ID="GrpStatus" runat="server"><br /> 
                        <asp:CheckBox ID="CheckBoxLive" runat="server" Text="Live Deals" /><br />
                        <asp:CheckBox ID="CheckBoxMatured" runat="server" Text="Matured Deals"  />
                        <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidatorDealType" runat="server" ControlToValidate="RadioButtonList1" ErrorMessage="Error !! Select Deal Type" ForeColor="Red"></asp:RequiredFieldValidator>--%></td>
                              </asp:Panel>
                    </div>  <div class="overlay" id="divdealtype" runat="server" visible="true">
                  <i class="fa fa-refresh fa-spin"></i>
                </div></div>

                </div>
              </div>

          <div class="row">
                <div class="col-md-6">
                  
                    </div></div>
                                           </ContentTemplate></asp:UpdatePanel>
      <div class="box box-info">
                <div class="box-header with-border">
                  <h3 class="box-title">Load Report</h3> 
                     
                </div><!-- /.box-header -->
                <!-- form start -->
        
             <%-- filter options table contents --%>
                  <div class="box-body">
                      
          
                   
                      <table style="width:100%"><tr><td colspan="4"><center><asp:Button ID="btnLoad" runat="server" Text="Load Report" CssClass="btn btn-primary btn-block btn-flats" Height="40px" Width="150px" /></center></td></tr></table>
                      </div></div>
   

    </form>


</asp:Content>
