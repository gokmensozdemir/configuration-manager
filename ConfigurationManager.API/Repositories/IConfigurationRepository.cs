using ConfigurationManager.API.Documents;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConfigurationManager.API.Repositories
{
    public interface IConfigurationRepository
    {
        Task<List<ConfigurationDocument>> GetConfigurationsAsync();
        Task<ConfigurationDocument> GetConfigurationAsync(string name, string applicationName);
        Task CreateConfigurationAsync(ConfigurationDocument configurationDocument);
        Task<ConfigurationDocument> FindAndUpdateConfigurationAsync(string id, string value, bool isActive);
    }
}
