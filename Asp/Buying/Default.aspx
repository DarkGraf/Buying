<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
  <asp:GridView ID="gridBuying" runat="server" AutoGenerateColumns="False" DataKeyNames="Id" OnRowDeleting="gridBuying_RowDeleting">
    <Columns>
      <asp:BoundField DataField="Name" HeaderText="Наименование" SortExpression="Name" />
      <asp:BoundField DataField="Priority" HeaderText="Приоритет" SortExpression="Priority" />
      <asp:BoundField DataField="InputDate" HeaderText="Дата ввода" SortExpression="InputDate" />
      <asp:BoundField DataField="Comment" HeaderText="Комментарий" SortExpression="Comment" />
      <asp:CommandField HeaderText="Удаление" ShowDeleteButton="true" DeleteText="Удалить" />
    </Columns>
  </asp:GridView>
</asp:Content>