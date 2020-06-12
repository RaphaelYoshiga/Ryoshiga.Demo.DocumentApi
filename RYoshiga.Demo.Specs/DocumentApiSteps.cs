using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using RYoshiga.Demo.Domain.Adapters;
using RYoshiga.Demo.WebApi.Controllers;
using Shouldly;
using TechTalk.SpecFlow;

namespace RYoshiga.Demo.Specs
{
    [Binding]
    public class DocumentApiSteps
    {
        private IActionResult _response;
        private Mock<IFormFile> _formFileMock;
        private readonly Mock<IFileSaver> _fileSaverMock;
        private readonly Stream _expectedFileStream;

        public DocumentApiSteps()
        {
            _fileSaverMock = new Mock<IFileSaver>();

            _expectedFileStream = Mock.Of<Stream>();
        }

        [Given(@"I have a PDF to upload")]
        public void GivenIHaveAPDFToUpload()
        {
            _formFileMock = new Mock<IFormFile>();
            _formFileMock.Setup(p => p.FileName)
                .Returns("test.pdf");
            _formFileMock.Setup(p => p.Length)
                .Returns((long)6000 * 1024 * 1024);
            _formFileMock.Setup(p => p.OpenReadStream())
                .Returns(_expectedFileStream);
        }

        [When(@"I send the file to the API")]
        public async Task WhenISendThePDFToTheAPI()
        {
            var documentApiController = GetController();
            _response = await documentApiController.Upload(_formFileMock.Object);
        }

        private DocumentApiController GetController()
        {
            var builder = new WebApiSpecHostBuilder();
            builder.AddInstance(_fileSaverMock.Object);
            var host = builder.Build();

            return (DocumentApiController) host.Services.GetRequiredService(typeof(DocumentApiController));
        }

        [Then(@"it is uploaded successfully")]
        public void ThenItIsUploadedSuccessfully()
        {
            _fileSaverMock.Verify(p => p.Save(_formFileMock.Object.FileName, _expectedFileStream));
            _response.ShouldBeOfType<OkResult>();
        }

        [Given(@"I have a non-pdf to upload")]
        public void GivenIHaveANon_PdfToUpload()
        {
            _formFileMock = new Mock<IFormFile>();
            _formFileMock.Setup(p => p.FileName)
                .Returns("test.png");
            _formFileMock.Setup(p => p.Length)
                .Returns((long)6000 * 1024 * 1024);
            _formFileMock.Setup(p => p.OpenReadStream())
                .Returns(_expectedFileStream);
        }

        [Then(@"the API does not accept the file and returns the appropriate messaging and status")]
        public void ThenTheAPIDoesNotAcceptTheFileAndReturnsTheAppropriateMessagingAndStatus()
        {
            _fileSaverMock.VerifyNoOtherCalls();

            _response.ShouldBeOfType<BadRequestResult>();
        }

    }
}
