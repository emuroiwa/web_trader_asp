﻿<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Main.Master" CodeBehind="error.aspx.vb" Inherits="WEBTDS._error" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="error-page">
            <h2 class="headline text-red">500</h2>
            <div class="error-content">
              <h3><i class="fa fa-warning text-red"></i> Oops! Something went wrong.</h3>
              <p>
                We will work on fixing that right away.
                Meanwhile, you may <a href="../../index.html">return to dashboard</a> or try using the search form.
              </p>
              <form class="search-form">
              
              </form>
            </div>
          </div><!-- /.error-page -->
</asp:Content>
