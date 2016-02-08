using System;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace Service
{
  /*
Добавление покупки
  выбор товаров
  добавление покупки с комментарием

Добавление товара
  добавление товара

+Покупки
  +вывод покупок
  +удаление покупок

Удаление товаров
  выбор товаров для удаления
  удаление товара

Редактирование товара
  выбор товаров
  редактирование товара

Товары
  +вывод товаров
  сделать (удаление товара если возмножно)*/

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
  }
}