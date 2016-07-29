using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public static class FileHelper
    {
        private static string DefaultPrefix = "Core.Resources.";

        public static Task<string> GetFile(
            Author author,
            bool isNT,
            int bookNumber,
            int chapterNumber)
        {
            string path = string.Empty;

            // Author
            if (author == Author.FrAntonios)
            {
                path += @"Father-Antonious-Fekry\";
            }
            else if (author == Author.FrTadros)
            {
                path += @"Father-Tadros-Yacoub-Malaty\";
            }
            else
            {
                throw new InvalidOperationException("Unexpected author: " + author);
            }

            string fileName = chapterNumber.ToString();

            // NT and OT
            if (isNT)
            {
                path += "nt";
            }
            else
            {
                path += "ot";

                if (bookNumber == 21)
                {
                    // Psalm 119 has multiple parts
                    if (chapterNumber >= 119 && chapterNumber <= 140)
                    {
                        int partNumber = chapterNumber - 118;
                        fileName = 119 + "(" + partNumber + ")";
                    }
                    else if (chapterNumber > 140)
                    {
                        fileName = (chapterNumber - 21).ToString();
                    }
                }
            }

            path = path + @"\" + bookNumber + @"\" + fileName + ".txt";

            return ReadCompressedFile(path);
        }

        public static Task<string> GetIndex(bool isNT)
        {
            string fileName = isNT ? "nt" : "ot";
            string path = DefaultPrefix + fileName + ".txt";
            return ReadFile(path);
        }

        private static async Task<string> ReadFile(string path)
        {
            var assembly = typeof(FileHelper).GetTypeInfo().Assembly;
            using (Stream stream = assembly.GetManifestResourceStream(path))
            {
                if (stream == null)
                {
                    throw new InvalidOperationException("File not found: " + path);
                }

                using (StreamReader reader = new StreamReader(stream))
                {
                    return await reader.ReadToEndAsync();
                }
            }
        }

        private static async Task<string> ReadCompressedFile(string path)
        {
            string content = string.Empty;
            var assembly = typeof(FileHelper).GetTypeInfo().Assembly;
            using (Stream stream = assembly.GetManifestResourceStream(DefaultPrefix + "data.zip"))
            {
                if (stream == null)
                {
                    throw new InvalidOperationException("File not found: " + path);
                }

                using (ZipArchive archive = new ZipArchive(stream, ZipArchiveMode.Read))
                {
                    ZipArchiveEntry entry = archive.GetEntry(path);
                    if (entry == null)
                    {
                        throw new InvalidOperationException("Entry not found: " + path);
                    }

                    using (Stream entryStream = entry.Open())
                    {
                        using (StreamReader sr = new StreamReader(entryStream))
                        {
                            while (!sr.EndOfStream)
                            {
                                // This fails if it's async
                                content += sr.ReadLine();
                            }
                        }
                    }
                }
            }

            return content;
        }
    }
}
