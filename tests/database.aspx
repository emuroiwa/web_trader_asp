<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Main.Master" CodeBehind="database.aspx.vb" Inherits="WEBTDS.database" %>

<%@ Import Namespace="System.Data.SqlClient" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%@ Import Namespace="System.Data.OleDb" %>

<%

'Sample Database Connection Syntax for ASP and SQL Server.

Dim oConn, oRs 
Dim qry, connectstr


dim fieldname = "your_field"
dim tablename = "your_table"

    connectstr = Session("ConnectionString")
oConn.Open(connectstr)

qry = "SELECT * FROM " & tablename

oRS = oConn.Execute(qry)

Do until oRs.EOF
   Response.Write(ucase(fieldname) & ": " & oRs.Fields(fieldname))
   oRS.MoveNext
Loop
oRs.Close


oRs = nothing
oConn = nothing
%>
</asp:Content>
