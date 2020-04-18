

<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Main.Master" CodeBehind="AddSecurityItem.aspx.vb" Inherits="WEBTDS.AddSecurityItem" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  
    
<head /> 
     <form id="form1" runat="server">
       <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
       <!-- Horizontal Form -->
         <asp:UpdatePanel ID="UpdatePanelAll" UpdateMode="Always" runat="server"><ContentTemplate>
              <div class="box box-info">
                <div class="box-header with-border">
                  <h3 class="box-title">Security Item</h3> 
                     
                </div><!-- /.box-header -->
                <!-- form start -->
             
                  <div class="box-body">
                      <%--<center>--%>
                       
                                  <table style="width:60%;">
                                      <tr>
                                          <td>
                                              <asp:Label ID="Label1" runat="server" Text="Customer"></asp:Label>
                                          </td>
                                          <td style="width: 409px">
                                              <asp:DropDownList ID="cmbcustomer" runat="server" AutoPostBack="True" class="form-control select2" Font-Size="Small" style="margin-left: 0px" Width="80%">
                                                  <asp:ListItem Text="Select Customer" Value="0"></asp:ListItem>
                                              </asp:DropDownList>
                                              <br />
                                              <br />
                                          </td>
                                          <td>&nbsp;</td>
                                      </tr>
                                      <tr>
                                          <td>
                                              <asp:Label ID="Label2" runat="server" Text="Security Type"></asp:Label>
                                          </td>
                                          <td style="width: 409px">
                                              <asp:DropDownList ID="cmbTypeDesc" runat="server" AutoPostBack="True" class="form-control select2" Font-Size="Small" style="margin-left: 0px" Width="80%">
                                                  <asp:ListItem Text="Select Security Type" Value="0"></asp:ListItem>
                                              </asp:DropDownList>
                                              <br />
                                              <br />
                                          </td>
                                          <td>
                                              <asp:Button ID="btnNew" runat="server" CssClass="btn btn-primary btn-block btn-flats" Height="40px" Text="New" Width="150px" />
                                          </td>
                                      </tr>
                                      <tr>
                                          <td>
                                              <asp:Label ID="Label3" runat="server" Text="Security Location"></asp:Label>
                                          </td>
                                          <td style="width: 409px">
                                              <asp:DropDownList ID="cmbLocationDesc" runat="server" AutoPostBack="True" class="form-control select2" Font-Size="Small" style="margin-left: 0px" Width="80%">
                                                  <asp:ListItem Text="Select Security Location" Value="0"></asp:ListItem>
                                              </asp:DropDownList>
                                              <br />
                                              <br />
                                          </td>
                                          <td>
                                              <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary btn-block btn-flats" Height="40px" Text="Save" Width="150px" />
                                          </td>
                                      </tr>
                                      <tr>
                                          <td>
                                              <asp:Label ID="Label4" runat="server" Text="Security Reference"></asp:Label>
                                          </td>
                                          <td style="width: 409px">
                                              <asp:Label ID="lblCollateralReference" runat="server"></asp:Label>
                                          </td>
                                          <td>
                                              <asp:Button ID="btnExit" runat="server" CssClass="btn btn-danger btn-block btn-flats" Height="40px" Text="Exit" Width="150px" />
                                          </td>
                                      </tr>
                                  </table>
                              <hr />
                           <div class="panel box box-success">
                      <div class="box-header with-border">
                        <h4 class="box-title">
                          <a data-toggle="collapse" data-parent="#accordion" href="#history1">
                             Details
                          </a>
                        </h4>
                      </div>
                      <div id="history1" class="panel-collapse collapse-in">
                        <div class="box-body">
                             <table style="width:60%;">
                                      <tr>
                                          <td style="width: 223px">
                                              <asp:Label ID="Label6" runat="server" Font-Bold="True" Font-Size="Medium" Text="Security Information"></asp:Label>
                                              <br />
                                              <asp:Label ID="Label7" runat="server" Text="Expiry Date"></asp:Label>
                                          </td>
                                          <td style="width: 403px">
                                              <asp:TextBox ID="dtExpiry" runat="server" Width="75%" Font-Size="Small"></asp:TextBox>
                                             
                                              <ajaxToolkit:CalendarExtender ID="dtExpiry_CalendarExtender" runat="server" BehaviorID="ExpiryDate_CalendarExtender" TargetControlID="dtExpiry" />
                                             
                                          </td>
                                          <td style="width: 277px">
                                              <asp:Label ID="Label11" runat="server" Text="User Security Reference"></asp:Label>
                                          </td>
                                           <td style="width: 485px">
                                               <asp:TextBox ID="txtUserReference" runat="server" style="margin-left: 0px" title="Please Enter Present value" Width="75%" Font-Size="Small"></asp:TextBox>
                                               <br />
                                          </td>
                                      </tr>
                                      <tr>
                                          <td style="width: 223px">
                                              <asp:Label ID="Label8" runat="server" Text="Currency"></asp:Label>
                                          </td>
                                          <td style="width: 403px">
                                              <asp:Label ID="lblCurrency" runat="server" Text="USD"></asp:Label>
                                          </td>
                                          <td style="width: 277px">
                                              <asp:Label ID="Label12" runat="server" Text="Description"></asp:Label>
                                          </td>
                                           <td style="width: 485px">
                                               <asp:TextBox ID="txtDesc" runat="server" style="margin-left: 0px" title="Please Enter Present value" Width="75%" Font-Size="Small"></asp:TextBox>
                                               <br />
                                               <br />
                                          </td>
                                      </tr>
                                      <tr>
                                          <td style="width: 223px">&nbsp;</td>
                                          <td style="width: 403px">
                                              <asp:CheckBox ID="CheckBox1" runat="server" Text="Approved" Enabled="False" />
                                          </td>
                                          <td style="width: 277px">
                                              <asp:Label ID="Label13" runat="server" Text="Assignment Option"></asp:Label>
                                          </td>
                                           <td style="width: 485px">
                                               <asp:DropDownList ID="cmbAssignment" runat="server" AutoPostBack="True" class="form-control select2" Font-Size="Small" style="margin-left: 0px" Width="80%">
                                                   <asp:ListItem Text="Select Assignment Option" Value="0"></asp:ListItem>
                                                   <asp:ListItem>Partial</asp:ListItem>
                                                   <asp:ListItem>Full</asp:ListItem>
                                               </asp:DropDownList>
                                               <br />
                                          </td>
                                      </tr>
                                       <tr>
                                          <td style="width: 223px">
                                              <asp:Label ID="Label9" runat="server" Text="Value"></asp:Label>
                                           </td>
                                          <td style="width: 403px">
                                              <asp:TextBox ID="txtvalue" runat="server" style="margin-left: 0px" title="Please Enter Present value" Width="75%" AutoPostBack="true" Font-Size="Small"></asp:TextBox>
                                              <br />
                                              <br />
                                           </td>
                                          <td style="width: 277px">
                                              <asp:Label ID="Label14" runat="server" Text="Bank Valuation"></asp:Label>
                                           </td>
                                            <td style="width: 485px">
                                                <asp:Label ID="lblBankValuation" runat="server" BackColor="#66FFFF"></asp:Label>
                                           </td>
                                      </tr>
                                       <tr>
                                          <td style="width: 223px">
                                              <asp:Label ID="Label10" runat="server" Text="Additional Information"></asp:Label>
                                           </td>
                                          <td style="width: 403px">
                                              <asp:TextBox ID="txtAdditionalInfo" runat="server" Font-Size="Small" Height="63px" TextMode="MultiLine" Width="110%"></asp:TextBox>
                                              <br />
                                           </td>
                                          <td style="width: 277px">&nbsp;</td>
                                            <td style="width: 485px">
                                                <asp:TextBox ID="txtcustnumber" runat="server" Enabled="False" Visible="False"></asp:TextBox>
                                           </td>
                                      </tr>
                                  </table>
                            </div></div></div>
                                 
                           

                   <%-- </center>--%>       <!-- <button type="button" class="btn btn-block btn-success btn-xs" data-toggle="modal" data-target="#DealCodeDetail">
  Launch demo modal
</button>-->
                  </div><!-- /.box-body -->
                
                 <%-- <div class="box-footer" style="width:100%;" > 
                     <asp:Button ID="btncancel" runat="server" Text="Cancel" CssClass="btn btn-default"  /><asp:Button ID="btnnew" runat="server" Text="New   " CssClass="btn btn-info pull-right" />
                    <%--<button type="submit" class="btn btn-default">Cancel</button>
                    <button type="submit" class="btn btn-info pull-right">Sign in</button>--%>
                  </div><!-- /.box-footer--%> -->
              
              </div><!-- /.box --> 

     
                 
         </ContentTemplate></asp:UpdatePanel>

  </form>
</head>
</asp:Content>
