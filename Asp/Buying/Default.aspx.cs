using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.ServiceModel;

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

  protected void gridBuying_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
  {
    using (BuyingClient client = BuyingClient.Create())
    {
      foreach (var key in e.Keys.OfType<DictionaryEntry>())
      {
        client.DeleteBuying(Guid.Parse(key.Value.ToString()));
      }
    }

    Server.Transfer("Default.aspx");
  }
}