using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Formatter.Formatters
{
    public static class BookNumbers
    {
        public static void Format()
        {
            string file = @"F:\git\inter\src\Data\Original\ot.txt";
            string[] lines = File.ReadAllLines(file);

            List<string> newLines = new List<string>();
            foreach (string line in lines)
            {
                string newLine = Regex.Replace(line, "(  )+", ":");
                newLine = NumbersFormatter.ReplaceNumbers(newLine);
                newLines.Add(newLine);
            }

            string newFile = @"F:\git\inter\src\ArabicInterpretation\ArabicInterpretation\Core\Resources\ot.txt";
            File.WriteAllLines(newFile, newLines.ToArray());
        }
    }
}
