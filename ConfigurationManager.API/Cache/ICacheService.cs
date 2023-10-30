using System.Threading.Tasks;

namespace ConfigurationManager.API.Cache
{
    public interface ICacheService
    {
        Task InvalidateCacheAsync(string key);
    }
}
