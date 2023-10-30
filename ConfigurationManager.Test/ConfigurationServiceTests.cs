using ConfigurationManager.API.Cache;
using ConfigurationManager.API.Documents;
using ConfigurationManager.API.Models;
using ConfigurationManager.API.Repositories;
using ConfigurationManager.API.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ConfigurationManager.Test
{
    public class ConfigurationServiceTests
    {
        [Fact]
        public async Task Test_GetConfigurationsAsync_Should_Return_Configurations_Successfully()
        {
            //Given
            var configurationRepositoryMock = new Mock<IConfigurationRepository>();
            var cacheServiceMock = new Mock<ICacheService>();

            var expectedResult = new List<ConfigurationDocument>
            {
                new ConfigurationDocument
                {
                    ApplicationName = "SERVICE-A",
                    Name = "SiteName",
                    Value = "beymen.com.tr",
                    Type = "String",
                    IsActive = true
                }
            };

            configurationRepositoryMock.Setup(_ => _.GetConfigurationsAsync()).ReturnsAsync(expectedResult);

            //When
            var configurationService = new ConfigurationService(configurationRepositoryMock.Object, cacheServiceMock.Object);

            var actualResult = await configurationService.GetConfigurationsAsync();

            //Then
            Assert.Equal(expectedResult.Count, actualResult.Count);
            Assert.Equal(expectedResult[0].ApplicationName, actualResult[0].ApplicationName);
            Assert.Equal(expectedResult[0].Name, actualResult[0].Name);
            Assert.Equal(expectedResult[0].Value, actualResult[0].Value);
            Assert.Equal(expectedResult[0].Type, actualResult[0].Type);
            Assert.Equal(expectedResult[0].IsActive, actualResult[0].IsActive);
            Assert.Equal(expectedResult[0].Id, actualResult[0].Id);

            configurationRepositoryMock.Verify(mock => mock.GetConfigurationsAsync(), Times.Once());
            configurationRepositoryMock.VerifyNoOtherCalls();
            cacheServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Test_CreateConfigurationAsync_Should_Create_Configuration_Successfully()
        {
            //Given
            var configurationRepositoryMock = new Mock<IConfigurationRepository>();
            var cacheServiceMock = new Mock<ICacheService>();

            var document = new ConfigurationDocument
            {
                ApplicationName = "SERVICE-A",
                Name = "SiteName",
                Value = "beymen.com.tr",
                Type = "String",
                IsActive = true
            };

            ConfigurationDocument existsDocument = null;

            configurationRepositoryMock.Setup(_ => _.GetConfigurationAsync("SiteName", "SERVICE-A")).ReturnsAsync(existsDocument);

            configurationRepositoryMock.Setup(_ => _.CreateConfigurationAsync(document)).Returns(Task.CompletedTask);

            cacheServiceMock.Setup(_ => _.InvalidateCacheAsync("SERVICE-A:SiteName")).Returns(Task.CompletedTask);

            //When
            var configurationService = new ConfigurationService(configurationRepositoryMock.Object, cacheServiceMock.Object);

            await configurationService.CreateConfigurationAsync(new CreateConfigurationRequest
            {
                ApplicationName = document.ApplicationName,
                Name = document.Name,
                Value = document.Value, 
                Type = document.Type,
                IsActive = document.IsActive
            });

            //Then
            configurationRepositoryMock.Verify(mock => mock.GetConfigurationAsync("SiteName", "SERVICE-A"), Times.Once());
            configurationRepositoryMock.Verify(mock => mock.CreateConfigurationAsync(It.IsAny<ConfigurationDocument>()), Times.Once());
            cacheServiceMock.Verify(mock => mock.InvalidateCacheAsync("SERVICE-A:SiteName"), Times.Once());
            configurationRepositoryMock.VerifyNoOtherCalls();
            cacheServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Test_CreateConfigurationAsync_Should_Throw_Error_When_Configuration_Name_Exists()
        {
            //Given
            var configurationRepositoryMock = new Mock<IConfigurationRepository>();
            var cacheServiceMock = new Mock<ICacheService>();

            var document = new ConfigurationDocument
            {
                ApplicationName = "SERVICE-A",
                Name = "SiteName",
                Value = "beymen.com.tr",
                Type = "String",
                IsActive = true
            };

            var existsDocument = new ConfigurationDocument();

            configurationRepositoryMock.Setup(_ => _.GetConfigurationAsync("SiteName", "SERVICE-A")).ReturnsAsync(existsDocument);

            //When
            var configurationService = new ConfigurationService(configurationRepositoryMock.Object, cacheServiceMock.Object);

            //Then
            await Assert.ThrowsAsync<InvalidOperationException>(() => configurationService.CreateConfigurationAsync(new CreateConfigurationRequest
            {
                ApplicationName = document.ApplicationName,
                Name = document.Name,
                Value = document.Value,
                Type = document.Type,
                IsActive = document.IsActive
            }));
            configurationRepositoryMock.Verify(mock => mock.GetConfigurationAsync("SiteName", "SERVICE-A"), Times.Once());
            configurationRepositoryMock.VerifyNoOtherCalls();
            cacheServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Test_UpdateConfigurationAsync_Should_Update_Configuration_Successfully()
        {
            //Given
            var configurationRepositoryMock = new Mock<IConfigurationRepository>();
            var cacheServiceMock = new Mock<ICacheService>();

            var updatedDocument = new ConfigurationDocument
            {
                Id = "653c29be2339fa040a401823",
                ApplicationName = "SERVICE-A",
                Name = "SiteName",
                Value = "newValue",
                Type = "String",
                IsActive = true
            };

            configurationRepositoryMock.Setup(_ => _.FindAndUpdateConfigurationAsync("653c29be2339fa040a401823", "newValue", true)).Returns(Task.FromResult(updatedDocument));

            cacheServiceMock.Setup(_ => _.InvalidateCacheAsync("SERVICE-A:SiteName")).Returns(Task.CompletedTask);

            //When
            var configurationService = new ConfigurationService(configurationRepositoryMock.Object, cacheServiceMock.Object);

            await configurationService.UpdateConfigurationAsync(new UpdateConfigurationRequest
            {
                Id = updatedDocument.Id,
                Value = updatedDocument.Value,
                Type = updatedDocument.Type,
                IsActive = updatedDocument.IsActive
            });

            //Then
            configurationRepositoryMock.Verify(mock => mock.FindAndUpdateConfigurationAsync("653c29be2339fa040a401823", "newValue", true), Times.Once());
            cacheServiceMock.Verify(mock => mock.InvalidateCacheAsync("SERVICE-A:SiteName"), Times.Once());
            configurationRepositoryMock.VerifyNoOtherCalls();
            cacheServiceMock.VerifyNoOtherCalls();
        }
    }
}
