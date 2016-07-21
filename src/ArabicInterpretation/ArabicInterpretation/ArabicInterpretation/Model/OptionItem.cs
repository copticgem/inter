using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabicInterpretation.Model
{
    class OptionItem
    {
        public OptionItem(string optionName, string displayName)
        {
            this.OptionName = optionName;
            this.DisplayName = displayName;
        }

        public string OptionName { get; set; }

        public string DisplayName { get; set; }
    }
}
