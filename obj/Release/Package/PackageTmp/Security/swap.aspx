


<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Main.Master" CodeBehind="swap.aspx.vb" Inherits="WEBTDS.swap" %>

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
                  <h3 class="box-title">Swap Required</h3> 
                    
                </div><!-- /.box-header -->
                <!-- form start -->
             
                  <div class="box-body">
                      <center>
                        
                          <table style="width:50%;">
                              <tr>
                                  <td style="height: 45px; width: 224px;">
                                      <asp:Label ID="Label1" runat="server" Text="Loan Deals"></asp:Label>
                                  </td>
                                  <td style="width: 454px; height: 45px">
                                      <asp:DropDownList ID="cmbLoanDeal" runat="server" AutoPostBack="True" class="form-control select2" Font-Size="Small" style="margin-left: 0px" Width="100%">
                                          <asp:ListItem Text="Select Loan Deal" Value="0"></asp:ListItem>
                                      </asp:DropDownList>
                                      <br />
                                  </td>
                                  <td style="height: 45px">
                                      &nbsp;</td>
                              </tr>
                              <tr>
                                  <td style="width: 224px">
                                      <asp:Label ID="Label2" runat="server" Text="Deposit Deals"></asp:Label>
                                  </td>
                                  <td style="width: 454px">
                                      <asp:DropDownList ID="cmbDepositDeal" runat="server" AutoPostBack="True" class="form-control select2" Font-Size="Small" style="margin-left: 0px" Width="100%">
                                          <asp:ListItem Text="Select DepositDeal" Value="0"></asp:ListItem>
                                      </asp:DropDownList>
                                      <br />
                                  </td>
                                  <td>
                                      &nbsp;</td>
                              </tr>
                              <tr>
                                  <td style="width: 224px">
                                      <asp:Button ID="btnExit" runat="server" CssClass="btn btn-danger btn-block btn-flats" Height="40px" Text="Exit" Width="150px" />
                                  </td>
                                  <td style="width: 454px">
                                      &nbsp;</td>
                                  <td>
                                      &nbsp;</td>
                              </tr>
                            
                          </table>
                      </center> 

                            <!-- <button type="button" class="btn btn-block btn-success btn-xs" data-toggle="modal" data-target="#DealCodeDetail">
  Launch demo modal
</button>-->
                  </div><!-- /.box-body -->
                
                 
              
              </div><!-- /.box --> 

     
                
         </ContentTemplate></asp:UpdatePanel>

  </form>
    </head>
     
</asp:Content>

