using System;
using System.Runtime.Serialization;

namespace Buying.Bll.Dto
{
    [DataContract]
    public class GoodsChangeDto
    {
        [DataMember]
        public Guid Id { get; set; }
        [DataMember]
        public string NewName { get; set; }
    }
}
