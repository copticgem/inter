using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabicInterpretation.Model
{
    public class BookInfo
    {
        public BookInfo(string name, string shortName, int chaptersCount)
        {
            this.Name = name;
            this.ShortName = shortName;
            this.ChaptersCount = chaptersCount;
        }

        public string Name { get; private set; }

        public string ShortName { get; private set; }

        public int ChaptersCount { get; private set; }
    }
}
