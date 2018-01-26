namespace AASTrader.Client
{
    public interface IClient
    {
        bool IsConnected
        {
            get;
        }

        int Connect();

        int Disconnect();
    }
}
