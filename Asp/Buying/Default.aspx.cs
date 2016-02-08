using System;
using System.Collections;
using System.Linq;
using System.Web.UI.WebControls;

using Service;

public partial class _Default : System.Web.UI.Page
{
  protected void Page_Load(object sender, EventArgs e)
  {
    using (BuyingClient client = BuyingClient.Create())
    {
      var buyings = from buying in client.GetBuyings()
                    select new
                    {
                      buying.Id,
                      Name = buying.Goods,
                      buying.Priority,
                      buying.InputDate,
                      buying.Comment
                    };
      gridBuying.DataSource = buyings;
      gridBuying.DataBind();
    }
  }

  protected void gridBuying_RowDeleting(object sender, GridViewDeleteEventArgs e)
  {
    using (BuyingClient client = BuyingClient.Create())
    {
      var key = e.Keys.OfType<DictionaryEntry>().ToArray()[0];
      client.DeleteBuying(Guid.Parse(key.Value.ToString()));
    }

    Server.Transfer("Default.aspx");
  }
}