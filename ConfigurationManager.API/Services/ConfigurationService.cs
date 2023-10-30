using ConfigurationManager.API.Cache;
using ConfigurationManager.API.Documents;
using ConfigurationManager.API.Models;
using ConfigurationManager.API.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConfigurationManager.API.Services
{
    public class ConfigurationService: IConfigurationService
    {
        private readonly IConfigurationRepository _configurationRepository;
        private readonly ICacheService _cacheService;

        public ConfigurationService(IConfigurationRepository configurationRepository, ICacheService cacheService)
        {
            _configurationRepository = configurationRepository;
            _cacheService = cacheService;
        }

        public async Task<List<ConfigurationModel>> GetConfigurationsAsync()
        {
            var allConfigurations = await _configurationRepository.GetConfigurationsAsync();

            return allConfigurations.Select(x => new ConfigurationModel
            {
                Id = x.Id,
                Name = x.Name,
                Type = x.Type,
                Value = x.Value,
                IsActive = x.IsActive,
                ApplicationName = x.ApplicationName
            }).ToList();
        }

        public async Task CreateConfigurationAsync(CreateConfigurationRequest request)
        {
            var configuration = await _configurationRepository.GetConfigurationAsync(request.Name, request.ApplicationName);
            
            if (configuration != null)
            {
                throw new InvalidOperationException("Configuration name is already exits");
            }            

            await _configurationRepository.CreateConfigurationAsync(new ConfigurationDocument
            {
                Name = request.Name,
                Type = request.Type,
                Value = request.Value,
                IsActive = request.IsActive,
                ApplicationName = request.ApplicationName
            });

            await _cacheService.InvalidateCacheAsync($"{request.ApplicationName}:{request.Name}");
        }

        public async Task UpdateConfigurationAsync(UpdateConfigurationRequest request)
        {
            var configuration = await _configurationRepository.FindAndUpdateConfigurationAsync(request.Id, request.Value, request.IsActive);

            if (configuration != null)
            {
                await _cacheService.InvalidateCacheAsync($"{configuration.ApplicationName}:{configuration.Name}");
            }
        }
    }
}
