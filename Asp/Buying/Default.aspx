<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
  <asp:GridView ID="gridBuying" runat="server" AutoGenerateColumns="False" DataSourceID="dsBuying" DataKeyNames="Id" >
    <Columns>
      <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
      <asp:BoundField DataField="Priority" HeaderText="Priority" SortExpression="Priority" />
      <asp:BoundField DataField="InputDate" HeaderText="InputDate" SortExpression="InputDate" />
      <asp:BoundField DataField="Comments" HeaderText="Comments" SortExpression="Comments" />
      <asp:CommandField ShowDeleteButton="true" />
    </Columns>
  </asp:GridView>
  <asp:SqlDataSource ID="dsBuying" runat="server" 
    ConnectionString="<%$ ConnectionStrings:GoodsBuyingConnectionString %>"
    SelectCommand=
"select
  cast(Buying.Id as nchar(36)) as Id,
  Goods.Name,
  Buying.Priority,
  Buying.InputDate,
  isnull(Comments.Description, '') as Comments
from Buying
  inner join Goods on Buying.Goods = Goods.Id
  left join Comments on Buying.Id = Comments.Id
order by Buying.InputDate"
    DeleteCommand="delete from Buying where Id = cast(@Id as uniqueidentifier)">
    <DeleteParameters>
      <asp:Parameter Name="Id" Type="String" />
    </DeleteParameters>
  </asp:SqlDataSource>
</asp:Content>

