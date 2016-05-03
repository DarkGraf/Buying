using System;
using System.Runtime.Serialization;

namespace Buying.Bll.Dto
{
    [DataContract]
    public class GoodsDto
    {
        [DataMember]
        public Guid Id { get; set; }
        [DataMember]
        public string Name { get; set; }
    }
}
