using FileDownLoadSystem.Entity.DomainModels;
using FileDownLoadSystem.System.IRespositories;
using FileDownLoadSystem.System.IService;
using Microsoft.AspNetCore.Mvc;

namespace FileDownLoadSystem.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IFileService _fileService;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IFileService fileService)
        {
            _logger = logger;
            _fileService = fileService;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            //var file = _fileService.FindFirst();
            SaveModel saveModel = new SaveModel();
            saveModel.MainData = new Dictionary<string, object>()
            {
                   { "Id", 3 },
                   { "DetailId", 123 },
                   { "FileName", "File_857.txt" },
                   { "FileIconUrl", "https://example.com/icon_3.png" },
                   { "UploadTime", "2024-12-16T14:23:47" },
                   { "ClickCount", 7345 },
                   { "DownloadCount", 189 },
                   { "FileDescription", "Description_72" }
            };
            saveModel.DetailData = new List<Dictionary<string, object>>()
            {
                new Dictionary<string, object>
                {
                    { "PackageUrl", "https://example.com/package_1.zip" },
                    { "PackageName", "Package_Alpha" },
                    { "PublishTime", "2024-12-15T08:30:00" }                    
                }
            };
            saveModel.DelKeys = new List<object> { 4, 3 };
            _fileService.Update(saveModel);
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
