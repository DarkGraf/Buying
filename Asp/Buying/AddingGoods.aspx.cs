using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

public partial class AddingGoodsPage : System.Web.UI.Page
{
  protected void Page_Load(object sender, EventArgs e)
  {

  }

  protected void btnOk_Click(object sender, EventArgs e)
  {
    SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["GoodsBuyingConnectionString"].ConnectionString);
    SqlCommand command = connection.CreateCommand();
    command.CommandText =
@"if not exists(select 1 from Goods where Name = @Name)
begin
  insert into Goods (Id, Name) values (newid(), @Name)
end";
    command.Parameters.Add("Name", SqlDbType.NChar, 60).Value = txtGood.Text;
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