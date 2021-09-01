using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ParksVictoriaApi.Services;

namespace ParksVictoriaApi.Controllers
{
    /// <summary>
    /// File controller 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IFileService _fileService;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="fileService"></param>
        public FileController(IFileService fileService, ILogger<FileController> logger)
        {            
            _fileService = fileService ?? throw new ArgumentNullException(nameof(IFileService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Get the file content
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="fileDirectory"></param>
        /// <returns></returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(List<FileStreamResult>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesErrorResponseType(typeof(BadRequestObjectResult))]
        public async Task<IActionResult> GetFileContent(string fileName, string fileDirectory)
        {
            _logger.LogInformation($"{nameof(FileController)} - {nameof(GetFileContent)} - Started");
            try
            {
                IFile file = new SystemFile(fileName, fileDirectory);
                var result = await _fileService.GetBytesArrayFromFile(file);

                if (result == null)
                {
                    _logger.LogError($"{nameof(FileController)} - {nameof(GetFileContent)} - Not found");
                    return NotFound();
                }

                _logger.LogInformation($"{nameof(FileController)} - {nameof(GetFileContent)} - Finished");
                return new FileStreamResult(new MemoryStream(result), "application/octet-stream");
            }

            catch (AggregateException aggEx)
            {
                string errorMsg = String.Empty;
                _logger.LogError("Error in GetFileContent. Request: " + " Error " + aggEx);
                aggEx.InnerExceptions.ToList().ForEach(ex => errorMsg += $" Error Message :{ex.Message} StackTace: {ex.StackTrace.ToString()} " + Environment.NewLine);
                return BadRequest(errorMsg);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in GetFileContent. Request: " + " Error " + ex);
                return BadRequest(ex);
            }
        }
    }
}
