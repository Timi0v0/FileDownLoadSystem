using FileDownLoadSystem.Core.BaseProvider;
using FileDownLoadSystem.Core.Enums;
using FileDownLoadSystem.Entity;
using FileDownLoadSystem.Entity.DomainModels;
using Moq;

namespace FileDownLoadSystem.Core.Test.BaseProvider
{
    public class BaseServiceTests
    {
        private readonly Mock<IRepository<BaseModel>> _mockRepository;
        private readonly BaseService<BaseModel, IRepository<BaseModel>> _baseService;

        public BaseServiceTests()
        {
            _mockRepository = new Mock<IRepository<BaseModel>>();
            _baseService = new BaseService<BaseModel, IRepository<BaseModel>>(_mockRepository.Object);
        }

        [Fact]
        public void Update_NullSaveModel_ReturnsParametersLackError()
        {
            // Arrange
            SaveModel saveModel = null;

            // Act
            var result = _baseService.Update<BaseModel>(saveModel);

            // Assert
            Assert.False(result.Status);
            Assert.Equal(ResponseType.ParametersLack.ToString(), result.Code);
        }

        [Fact]
        public void Update_NoKeyProperty_ReturnsNoKeyError()
        {
            // Arrange
            var saveModel = new SaveModel
            {
                MainData = new Dictionary<string, object>()
            };

            // Act
            var result = _baseService.Update<BaseModel>(saveModel);

            // Assert
            Assert.False(result.Status);
            Assert.Equal(ResponseType.NoKey.ToString(), result.Code);
        }

        [Fact]
        public void Update_ValidSaveModel_UpdatesMainData()
        {
            // Arrange
            var saveModel = new SaveModel
            {
                MainData = new Dictionary<string, object>
                {
                    { "Id", 1 }
                }
            };

            _mockRepository.Setup(r => r.Update(It.IsAny<BaseModel>())).Verifiable();

            // Act
            var result = _baseService.Update<BaseModel>(saveModel);

            // Assert
            Assert.True(result.Status);
            _mockRepository.Verify(r => r.Update(It.IsAny<BaseModel>()), Times.Once);
        }

        [Fact]
        public void Update_ExceptionThrown_ReturnsError()
        {
            // Arrange
            var saveModel = new SaveModel
            {
                MainData = new Dictionary<string, object>
                {
                    { "Id", 1 }
                }
            };

            _mockRepository.Setup(r => r.Update(It.IsAny<BaseModel>())).Throws(new Exception("Test exception"));

            // Act
            var result = _baseService.Update<BaseModel>(saveModel);

            // Assert
            Assert.False(result.Status);
            Assert.Contains("Test exception", result.Message);
        }
    }
}
