using API.Controllers.Base;
using Microsoft.AspNetCore.Mvc;
using Service.PdfFile;

namespace API.Controllers.Api
{
    [ApiController]
    [Route("api/pdfFile")]
    public class PdfFileController : BaseController
    {
        private readonly PdfFileService _pdfFileService;
        public PdfFileController(PdfFileService pdfFileService)
        {
            _pdfFileService = pdfFileService;
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetById(long id)
        {
            var fileContent = await _pdfFileService.GetPdfFile(id);
            if (fileContent == null)
                return NotFound();

            return new FileContentResult(fileContent, "application/pdf")
            {
                FileDownloadName = $"{id}.pdf"
            };
        }

    }
}
