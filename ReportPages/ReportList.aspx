<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Main.Master" CodeBehind="ReportList.aspx.vb" Inherits="WEBTDS.ReportList" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <head runat="server" />
    <form id="form1" runat="server">
            <script type="text/javascript">
                function openModal() {
                    alert("ttt")
                 
                    $('#customer').modal('show');
                }
                function openDealer() {
                    alert("ttt")

                    $('#customer').modal('show');
                }
                function openTopDepositors() {
                    alert("ttt")

                    $('#customer').modal('show');
                }
</script>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>

         <%-- end of rightcol div --%>
       
        <style>
            #leftcol{
   
    margin:0 auto;
    width:45%;
    float:left;
	    height:100%;
}

#rightcol{
    float:left;
    width:45%;
    height:100%;
    margin:0 auto;
    /*border:1px solid black;
*/
}
        </style>
<body>         <asp:UpdatePanel ID="UpdatePanelAll" UpdateMode="Always" runat="server"><ContentTemplate>
       
      <div class="box box-info">
                <div class="box-header with-border">
                  <h3 class="box-title">Report&nbsp; List</h3> 
                     
                </div><!-- /.box-header -->
                <!-- form start -->
             
                  <div class="box-body">
    <table  style="width:100%"><tr><td>
        <div id="leftcol">
          
              <div class="box box-info">
                <div class="box-header with-border">
                  <h3 class="box-title">Report Catergories</h3> 
                     
                </div><!-- /.box-header -->
                <!-- form start -->
             
                  <div class="box-body">

                      <table style="width:100%"><tr><td style="width:90%">
                              <asp:DropDownList ID="cmbClass" runat="server" AutoPostBack="False" class="form-control select2" style="width: 100%;"></asp:DropDownList><hr />
     
                       <asp:Label ID="Label1" runat="server" Text="Choose Product" BorderColor="#66FFFF" BackColor="#FFFFCC"></asp:Label> <br /><asp:ListBox ID="lstCat" runat="server" Height="400px" Width="100%" OnSelectedIndexChanged="lstCat_SelectedIndexChanged" AutoPostBack="true"></asp:ListBox><br />
                                 
                      </td></tr></table>
                  </div></div>
              <%-- end of rightcol div --%>
                             </div>
        <%--</ContentTemplate></asp:UpdatePanel>--%>
                        <%-- right col div --%>
        <div id="rightcol">
        
              <div class="box box-info">
                <div class="box-header with-border">
                  <h3 class="box-title">Reports</h3> 
                     
                </div><!-- /.box-header -->
                <!-- form start -->
             
                  <div class="box-body">
                    <table style="width:100%"><tr><td>
                       Find <asp:TextBox ID="TextBox1" runat="server" Width="80%"></asp:TextBox><hr />
                        <asp:Label ID="Label2" runat="server" Text="Choose Report" BorderColor="#66FFFF" BackColor="#FFFFCC"></asp:Label> <br />

                      <asp:ListBox ID="listReports" runat="server" style="width:100%;height:400px" AutoPostBack="true"></asp:ListBox>                          <asp:Label ID="lblReportID" runat="server" ForeColor="White"></asp:Label>
                      </td></tr></table>

                  </div></div>
          

         <%-- end of rightcol div --%>
                             </div>
    
                              <%-- end of rightcol div --%>                
                      </td><td style="width:10%">
                          <asp:Button ID="cmdView" runat="server" Text="View Report"  CssClass="btn btn-primary btn-block btn-flats" Height="40px" Width="150px" />
                                <br />  <asp:Button ID="cmdRefresh" runat="server" Text="Refresh"  CssClass="btn btn-primary btn-block btn-flats" Height="40px" Width="150px" />
                                <br />  <asp:Button ID="cmdExit" runat="server" Text="Exit"  CssClass="btn btn-danger btn-block btn-flats" Height="40px" Width="150px"/>
                                <br />
                           </td></tr></table>        </ContentTemplate></asp:UpdatePanel>
            <div class="example-modal1 modal" id="customer" >
                        <div class="modal-dialog" >
                            <div class="modal-content"><div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                <h4 class="modal-title"> Report Parameter </h4></div>
                
                             
                                <div class="modal-body">
                                    <table style="100%"><tr><td>
                                        Report Type <asp:DropDownList ID="cmbReportType" runat="server">
                                            <asp:ListItem>All Deals - Live / Matured</asp:ListItem>
                                                    <asp:ListItem>Live Deals Only</asp:ListItem>
                                                    </asp:DropDownList>
                                                       </td></tr>
                                        <tr><td>
                                       Currency <asp:DropDownList ID="cmbCurrency" runat="server"></asp:DropDownList>
                                                            </td></tr>
                                          <tr><td>
                                        Date <asp:TextBox ID="txtDate" runat="server" required="required"></asp:TextBox>
                                                  <ajaxToolkit:CalendarExtender ID="txtDate_CalendarExtender" runat="server" BehaviorID="txtDate_CalendarExtender" DaysModeTitleFormat="dd/MM/yyyy" TargetControlID="txtDate" />
                                                            </td></tr>
                                         <tr><td>
                                        <asp:Button ID="btnOk" runat="server" Text="Ok" CssClass="btn btn-primary btn-block btn-flats" Height="40px" Width="150px" />
                                                            </td></tr>
                                    </table>
                                    </div></div></div></div>
   
         <div class="example-modal1 modal" id="user" >
                        <div class="modal-dialog" >
                            <div class="modal-content"><div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                <h4 class="modal-title">User  </h4></div>
                
                             
                                <div class="modal-body">
                                    <table style="100%">
                                        <tr><td>
                                       Currency <asp:DropDownList ID="cmbUser" runat="server"></asp:DropDownList>
                                                               </td></tr>
                                          
                                         <tr><td>
                                        <asp:Button ID="btnOk1" runat="server" Text="Ok" CssClass="btn btn-primary btn-block btn-flats" Height="40px" Width="150px" />
                                                            </td></tr>
                                    </table>
                                    </div></div></div></div>
    <div class="example-modal1 modal" id="topdeposits" >
                        <div class="modal-dialog" >
                            <div class="modal-content"><div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                <h4 class="modal-title">User  </h4></div>
                
                             
                                <div class="modal-body">
                                    <table style="100%">
                                        <tr><td>
                                     Number <asp:TextBox ID="txtNumber" runat="server" required="required"  min="1" type="number"></asp:TextBox>
                                                            </td></tr>     
                                         <tr><td>
                                       Currency <asp:DropDownList ID="cmbCurrency1" runat="server"></asp:DropDownList>
                                                            </td></tr>
                                          
                                         <tr><td>
                                        <asp:Button ID="btnOk2" runat="server" Text="Ok" CssClass="btn btn-primary btn-block btn-flats" Height="40px" Width="150px" />
                                                            </td></tr>
                                    </table>
                                    </div></div></div></div>
    </body>


    </form>
</asp:Content>
