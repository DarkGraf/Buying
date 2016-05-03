using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

using ASP;
using Buying.Bll.Dto;
using Buying.Bll.Service;

public partial class Default2 : System.Web.UI.Page
{
  GoodsDto[] usedGoods;
  GoodsDto[] notUsedGoods;

  protected void Page_Load(object sender, EventArgs e)
  {
    gridGoods.DataSource = usedGoods;
    gridGoods.DataBind();
  }

  protected override void OnInit(EventArgs e)
  {
    base.OnInit(e);
    gridGoods.RowDataBound += gridGoods_RowDataBound;

    using (BuyingClient client = BuyingClient.Create(Global.Address))
    {
      // Запомним неиспользуемые товары.
      // Они нужны в gridGoods_RowDataBound для отображения
      // кнопок удаления тех сущностей, для которых это разрешено.
      notUsedGoods = client.GetNotUsedGoods();

      // Запомним все товары. Воспользуемся ими в Page_Load.
      usedGoods = client.GetGoods();
    }
  }

  void gridGoods_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
  {
    if (e.Row.RowType != DataControlRowType.DataRow)
      return;

    GoodsDto goods = e.Row.DataItem as GoodsDto;

    // Отключим кнопки удаления товаров, для которых это запрещено.
    LinkButton btn = (LinkButton)e.Row.FindControl("btnDelete");
    btn.Enabled = notUsedGoods.Where(v => v.Id == goods.Id).Count() > 0;
  }

  protected void btnDelete_Click(object sender, EventArgs e)
  {
    GridViewRow row = (GridViewRow)((Control)sender).NamingContainer;
    DataKey key = gridGoods.DataKeys[row.DataItemIndex];
    using (BuyingClient client = BuyingClient.Create(Global.Address))
    {
      client.DeleteGoods((Guid)key.Value);
    }
    Response.Redirect("GoodsDirectory.aspx");
  }
}