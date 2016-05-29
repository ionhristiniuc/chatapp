using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.NSEntities
{
    public enum NSMessageTypeEnum
    {
        ConnectRequest,
        ConnectResponse,
        KeepAliveRequest,
        KeepAliveResponse,
        GetOnlineContactsRequest,
        GetOnlineContactsResponse,
        DisconnectRequest,
        DisconnectResponse,
        NotifyClientOnlineRequest,
        NotifyClientOnlineResponse,
        NotifyClientOfflineRequest,
        NotifyClientOfflineResponse
    }
}
