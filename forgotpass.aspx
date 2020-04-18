
<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="forgotpass.aspx.vb" Inherits="WEBTDS.forgotpass" %>

<!DOCTYPE html>

<html>
  <head>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <title>TDS | ForgotPass</title>
    <!-- Tell the browser to be responsive to screen width -->
    <meta content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" name="viewport">
    <!-- Bootstrap 3.3.5 -->
    <link rel="stylesheet" href="bootstrap/css/bootstrap.min.css">
    <!-- Font Awesome -->
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/font-awesome/4.4.0/css/font-awesome.min.css">
    <!-- Ionicons -->
    <link rel="stylesheet" href="https://code.ionicframework.com/ionicons/2.0.1/css/ionicons.min.css">
    <!-- Theme style -->
    <link rel="stylesheet" href="dist/css/AdminLTE.min.css">
    <!-- iCheck -->
    <link rel="stylesheet" href="plugins/iCheck/square/blue.css">

    <!-- HTML5 Shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
    <!--[if lt IE 9]>
        <script src="https://oss.maxcdn.com/html5shiv/3.7.3/html5shiv.min.js"></script>
        <script src="https://oss.maxcdn.com/respond/1.4.2/respond.min.js"></script>
    <![endif]-->
  </head>
  <body class="hold-transition login-page" style="background-image:url(dist/img/money.jpg)">
    <div class="login-box">
      <div class="login-logo">
        <a href=""><b>TDS</b>WEB</a>
      </div><!-- /.login-logo -->
         <asp:Label ID="lblError" runat="server" Text=""></asp:Label>
            <asp:Label ID="lblsuccess" runat="server" Text=""></asp:Label>
      <div class="login-box-body">
        <p class="login-box-msg"><b> Forgot Password? Enter your username</b></p>
          
     <form id="form1" runat="server">
         <%--<i class="fa fa-fw fa-suitcase"></i><asp:Label ID="lblCompName" runat="server" Text="Label"></asp:Label><br />
           <i class="fa fa-fw fa-calendar-plus-o"></i>    <asp:Label ID="lblDate" runat="server" Text="Label"></asp:Label><br>--%>
          <%--<div class="form-group has-feedback">
              
              <asp:TextBox ID="username" runat="server" CssClass="form-control" ></asp:TextBox>
            
            <span class="glyphicon glyphicon-envelope form-control-feedback"></span>
          </div>--%>
          <div class="form-group has-feedback">
              
              <asp:TextBox ID="txtUser" runat="server" CssClass="form-control" ></asp:TextBox>
            
            <span class="glyphicon glyphicon-envelope form-control-feedback"></span>
          </div>
          
          <div class="row">
            <div class="col-xs-8">
              
            </div><!-- /.col -->
       
                <table style="width:100%; align-content:center"><tr><td>
               <a href="login.aspx">Cancel</a></td> <td> <asp:Button ID="btnEmaillink" runat="server" Text="Email Link" CssClass="btn btn-info pull-right"/>
              </td></tr></table>
           
          </div>
         
     </form>
<!-- /.social-auth-links --><br>

       
        
      </div><!-- /.login-box-body -->
      
<%--<div class="box box-default collapsed-box">
                <div class="box-header with-border">
                  <h3 class="box-title">Settings</h3>
                  <div class="box-tools pull-right">
                    <button class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-plus"></i></button>
                  </div><!-- /.box-tools -->
                </div><!-- /.box-header -->
                <div class="box-body">
                  The body of the box
                </div><!-- /.box-body -->
              </div><!-- /.box -->--%>
    </div><!-- /.login-box -->
      <script>
          window.history.forward();
    </script>
    <!-- jQuery 2.1.4 -->
    <script src="../../plugins/jQuery/jQuery-2.1.4.min.js"></script>
    <!-- Bootstrap 3.3.5 -->
    <script src="../../bootstrap/js/bootstrap.min.js"></script>
    <!-- iCheck -->
    <script src="../../plugins/iCheck/icheck.min.js"></script>
    <script>
        $(function () {
            $('input').iCheck({
                checkboxClass: 'icheckbox_square-blue',
                radioClass: 'iradio_square-blue',
                increaseArea: '20%' // optional
            });
        });
    </script>
  </body>
</html>
