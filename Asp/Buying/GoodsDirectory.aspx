<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="GoodsDirectory.aspx.cs" Inherits="Default2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
  <asp:GridView ID="gridGoods" runat="server" AutoGenerateColumns="False" DataSourceID="dsGoods">
    <Columns>
      <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
    </Columns>
  </asp:GridView>
  <asp:SqlDataSource ID="dsGoods" runat="server" ConnectionString="<%$ ConnectionStrings:GoodsBuyingConnectionString %>" SelectCommand="SELECT * FROM [Goods] ORDER BY [Name]"></asp:SqlDataSource>
</asp:Content>

