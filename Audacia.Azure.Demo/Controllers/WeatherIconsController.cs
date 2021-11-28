using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Audacia.Azure.BlobStorage.Services.Interfaces;
using Audacia.Azure.Demo.Models.Requests;
using Audacia.Azure.ReturnOptions.ImageOption;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Audacia.Azure.Demo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IGetAzureBlobStorageService _getAzureBlobStorageService;
        private readonly IAddAzureBlobStorageService _addAzureBlobStorageService;
        private readonly IUpdateAzureBlobStorageService _updateAzureBlobStorageService;
        private readonly IDeleteAzureBlobStorageService _deleteAzureBlobStorageService;

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(IGetAzureBlobStorageService getAzureBlobStorageService,
            IAddAzureBlobStorageService addAzureBlobStorageService,
            IUpdateAzureBlobStorageService updateAzureBlobStorageService,
            IDeleteAzureBlobStorageService deleteAzureBlobStorageService,
            ILogger<WeatherForecastController> logger)
        {
            _getAzureBlobStorageService = getAzureBlobStorageService;
            _addAzureBlobStorageService = addAzureBlobStorageService;
            _updateAzureBlobStorageService = updateAzureBlobStorageService;
            _deleteAzureBlobStorageService = deleteAzureBlobStorageService;

            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IDictionary<string, string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get(string containerName)
        {
            try
            {
                var blobs = await _getAzureBlobStorageService.GetAllAsync<string, ReturnUrlOption>(containerName);

                return Ok(blobs);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);

                return BadRequest();
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromForm] AddBlobRequest request)
        {
            var uniqueBlobName = Guid.NewGuid().ToString();

            await using var fileStream = request.File.OpenReadStream();

            var addBlobResult = await _addAzureBlobStorageService.ExecuteAsync(request.ContainerName,
                uniqueBlobName,
                fileStream);

            if (addBlobResult)
            {
                return Ok(uniqueBlobName);
            }

            return BadRequest();
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Put([FromForm] UpdateBlobRequest updateBlobRequest)
        {
            byte[] fileBytes;
            await using (var ms = new MemoryStream())
            {
                await updateBlobRequest.File.CopyToAsync(ms);

                fileBytes = ms.ToArray();
            }

            var updateBlobResult = await _updateAzureBlobStorageService.ExecuteAsync(updateBlobRequest.ContainerName,
                updateBlobRequest.BlobName,
                fileBytes);

            if (updateBlobResult)
            {
                return Ok();
            }

            return BadRequest();
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete([FromForm] DeleteBlobRequest deleteBlobRequest)
        {
            var deleteBlobResult = await _deleteAzureBlobStorageService.ExecuteAsync(deleteBlobRequest.ContainerName,
                deleteBlobRequest.BlobName);

            if (deleteBlobResult)
            {
                return Accepted();
            }

            return BadRequest();
        }
    }
}