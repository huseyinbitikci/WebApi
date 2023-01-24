using WebApi.Entities;

namespace WebApi.Business.Abstract
{
    public interface IDeviceService
    {
        string Login(User user);
        Task<string> GetsnapshotAsync(string hash);
    }
}
