using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.P2PEntities.Messages
{
    public class TextMessage : P2PMessageBase
    {
        public string Text { get; set; }

        public TextMessage()
            : base(P2PMessageTypeEnum.TextMessage)
        {
        }
    }
}
