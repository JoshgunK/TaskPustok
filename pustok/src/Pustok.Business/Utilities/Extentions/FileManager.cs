using Microsoft.AspNetCore.Http;

namespace Pustok.Business.Utilities.Extentions;

public static class FileManager
{
    //public static string SaveFile(string rootPath,string folderName, IFormFile file)
    //{
    //    string fileName = file.FileName;

    //    if (fileName.Length > 64)
    //    {
    //        fileName = fileName.Substring(fileName.Length - 64, 64);
    //    }
    //    fileName = Guid.NewGuid().ToString() + fileName;

    //    string path = Path.Combine(rootPath, folderName, fileName);

    //    using (FileStream stream = new FileStream(path, FileMode.Create))
    //    {
    //        file.CopyTo(stream);
    //    }

    //    return fileName;
    //}

    public static string SaveFile(this IFormFile file, string rootPath, string folderName)
    {
        string fileName = file.FileName;

        if (fileName.Length > 64)
        {
            fileName = fileName.Substring(fileName.Length - 64, 64);
        }
        fileName = Guid.NewGuid().ToString() + fileName;

        string path = Path.Combine(rootPath, folderName, fileName);

        using (FileStream stream = new FileStream(path, FileMode.Create))
        {
            file.CopyTo(stream);
        }

        return fileName;
    }

    public static void DeleteFile(this string imageUrl, string rootPath, string folderName)
    {
        string path = Path.Combine(rootPath, folderName, imageUrl);
        if(File.Exists(path))  File.Delete(path);
    }
}
