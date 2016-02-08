using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

using Service;

public partial class AddingBuyingPage : System.Web.UI.Page
{
  protected void Page_Load(object sender, EventArgs e)
  {
    if (!IsPostBack)
    {
      using (BuyingClient client = BuyingClient.Create())
      {
        var goods = client.GetGoods();
        lstGood.DataSource = goods;
        lstGood.DataTextField = "Name";
        lstGood.DataValueField = "Id";
        lstGood.DataBind();
      }
    }
  }

  protected void btnOk_Click(object sender, EventArgs e)
  {
    /*SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["GoodsBuyingConnectionString"].ConnectionString);
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
    }*/

    using (BuyingClient client = BuyingClient.Create())
    {
      BuyingAddDataContract buyingInfo = new BuyingAddDataContract
      {
        Goods = Guid.Parse(lstGood.SelectedValue),
        Priority = int.Parse(txtPriority.Text),
        Comment = txtComment.Text
      };
      client.AddBuying(buyingInfo);
    }

    Response.Redirect("Default.aspx");
  }

  protected void btnCancel_Click(object sender, EventArgs e)
  {
    Response.Redirect("Default.aspx");
  }
}