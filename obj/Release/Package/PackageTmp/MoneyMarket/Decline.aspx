

<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Main.Master" CodeBehind="Decline.aspx.vb" Inherits="WEBTDS.Decline" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  
    <asp:Label ID="lblError" runat="server"></asp:Label>
        <asp:Label ID="lblsuccess" runat="server"></asp:Label>
                  <asp:Label ID="lblwarning" runat="server"> </asp:Label>
  <head />
  <form id="form1" runat="server">
       <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
      <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>

       <!-- Horizontal Form -->
              <div class="box box-info">
                <div class="box-header with-border">
                  <h3 class="box-title">Decline Deal Verification</h3>  
    
                </div><!-- /.box-header -->
                <!-- form start -->
             
                  <div class="box-body">
                    </HeaderTemplate>
                              <ContentTemplate>
                               <center>   <table style="width:100%;">
                                      <tr>
                                          <td style="height: 23px; width: 233px">
                                              <asp:Label ID="Label40" runat="server" Text="Deal Reference"></asp:Label>
                                          </td>
                                          <td style="height: 23px; width: 344px;">
                                              <asp:Label ID="lbldealref" runat="server"></asp:Label>
                                          </td>
                                          <td style="align-content:center ;padding-bottom:10px">
                                                    &nbsp;</td>
                                      </tr>
                                      <tr>
                                          <td style="width: 233px">
                                              <asp:Label ID="Label42" runat="server" Text="Deal Cancelled"></asp:Label>
                                          </td>
                                          <td style="width: 344px">
                                              <asp:Label ID="lbldealcancelled" runat="server"></asp:Label>
                                          </td>
                                          <td style="align-content:center ;padding-bottom:10px">
                                                    &nbsp;</td>
                                      </tr>
                                      <tr>
                                          <td style="width: 233px">
                                              <asp:Label ID="Label43" runat="server" Text="Dealer"></asp:Label>
                                          </td>
                                          <td style="width: 344px">
                                              <asp:Label ID="lbldealer" runat="server"></asp:Label>
                                          </td>
                                          <td style="align-content:center ;padding-bottom:10px">
                                                   <asp:Button ID="btnDecline" runat="server" CssClass="btn btn-primary btn-block btn-flats" Height="40px" Text="Decline Verification" Width="150px" />
                                                </td>
                                      </tr>
                                      <tr>
                                          <td style="width: 233px">
                                              <asp:Label ID="Label41" runat="server" Text="Comment"></asp:Label>
                                          </td>
                                          <td style="width: 344px">
                                              <asp:TextBox ID="txtComment" runat="server" Font-Size="Small" Width="75%" TextMode="MultiLine" Height="57px"  ></asp:TextBox>
                                          </td>
                                          <td><a href="MMDealBlotter.aspx">
                                              <asp:Button ID="btnExit0" runat="server" CssClass="btn btn-danger btn-block btn-flats" Height="40px" Text="Exit" Width="150px" />
                                              </a></td>
                                      </tr>
                                      <tr>
                                          <td style="width: 233px">&nbsp;</td>
                                          <td style="width: 344px">&nbsp;</td>
                                          <td>&nbsp;</td>
                                      </tr>
                                  </table></center>
                                   <hr>
                             <ContentTemplate>
                               
                            </ContentTemplate>
                            </hr>
                              </ContentTemplate>


                         <!-- <button type="button" class="btn btn-block btn-success btn-xs" data-toggle="modal" data-target="#DealCodeDetail">
  Launch demo modal
</button>-->
                  </div><!-- /.box-body -->
                 <%-- <div class="box-footer"> 
                     <asp:Button ID="btncancel" runat="server" Text="Cancel" CssClass="btn btn-default"  /><asp:Button ID="btnnew" runat="server" Text="New   " CssClass="btn btn-info pull-right" />
                    <%--<button type="submit" class="btn btn-default">Cancel</button>
                    <button type="submit" class="btn btn-info pull-right">Sign in</button>--%>
                  </div><!-- /.box-footer --> 
       
         
          </ContentTemplate>
    </asp:UpdatePanel>
  </form>
    </head>
</asp:Content>

