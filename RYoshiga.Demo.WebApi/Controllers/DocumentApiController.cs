using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RYoshiga.Demo.Domain.Adapters;

namespace RYoshiga.Demo.WebApi.Controllers
{
    [Route("documents")]
    public class DocumentApiController : Controller
    {
        private readonly IFileSaver _fileSaver;

        public DocumentApiController(IFileSaver fileSaver)
        {
            _fileSaver = fileSaver;
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile formFile)
        {
            if (!formFile.FileName.EndsWith(".pdf", StringComparison.InvariantCultureIgnoreCase))
            {
                return BadRequest();
            }

            await _fileSaver.Save(formFile.FileName, formFile.OpenReadStream());
            return Ok();
        }
    }
}
