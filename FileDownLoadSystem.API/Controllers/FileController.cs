using AutoMapper;
using FileDownLoadSystem.Core.Utilities.Response;
using FileDownLoadSystem.Entity.FileInfo;
using FileDownLoadSystem.System.Dto;
using FileDownLoadSystem.System.IService;
using Microsoft.AspNetCore.Mvc;

namespace FileDownLoadSystem.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FileController : Controller
    {
        private readonly IFileService _fileService;
        public FileController(IFileService fileService)
        {
            _fileService = fileService;
        }
        [HttpPost]
        [Route("GetFilesByType")]
        public async Task<WebResponseContent> GetFilesByType(int typeId)
        {
            return await _fileService.GetFilesByType(typeId);
        }
    }
}
