using System;
using System.Runtime.Serialization;

namespace Buying.Bll.Dto
{
    [DataContract]
    public class BuyingDto
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
}
