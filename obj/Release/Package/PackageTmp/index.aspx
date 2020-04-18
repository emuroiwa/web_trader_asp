<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Main.Master" CodeBehind="index.aspx.vb" Inherits="WEBTDS.index" %>

<%@ Register Assembly="EO.Web" Namespace="EO.Web" TagPrefix="eo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        //function MessageBox(msg) {

        //    alert(msg)
        //}

    function haya() {

        alert("haya")
    }
    //hide login label after 3 seconds
    function HideLabel() {
        var seconds = 3;
        setTimeout(function () {
            document.getElementById("<%=lblLogin.ClientID %>").style.display = "none";
        }, seconds * 1000);
    };
</script>

    <!-- Morris charts -->
    <link rel="stylesheet" href="../../plugins/morris/morris.css">
    <!-- Theme style -->

   
    
  <form id="frmLogin" runat="server">
      <asp:Label ID="lblLogin" runat="server" Text=""   ></asp:Label>
    <div class="box box-success">
           <div class="box-header with-border">
                  <h3 class="box-title">Start Up <asp:Label ID="lblDealUsers" runat="server" Text="Label"></asp:Label></h3>
                  <div class="box-tools pull-right"> 
                    <button class="btn btn-box-tool" data-widget="remove"><i class="fa fa-times"></i></button>
                  </div><!-- /.box-tools -->
                </div><!-- /.box-header -->
                <div class="box-body">

                    <asp:Button ID="btnAll" runat="server" Text="View All Deals" CssClass="btn btn-block btn-primary btn-xs" />  
                          
     <asp:Label ID="lblCancelledFX" runat="server" Text="" Font-Size="XX-Small" ForeColor="White"></asp:Label>
     <asp:Label ID="lblMaturedFX" runat="server" Text="" Font-Size="XX-Small" ForeColor="White"></asp:Label>
     <asp:Label ID="lblLiveFX" runat="server" Text="" Font-Size="XX-Small" ForeColor="White"></asp:Label>
     <asp:Label ID="lblCurrencySwaps" runat="server" Text="" Font-Size="XX-Small" ForeColor="White"></asp:Label>
     <asp:Label ID="lblSwap" runat="server" Text="" Font-Size="XX-Small" ForeColor="White"></asp:Label>
     <asp:Label ID="lblSwap1" runat="server" Text="" Font-Size="XX-Small" ForeColor="White"></asp:Label>
     <asp:Label ID="lblSpot" runat="server" Text="" Font-Size="XX-Small" ForeColor="White"></asp:Label>
     <asp:Label ID="lblDealNot" runat="server" Text="" Font-Size="XX-Small" ForeColor="White"></asp:Label>
     <asp:Label ID="lblCancelledMM" runat="server" Text="" Font-Size="XX-Small" ForeColor="White"></asp:Label>
     <asp:Label ID="lblMaturedMM" runat="server" Text="" Font-Size="XX-Small" ForeColor="White"></asp:Label>
     <asp:Label ID="lblLiveMM" runat="server" Text="" Font-Size="XX-Small" ForeColor="White"></asp:Label>
     <asp:Label ID="lblFEC" runat="server" Text="" Font-Size="XX-Small" ForeColor="White" 
        ></asp:Label>
                    <asp:Button ID="btnMe" runat="server" Text="View All My Deals" CssClass="btn btn-block btn-primary btn-xs"  />
                     </div></div>
          <div class="row">
            <div class="col-md-6">
              <!-- AREA CHART -->
              <div class="box box-primary">
                <div class="box-header with-border">
                  <h3 class="box-title">Money Market</h3>
                  <div class="box-tools pull-right">
                    <button class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i></button>
                    <button class="btn btn-box-tool" data-widget="remove"><i class="fa fa-times"></i></button>
                  </div>
                </div>
                <div class="box-body chart-responsive">
                  <div class="chart" id="mm" style="height: 300px; position: relative;"></div>
                </div><!-- /.box-body -->
              </div><!-- /.box -->

              <!-- DONUT CHART -->
              <div class="box box-danger">
                <div class="box-header with-border">
                  <h3 class="box-title">FX Deals Settling Next Business Day</h3>
                  <div class="box-tools pull-right">
                    <button class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i></button>
                    <button class="btn btn-box-tool" data-widget="remove"><i class="fa fa-times"></i></button>
                  </div>
                </div>
                <div class="box-body chart-responsive">
                  <div class="chart" id="fx" style="height: 300px; position: relative;"></div>
                </div><!-- /.box-body -->
              </div><!-- /.box -->

            </div><!-- /.col (LEFT) -->
            <div class="col-md-6">
              <!-- LINE CHART -->
              <div class="box box-info">
                <div class="box-header with-border">
                  <h3 class="box-title">VFX Placements and Deposits</h3>
                  <div class="box-tools pull-right">
                    <button class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i></button>
                    <button class="btn btn-box-tool" data-widget="remove"><i class="fa fa-times"></i></button>
                  </div>
                </div>
                <div class="box-body chart-responsive">
                  <div class="chart" id="fxdeposit" style="height: 300px; position: relative;"></div>
                </div><!-- /.box-body -->
              </div><!-- /.box -->

              <!-- BAR CHART -->
              <div class="box box-success">
                <div class="box-header with-border">
                  <h3 class="box-title">Deal Detail</h3>
                  <div class="box-tools pull-right">
                    <button class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i></button>
                    <button class="btn btn-box-tool" data-widget="remove"><i class="fa fa-times"></i></button>
                  </div>
                </div>
                <div class="box-body chart-responsive">
                 <div class="panel box box-primary">
                      <div class="box-header with-border">
                        <h4 class="box-title">
                          <a data-toggle="collapse" data-parent="#accordion" href="#collapseOne">
                          Deals On Dashboard
                          </a>
                        </h4>
                      </div>
                      <div id="collapseOne" class="panel-collapse collapse">
                        <div class="box-body">
                         

<input type="button" value = "Test the alert" onclick="tdsalert('This is a custom alert dialog that was created by over-riding the window.alert method.','#','error');" />
                            <asp:Button ID="Button1" runat="server" Text="Button" />
                         <eo:grid ID="GridDashboard" runat="server" BorderColor="#C7D1DF" 
                                        BorderWidth="1px" ColumnHeaderAscImage="00050303" 
                                        ColumnHeaderDescImage="00050304" ColumnHeaderDividerImage="00050302" 
                                        FixedColumnCount="1" Font-Bold="False" Font-Italic="False" Font-Names="Verdana" 
                                        Font-Overline="False" Font-Size="9pt" Font-Strikeout="False" 
                                        Font-Underline="False" GridLineColor="199, 209, 223" GridLines="Both" 
                                        Height="75px" ItemHeight="19" Width="100%">
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
                                            <eo:CustomColumn DataField="dealreference" HeaderText="Deal Reference" 
                                                MinWidth="20" ReadOnly="True" Width="200">
                                                <CellStyle CssText="font-size:11px; color: #000000; font-family: Calibri;" />
                                            </eo:CustomColumn>
                                            <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="dealamount" 
                                                HeaderText="Deal Amount" MinWidth="10" ReadOnly="True" Width="150">
                                                <CellStyle CssClass="" 
                                                    CssText="font-size:11px; color: #000000; font-family: Calibri;" />
                                            </eo:CustomColumn>
                                            <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="dateentered" 
                                                HeaderText="Date Entered" MinWidth="25" ReadOnly="True" Width="150">
                                                <CellStyle CssClass="" 
                                                    CssText="font-size:11px; color: #000000; font-family: Calibri;" />
                                            </eo:CustomColumn>
                                            <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="startdate" 
                                                HeaderText="Start Date" MinWidth="20" ReadOnly="True" Width="150">
                                                <CellStyle CssClass="" 
                                                    CssText="font-size:11px; color: #000000; font-family: Calibri;" />
                                            </eo:CustomColumn>
                                             <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="maturitydate" 
                                                HeaderText="Maturity Date" MinWidth="20" ReadOnly="True" Width="150">
                                                <CellStyle CssClass="" 
                                                    CssText="font-size:11px; color: #000000; font-family: Calibri;" />
                                            </eo:CustomColumn>
                                                 <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="currency" 
                                                HeaderText="Currency" MinWidth="20" ReadOnly="True" Width="50">
                                                <CellStyle CssClass="" 
                                                    CssText="font-size:11px; color: #000000; font-family: Calibri;" />
                                            </eo:CustomColumn>
                                                                                             <eo:CustomColumn AllowResize="True" AllowSort="True" DataField="expired" 
                                                HeaderText="Expired" MinWidth="20" ReadOnly="True" Width="100">
                                                <CellStyle CssClass="" 
                                                    CssText="font-size:11px; color: #000000; font-family: Calibri;" />
                                            </eo:CustomColumn>
                                        </Columns>
                                    </eo:grid>
                               
                        </div>
                      </div>
                    </div>
                      <!-- panel 2-->
                    <div class="panel box box-success">
                      <div class="box-header with-border">
                        <h4 class="box-title">
                          <a data-toggle="collapse" data-parent="#accordion" href="#collapseTwo">
                            Deal Notification
                          </a>
                        </h4>
                      </div>
                      <div id="collapseTwo" class="panel-collapse collapse">
                        <div class="box-body">
                      
                         <eo:grid ID="GridNoti" runat="server" BorderColor="#C7D1DF" 
                                        BorderWidth="1px" ColumnHeaderAscImage="00050303" 
                                        ColumnHeaderDescImage="00050304" ColumnHeaderDividerImage="00050302" 
                                        FixedColumnCount="1" Font-Bold="False" Font-Italic="False" Font-Names="Verdana" 
                                        Font-Overline="False" Font-Size="9pt" Font-Strikeout="False" 
                                        Font-Underline="False" GridLineColor="199, 209, 223" GridLines="Both" 
                                        Height="75px" ItemHeight="19" Width="100%">
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
                                            <eo:CustomColumn DataField="dealreference" HeaderText="Deal Reference" 
                                                MinWidth="20" ReadOnly="True" Width="200">
                                                <CellStyle CssText="font-size:11px; color: #000000; font-family: Calibri;" />
                                            </eo:CustomColumn>
                                            <eo:CustomColumn AllowResize="False" AllowSort="True" DataField="maturitydate" 
                                                HeaderText="Maturity Date" MinWidth="10" ReadOnly="True">
                                                <CellStyle CssClass="" 
                                                    CssText="font-size:11px; color: #000000; font-family: Calibri;" />
                                            </eo:CustomColumn>
                                            <eo:CustomColumn AllowResize="False" AllowSort="True" DataField="recipname" 
                                                HeaderText="Recipient Name" MinWidth="25" ReadOnly="True" Width="200">
                                                <CellStyle CssClass="" 
                                                    CssText="font-size:11px; color: #000000; font-family: Calibri;" />
                                            </eo:CustomColumn>
                                            <eo:CustomColumn AllowResize="False" AllowSort="True" DataField="reciptype" 
                                                HeaderText="Recipient Type" MinWidth="20" ReadOnly="True" Width="150">
                                                <CellStyle CssClass="" 
                                                    CssText="font-size:11px; color: #000000; font-family: Calibri;" />
                                            </eo:CustomColumn>
                                        </Columns>
                                    </eo:grid>
                        </div>
                      </div>
                    </div>
                       <!-- panel 2-->
                    <div class="panel box box-success">
                      <div class="box-header with-border">
                        <h4 class="box-title">
                          <a data-toggle="collapse" data-parent="#accordion" href="#security">
                           Security Swap 
                          </a>
                        </h4>
                      </div>
                      <div id="security" class="panel-collapse collapse">
                        <div class="box-body">
                           
                         <eo:grid ID="GridSecurity" runat="server" BorderColor="#C7D1DF" 
                                        BorderWidth="1px" ColumnHeaderAscImage="00050303" 
                                        ColumnHeaderDescImage="00050304" ColumnHeaderDividerImage="00050302" 
                                        FixedColumnCount="1" Font-Bold="False" Font-Italic="False" Font-Names="Verdana" 
                                        Font-Overline="False" Font-Size="9pt" Font-Strikeout="False" 
                                        Font-Underline="False" GridLineColor="199, 209, 223" GridLines="Both" 
                                        Height="75px" ItemHeight="19" Width="100%">
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
                                            <eo:CustomColumn DataField="dealreference" HeaderText="Deal Reference" 
                                                MinWidth="20" ReadOnly="True" Width="200">
                                                <CellStyle CssText="font-size:11px; color: #000000; font-family: Calibri;" />
                                            </eo:CustomColumn>
                                            <eo:CustomColumn AllowResize="False" AllowSort="True" DataField="collateralExtReference" 
                                                HeaderText="Collateral ExtReference" MinWidth="10" ReadOnly="True">
                                                <CellStyle CssClass="" 
                                                    CssText="font-size:11px; color: #000000; font-family: Calibri;" />
                                            </eo:CustomColumn>
                                            <eo:CustomColumn AllowResize="False" AllowSort="True" DataField="collateralExpiry" 
                                                HeaderText="Collateral Expiry" MinWidth="25" ReadOnly="True" Width="200">
                                                <CellStyle CssClass="" 
                                                    CssText="font-size:11px; color: #000000; font-family: Calibri;" />
                                            </eo:CustomColumn>
                                            <eo:CustomColumn AllowResize="False" AllowSort="True" DataField="collateralDescription" 
                                                HeaderText="Collateral Description" MinWidth="20" ReadOnly="True" Width="150">
                                                <CellStyle CssClass="" 
                                                    CssText="font-size:11px; color: #000000; font-family: Calibri;" />
                                            </eo:CustomColumn>
                                        </Columns>
                                    </eo:grid>
                        </div>
                      </div>
                    </div>
                </div><!-- /.box-body -->
              </div><!-- /.box -->

            </div><!-- /.col (RIGHT) -->
          </div><!-- /.row -->
       </form>
       
    

    <!-- jQuery 2.1.4 -->
    <script src="../../plugins/jQuery/jQuery-2.1.4.min.js"></script>
  
    <!-- Morris.js charts -->
    <script src="https://cdnjs.cloudflare.com/ajax/libs/raphael/2.1.0/raphael-min.js"></script>
    <script src="../../plugins/morris/morris.min.js"></script>

    <script>
        $(function () {
            "use strict";
            var fxspot = document.getElementById("<%=lblSpot.ClientID%>").innerHTML
            var fxswap = document.getElementById("<%=lblSwap.ClientID%>").innerHTML
            var fxfec = document.getElementById("<%=lblFEC.ClientID%>").innerHTML
            var fxCurrencySwaps = document.getElementById("<%=lblCurrencySwaps.ClientID%>").innerHTML
            var fxswap = document.getElementById("<%=lblSwap1.ClientID%>").innerHTML

            var mmlive = document.getElementById("<%=lblLiveMM.ClientID%>").innerHTML
            var mmMatured = document.getElementById("<%=lblMaturedMM.ClientID%>").innerHTML
            var mmcanceled = document.getElementById("<%=lblCancelledMM.ClientID%>").innerHTML
            var mmnoti = document.getElementById("<%=lblDealNot.ClientID%>").innerHTML

            var fxlive = document.getElementById("<%=lblMaturedFX.ClientID%>").innerHTML
            var fxMatured = document.getElementById("<%=lblMaturedFX.ClientID%>").innerHTML
            var fxcanceled = document.getElementById("<%=lblCancelledFX.ClientID%>").innerHTML
            //FX CHART


            //  alert(fxswap)
            var donut = new Morris.Donut({
                element: 'fx',
                resize: true,
                colors: ["#3c8dbc", "#f56954", "#00a65a", "#df0909", "#09df0c"],
                data: [
                  { label: "Spot", value: fxspot },
                  { label: "Swap", value: fxswap },
                  { label: "F.E.C", value: fxfec },
                  { label: "Currency Swap", value: fxCurrencySwaps },
                  { label: "Int Rate Swap", value: fxswap }
                ],
                hideHover: 'auto'
            });

            //MM donut

            var donut = new Morris.Donut({
                element: 'mm',
                resize: true,
                colors: ["#3c8dbc", "#f56954", "#00a65a", "#df0909"],
                data: [
                  { label: "Live Deals", value: mmlive },
                  { label: "Matured Deals", value: mmMatured },
                  { label: "Cancelled Deals", value: mmcanceled },
                  { label: "Deal Notifications", value: mmnoti }
                ],
                hideHover: 'auto'
            });

            //FX deposit donut
            var donut = new Morris.Donut({
                element: 'fxdeposit',
                resize: true,
                colors: ["#3c8dbc", "#f56954", "#00a65a"],
                data: [
                  { label: "Live Deals", value: fxMatured },
                  { label: "Matured Deals ", value: fxMatured },
                  { label: "Cancelled Deals", value: fxcanceled }
                ],
                hideHover: 'auto'
            });
        });
    </script>
 
    
</asp:Content>
