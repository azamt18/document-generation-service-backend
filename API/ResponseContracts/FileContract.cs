using Core;
using Core.Enums;
using Database.Entities;
using Newtonsoft.Json;

namespace API.ResponseContracts;

public class FileContract
{
    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("createdOn")]
    public string CreatedOn { get; set; }

    [JsonProperty("updatedOn")]
    public string UpdatedOn { get; set; }

    [JsonProperty("isDeleted")]
    public bool IsDeleted { get; set; }

    [JsonProperty("deletedOn")]
    public string? DeletedOn { get; set; }

    [JsonProperty("title")]
    public string Title { get; set; }

    [JsonProperty("description")]
    public string Description { get; set; }

    [JsonProperty("status")]
    public FileStatus Status { get; set; }

    [JsonProperty("guid")]
    public string FileGuid { get; set; }


    public static FileContract ConvertToContract(InputHtmlFileEntity fileEntity)
    {
        return new FileContract()
        {
            Id = fileEntity.Id,
            CreatedOn = fileEntity.CreatedOn.ConvertToDateTime(),
            UpdatedOn = fileEntity.UpdatedOn.ConvertToDateTime(),
            IsDeleted = fileEntity.IsDeleted,
            DeletedOn = fileEntity.DeletedOn?.ConvertToDateTime(),
            Title = fileEntity.Title,
            Description = fileEntity.Description,
            Status = fileEntity.Status,
            FileGuid = fileEntity.FileGuid,
        };
    }
    
}