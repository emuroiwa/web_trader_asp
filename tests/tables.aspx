<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Main.Master" CodeBehind="tables.aspx.vb" Inherits="WEBTDS.tables" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form runat="server">
 <script>
     function getdate() {
         var tt = '03/12/2015'

         var date = new Date(tt);
         var newdate = new Date(date);

         newdate.setDate(newdate.getDate() + 3);

         var dd = newdate.getDate();
         var mm = newdate.getMonth() + 1;
         var y = newdate.getFullYear();

         var someFormattedDate = mm + '/' + dd + '/' + y;
         return someFormattedDate;
     }
     function getDateDiff(time1, time2) {
         var str1 = time1.split('/');
         var str2 = time2.split('/');
         var t1 = new Date(str1[2], str1[0] - 1, str1[1]);
         var t2 = new Date(str2[2], str2[0] - 1, str2[1]);

         var diffMS = t1 - t2;
         console.log(diffMS + ' ms');

         var diffS = diffMS / 1000;
         console.log(diffS + ' ');

         var diffM = diffS / 60;
         console.log(diffM + ' minutes');

         var diffH = diffM / 60;
         console.log(diffH + ' hours');

         var diffD = diffH / 24;
         console.log(diffD + ' days');
         alert(diffD);
     }
     function handle(e) {
         document.getElementById("<%= TextBox1.ClientID %>").value = "ddfdddd";
        // getdate()
        getDateDiff('10/18/2013', '10/14/2013')
        //document.getElementById('MaturityDate').value = 'Track'
        // document.getElementById('MaturityDate').value = getdate();
        $("#MaturityDate").val(getdate());
        return false;
    }
</script>

        <asp:Calendar ID="Calendar1" runat="server" VisibleDate="2015-06-03"></asp:Calendar>


    <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>

    <asp:TextBox ID="TextBox2" runat="server" onkeypress="handle(event)"></asp:TextBox>

    <!-- DataTables -->
    <link rel="stylesheet" href="../../plugins/datatables/dataTables.bootstrap.css">
 

 

    <!-- jQuery 2.1.4 -->
    <script src="../../plugins/jQuery/jQuery-2.1.4.min.js"></script>
  
    <!-- DataTables -->
    <script src="../../plugins/datatables/jquery.dataTables.min.js"></script>
    <script src="../../plugins/datatables/dataTables.bootstrap.min.js"></script>
    <!-- SlimScroll -->
    
    <script>
        $(function () {
            $("#example1").DataTable();
            $('#example2').DataTable({
                "paging": true,
                "lengthChange": false,
                "searching": false,
                "ordering": true,
                "info": true,
                "autoWidth": false
            });
        });
    </script> </form>
</asp:Content>
