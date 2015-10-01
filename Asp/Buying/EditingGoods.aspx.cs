using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

public partial class EditingGoods : System.Web.UI.Page
{
  protected void Page_Load(object sender, EventArgs e)
  {
    if (!IsPostBack)
    {
      SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["GoodsBuyingConnectionString"].ConnectionString);
      string commandText = "select Id, Name from Goods order by Name";
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
begin
  update Goods
  set
    Name = @Name
  where Id = @Id
end";
    command.Parameters.Add("Id", SqlDbType.UniqueIdentifier).Value = Guid.Parse(lstGood.SelectedValue);
    command.Parameters.Add("Name", SqlDbType.NChar, 60).Value = txtNewGood.Text;
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