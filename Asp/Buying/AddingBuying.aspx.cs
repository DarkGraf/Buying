using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

public partial class AddingBuyingPage : System.Web.UI.Page
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
@"declare @CommentId uniqueidentifier
if @Comment is not null and rtrim(@Comment) <> ''
begin
  set @CommentId = newid()
end

insert into Buying (Id, Goods, Priority, InputDate, Comment)
values (newid(), @Goods, @Priority, getdate(), @CommentId)

if @CommentId is not null
begin
  insert into Comments(Id, Description)
  values (@CommentId, @Comment)
end";
    command.Parameters.Add("Goods", SqlDbType.UniqueIdentifier).Value = Guid.Parse(lstGood.SelectedValue);
    command.Parameters.Add("Priority", SqlDbType.Int).Value = int.Parse(txtPriority.Text);
    command.Parameters.Add("Comment", SqlDbType.NChar, 200).Value = txtComment.Text;
    connection.Open();
    try
    {
      command.ExecuteNonQuery();
    }
    finally
    {
      connection.Close();
    }
    Response.Redirect("Default.aspx");
  }
  protected void btnCancel_Click(object sender, EventArgs e)
  {
    Response.Redirect("Default.aspx");
  }
}