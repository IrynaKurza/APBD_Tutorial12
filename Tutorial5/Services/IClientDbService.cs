namespace Tutorial5.Services;

public interface IClientDbService
{
    Task DeleteClient(int clientId);
}