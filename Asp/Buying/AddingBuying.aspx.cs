using System;

using ASP;
using Buying.Bll.Service;
using Buying.Bll.Dto;
public partial class AddingBuyingPage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            using (BuyingClient client = BuyingClient.Create(Global.Address))
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
        using (BuyingClient client = BuyingClient.Create(Global.Address))
        {
          BuyingAddDto buyingInfo = new BuyingAddDto
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