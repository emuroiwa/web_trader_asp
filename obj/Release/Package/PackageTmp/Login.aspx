<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Login.aspx.vb" Inherits="WEBTDS.Login" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<!DOCTYPE html>

<html>  
 <head runat="server" />
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <title>TDS | Log in</title>
      <script>
          //check if master is on pop up
          if (window.opener != null) {            ////PopUp
              //window.close();
          }
          else {
              //Not a Popup
              window.location = "notpopup.aspx";
          }
          var username = document.getElementById("txtUsername").value
          function openWindow() {
             
              window.open('starting.aspx', 'a', 'height=' + screen.height + ', width=' + screen.width + ',scrollbars=1,resizable=1');
              window.location = "loggedin.aspx";
          }
</script><script>
             // constants to define the title of the alert and button text.

             var ALERT_BUTTON_TEXT = "Ok";

             // over-ride the alert method only if this a newer browser.
             // Older browser will see standard alerts
             if (document.getElementById) {
                 window.tdsalert = function (txt, link, type) {
                     createCustomAlert(txt, link, type);
                 }
             }

             function createCustomAlert(txt, link, type) {
                 // shortcut reference to the document object
                 d = document;
                 if (type == "error") {
                     var ALERT_TITLE = "An error occured!";
                     txt = "<img src='../../dist/img/errorwebtds.png'><br>" + txt;
                 } else {
                     var ALERT_TITLE = "Success!";
                     txt = "<img src='../../dist/img/successwebtds.png'><br>" + txt;

                 }
                 // if the modalContainer object already exists in the DOM, bail out.
                 if (d.getElementById("modalContainer")) return;

                 // create the modalContainer div as a child of the BODY element
                 mObj = d.getElementsByTagName("body")[0].appendChild(d.createElement("div"));
                 mObj.id = "modalContainer";
                 // make sure its as tall as it needs to be to overlay all the content on the page
                 mObj.style.height = document.documentElement.scrollHeight + "px";

                 // create the DIV that will be the alert 
                 alertObj = mObj.appendChild(d.createElement("div"));
                 alertObj.id = "alertBox";
                 // MSIE doesnt treat position:fixed correctly, so this compensates for positioning the alert
                 if (d.all && !window.opera) alertObj.style.top = document.documentElement.scrollTop + "px";
                 // center the alert box
                 alertObj.style.left = (d.documentElement.scrollWidth - alertObj.offsetWidth) / 2 + "px";

                 // create an H1 element as the title bar
                 h1 = alertObj.appendChild(d.createElement("h1"));
                 h1.appendChild(d.createTextNode(ALERT_TITLE));

                 // create a paragraph element to contain the txt argument
                 msg = alertObj.appendChild(d.createElement("p"));
                 msg.innerHTML = txt;

                 // create an anchor element to use as the confirmation button.

                 btn = alertObj.appendChild(d.createElement("a"));
                 btn1 = alertObj.appendChild(d.createElement("b"));
                 btn.id = "closeBtn";
                 btn.appendChild(d.createTextNode(ALERT_BUTTON_TEXT));
                 btn.href = link;
                 // create an anchor element to use as the confirmation button.
                 //btn = alertObj.appendChild(d.createElement("a"));
                 //btn1.id = "cancelBtn";
                 //btn1.appendChild(d.createTextNode("Cancel"));
                 //btn1.href = "#";

                 // set up the onclick event to remove the alert when the anchor is clicked
                 btn.onclick = function () { removeCustomAlert(); window.location = link; return false; }
                 // btn1.onclick = function () { removeCustomAlert(); return false; }


             }

             // removes the custom alert from the DOM
             function removeCustomAlert() {
                 document.getElementsByTagName("body")[0].removeChild(document.getElementById("modalContainer"));
             }</script> <!-- Modals please ur id should be example-modal -->
    <style type="text/css">

#modalContainer {
	background-color:transparent;
	position:absolute;
	width:100%;
	height:100%;
	top:0px;
	left:0px;
	z-index:10000;
	background-image:url(tp.png); /* required by MSIE to prevent actions on lower z-index elements */
}

#alertBox {
	position:relative;
	width:25%;
	min-height:100px;
	margin-top:18%;
	border:2px solid #000;
	background-color:#F2F5F6;
	background-image:url(alert.png);
	background-repeat:no-repeat;
	background-position:20px 30px;
}

#modalContainer > #alertBox {
	position:fixed;
}

#alertBox h1 {
	margin:0;
	font:bold 0.9em verdana,arial;
	background-color:#3c8cbc;
	color:#FFF;
	border-bottom:1px solid #000;
	padding:2px 0 2px 5px;
}

#alertBox p {
	font:0.7em verdana,arial;
	height:100px;
	padding-left:5px;
	margin-left:55px;
}

#alertBox #closeBtn {
	display:block;
	position:relative;
	margin:5px auto;
	padding:3px;
	border:2px solid #000;
	width:70px;
	font:0.7em verdana,arial;
	text-transform:uppercase;
	text-align:center;
	color:#FFF;
	background-color:#3c8cbc;
	text-decoration:none;
}
#alertBox #cancelBtn {
	display:block;
	position:relative;
	margin:5px auto;
	padding:3px;
	border:2px solid #000;
	width:70px;
	font:0.7em verdana,arial;
	text-transform:uppercase;
	text-align:center;
	color:#FFF;
	background-color:#3c8cbc;
	text-decoration:none;
}
/* unrelated styles */

#mContainer {
	position:relative;
	width:600px;
	margin:auto;
	padding:5px;
	border-top:2px solid #000;
	border-bottom:2px solid #000;
	font:0.7em verdana,arial;
}

h1,h2 {
	margin:0;
	padding:4px;
	font:bold 1.5em verdana;
	border-bottom:1px solid #000;
}

code {
	font-size:1.2em;
	color:#069;
}

#credits {
	position:relative;
	margin:25px auto 0px auto;
	width:350px; 
	font:0.7em verdana;
	border-top:1px solid #000;
	border-bottom:1px solid #000;
	height:90px;
	padding-top:4px;
}

#credits img {
	float:left;
	margin:5px 10px 5px 0px;
	border:1px solid #000000;
	width:80px;
	height:79px;
}

.important {
	background-color:#F5FCC8;
	padding:2px;
}

code span {
	color:#3c8dbc;
}
</style>
     
        <noscript><meta http-equiv="refresh" content="0; url=enablejavascript.html" /></noscript>
       <link rel="icon" href="dist/img/goldbars.ico">
     <link rel="shortcut icon" href="dist/img/goldbars.ico" />
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
          <form id="frmLogin" runat="server">
      <div class="login-logo">
        <a href=""><b>TDS</b>WEB</a>
      </div><!-- /.login-logo -->
         <asp:Label ID="lblError" runat="server" Text=""></asp:Label>
         <asp:Label ID="lblsuccess" runat="server" Text=""></asp:Label>
      <div class="login-box-body">
        <p class="login-box-msg">Sign in to start your session</p>
  
         <i class="fa fa-fw fa-suitcase"></i><asp:Label ID="lblCompName" runat="server" Text=""></asp:Label><br />
           <i class="fa fa-fw fa-calendar-plus-o"></i>    <asp:Label ID="lblDate" runat="server" Text=""></asp:Label><br>
          <div class="form-group has-feedback">
              
              <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" ></asp:TextBox>
            
            <span class="glyphicon glyphicon-envelope form-control-feedback"></span>
          </div>
          <div class="form-group has-feedback">
             <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control"></asp:TextBox>
            
            <span class="glyphicon glyphicon-lock form-control-feedback"></span>
          </div>
          <div class="row">
            <div class="col-xs-8">
              
            </div><!-- /.col -->
            <div class="col-xs-4">
              
                <asp:Button ID="btnLogin" runat="server" Text="Login" CssClass="btn btn-primary btn-block btn-flats" />
              
            </div><!-- /.col -->
              <a href="forgotpass.aspx">Forgot Password</a>
              
          </div>
         
   
<!-- /.social-auth-links --><br>

       
        
      </div><!-- /.login-box-body -->
      
  <div class="box-group" id="accordion">
                    <!-- we are adding the .panel class so bootstrap.js collapse plugin detects it -->
                    <div class="panel box box-primary">
                      <div class="box-header with-border">
                        <h4 class="box-title">
                          <a data-toggle="collapse" data-parent="#accordion" href="#collapseOne">
                           Settings
                          </a>
                        </h4>
                      </div>
                      <div id="collapseOne" class="panel-collapse collapse">
                        <div class="box-body">
                             
          Server Name:<asp:TextBox ID="txtServerName" runat="server" CssClass="form-control"></asp:TextBox><br />
          DataBase Name:<asp:TextBox ID="txtDatabaseName" runat="server" CssClass="form-control"></asp:TextBox><br />
         DataBase Password:<asp:TextBox ID="txtDatabasePassword" runat="server" CssClass="form-control"></asp:TextBox><br />
         <asp:Button ID="btnSave" runat="server" Text="Save"  CssClass="btn btn-primary btn-block btn-flats"  />
                            
     </form>
                        </div>
                      </div></div></div>
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
