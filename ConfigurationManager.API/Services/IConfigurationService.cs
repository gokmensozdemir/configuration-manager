using ConfigurationManager.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConfigurationManager.API.Services
{
    public interface IConfigurationService
    {
        Task<List<ConfigurationModel>> GetConfigurationsAsync();
        Task CreateConfigurationAsync(CreateConfigurationRequest request);
        Task UpdateConfigurationAsync(UpdateConfigurationRequest request);
    }
}
