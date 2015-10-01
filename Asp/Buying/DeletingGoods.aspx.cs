using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

public partial class DeletingGoods : System.Web.UI.Page
{
  protected void Page_Load(object sender, EventArgs e)
  {
    if (!IsPostBack)
    {
      SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["GoodsBuyingConnectionString"].ConnectionString);
      string commandText =
@"select Goods.Id, Goods.Name 
from Goods
  left join Buying on Goods.Id = Buying.Goods
where
  Buying.Goods is null
order by Name";
      DataTable table = new DataTable();
      SqlDataAdapter adapter = new SqlDataAdapter(commandText, connection);
      adapter.Fill(table);

      lstGood.DataSource = table;
      lstGood.DataTextField = "Name";
      lstGood.DataValueField = "Id";
      lstGood.DataBind();
    }
  }
  protected void btnOk_Click(object sender, EventArgs e)
  {
    SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["GoodsBuyingConnectionString"].ConnectionString);
    SqlCommand command = connection.CreateCommand();
    command.CommandText =
@"if exists(select 1 from Goods where Id = @Id)
and not exists(select 1 from Buying where Goods = @Id)
begin
  delete from Goods
  where Id = @Id
end";
    command.Parameters.Add("Id", SqlDbType.UniqueIdentifier).Value = Guid.Parse(lstGood.SelectedValue);
    connection.Open();
    try
    {
      command.ExecuteNonQuery();
    }
    finally
    {
      connection.Close();
    }
    Response.Redirect("GoodsDirectory.aspx");
  }
  protected void btnCancel_Click(object sender, EventArgs e)
  {
    Response.Redirect("GoodsDirectory.aspx");
  }
}