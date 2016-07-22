using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ArabicInterpretation.Model
{
    public class ReadingColor
    {
        public ReadingColor(Color textColor, Color backgroundColor)
        {
            this.TextColor = textColor;
            this.BackgroundColor = backgroundColor;
        }

        public Color TextColor { get; private set; }
        public Color BackgroundColor { get; private set; }
    }
}
