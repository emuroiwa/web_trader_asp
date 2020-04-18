<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Main.Master" CodeBehind="test.aspx.vb" Inherits="WEBTDS.test" %>

<%@ Register Assembly="EO.Web" Namespace="EO.Web" TagPrefix="eo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
      <form id="form1" runat="server">
       <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
       <!-- Horizontal Form -->  <eo:CallbackPanel ID="CallbackPanel1" runat="server" Height="150px" Width="200px" UpdateMode="Always" Triggers="{ControlID:Button1;Parameter:}">
      <%--    <eo:Callback ID="Callback1" runat="server" >--%>
             <script>
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
                 }</script>
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
    <%-- loader --%>
    <style>.loader {
	position: fixed;
	left: 0px;
	top: 0px;
	width: 100%;
	height: 100%;
	z-index: 9999;
	background: url('../../dist/img/page-loader.gif') 50% 50% no-repeat rgb(249,249,249);
}</style>
<eo:MsgBox runat="server" id="MsgBox1" HeaderHtml="Dialog Title" MinWidth="150" Width="320px"
    HeaderImageUrl="00020441" HeaderHtmlFormat='<div style="padding-top:4px">{0}</div>' Height="216px"
    ControlSkinID="None" MinHeight="100" HeaderImageHeight="27" AllowResize="True" CloseButtonUrl="00020440" OnButtonClick="MsgBox1_ButtonClick">
    <FooterStyleActive CssText="background-color:#f0f0f0; padding-right: 4px; padding-left: 4px; font-size: 8pt; padding-bottom: 4px; padding-top: 4px; font-family: tahoma"></FooterStyleActive>
    <HeaderStyleActive CssText="background-image:url(00020442);color:#444444;font-family:'trebuchet ms';font-size:10pt;font-weight:bold;padding-bottom:7px;padding-left:8px;padding-right:0px;padding-top:0px;"></HeaderStyleActive>
    <ContentStyleActive CssText="background-color:#f0f0f0;font-family:tahoma;font-size:8pt;padding-bottom:4px;padding-left:4px;padding-right:4px;padding-top:4px"></ContentStyleActive>
    <BorderImages BottomBorder="00020409,00020429" RightBorder="00020407,00020427" TopRightCornerBottom="00020405,00020425"
        TopRightCorner="00020403,00020423" LeftBorder="00020406,00020426" TopLeftCorner="00020401,00020421"
        BottomRightCorner="00020410,00020430" TopLeftCornerBottom="00020404,00020424" BottomLeftCorner="00020408,00020428"
        TopBorder="00020402,00020422"></BorderImages>
</eo:MsgBox>
    <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
                  <asp:Button ID="Button1" runat="server" Text="Button" /><%--</eo:Callback>--%></eo:CallbackPanel> 

             </form>
</asp:Content>
