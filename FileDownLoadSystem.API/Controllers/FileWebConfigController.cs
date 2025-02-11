using FileDownLoadSystem.Core.Utilities.Response;
using FileDownLoadSystem.System.IService;
using FileDownLoadSystem.System.Service;
using Microsoft.AspNetCore.Mvc;

namespace FileDownLoadSystem.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FileWebConfigController : Controller
    {
        private readonly IFileWebConfigsService _fileWebConfigsService;
        public FileWebConfigController(IFileWebConfigsService fileWebConfigsService)
        {
            _fileWebConfigsService = fileWebConfigsService;
        }
        [HttpGet]
        public async Task<WebResponseContent> GetFileWebConfigs()
        {
            return await _fileWebConfigsService.GetFileWebConfigs();
        }
    }
}
