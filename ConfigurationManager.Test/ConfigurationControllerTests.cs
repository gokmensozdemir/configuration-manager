using ConfigurationManager.API.Controllers;
using ConfigurationManager.API.Models;
using ConfigurationManager.API.Services;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace ConfigurationManager.Test
{
    public class ConfigurationControllerTests
    {
        [Fact]
        public async Task Test_GetConfigurationsAsync_Should_Return_Configurations_Successfully()
        {
            //Given
            var configurationServiceMock = new Mock<IConfigurationService>();

            var expectedResult = new List<ConfigurationModel>
            {
                new ConfigurationModel
                {
                    ApplicationName = "SERVICE-A",
                    Name = "SiteName",
                    Value = "beymen.com.tr",
                    Type = "String",
                    IsActive = true
                }
            };

            configurationServiceMock.Setup(_ => _.GetConfigurationsAsync()).ReturnsAsync(expectedResult);

            //When
            var configurationController = new ConfigurationController(configurationServiceMock.Object);

            var actualResult = await configurationController.GetConfigurationsAsync();

            //Then
            configurationServiceMock.Verify(mock => mock.GetConfigurationsAsync(), Times.Once());
            configurationServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Test_CreateConfigurationAsync_Should_Create_Configuration_Successfully()
        {
            //Given
            var configurationServiceMock = new Mock<IConfigurationService>();

            var request = new CreateConfigurationRequest
            {
                ApplicationName = "SERVICE-A",
                Name = "SiteName",
                Value = "beymen.com.tr",
                Type = "String",
                IsActive = true
            };

            configurationServiceMock.Setup(_ => _.CreateConfigurationAsync(request)).Returns(Task.CompletedTask);

            //When
            var configurationController = new ConfigurationController(configurationServiceMock.Object);

            var actualResult = await configurationController.CreateConfigurationAsync(request);

            //Then
            configurationServiceMock.Verify(mock => mock.CreateConfigurationAsync(request), Times.Once());
            configurationServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Test_UpdateConfigurationAsync_Should_Update_Configuration_Successfully()
        {
            //Given
            var configurationServiceMock = new Mock<IConfigurationService>();

            var request = new UpdateConfigurationRequest
            {
                Id = "653c29be2339fa040a401823",
                Value = "beymen.com.tr",
                Type = "String",
                IsActive = true
            };

            configurationServiceMock.Setup(_ => _.UpdateConfigurationAsync(request)).Returns(Task.CompletedTask);

            //When
            var configurationController = new ConfigurationController(configurationServiceMock.Object);

            var actualResult = await configurationController.UpdateConfigurationAsync(request.Id, request);

            //Then
            configurationServiceMock.Verify(mock => mock.UpdateConfigurationAsync(request), Times.Once());
            configurationServiceMock.VerifyNoOtherCalls();
        }
    }
}
