<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="loggedout.aspx.vb" Inherits="WEBTDS.loggedout" %>


<!DOCTYPE html>
<html>
  <head>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <title>TDS | LoggedOut</title>
    <!-- Tell the browser to be responsive to screen width -->
    <meta content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" name="viewport">
    <!-- Bootstrap 3.3.5 -->
    <link rel="stylesheet" href="../../bootstrap/css/bootstrap.min.css">
    <!-- Font Awesome -->
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/font-awesome/4.4.0/css/font-awesome.min.css">
    <!-- Ionicons -->
    <link rel="stylesheet" href="https://code.ionicframework.com/ionicons/2.0.1/css/ionicons.min.css">
    <!-- Theme style -->
    <link rel="stylesheet" href="../../dist/css/AdminLTE.min.css">

    <!-- HTML5 Shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
    <!--[if lt IE 9]>
        <script src="https://oss.maxcdn.com/html5shiv/3.7.3/html5shiv.min.js"></script>
        <script src="https://oss.maxcdn.com/respond/1.4.2/respond.min.js"></script>
    <![endif]-->
  </head>
  <body class="hold-transition lockscreen" style="background-image:url(dist/img/money.jpg)">
    <!-- Automatic element centering -->
    <div class="lockscreen-wrapper">
      <div class="lockscreen-logo">
        <a href=""><b>TDS</b>Web</a>
      </div>
      

      <!-- START LOCK SCREEN ITEM -->
      <div class="lockscreen-item">
      

        <!-- lockscreen credentials (contains the form) 
           <form id="form1" runat="server" class="lockscreen-credentials">
  <div class="input-group">
         <asp:TextBox ID="password" runat="server" TextMode="Password"  placeholder="password" CssClass="form-control"></asp:TextBox>
            <div class="input-group-btn">
                <asp:Button ID="btnReLogin" runat="server" Text="Login"  CssClass="btn"/>
            
            </div>
          </div>
    </form>-->
   

      </div><!-- /.lockscreen-item -->
      <div class="help-block text-center">
   You have successfully<strong> LOGGED-OUT</strong> of WEBTDS Portal
The application is available in the popup window.
If you don't get any pop-up then please check the pop-up settings in your browser settings.
      </div>
      <div class="text-center">
        <a href="default.aspx">Login Page</a>
      </div>
      <div class="lockscreen-footer text-center">
        Copyright &copy; 2015 <b><a href="" class="text-black">ECB</a></b><br>
        All rights reserved
      </div>
    </div><!-- /.center -->

    <!-- jQuery 2.1.4 -->
    <script src="../../plugins/jQuery/jQuery-2.1.4.min.js"></script>
    <!-- Bootstrap 3.3.5 -->
    <script src="../../bootstrap/js/bootstrap.min.js"></script>
  </body>
</html>
