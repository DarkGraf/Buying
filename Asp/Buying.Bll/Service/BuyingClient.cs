using System;
using System.ServiceModel;
using System.ServiceModel.Channels;

using Buying.Bll.Dto;

namespace Buying.Bll.Service
{
    public class BuyingClient : ClientBase<IBuyingService>, IBuyingService
    {
        public static BuyingClient Create(string address)
        {
            return new BuyingClient(new BasicHttpBinding(), new EndpointAddress(address));
        }

        protected BuyingClient(Binding binding, EndpointAddress remoteAddress) : base(binding, remoteAddress) { }

        #region IBuyingService

        public DateTime GetSyncDate()
        {
            return Channel.GetSyncDate();
        }

        public BuyingDto[] GetBuyings()
        {
            return Channel.GetBuyings();
        }

        public GoodsDto[] GetNotUsedGoods()
        {
            return Channel.GetNotUsedGoods();
        }

        public void AddBuying(BuyingAddDto buyingInfo)
        {
            Channel.AddBuying(buyingInfo);
        }

        public void DeleteBuying(Guid id)
        {
            Channel.DeleteBuying(id);
        }

        public GoodsDto[] GetGoods()
        {
            return Channel.GetGoods();
        }

        public void AddGoods(GoodsAddDto goodsInfo)
        {
            Channel.AddGoods(goodsInfo);
        }

        public void DeleteGoods(Guid id)
        {
            Channel.DeleteGoods(id);
        }

        public void ChangeGoods(GoodsChangeDto goodsInfo)
        {
            Channel.ChangeGoods(goodsInfo);
        }

        #endregion
    }
}