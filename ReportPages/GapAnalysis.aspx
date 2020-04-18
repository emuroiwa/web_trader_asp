<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Main.Master" CodeBehind="GapAnalysis.aspx.vb" Inherits="WEBTDS.GapAnalysis" %>
<%@ Register Assembly="EO.Web" Namespace="EO.Web" TagPrefix="eo" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
            <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>

     <div class="box box-info">
                <div class="box-header with-border">
                  <h3 class="box-title">Gap Analysis</h3> 
                     
                </div><!-- /.box-header -->
                <!-- form start -->
             
                  <div class="box-body">
                 

         <asp:UpdatePanel ID="UpdatePanelAll" UpdateMode="Always" runat="server"><ContentTemplate>


           <center>  <table style="width:60%">
                 <tr><td><table style="width:100%"><tr><td style="width:30%">
                     <asp:Label ID="lbltext1" runat="server" Text="Choose Currency" BackColor="#FFFFCC" BorderColor="#66FFFF"></asp:Label><asp:DropDownList ID="cmbCurrency" runat="server" AutoPostBack="False" class="form-control select2" style="width: 100%;"></asp:DropDownList>
                     </td>
                     <td style="width:10%">
                         
                     </td>      
                     <td style="width:50%">
                         <table><tr><td>
                             <asp:Button ID="btnGap" runat="server" CssClass="btn btn-primary btn-block btn-flats" Height="40px" Text="Load Report" Width="150px" /></td>
                                     <td>
                          <asp:Button ID="btnPrint" runat="server" CssClass="btn btn-primary btn-block btn-flats" Height="40px" Text="Print" Width="150px"  Enabled="false"/></td>
                             <td>
                                 &nbsp;</td>
                     
                                </tr></table>
                     </td>
                                                   </tr></table>
                        </td></tr>
                       <tr><td>
                        
                           &nbsp;</td></tr>
             </table></center>
                <eo:grid ID="GridGap" runat="server" BorderColor="#C7D1DF" 
                                        BorderWidth="1px" ColumnHeaderAscImage="00050303" 
                                        ColumnHeaderDescImage="00050304" ColumnHeaderDividerImage="00050302" 
                                        FixedColumnCount="1" Font-Bold="False" Font-Italic="False" Font-Names="Verdana" 
                                        Font-Overline="False" Font-Size="9pt" Font-Strikeout="False" 
                                        Font-Underline="False" GridLineColor="199, 209, 223" GridLines="Both" 
                                        Height="187px" ItemHeight="19" Width="100%" >
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
                                            <eo:RowNumberColumn Width="30">
                                            </eo:RowNumberColumn>
                                            <eo:CustomColumn DataField="ItemType" HeaderText="Item Type" 
                                                MinWidth="20" ReadOnly="True" Width="200">
                                                
                                            </eo:CustomColumn>
                                            <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="ItemDescription" 
                                                HeaderText="Item Description" MinWidth="10" ReadOnly="True"  Width="200">
                                                
                                            </eo:CustomColumn>
                                            <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="period1to7" 
                                                HeaderText="Period1to7" MinWidth="25" ReadOnly="True" Width="200" >
                                               
                                            </eo:CustomColumn>
                                            <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="period8to14" 
                                                HeaderText="Period8to14" MinWidth="20" ReadOnly="True" Width="150">
                                               
                                            </eo:CustomColumn>
                                            <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="period15to21" 
                                                HeaderText="Period15to21" MinWidth="20" ReadOnly="True" Width="150">
                                               
                                            </eo:CustomColumn>
                               
                                             <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="period22to30" 
                                                HeaderText="Period22to30" MinWidth="20" ReadOnly="True" Width="150">
                                               
                                            </eo:CustomColumn>
                                             <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="period31to60" 
                                                HeaderText="Period31to60" MinWidth="20" ReadOnly="True" Width="150">
                                               
                                            </eo:CustomColumn>
                                             <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="period61to90" 
                                                HeaderText="Period61to90" MinWidth="20" ReadOnly="True" Width="150">
                                               
                                            </eo:CustomColumn>
                                             <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="periodOver90" 
                                                HeaderText="periodOver90s" MinWidth="20" ReadOnly="True" Width="150">
                                               
                                            </eo:CustomColumn>
                                           
                                        </Columns>
                                    </eo:grid>

       </ContentTemplate></asp:UpdatePanel>
               </div></div>
                </form>
</asp:Content>
