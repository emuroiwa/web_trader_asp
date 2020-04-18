<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Main.Master" CodeBehind="LoanSecurity.aspx.vb" Inherits="WEBTDS.LoanSecurity" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  
   
  <form id="form1" runat="server">
       <!-- Horizontal Form -->
              <div class="box box-info">
                <div class="box-header with-border">
                  <h3 class="box-title">Attach&nbsp; Loan Security</h3>
                </div><!-- /.box-header -->
                <!-- form start -->
             
                  <div class="box-body">
                      <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                      <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                          <ContentTemplate>
                              <table style="width:100%;">
                                  <tr>
                                      <td style="width: 94px">Find</td>
                                      <td style="width: 620px">
                                          <asp:TextBox ID="txtFundingLoan" runat="server" Width="80%"></asp:TextBox>
                                      </td>
                                      <td>&nbsp;</td>
                                  </tr>
                                  <tr>
                                      <td style="width: 94px">&nbsp;</td>
                                      <td style="width: 620px">
                                          &nbsp;</td>
                                      
                                  </tr>
                                  <tr>
                                      <td style="width: 94px; height: 140px;"></td>
                                      <td style="width: 620px; height: 140px">
                                          <table style="width:78%;">
                                              <tr>
                                                  <td style="width: 193px">Currency</td>
                                                  <td style="width: 492px">
                                                      <asp:TextBox ID="lblCollCurrency" runat="server" Width="80%"></asp:TextBox>
                                                  </td><td></td>
                                                  
                                              </tr>
                                              <tr>
                                                  <td style="width: 193px">Collateral Value</td>
                                                  <td style="width: 492px">
                                                      <asp:TextBox ID="lblValue" runat="server" Width="80%"></asp:TextBox>
                                                  </td><td></td>
                                                  
                                              </tr>
                                              <tr>
                                                  <td style="width: 193px">Expiry Date</td>
                                                  <td style="width: 492px">
                                                      <asp:TextBox ID="lblExpiry" runat="server" Width="80%"></asp:TextBox>
                                                  </td><td></td>
                                                  
                                              </tr>
                                              <tr>
                                                  <td style="width: 193px">Amount Available to Secure</td>
                                                  <td style="width: 492px">
                                                      <asp:TextBox ID="lblCollAvailable" runat="server" Width="80%"></asp:TextBox>
                                                  </td><td></td>
                                                  
                                              </tr>
                                              <tr>
                                                  <td style="width: 193px">Security Amount</td>
                                                  
                                                  <td style="width: 492px">
                                                      <asp:TextBox ID="txtCollateralLoan" runat="server" Width="80%"></asp:TextBox>
                                                  </td><td><asp:Button ID="btnAddCollateralLoan" runat="server" Text="Add" CssClass="btn btn-default"  />     </td>
                                              </tr>
                                          </table>
                                      </td>
                                      <td style="height: 140px"></td>
                                  </tr>
                                   <tr>
                                      <td style="width: 94px">&nbsp;</td>
                                      <td style="width: 620px">
                                          &nbsp;</td>
                                      <td>&nbsp;</td>
                                  </tr>
                                   <tr>
                                      <td style="width: 94px">&nbsp;</td>
                                      <td style="width: 620px">Total Security:<button class="btn btn-block btn-success disabled"><asp:Label ID="lblDescription" runat="server"></asp:Label></td>
                                      <td>&nbsp;</td>
                                  </tr>
                              </table>
                          </ContentTemplate>


                      </asp:UpdatePanel>
                  <!-- <button type="button" class="btn btn-block btn-success btn-xs" data-toggle="modal" data-target="#DealCodeDetail">
  Launch demo modal
</button>-->
                  </div><!-- /.box-body -->
                  <div class="box-footer"> 
                     <asp:Button ID="btncancel" runat="server" Text="Cancel" CssClass="btn btn-default"  />
                       <asp:Button ID="btnRemove" runat="server" Text="Remove" CssClass="btn btn-default"  />
                      
                      <asp:Button ID="btnSave" runat="server" Text="Secure   " CssClass="btn btn-info pull-right" />
                    <%--<button type="submit" class="btn btn-default">Cancel</button>
                    <button type="submit" class="btn btn-info pull-right">Sign in</button>--%>
                  </div><!-- /.box-footer -->
               
              </div><!-- /.box --> 
   
       
         

  </form>

</asp:Content>
