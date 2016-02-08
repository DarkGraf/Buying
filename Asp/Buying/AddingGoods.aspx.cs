using System;

using Service;

public partial class AddingGoodsPage : System.Web.UI.Page
{
  protected void Page_Load(object sender, EventArgs e)
  {

  }

  protected void btnOk_Click(object sender, EventArgs e)
  {
    using (BuyingClient client = BuyingClient.Create())
    {
      GoodsAddDataContract goods = new GoodsAddDataContract { Name = txtGood.Text };
      client.AddGoods(goods);
    }

    Response.Redirect("GoodsDirectory.aspx");
  }

  protected void btnCancel_Click(object sender, EventArgs e)
  {
    Response.Redirect("GoodsDirectory.aspx");
  }
}