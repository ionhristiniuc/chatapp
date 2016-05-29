using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DTO.NSEntities.Messages
{
    public class NSBaseMessage
    {
        public NSBaseMessage(NSMessageTypeEnum messageType)
        {
            MessageType = messageType;
        }

        public NSBaseMessage()
        {
            
        }

        [JsonProperty]
        [JsonConverter(typeof(StringEnumConverter))]
        public NSMessageTypeEnum? MessageType { get; set; }
    }
}
