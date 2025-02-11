using FileDownLoadSystem.Core.Utilities.Response;
using FileDownLoadSystem.System.IService;
using Microsoft.AspNetCore.Mvc;

namespace FileDownLoadSystem.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FileTypeController : Controller
    {
        private readonly IFileTypeService _fileTypeService;
        public FileTypeController(IFileTypeService fileTypeService)
        {
            _fileTypeService = fileTypeService;
        }
        [HttpGet]
        [Route("GetFileTypeList")]
        public async Task<WebResponseContent> GetFileTypeList()
        {
            return await _fileTypeService.GetFileTypeList();
        }
    }
}
