namespace BasicConfig.Services
{
    public interface IServicesProvider
    {
        ISomeSerice Get(string clientName);
    }
}