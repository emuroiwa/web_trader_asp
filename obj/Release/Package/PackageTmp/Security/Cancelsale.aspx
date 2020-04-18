﻿



<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Main.Master" CodeBehind="Cancelsale.aspx.vb" Inherits="WEBTDS.Cancelsale" %>
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
       <!-- Horizontal Form -->
              <div class="box box-info">
                <div class="box-header with-border">
                  <h3 class="box-title">Cancel Deal</h3>  
   
                </div><!-- /.box-header -->
                <!-- form start -->
             
                  <div class="box-body">
                     </HeaderTemplate>
                              <ContentTemplate>
                                  <table style="width:100%;">
                                      <tr>
                                          <td>
                                              <asp:Label ID="Label50" runat="server" Text="Reason"></asp:Label>
                                          </td>
                                           <td>
                                               <asp:TextBox ID="txtReasonCancel" runat="server" Font-Size="Small" Height="48px" TextMode="MultiLine" Width="75%"></asp:TextBox>
                                          </td>
                                           <td style="width: 172px">
                                               <asp:Label ID="Label57" runat="server" Text="Purchase Deal Reference" Font-Size="Small"></asp:Label>
                                          </td>
                                           <td style="width: 379px">
                                               <asp:TextBox ID="lblPurRef" runat="server" Enabled="False" Font-Size="Small" required="required" style="margin-left: 0px" title="Please Enter Present value" Width="75%"></asp:TextBox>
                                          </td>
                                           <td>
                                               &nbsp;</td>

                                      </tr>
                                      <tr>
                                          <td style="width: 147px">
                                              <asp:Label ID="Label60" runat="server" Font-Size="Small" Text="Maturity Date"></asp:Label>
                                          </td>
                                          <td style="width: 398px">
                                              <asp:TextBox ID="lblMaturityDate" runat="server" Font-Size="Small" required="required" title="Please Enter Start Date" Width="75%" AutoPostBack="true" Enabled="False"></asp:TextBox>
                                              
                                          </td>
                                          <td style="align-content:center;padding-bottom:10px; width: 172px;">
                                                    <asp:Label ID="Label51" runat="server" Font-Size="Small" Text="SaleRef"></asp:Label>
                                          </td>
                                           <td style="width: 379px">
                                               <asp:TextBox ID="lblsaleref" runat="server" Font-Size="Small" required="required" style="margin-left: 0px" title="Please Enter Present value" Width="75%" Enabled="False"></asp:TextBox>
                                          </td>
                                           <td>
                                               &nbsp;</td>
                                           
                                      </tr>
                                      <tr>
                                          <td style="width: 147px"> 
                                              <asp:Label ID="Label42" runat="server" Font-Size="Small" Text="Start Date"></asp:Label>
                                          </td>
                                          <td style="width: 398px" >
                                              <asp:TextBox ID="lblStart" runat="server" AutoPostBack="True" Font-Size="Small" required="required" title="Please Enter Maturity Date" Width="75%" Enabled="False"></asp:TextBox>
                                              
                                          </td>
                                         <td style="align-content:center;padding-bottom:10px; width: 172px;">
                                                    <asp:Label ID="Label52" runat="server" Font-Size="Small" Text="Discount Rate"></asp:Label>
                                          </td>
                                           <td style="width: 379px">
                                               <asp:TextBox ID="lbldiscount" runat="server" Font-Size="Small" required="required" style="margin-left: 0px" title="Please Enter Present value" Width="75%" Enabled="False"></asp:TextBox>
                                          </td>
                                           <td></td>
                                         
                                      </tr>
                                      <tr>
                                          <td style="width: 147px">
                                              <asp:Label ID="Label44" runat="server" Font-Size="Small" Text="Tenor"></asp:Label>
                                          </td>
                                          <td style="width: 398px">
                                              <asp:TextBox ID="lblTenor" runat="server" AutoPostBack="True" Font-Size="Small" min="1" type="number" Width="75%" Enabled="False" ></asp:TextBox>
                                          </td>
                                          <td style="align-content:center;padding-bottom:10px; width: 172px;">
                                                    <asp:Label ID="Label53" runat="server" Font-Size="Small" Text="Acrued To Date"></asp:Label>
                                          </td>
                                           <td style="width: 379px">
                                               <asp:TextBox ID="lblAccrued" runat="server" Font-Size="Small" required="required" style="margin-left: 0px" title="Please Enter Present value" Width="75%" Enabled="False"></asp:TextBox>
                                          </td>
                                           <td>
                                               <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary btn-block btn-flats" Height="40px" Text="Cancel Sale" Width="150px" />
                                          </td>
                                          
                                      </tr>
                                       <tr>
                                          <td style="width: 147px">
                                              <asp:Label ID="Label59" runat="server" Text="Sale Maturity Amount"></asp:Label>
                                           </td>
                                          <td style="width: 398px">
                                              <asp:TextBox ID="lblMaturityAmount" runat="server" Font-Size="Small" required="required" style="margin-left: 0px" title="Please Enter Present value" Width="75%" Enabled="False"></asp:TextBox>
                                           </td>
                                          <td style="align-content:center;padding-bottom:10px; width: 172px;">
                                                   <asp:Label ID="Label62" runat="server" Text="Deal Amount"></asp:Label>
                                           </td>
                                            <td style="width: 379px">
                                                <asp:TextBox ID="lblAmount" runat="server" Enabled="False" Font-Size="Small" required="required" style="margin-left: 0px" title="Please Enter Present value" Width="75%"></asp:TextBox>
                                           </td>
                                            <td>
                                                &nbsp;</td>
                                            
                                      </tr>
                                       <tr>
                                          <td style="width: 147px">
                                              <asp:Label ID="Label63" runat="server" Text="Yield Rate"></asp:Label>
                                           </td>
                                          <td style="width: 398px">
                                              <asp:TextBox ID="lblYieldRate" runat="server" Enabled="False" Font-Size="Small" required="required" style="margin-left: 0px" title="Please Enter Present value" Width="75%"></asp:TextBox>
                                           </td>
                                          <td style="width: 172px">
                                              <asp:Label ID="Label64" runat="server" Font-Size="Small" Text="Sale Currency"></asp:Label>
                                           </td>
                                            <td style="width: 379px">
                                                <asp:TextBox ID="lblSalecurr" runat="server" Enabled="False" Font-Size="Small" required="required" style="margin-left: 0px" title="Please Enter Present value" Width="75%"></asp:TextBox>
                                           </td>
                                            <td>
                                                <asp:Button ID="btnExit" runat="server" CssClass="btn btn-danger btn-block btn-flats" Height="40px" Text="Exit" Width="150px" />
                                           </td>
                                            

                                      </tr>
                                       <tr>
                                          <td style="width: 147px">
                                              &nbsp;</td>
                                          <td style="width: 398px">
                                              &nbsp;</td>
                                          <td style="width: 172px">
                                              &nbsp;</td>
                                            <td style="width: 379px">
                                                &nbsp;</td>
                                            <td></td>
                                            
                                      </tr>
                                      <tr>
                                          <td>
                                              &nbsp;</td>
                                          <td>
                                              &nbsp;</td>
                                          <td style="width: 172px"></td>
                                      </tr>
                                  </table>
                                  
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
