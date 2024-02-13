using Database;
using Microsoft.EntityFrameworkCore;
using Service.FileStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.PdfFile
{
    public class PdfFileService
    {
        private readonly DatabaseContext _databaseContext;
        private readonly FileStorageService _fileStorageService;

        public PdfFileService(DatabaseContext databaseContext, FileStorageService fileStorageService)
        {
            this._databaseContext = databaseContext;
            this._fileStorageService = fileStorageService;
        }


        public async Task<byte[]?> GetPdfFile(long inputFileId)
        {
            try
            {
                var inputFileEntity = await _databaseContext.InputHtmlFiles.FirstOrDefaultAsync(f => f.Id == inputFileId);
                if (inputFileEntity == null)
                    return null;

                return _fileStorageService.SelectOutputPdfFile(inputFileEntity.CreatedOn, inputFileEntity.FileGuid);
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
