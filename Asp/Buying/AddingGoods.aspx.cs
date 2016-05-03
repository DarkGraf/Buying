using System;

using ASP;
using Buying.Bll.Service;
using Buying.Bll.Dto;

public partial class AddingGoodsPage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnOk_Click(object sender, EventArgs e)
    {
        using (BuyingClient client = BuyingClient.Create(Global.Address))
        {
            GoodsAddDto goods = new GoodsAddDto { Name = txtGood.Text };
            client.AddGoods(goods);
        }

        Response.Redirect("GoodsDirectory.aspx");
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("GoodsDirectory.aspx");
    }
}