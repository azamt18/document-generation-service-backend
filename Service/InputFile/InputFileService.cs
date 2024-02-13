using System.Globalization;
using System.Linq.Expressions;
using System.Net;
using Core.Enums;
using Core.Settings;
using Database;
using Database.Entities;
using EventPublisher;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Service.Converter;
using Service.FileStorage;

namespace Service.File;

public partial class InputFileService
{
    private readonly DatabaseContext _databaseContext;
    private readonly FileStorageService _fileStorageService;
    private readonly RabbitMqPublisher _rabbitMqPublisher;
    private readonly string _inputFileBaseDirectory;
    private readonly string[] _allowedExtensions;

    public InputFileService(DatabaseContext databaseContext, FileStorageService fileStorageService, RabbitMqPublisher rabbitMqPublisher, IOptions<FilesStorageSettings> fileDirSettings, IOptions<ExtensionSettings> extSettings)
    {
        _databaseContext = databaseContext;
        _fileStorageService = fileStorageService;
        _rabbitMqPublisher = rabbitMqPublisher;
        _inputFileBaseDirectory = fileDirSettings.Value.InputFileBaseDirectory;
        _allowedExtensions = extSettings.Value.AllowedExtensions;
    }

    public async Task<GetAllFilesResult> GetAllFiles(GetAllFilesModel model)
    {
        var result = new GetAllFilesResult();

        try
        {
            var query = _databaseContext.InputHtmlFiles
                .Where(c => c.IsDeleted == false)
                .AsQueryable();

            // created date from
            if (model.DateStart != null)
            {
                var dateTime = DateTime.ParseExact(model.DateStart, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None);
                query = query.Where(m => m.CreatedOn >= dateTime);
            }

            // created date to
            if (!string.IsNullOrEmpty(model.DateEnd))
            {
                var dateTime = DateTime.ParseExact(model.DateEnd, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None)
                    .Date
                    .AddDays(1)
                    .AddMilliseconds(-1);

                query = query.Where(m => m.CreatedOn <= dateTime);
            }

            // status
            if (model.Status != null)
                query = query.Where(c => c.Status == model.Status);

            query = query.OrderByDescending(b => b.Id);

            if (model.Skip.HasValue)
                query = query.Skip(Math.Max(0, model.Skip.Value));

            if (model.Limit.HasValue)
                query = query.Take(Math.Max(1, model.Limit.Value));

            result.Success = true;
            result.Files = await query.ToArrayAsync();
        }
        catch (Exception e)
        {
            result.Error = e.Message;
        }

        return result;
    }

    public async Task<GetAllFilesCountResult> GetAllFilesCount(GetAllFilesCountModel model)
    {
        var result = new GetAllFilesCountResult();

        try
        {
            var query = _databaseContext.InputHtmlFiles
                .Where(c => c.IsDeleted == false)
                .AsQueryable();

            // created date from
            if (model.DateStart != null)
            {
                var dateTime = DateTime.ParseExact(model.DateStart, "yyyy-MM-dd", CultureInfo.InvariantCulture,
                    DateTimeStyles.None);
                query = query.Where(m => m.CreatedOn >= dateTime);
            }

            // created date to
            if (!string.IsNullOrEmpty(model.DateEnd))
            {
                var dateTime = DateTime.ParseExact(model.DateEnd, "yyyy-MM-dd", CultureInfo.InvariantCulture,
                        DateTimeStyles.None)
                    .Date
                    .AddDays(1)
                    .AddMilliseconds(-1);

                query = query.Where(m => m.CreatedOn <= dateTime);
            }

            // status
            if (model.Status != null)
                query = query.Where(c => c.Status == model.Status);

            query = query.OrderByDescending(b => b.Id);

            result.Success = true;
            result.Count = await query.CountAsync();
        }
        catch (Exception e)
        {
            result.Error = e.Message;
        }

        return result;
    }

    public async Task<InputHtmlFileEntity?> GetFileById(long id)
    {
        return await GetOne(c => c.Id == id);
    }
    
    private async Task<InputHtmlFileEntity?> GetOne(Expression<Func<InputHtmlFileEntity?, bool>> expression, CancellationToken cancellationToken = default)
    {
        return await _databaseContext.InputHtmlFiles.FirstOrDefaultAsync(expression, cancellationToken: cancellationToken);
    }

    public async Task<RegisterFileResult> RegisterFile(RegisterFileModel model)
    {
        var result = new RegisterFileResult();

        try
        {
            // prepare metadata
            var content = Convert.FromBase64String(model.Base64Content);
            string fileGuid = Guid.NewGuid().ToString();

            // validate input file extension
            //ValidateFileExtension(Path.GetExtension(model.Base64Content));

            // register input file
            InputHtmlFileEntity fileEntity;
            {
                fileEntity = new InputHtmlFileEntity()
                {
                    CreatedOn = DateTime.Now,
                    UpdatedOn = DateTime.Now,
                    Title = model.Title,
                    Description = model.Description,
                    Status = FileStatus.Draft,
                    FileGuid = fileGuid
                };

                _databaseContext.Set<InputHtmlFileEntity>().Add(fileEntity);
                _databaseContext.InputHtmlFiles.Add(fileEntity);
                await _databaseContext.SaveChangesAsync();
            }

            // save input file in storage
            {
                _fileStorageService.SaveInputHtmlFile(fileEntity.CreatedOn, fileEntity.FileGuid, content);
            }

            // fire event
            {
                _rabbitMqPublisher.PublishInputFileCreatedEvent(fileEntity);
            }

            result.Success = true;
            result.FileEntity = fileEntity;
        }
        catch (Exception e)
        {
            result.Error = e.Message;
        }

        return result;
    }

    public async Task<UpdateFileResult> UpdateTitle(long id, string title)
    {
        var result = new UpdateFileResult();

        try
        {
            var existingFile = await GetOne(c => c.Id == id);
            if (existingFile == null)
            {
                result.FileNotExists = true;
                return result;
            }

            existingFile.Title = title;
            existingFile.UpdatedOn = DateTime.Now;
            _databaseContext.Set<InputHtmlFileEntity>().Update(existingFile);
            await _databaseContext.SaveChangesAsync();

            result.Success = true;
            result.FileEntity = existingFile;
        }
        catch (Exception e)
        {
            result.Error = e.Message;
        }

        return result;
    }

    public async Task<UpdateFileResult> UpdateDescription(long id, string description)
    {
        var result = new UpdateFileResult();

        try
        {
            var existingFile = await GetOne(c => c.Id == id);
            if (existingFile == null)
            {
                result.FileNotExists = true;
                return result;
            }

            existingFile.Description = description;
            existingFile.UpdatedOn = DateTime.Now;
            _databaseContext.Set<InputHtmlFileEntity>().Update(existingFile);
            await _databaseContext.SaveChangesAsync();

            result.Success = true;
            result.FileEntity = existingFile;
        }
        catch (Exception e)
        {
            result.Error = e.Message;
        }

        return result;
    }

    public async Task<UpdateFileResult> UpdateStatus(long id, FileStatus status)
    {
        var result = new UpdateFileResult();

        try
        {
            var existingFile = await GetOne(c => c.Id == id);
            if (existingFile == null)
            {
                result.FileNotExists = true;
                return result;
            }

            existingFile.Status = status;
            existingFile.UpdatedOn = DateTime.Now;
            _databaseContext.Set<InputHtmlFileEntity>().Update(existingFile);
            await _databaseContext.SaveChangesAsync();

            result.Success = true;
            result.FileEntity = existingFile;
        }
        catch (Exception e)
        {
            result.Error = e.Message;
        }

        return result;
    }
    
    public async Task<DeleteFileResult> DeleteFile(long id)
    {
        var result = new DeleteFileResult();

        try
        {
            var existingFile = await GetOne(c => c.Id == id);
            if (existingFile == null)
            {
                result.FileNotExists = true;
                return result;
            }

            existingFile.IsDeleted = true;
            existingFile.DeletedOn = DateTime.Now;
            _databaseContext.Set<InputHtmlFileEntity>().Update(existingFile);
            await _databaseContext.SaveChangesAsync();

            result.Success = true;
        }
        catch (Exception e)
        {
            result.Error = e.Message;
        }

        return result;
    }
}