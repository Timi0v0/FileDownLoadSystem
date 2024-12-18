using FileDownLoadSystem.Core.BaseProvider;
using FileDownLoadSystem.Core.Enums;
using FileDownLoadSystem.Entity;
using FileDownLoadSystem.Entity.DomainModels;
using FileDownLoadSystem.Entity.FileInfo;
using Moq;

namespace FileDownLoadSystem.Core.Test.BaseProvider
{
    public class BaseServiceTests
    {
        private readonly Mock<IRepository<FileModel>> _mockRepository;
        private readonly BaseService<FileModel, IRepository<FileModel>> _baseService;

        public BaseServiceTests()
        {
            _mockRepository = new Mock<IRepository<FileModel>>();
            _baseService = new BaseService<FileModel, IRepository<FileModel>>(_mockRepository.Object);
        }

        [Fact]
        public void Update_NullSaveModel_ReturnsParametersLackError()
        {
            // Arrange
            SaveModel saveModel = null;

            // Act
            var result = _baseService.Update<FileModel>(saveModel);

            // Assert
            Assert.False(result.Status);
            string exceptedCode = ResponseType.ParametersLack.GetHashCode().ToString();
            Assert.Equal(exceptedCode, result.Code);
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
            var result = _baseService.Update<FileModel>(saveModel);

            // Assert
            Assert.False(result.Status);
            string exceptedCode = ResponseType.NoKey.GetHashCode().ToString();
            Assert.Equal(exceptedCode, result.Code);
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

            _mockRepository.Setup(r => r.Update(It.IsAny<FileModel>())).Verifiable();

            // Act
            var result = _baseService.Update<FileModel>(saveModel);

            // Assert
            Assert.True(result.Status);
            //验证Update方法是否按照预期被调用 表示Update方法必须被调用一次
            _mockRepository.Verify(r => r.Update<BaseModel>(It.IsAny<BaseModel>()), Times.Once);
        }

        [Fact]
        public void Update_DetailDataWithNoKeys_ReturnsNoKeyError()
        {
            // Arrange
            var saveModel = new SaveModel
            {
                MainData = new Dictionary<string, object>
                {
                    { "Id", 1 }
                },
                DetailData = new List<Dictionary<string, object>>
                {
                    new Dictionary<string, object>()
                }
            };

            // Act
            var result = _baseService.Update<FileModel>(saveModel);

            // Assert
            Assert.False(result.Status);
            Assert.Equal(ResponseType.NoKey.ToString(), result.Code);
        }

        [Fact]
        public void Update_ValidSaveModelWithDetailData_UpdatesMainAndDetailData()
        {
            // Arrange
            var saveModel = new SaveModel
            {
                MainData = new Dictionary<string, object>
                {
                    { "Id", 1 }
                },
                DetailData = new List<Dictionary<string, object>>
                {
                    new Dictionary<string, object>
                    {
                        { "DetailId", 1 },
                        { "FileModelId", 1 }
                    }
                }
            };

            _mockRepository.Setup(r => r.Update(It.IsAny<FileModel>())).Verifiable();

            // Act
            var result = _baseService.Update<FileModel>(saveModel);

            // Assert
            Assert.True(result.Status);
            _mockRepository.Verify(r => r.Update(It.IsAny<BaseModel>()), Times.Once);
        }

        [Fact]
        public void Update_DetailDataWithInvalidForeignKey_ReturnsNoKeyError()
        {
            // Arrange
            var saveModel = new SaveModel
            {
                MainData = new Dictionary<string, object>
                {
                    { "Id", 1 }
                },
                DetailData = new List<Dictionary<string, object>>
                {
                    new Dictionary<string, object>
                    {
                        { "DetailId", 1 },
                        { "FileModelId", 0 } // Invalid foreign key
                    }
                }
            };

            // Act
            var result = _baseService.Update<FileModel>(saveModel);

            // Assert
            Assert.False(result.Status);
            Assert.Equal(ResponseType.NoKey.ToString(), result.Code);
        }

        [Fact]
        public void Update_ValidSaveModelWithEmptyDetailData_UpdatesMainDataOnly()
        {
            // Arrange
            var saveModel = new SaveModel
            {
                MainData = new Dictionary<string, object>
                {
                    { "Id", 1 }
                },
                DetailData = new List<Dictionary<string, object>>()
            };

            _mockRepository.Setup(r => r.Update(It.IsAny<FileModel>())).Verifiable();

            // Act
            var result = _baseService.Update<FileModel>(saveModel);

            // Assert
            Assert.True(result.Status);
            _mockRepository.Verify(r => r.Update(It.IsAny<BaseModel>()), Times.Once);
        }
    }
}
