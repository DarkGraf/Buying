<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="AddingGoods.aspx.cs" Inherits="AddingGoodsPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
  <asp:Label Text="Наименование товара:&nbsp" runat="server" />
  <asp:TextBox ID="txtGood" runat="server" />
  <br />
  <asp:Button ID="btnOk" runat="server" OnClick="btnOk_Click" Text="ОК" />
  <asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_Click" Text="Отмена" />
</asp:Content>

