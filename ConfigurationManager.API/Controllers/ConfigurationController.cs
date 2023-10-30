using ConfigurationManager.API.Models;
using ConfigurationManager.API.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConfigurationManager.API.Controllers
{
    [ApiController]
    [Route("api/configurations")]
    public class ConfigurationController : ControllerBase
    {
        private readonly IConfigurationService _configurationService;

        public ConfigurationController(IConfigurationService configurationService)
        {
            _configurationService = configurationService;
        }

        [HttpGet]
        public async Task<ActionResult<List<ConfigurationModel>>> GetConfigurationsAsync()
        {
            return Ok(await _configurationService.GetConfigurationsAsync());
        }

        [HttpPost]
        public async Task<ActionResult> CreateConfigurationAsync([FromBody]CreateConfigurationRequest request)
        {
            await _configurationService.CreateConfigurationAsync(request);

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateConfigurationAsync(string id, [FromBody]UpdateConfigurationRequest request)
        {
            await _configurationService.UpdateConfigurationAsync(request);

            return NoContent();
        }
    }
}
