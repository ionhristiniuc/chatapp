namespace CommunicationLibrary.Net.Client.Communication
{
    interface ICommunicator
    {
        byte[] Read();
        void Write(byte[] buffer);
    }
}