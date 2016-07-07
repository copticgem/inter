using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public static class FileHelper
    {
        private static string DefaultPrefix = "Core.Resources.";

        public static async Task<string> GetFile(
            Author author,
            bool isNT,
            int bookNumber,
            int chapterNumber)
        {
            string path = string.Empty;

            // Author
            if (author == Author.FrAntonios)
            {
                path += "Father_Antonious_Fekry.";
            }
            else if (author == Author.FrTadros)
            {
                path += "Father_Tadros_Yacoub_Malaty.";
            }
            else
            {
                throw new InvalidOperationException("Unexpected author: " + author);
            }

            // NT and OT
            if (isNT)
            {
                path += "nt.";
            }
            else
            {
                path += "ot.";
            }

            path = DefaultPrefix + path + "_" + bookNumber + "." + chapterNumber + ".txt";

            return await ReadFile(path);
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
    }
}
