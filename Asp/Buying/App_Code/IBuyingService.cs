using System;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace Service
{
  [DataContract]
  public class BuyingDataContract
  {
    [DataMember]
    public Guid Id { get; set; }
    [DataMember]
    public string Goods { get; set; }
    [DataMember]
    public int Priority { get; set; }
    [DataMember]
    public DateTime InputDate { get; set; }
    [DataMember]
    public string Comment { get; set; }
  }

  [DataContract]
  public class BuyingAddDataContract
  {
    [DataMember]
    public Guid Goods { get; set; }
    [DataMember]
    public int Priority { get; set; }
    [DataMember]
    public string Comment { get; set; }
  }

  [DataContract]
  public class GoodsDataContract
  {
    [DataMember]
    public Guid Id { get; set; }
    [DataMember]
    public string Name { get; set; }
  }

  [DataContract]
  public class GoodsAddDataContract
  {
    [DataMember]
    public string Name { get; set; }
  }

  [DataContract]
  public class GoodsChangeDataContract
  {
    [DataMember]
    public Guid Id { get; set; }
    [DataMember]
    public string NewName { get; set; }
  }

  [ServiceContract]
  public interface IBuyingService
  {
    /// <summary>
    /// Получение покупок.
    /// </summary>
    /// <returns></returns>
    [OperationContract]
    BuyingDataContract[] GetBuyings();
    /// <summary>
    /// Добавление покупки.
    /// </summary>
    /// <param name="buyingInfo"></param>
    [OperationContract]
    void AddBuying(BuyingAddDataContract buyingInfo);
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
    GoodsDataContract[] GetGoods();
    /// <summary>
    /// Получение неиспользуемых товаров.
    /// </summary>
    [OperationContract]
    GoodsDataContract[] GetNotUsedGoods();
    /// <summary>
    /// Добавление товара.
    /// </summary>
    /// <param name="goodsInfo"></param>
    [OperationContract]
    void AddGoods(GoodsAddDataContract goodsInfo);
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
    void ChangeGoods(GoodsChangeDataContract goodsInfo);
  }
}