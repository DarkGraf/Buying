using System;
using System.ServiceModel;
using System.ServiceModel.Web;

using Buying.Bll.Dto;

namespace Buying.Bll.Service
{
    [ServiceContract]
    public interface IBuyingService
    {
        /// <summary>
        /// Получение даты синхронизации.
        /// </summary>
        /// <returns></returns>
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/GetSyncDate")]
        [OperationContract]
        DateTime GetSyncDate();

        /// <summary>
        /// Получение покупок.
        /// </summary>
        /// <returns></returns>
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "/GetBuyings")]
        [OperationContract]
        BuyingDto[] GetBuyings();

        /// <summary>
        /// Добавление покупки.
        /// </summary>
        /// <param name="buyingInfo"></param>
        [OperationContract]
        void AddBuying(BuyingAddDto buyingInfo);

        /// <summary>
        /// Удаление покупки.
        /// </summary>
        /// <param name="id"></param>
        [OperationContract]
        void DeleteBuying(Guid id);

        /// <summary>
        /// Получение товаров.
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        GoodsDto[] GetGoods();

        /// <summary>
        /// Получение неиспользуемых товаров.
        /// </summary>
        [OperationContract]
        GoodsDto[] GetNotUsedGoods();

        /// <summary>
        /// Добавление товара.
        /// </summary>
        /// <param name="goodsInfo"></param>
        [OperationContract]
        void AddGoods(GoodsAddDto goodsInfo);

        /// <summary>
        /// Удаление товара.
        /// </summary>
        /// <param name="id"></param>
        [OperationContract]
        void DeleteGoods(Guid id);

        /// <summary>
        /// Изменение информации о товаре.
        /// </summary>
        /// <param name="goodsInfo"></param>
        [OperationContract]
        void ChangeGoods(GoodsChangeDto goodsInfo);
    }
}