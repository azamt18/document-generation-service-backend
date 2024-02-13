namespace Service.File;

public partial class InputFileService
{
    private void ValidateFileExtension(string extension)
    {
        if (string.IsNullOrEmpty(extension))
            throw new Exception("empty extension");

        if (!_allowedExtensions.Contains(extension))
            throw new Exception("not allowed extension");
    }
}