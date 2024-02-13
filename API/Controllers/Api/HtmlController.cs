using API.Controllers.Base;
using API.RequestModels;
using API.ResponseContracts;
using Microsoft.AspNetCore.Mvc;
using Service.File;

namespace API.Controllers.Api;

[ApiController]
[Route("api/html")]
public class HtmlController : BaseController
{
    private readonly InputFileService _fileService;

    public HtmlController(InputFileService fileService)
    {
        _fileService = fileService;
    }

    [HttpGet("get")]
    public async Task<IActionResult> GetAll([FromQuery] GetAllFilesRequestModel requestModel)
    {
        var result = await _fileService.GetAllFiles(new GetAllFilesModel()
        {
            Limit = requestModel.Limit,
            Skip = requestModel.Skip,
            Status = requestModel.Status,
            DateStart = requestModel.DateStart,
            DateEnd = requestModel.DateEnd
        });

        if (result.Success)
            return Ok(result.Files.Select(FileContract.ConvertToContract));

        return BadRequest(result.Error);
    }

    [HttpGet("count")]
    public async Task<IActionResult> GetCount([FromQuery] GetAllFilesCountRequestModel requestModel)
    {
        var result = await _fileService.GetAllFilesCount(new GetAllFilesCountModel()
        {
            Status = requestModel.Status,
            DateStart = requestModel.DateStart,
            DateEnd = requestModel.DateEnd
        });

        if (result.Success)
            return Ok(result.Count);

        return BadRequest(result.Error);
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetById(long id)
    {
        var file = await _fileService.GetFileById(id);
        if (file != null)
            return Ok(FileContract.ConvertToContract(file));

        return NotFound();
    }

    [HttpPost("post")]
    public async Task<IActionResult> RegisterFile([FromBody] RegisterFileRequestModel requestModel)
    {
        var result = await _fileService.RegisterFile(new RegisterFileModel()
        {
            Title = requestModel.Title,
            Description = requestModel.Description,
            Base64Content = requestModel.Base64Content,
        });

        if (result.Success)
            return Ok(FileContract.ConvertToContract(result.FileEntity));
        
        return BadRequest(result.Error);
    }

    [HttpPut("{id}/title")]
    public async Task<IActionResult> UpdateTitle(long id, [FromBody] UpdateTitleRequestModel requestModel)
    {
        var result = await _fileService.UpdateTitle(id, requestModel.Title);
        if (result.Success)
            return Ok(FileContract.ConvertToContract(result.FileEntity));

        if (result.FileNotExists)
        {
            return Conflict(new
            {
                FileNotFound = true
            });
        }

        return BadRequest(result.Error);
    }

    [HttpPut("{id}/description")]
    public async Task<IActionResult> UpdateDescription(long id, [FromBody] UpdateDescriptionRequestModel requestModel)
    {
        var result = await _fileService.UpdateDescription(id, requestModel.Description);
        if (result.Success)
            return Ok(FileContract.ConvertToContract(result.FileEntity));

        if (result.FileNotExists)
        {
            return Conflict(new
            {
                FileNotFound = true
            });
        }

        return BadRequest(result.Error);
    }

    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateStatus(long id, [FromBody] UpdateStatusRequestModel requestModel)
    {
        var result = await _fileService.UpdateStatus(id, requestModel.Status);
        if (result.Success)
            return Ok(FileContract.ConvertToContract(result.FileEntity));

        if (result.FileNotExists)
        {
            return Conflict(new
            {
                FileNotFound = true
            });
        }

        return BadRequest(result.Error);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFile(long id)
    {
        var result = await _fileService.DeleteFile(id);
        if (result.Success)
            return Ok();

        if (result.FileNotExists)
        {
            return Conflict(new
            {
                FileNotFound = true
            });
        }

        return BadRequest(result.Error);
    }
}