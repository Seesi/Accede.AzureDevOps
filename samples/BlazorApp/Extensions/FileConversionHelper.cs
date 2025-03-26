using System;
using Microsoft.AspNetCore.Components.Forms;

namespace BlazorApp.Extensions;
public static class FileConversionHelper
{
    public static async Task<List<IFormFile>> ConvertToIFormFileListAsync(List<IBrowserFile> browserFiles)
    {
        var formFiles = new List<IFormFile>();

        foreach (var browserFile in browserFiles)
        {
            var memoryStream = new MemoryStream();
            await browserFile.OpenReadStream().CopyToAsync(memoryStream);
            memoryStream.Position = 0; // Reset stream position

            var formFile = new FormFile(memoryStream, 0, memoryStream.Length, browserFile.Name, browserFile.Name)
            {
                Headers = new HeaderDictionary(),
                ContentType = browserFile.ContentType
            };

            formFiles.Add(formFile);
        }

        return formFiles;
    }
}
