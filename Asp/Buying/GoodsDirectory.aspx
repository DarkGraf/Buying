<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="GoodsDirectory.aspx.cs" Inherits="Default2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
  <asp:GridView ID="gridGoods" runat="server" AutoGenerateColumns="False" DataKeyNames="Id">
    <Columns>
      <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
      <asp:TemplateField>
        <ItemTemplate>
          <asp:LinkButton ID="btnDelete" Text="Удалить" runat="server" OnClick="btnDelete_Click" />
        </ItemTemplate>
      </asp:TemplateField>
    </Columns>
  </asp:GridView>
</asp:Content>

