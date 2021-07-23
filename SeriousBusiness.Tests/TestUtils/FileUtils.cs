using System;
using System.IO;

namespace SeriousBusiness.Tests.TestUtils
{
    public static class FileUtils
    {
        public static string GetFileContentString(string filePath)
        {
            var currentLocation = Directory.GetCurrentDirectory();
            var path = Path.Combine(currentLocation, filePath);
            if (!File.Exists(path))
                throw new Exception($"File on path {path} does not exist");

            return File.ReadAllText(path);
        }
    }
}
