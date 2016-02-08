using System;

using Service;

public partial class Default2 : System.Web.UI.Page
{
  protected void Page_Load(object sender, EventArgs e)
  {
    using (BuyingClient client = BuyingClient.Create())
    {
      var goods = client.GetGoods();
      gridGoods.DataSource = goods;
      gridGoods.DataBind();
    }
  }
}