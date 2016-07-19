using ArabicInterpretation.Model;
using Core;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ArabicInterpretation.Helpers
{
    class ReadingPositionManager
    {
        public static Task SaveLastPosition(ReadingInfo readingInfo, double x, double y)
        {
            string value = readingInfo.ToPositionString(x, y);
            Application.Current.Properties[Constants.Properties.LastPosition] = value;
            return Task.FromResult(true);
        }

        public static ReadingInfo GetLastPosition(out double x, out double y)
        {
            x = 0;
            y = 0;

            object lastPosition;
            if (Application.Current.Properties.TryGetValue(Constants.Properties.LastPosition, out lastPosition))
            {
                return ReadingInfo.FromPositionString(
                    position: (string)lastPosition,
                    x: out x,
                    y: out y);
            }

            // Nothing is saved, load default
            return new ReadingInfo(
                Author.FrTadros,
                false,
                1,
                1);
        }
    }
}
