using ConfigurationManager.API.Documents;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConfigurationManager.API.Repositories
{
    public class ConfigurationRepository: IConfigurationRepository
    {
        private IMongoCollection<ConfigurationDocument> _configurationCollection;
        public ConfigurationRepository(IMongoDatabase mongoDatabase)
        {
            _configurationCollection = mongoDatabase.GetCollection<ConfigurationDocument>("configurations");
        }

        public Task<List<ConfigurationDocument>> GetConfigurationsAsync()
        {
            return _configurationCollection.Find(Builders<ConfigurationDocument>.Filter.Empty).ToListAsync();
        }

        public Task<ConfigurationDocument> GetConfigurationAsync(string name, string applicationName)
        {
            var filter = Builders<ConfigurationDocument>.Filter.Eq(n => n.Name, name) 
                & Builders<ConfigurationDocument>.Filter.Eq(n => n.ApplicationName, applicationName);

            return _configurationCollection.Find(filter).FirstOrDefaultAsync();
        }

        public Task CreateConfigurationAsync(ConfigurationDocument configurationDocument)
        {
            return _configurationCollection.InsertOneAsync(configurationDocument);
        }

        public Task<ConfigurationDocument> FindAndUpdateConfigurationAsync(string id, string value, bool isActive)
        {
            var filter = Builders<ConfigurationDocument>.Filter.Eq(n => n.Id, id);

            var updateDefn = Builders<ConfigurationDocument>.Update
                .Set(x => x.Value, value)
                .Set(x => x.IsActive, isActive);

            return _configurationCollection.FindOneAndUpdateAsync(filter, updateDefn);
        }
    }
}