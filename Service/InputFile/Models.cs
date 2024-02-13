using Core.Enums;
using Database.Entities;

namespace Service.File;

public class GetAllFilesModel
{
    public string? DateStart { get; set; }
    public string? DateEnd { get; set; }
    public FileStatus? Status { get; set; }
    public int? Skip { get; set; }
    public int? Limit { get; set; }
}

public class GetAllFilesResult
{
    public bool Success { get; set; }
    public string? Error { get; set; }
    public IEnumerable<InputHtmlFileEntity> Files { get; set; }
}

public class GetAllFilesCountModel
{
    public string? DateStart { get; set; }
    public string? DateEnd { get; set; }
    public FileStatus? Status { get; set; }
}

public class GetAllFilesCountResult
{
    public bool Success { get; set; }
    public string? Error { get; set; }
    public int Count { get; set; }
}

public record struct RegisterFileModel
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string Base64Content { get; set; }
}
public record struct RegisterFileResult
{
    public bool Success { get; set; }
    public string? Error { get; set; }
    public InputHtmlFileEntity FileEntity { get; set; }
}

public record struct UpdateFileResult
{
    public bool Success { get; set; }
    public string? Error { get; set; }
    public InputHtmlFileEntity FileEntity { get; set; }
    public bool FileNotExists { get; set; }
}

public record struct DeleteFileResult
{
    public bool Success { get; set; }
    public string? Error { get; set; }
    public bool FileNotExists { get; set; }
}