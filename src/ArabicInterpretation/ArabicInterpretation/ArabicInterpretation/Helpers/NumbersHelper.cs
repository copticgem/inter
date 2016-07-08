using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabicInterpretation.Helpers
{
    public static class NumbersHelper
    {
        public static string TranslateNumber(int number)
        {
            StringBuilder newNumber = new StringBuilder();

            string numberString = number.ToString();
            for (int i = 0; i < numberString.Length; i++)
            {
                switch (numberString[i])
                {
                    case '0':
                        newNumber.Append('٠');
                        break;
                    case '1':
                        newNumber.Append('١');
                        break;
                    case '2':
                        newNumber.Append('٢');
                        break;
                    case '3':
                        newNumber.Append('٣');
                        break;
                    case '4':
                        newNumber.Append('٤');
                        break;
                    case '5':
                        newNumber.Append('٥');
                        break;
                    case '6':
                        newNumber.Append('٦');
                        break;
                    case '7':
                        newNumber.Append('٧');
                        break;
                    case '8':
                        newNumber.Append('٨');
                        break;
                    case '9':
                        newNumber.Append('٩');
                        break;
                }
            }

            return newNumber.ToString();
        }
    }
}
