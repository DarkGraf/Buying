using System;
using System.Runtime.Serialization;

namespace Buying.Bll.Dto
{
    [DataContract]
    public class BuyingAddDto
    {
        [DataMember]
        public Guid Goods { get; set; }
        [DataMember]
        public int Priority { get; set; }
        [DataMember]
        public string Comment { get; set; }
    }
}
