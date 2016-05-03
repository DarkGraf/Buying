using System;

using ASP;
using Buying.Bll.Service;
using Buying.Bll.Dto;

public partial class EditingGoods : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        using (BuyingClient client = BuyingClient.Create(Global.Address))
        {
            lstGood.DataSource = client.GetGoods();
            lstGood.DataTextField = "Name";
            lstGood.DataValueField = "Id";
            lstGood.DataBind();
        }
    }

    protected void btnOk_Click(object sender, EventArgs e)
    {
        using (BuyingClient client = BuyingClient.Create(Global.Address))
        {
            GoodsChangeDto goods = new GoodsChangeDto
          {
              Id = Guid.Parse(lstGood.SelectedValue),
              NewName = txtNewGood.Text
          };

            client.ChangeGoods(goods);
        }
        Response.Redirect("GoodsDirectory.aspx");
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("GoodsDirectory.aspx");
    }
}