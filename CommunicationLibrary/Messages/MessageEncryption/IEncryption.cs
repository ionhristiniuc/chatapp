namespace CommunicationLibrary.Messages.MessageEncryption
{

    public interface IEncryptor
    {
        byte[] GetEncryption(byte[] buffer);
        byte[] GetDecryption(byte[] buffer);
    }

}
