using NUnit.Framework;
using TradePlatform.Commons.Sistem;
using Microsoft.Practices.Unity;
using Moq;
using TradePlatform;
using System.Collections.Generic;
using TradePlatform.Sandbox.Results.Storing;

namespace Trade_platform.tests.Sandbox.ResultStoring
{
    [TestFixture]
    public class StoringToFileTests
    {
        private Mock<IFileManager> _fileManagerMock;

        [SetUp]
        public void SetUp()
        {
            _fileManagerMock = new Mock<IFileManager>();
            ContainerBuilder.Container.RegisterInstance(_fileManagerMock.Object);
        }

        [Test]
        public void When_creat_instance_will_automaticaly_created_root_folder()
        {
            _fileManagerMock.Setup(x => x.IsDirectoryExist(StoringToFile.STORAGE_FOLDER)).Returns(false);
            new StoringToFile();
            _fileManagerMock.Verify(x => x.CreateFolder(StoringToFile.STORAGE_FOLDER),
                Times.Exactly(1));
        }

        [Test]
        public void root_folder_will_create_only_one_time()
        {
            _fileManagerMock.Setup(x => x.IsDirectoryExist(StoringToFile.STORAGE_FOLDER)).Returns(true);
            new StoringToFile();
            _fileManagerMock.Verify(x => x.CreateFolder(StoringToFile.STORAGE_FOLDER),
                Times.Never);
        }

        [Test]
        public void file_with_empty_data_will_not_stored()
        {
            new StoringToFile().Store(new List<Dictionary<string, string>>(), "/");
            _fileManagerMock.Verify(x => x.CreateFile(It.IsAny<string>(), It.IsAny<string>()),
                Times.Never);
        }

        [Test]
        public void file_with_result_will_stored()
        {
            new StoringToFile().Store(new List<Dictionary<string, string>>
            {
                new Dictionary<string, string>
                {
                    { "id", "123"},
                    { "name", "test"}

                },
                new Dictionary<string, string>
                {
                    { "id", "123"},
                    { "name", "test"}

                },
               new Dictionary<string, string>
                {
                    { "id", "123"},
                    { "name", "test"}

                }
            }, "/");

            _fileManagerMock.Verify(x => x.CreateFile("id/name\r\n123/test\r\n123/test\r\n123/test\r\n", It.IsAny<string>()), Times.Once);
        }
    }
}
