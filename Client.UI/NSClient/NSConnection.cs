using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Client.UI.NSClient.Configuration;
using Client.UI.NSClient.Jobs;
using CommunicationLibrary.Messages;
using CommunicationLibrary.Messages.MessageHierarchy;
using CommunicationLibrary.Net.Client;
using CommunicationLibrary.NS;
using CommunicationLibrary.Util.Serialization;
using DTO.Entities;
using DTO.NSEntities;
using DTO.NSEntities.Messages;
using DTO.NSEntities.Messages.Connectivity;
using DTO.NSEntities.Messages.Contacts;
using DTO.NSEntities.Messages.KeepAlive;

namespace Client.UI.NSClient
{
    class NSConnection
    {        
        private bool _isWorking;
        private IClient _nsClient;
        private IPEndPoint _nsAddress;
        private CommunicationUtils _communicationHelper = new CommunicationUtils();
        private readonly UserModel _user;

        public NSConnection(UserModel user)
        {
            _user = user;
        }

        public bool IsWorking
        {
            get { return _isWorking; }
        }

        public void Process(IProgress<IJob> progress)
        {
            _nsAddress = new IPEndPoint(IPAddress.Parse(NSConfig.Url), NSConfig.Port);
            _nsClient = new ClientTcp(_nsAddress, new MessageManager());

            _isWorking = true;            

            while (IsWorking)
            {
                SendConnectionMessage();
                ReadMessages(progress);
                return;
            }
        }

        private void ReadMessages(IProgress<IJob> progress)
        {
            SendObject(new GetOnlineContactsRequest());
            
            var objMess = ReadObject();

            while (objMess != null)
            {
                ProcessMessage(objMess, progress);
                
                //Thread.Sleep(3000);

                objMess = ReadObject();
            }
        }

        private void ProcessMessage(object obj, IProgress<IJob> progress)
        {            
            if (obj is ConnectResponse)
            {

            }
            else if (obj is KeepAliveResponse)
            {
                MessageBox.Show("keep alive response received");
            }
            else if (obj is GetOnlineContactsResponse)
            {
                var resp = (GetOnlineContactsResponse) obj;
                IJob job = new SetFriendsOnlineJob(resp.Contacts.ToArray());
                progress.Report(job);
            }
            else if (obj is NotifyClientOnlineRequest)
            {
                var req = (NotifyClientOnlineRequest) obj;
                IJob job = new SetFriendsOnlineJob(req.Contact);
                progress.Report(job);
            }
            else if (obj is NotifyClientOfflineRequest)
            {
                var req = (NotifyClientOfflineRequest)obj;
                IJob job = new SetFriendOfflineJob(req.Contact);
                progress.Report(job);
            }
        }

        private object ReadObject()
        {
            var message = _nsClient.Read();
            var objMess = _communicationHelper.GetMessageObject(message);
            return objMess;
        }

        
        private void SendObject(object obj)
        {
            _nsClient.Send(_communicationHelper.GetBinaryMessage(obj));
        }

        private void SendConnectionMessage()
        {
            SendObject(new ConnectRequest() {Username = _user.Id});
        }

        private void SendDisconnectMessage()
        {
            SendObject(new DisconnectRequest());
        }

        public void Stop()
        {
            //MessageBox.Show("Stop called!");
            SendDisconnectMessage();
            _isWorking = false;
        }
    }
}
