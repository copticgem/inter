using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabicInterpretation.Helpers
{
    public class PageData
    {
        public PageData()
        {
            this.Data = new Dictionary<string, string>();
        }

        public Dictionary<string, string> Data { get; private set; }
    }
}
