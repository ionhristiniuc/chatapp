using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Client.UI.NSClient;
using CommunicationLibrary.Util.Serialization;
using DTO.NSEntities;
using DTO.P2PEntities.Messages;

namespace Client.UI.P2PCommunication
{
    public abstract class P2PConnection
    {
        private readonly ISerializer _serializer;
        public bool IsListening { get; set; }        

        protected P2PConnection(ISerializer serializer)
        {
            _serializer = serializer;
        }

        public MessageReceivedEvent MessageReceivedEvent { get; set; }

        public string UserId { get; set; }

        public abstract bool SendMessage(P2PMessageBase message);

        public abstract void ReadMessages();

        public abstract void Stop();

        public abstract void Run();

        protected P2PMessageBase GetMessage(byte[] bytes)
        {
            var str = System.Text.Encoding.UTF8.GetString(bytes);
            var baseMess = _serializer.Deserialize<P2PMessageBase>(str);

            switch (baseMess.MessageType)
            {
                case P2PMessageTypeEnum.TextMessage:
                    return _serializer.Deserialize<TextMessage>(str);
            }

            throw new ArgumentException("Unexpected p2p message type " + baseMess.MessageType);
        }

        protected byte[] GetBytes(P2PMessageBase mess)
        {
            var str = _serializer.Serialize(mess);
            return Encoding.UTF8.GetBytes(str);
        }

        public abstract PeerAddressContract GetPeerAddress();
    }
}
