using System;
using System.Runtime.Serialization;

namespace Buying.Bll.Dto
{
    [DataContract]
    public class GoodsAddDto
    {
        [DataMember]
        public string Name { get; set; }
    }
}
