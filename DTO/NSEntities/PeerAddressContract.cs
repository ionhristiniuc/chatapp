using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using P2PCommunicationLibrary;

namespace DTO.NSEntities
{
    public class PeerAddressContract
    {
        public string PrivateIP { get; set; }
        public int PrivatePort { get; set; }
        public string PublicIP { get; set; }
        public int PublicPort { get; set; }

        public PeerAddressContract(string privateIp, int privatePort, string publicIp, int publicPort)
        {
            PrivateIP = privateIp;
            PrivatePort = privatePort;
            PublicIP = publicIp;
            PublicPort = publicPort;
        }

        public PeerAddressContract(PeerAddress address)
        {
            PrivateIP = address.PrivateEndPoint.Address.ToString();
            PrivatePort = address.PrivateEndPoint.Port;
            PublicIP = address.PublicEndPoint.Address.ToString();
            PublicPort = address.PublicEndPoint.Port;
        }

        public PeerAddressContract()
        {
        }

        public PeerAddress ToPeerAddress()
        {
            return new PeerAddress(
                new IPEndPoint(IPAddress.Parse(PrivateIP), PrivatePort),
                    new IPEndPoint(IPAddress.Parse(PublicIP), PublicPort));
        }
    }
}
