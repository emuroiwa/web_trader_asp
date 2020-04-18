


<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Main.Master" CodeBehind="SecIDEnquiry.aspx.vb" Inherits="WEBTDS.SecIDEnquiry" %>

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
                  <h3 class="box-title">Security ID Enquiry</h3> 
                    
                </div><!-- /.box-header -->
                <!-- form start -->
             
                  <div class="box-body">
                      <center>
                        
                          <table style="width:50%;">
                              <tr>
                                  <td style="height: 45px; width: 224px;">
                                      <asp:Label ID="Label1" runat="server" Text="Deal Reference"></asp:Label>
                                  </td>
                                  <td style="width: 454px; height: 45px">
                                      <asp:TextBox ID="txtSecDealref" runat="server" style="margin-left: 0px" title="Please Enter Present value" Width="75%" Font-Size="Small"></asp:TextBox>
                                      <br />
                                  </td>
                                  <td style="height: 45px">
                                      &nbsp;</td>
                              </tr>
                              <tr>
                                  <td style="width: 224px">
                                      <asp:Label ID="Label2" runat="server" Text="Security ID"></asp:Label>
                                  </td>
                                  <td style="width: 454px">
                                      <asp:Label ID="lblDealSecID" runat="server" ForeColor="Yellow"></asp:Label>
                                      <br />
                                  </td>
                                  <td>
                                      &nbsp;</td>
                              </tr>
                              <tr>
                                  <td style="width: 224px">
                                      <asp:Button ID="btnFind" runat="server" CssClass="btn btn-primary btn-block btn-flats" Height="40px" Text="Find" Width="150px" />
                                  </td>
                                  <td style="width: 454px">
                                      <asp:Button ID="btnExit" runat="server" CssClass="btn btn-danger btn-block btn-flats" Height="40px" Text="Exit" Width="150px" />
                                  </td>
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
