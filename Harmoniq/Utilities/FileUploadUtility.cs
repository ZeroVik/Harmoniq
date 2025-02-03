using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

public static class FileUploadUtility
{
    public static async Task<string> SaveFileAsync(IFormFile file, string uploadFolderPath)
    {
        if (file == null || file.Length == 0)
            return null;

        string fileExtension = Path.GetExtension(file.FileName);
        string uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
        string filePath = Path.Combine(uploadFolderPath, uniqueFileName);

        // Ensure the directory exists
        Directory.CreateDirectory(uploadFolderPath);

        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(fileStream);
        }

        return uniqueFileName; // Returning only the file name, not full path
    }
}
