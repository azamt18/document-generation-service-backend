using System.ComponentModel.DataAnnotations;
using Core.Enums;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace API.RequestModels;

public class GetAllFilesRequestModel
{
    [FromQuery(Name = "dateStart")] public string? DateStart { get; set; }
    [FromQuery(Name = "dateEnd")] public string? DateEnd { get; set; }
    [FromQuery(Name = "status")] public FileStatus? Status { get; set; }
    [FromQuery(Name = "skip")] public int? Skip { get; set; }
    [FromQuery(Name = "limit")] public int? Limit { get; set; }
}

public class GetAllFilesCountRequestModel
{
    [FromQuery(Name = "dateStart")] public string? DateStart { get; set; }
    [FromQuery(Name = "dateEnd")] public string? DateEnd { get; set; }
    [FromQuery(Name = "status")] public FileStatus? Status { get; set; }
}

public class RegisterFileRequestModel
{
    [Required(AllowEmptyStrings = false)]
    [JsonProperty("title")]
    public string Title { get; set; }

    [Required(AllowEmptyStrings = false)]
    [JsonProperty("description")]
    public string Description { get; set; }

    [Required(AllowEmptyStrings = false)]
    [JsonProperty("base64Content")]
    public string Base64Content { get; set; }
}

public class UpdateTitleRequestModel
{
    [Required(AllowEmptyStrings = false)]
    [JsonProperty("title")]
    public string Title { get; set; }
}
public class UpdateDescriptionRequestModel
{
    [Required(AllowEmptyStrings = false)]
    [JsonProperty("description")]
    public string Description { get; set; }
}
public class UpdateStatusRequestModel
{
    [Required]
    [JsonProperty("status")]
    public FileStatus Status { get; set; }
}

public class MoveFileRequestModel
{
    [Required]
    [JsonProperty("sourceListId")]
    public int SourceListId { get; set; }
    
    [Required]
    [JsonProperty("targetListId")]
    public int TargetListId { get; set; }
}
