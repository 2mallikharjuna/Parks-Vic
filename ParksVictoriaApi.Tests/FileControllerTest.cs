using Moq;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ParksVictoriaApi.Services;
using ParksVictoriaApi.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using System.IO;

namespace ParksVictoriaApi.Tests
{
    [TestClass]
    public class FileControllerTest
    {
        private Mock<IFileService> _fileService;
        private Mock<ILogger<FileController>> _logger;
        private FileController _controller;

        [TestInitialize]
        public void FileControllerTestInitialize()
        {
            _fileService = new Mock<IFileService>();
            _logger = new Mock<ILogger<FileController>>();
            _controller = new FileController(_fileService.Object, _logger.Object);
        }

        [TestMethod]
        public async Task GetFileContent_NullResponse_Notfound()
        {
            string fileName = string.Empty;
            string fileDirectory = string.Empty;

            //arrange
            _fileService.Setup(x => x.GetBytesArrayFromFile(It.IsAny<IFile>(), It.IsAny<CancellationToken>())).ReturnsAsync((Byte[])null);

            //act
            var result = await _controller.GetFileContent(fileName, fileDirectory);

            //assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task GetFileContent_Datafound_OkResponse()
        {
            //arrange
            string fileName = string.Empty;
            string fileDirectory = string.Empty;
            var response = new byte[10];

            //arrange
            _fileService.Setup(x => x.GetBytesArrayFromFile(It.IsAny<IFile>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(response));

            //act
            var result = await _controller.GetFileContent(fileName, fileDirectory) as FileStreamResult;

            //assert
            Assert.IsInstanceOfType(result, typeof(FileStreamResult));
            Assert.AreEqual((result.FileStream as MemoryStream).ToArray().Length, response.Length);
        }

        [TestMethod]
        public async Task GetFileContent_Datafound_ReturnsBadRequest()
        {
            //arrange
            string fileName = string.Empty;
            string fileDirectory = string.Empty;

            _fileService.Setup(x => x.GetBytesArrayFromFile(It.IsAny<IFile>(), It.IsAny<CancellationToken>())).Throws<Exception>();

            //act
            var result = await _controller.GetFileContent(fileName, fileDirectory);

            //assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }
    }
}
