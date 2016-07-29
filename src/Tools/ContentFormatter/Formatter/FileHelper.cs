using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formatter
{
    static class FileHelper
    {
        public static void Save(string filePath, string content)
        {
            string newPath = filePath.Replace(
                    @"Data\Original\",
                    @"Data\Formatted\");

            newPath = Path.ChangeExtension(newPath, ".txt");
            Directory.CreateDirectory(Path.GetDirectoryName(newPath));

            File.WriteAllText(newPath, content, Encoding.UTF8);

            newPath = newPath.Replace(
                @"F:\git\inter\src\Data\Formatted\",
                string.Empty);

            using (FileStream zipToOpen = new FileStream(@"F:\git\inter\src\ArabicInterpretation\ArabicInterpretation\Core\Resources\data.zip", FileMode.Open))
            {
                using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update))
                {
                    ZipArchiveEntry entry = archive.GetEntry(newPath);
                    if (entry != null)
                    {
                        entry.Delete();
                    }

                    ZipArchiveEntry readmeEntry = archive.CreateEntry(newPath);
                    using (StreamWriter writer = new StreamWriter(readmeEntry.Open()))
                    {
                        writer.Write(content);
                    }
                }
            }

            //string newPath = file.Replace(
            //        @"Data\Original\",
            //        @"ArabicInterpretation\ArabicInterpretation\Core\Resources\");

            //newPath = Path.ChangeExtension(newPath, ".txt");
            //Directory.CreateDirectory(Path.GetDirectoryName(newPath));
            //File.WriteAllText(newPath, formattedPage, Encoding.UTF8);
        }
    }
}
