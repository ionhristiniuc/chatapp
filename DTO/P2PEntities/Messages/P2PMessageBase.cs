using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DTO.P2PEntities.Messages
{
    public class P2PMessageBase
    {
        public P2PMessageBase(P2PMessageTypeEnum messageType)
        {
            MessageType = messageType;
        }

        public P2PMessageBase()
        {

        }

        [JsonProperty]
        [JsonConverter(typeof(StringEnumConverter))]
        public P2PMessageTypeEnum? MessageType { get; set; }
    }
}
