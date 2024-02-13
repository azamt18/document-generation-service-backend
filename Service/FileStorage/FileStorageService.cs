using Core;
using Core.Settings;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.FileStorage
{
    public class FileStorageService
    {
        private FilesStorageSettings FileStorageSettings { get; }

        public FileStorageService(IOptions<FilesStorageSettings> fileStorageSettings)
        {
            FileStorageSettings = fileStorageSettings.Value;
        }

        public string SelectInputHtmlFilePath(DateTime createdDate, string fileGuid)
        {
            var directoryPath = Path.Combine(FileStorageSettings.InputFileBaseDirectory, createdDate.ConvertToDate(), fileGuid);
            var filePath = Path.Combine(directoryPath, String.Concat(fileGuid, ".html")).Replace('\\', '/');
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            return filePath;
        }

        public string SelectOutputPdfFilePath(DateTime createdDate, string fileGuid)
        {
            var directoryPath = Path.Combine(FileStorageSettings.OutputFileBaseDirectory, createdDate.ConvertToDate(), fileGuid);
            var filePath = Path.Combine(directoryPath, String.Concat(fileGuid, ".pdf")).Replace('\\', '/');
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            return filePath;
        }

        public void SaveInputHtmlFile(DateTime createdDate, string fileGuid, byte[] content)
        {
            System.IO.File.WriteAllBytes(SelectInputHtmlFilePath(createdDate, fileGuid), content);
        }

        public byte[] SelectOutputPdfFile(DateTime createdDate, string fileGuid)
        {
            var directoryPath = Path.Combine(FileStorageSettings.OutputFileBaseDirectory, createdDate.ConvertToDate());
            var filePath = Path.Combine(directoryPath, String.Concat(fileGuid, ".pdf"));
            if (!System.IO.File.Exists(directoryPath))
                return Array.Empty<byte>();

            return System.IO.File.ReadAllBytes(filePath);
        }
    }
}
