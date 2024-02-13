using Database;
using Core.Settings;
using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Service.Converter;
using Service.FileStorage;

namespace Service.Generator
{
    public class PdfGeneratorService
    {
        private readonly DatabaseContext _databaseContext;
        private readonly FileStorageService _fileStorageService;
        private readonly string _inputFileBaseDirectory;
        private readonly string _outputFileBaseDirectory;


        public PdfGeneratorService(DatabaseContext databaseContext, FileStorageService fileStorageService, IOptions<FilesStorageSettings> fileDirSettings)
        {
            _databaseContext = databaseContext;
            _fileStorageService = fileStorageService;
            _inputFileBaseDirectory = fileDirSettings.Value.InputFileBaseDirectory;
            _outputFileBaseDirectory = fileDirSettings.Value.OutputFileBaseDirectory;
        }

        public async Task<bool> OnInputFileCreated(string inputFileGuid)
        {
            using (var transaction = _databaseContext.Database.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
            {
                try
                {
                    // fetch created file
                    var inputFileEntity = await _databaseContext.InputHtmlFiles.FirstOrDefaultAsync(f => f.FileGuid == inputFileGuid);
                    if (inputFileEntity == null)
                        return false;

                    // allow creating multiple pdf outputs

                    // declare file path
                    var inputFilePath = _fileStorageService.SelectInputHtmlFilePath(inputFileEntity.CreatedOn, inputFileEntity.FileGuid);
                    var outputFilePath = _fileStorageService.SelectOutputPdfFilePath(inputFileEntity.CreatedOn, inputFileEntity.FileGuid);

                    // check input file existence
                    {
                        if (!System.IO.File.Exists(inputFilePath))
                            return true; // if file doesn't exist for some reason, don't convert it
                    }

                    // update input file status
                    {
                        inputFileEntity.Status = Core.Enums.FileStatus.Active;
                        //_databaseContext.Set<InputHtmlFileEntity>().Update(inputFile);
                    }

                    // register output file
                    {
                        var outputFileEntity = new OutputPdfFileEntity()
                        {
                            CreatedOn = DateTime.Now,
                            UpdatedOn = DateTime.Now,
                            InputHtmlFileId = inputFileEntity.Id
                        };

                        //_databaseContext.Set<OutputPdfFileEntity>().Add(outputFile);
                        _databaseContext.OutputPdfFiles.Add(outputFileEntity);

                        // convert to pdf
                        await ConverterService.ConvertHtmlToPdf(inputFilePath, outputFilePath);
                    }

                    // commit changes
                    await _databaseContext.SaveChangesAsync();
                    transaction.Commit();

                    return true;
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}
