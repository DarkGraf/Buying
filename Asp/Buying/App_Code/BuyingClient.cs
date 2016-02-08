using System;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Service
{
  public class BuyingClient : ClientBase<IBuyingService>, IBuyingService
  {
    public static BuyingClient Create()
    {
      Uri uri = System.Web.HttpContext.Current.Request.Url;
      string address = string.Format("{0}{1}{2}:{3}/Service.svc", uri.Scheme, Uri.SchemeDelimiter, uri.Host, uri.Port);
      return new BuyingClient(new BasicHttpBinding(), new EndpointAddress(address));
    }

    protected BuyingClient(Binding binding, EndpointAddress remoteAddress) : base(binding, remoteAddress) { }

    #region IBuyingService
    
    public BuyingDataContract[] GetBuyings()
    {
      return Channel.GetBuyings();
    }

    public void AddBuying(BuyingAddDataContract buyingInfo)
    {
      Channel.AddBuying(buyingInfo);
    }

    public void DeleteBuying(Guid id)
    {
      Channel.DeleteBuying(id);
    }

    public GoodsDataContract[] GetGoods()
    {
      return Channel.GetGoods();
    }

    #endregion
  }
}