using Microsoft.AspNetCore.Http;

namespace GenericNotification.Tests.Utils;

public class FileUtils
{
    public static FormFile GetFormFile(string path, string contentType)
    {
        Stream stream = new StreamReader($"../../../{path}").BaseStream;
        string fileName = path.Split('/')[^1];
        FormFile file = new FormFile(stream, 0, stream.Length, fileName, fileName)
        {
            Headers = new HeaderDictionary()
        };
        file.ContentType = contentType;

        return file;
    }
}