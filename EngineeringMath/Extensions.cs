using EngineeringMath.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EngineeringMath
{
    public static class Extensions
    {
        public static string ToSuperScript(this double num)
        {
            string numStr = num.ToString();
            TryStringConvert(numStr, RegularToSuperScriptChar, out string result);
            return result;
        }


        public static bool TryToConvertFromSuperScript(this string str, out double result)
        {
            bool isSuccess = TryStringConvert(str, SuperScriptToRegularChar, out string temp);
            if (!isSuccess)
            {
                result = double.NaN;
                return isSuccess;
            }
            return double.TryParse(temp, out result);
        }

        public static string RaiseToTheNPower(this string str, double n)
        {
            if (n == 1) return str;

            StringBuilder builder = new StringBuilder(str.Length);
            bool noSuperScriptFound = true;

            for (int i = 0; i < str.Length; i++)
            {
                char curChar = str[i];
                if (SuperScriptToRegularChar.ContainsKey(curChar))
                {
                    noSuperScriptFound = false;
                    int j = i;
                    while (j < str.Length && SuperScriptToRegularChar.ContainsKey(str[j]) )
                    {
                        j++;
                    }
                    bool isSuccess = str.Substring(i, j - i).TryToConvertFromSuperScript(out double result);

                    builder.Append(isSuccess ? (result * n).ToSuperScript() : double.NaN.ToString());
                    i = j;
                }
                else
                {
                    builder.Append(curChar);
                }
            }
            if (noSuperScriptFound)
            {
                builder.Append(n.ToSuperScript());
            }
            return builder.ToString();
        }

        /// <summary>
        /// Trys to find the string in Library Resources else the passed string is returned
        /// </summary>
        /// <param name="units"></param>
        /// <param name="storedName"></param>
        /// <returns></returns>
        public static string TryToFindStringInLibraryResources(this string str)
        {
            string name = typeof(LibraryResources).GetProperty(str)?.GetMethod.Invoke(null, null) as string;
            name = string.IsNullOrEmpty(name) ? str : name;
            return name;
        }

        private static Dictionary<char, char> RegularToSuperScriptChar { get; } = new Dictionary<char, char>()
        {
            { '0', '\u2070'},
            { '1', '\u00B9'},
            { '2', '\u00B2'},
            { '3', '\u00B3'},
            { '4', '\u2074'},
            { '5', '\u2075'},
            { '6', '\u2076'},
            { '7', '\u2077'},
            { '8', '\u2078'},
            { '9', '\u2079'},
            { '.', '\u22C5'},
            { '-', '\u207B'},
        };

        private static Dictionary<char, char> _SuperScriptToRegularChar;
        private static Dictionary<char, char> SuperScriptToRegularChar
        {
            get
            {
                if(_SuperScriptToRegularChar == null)
                {
                    _SuperScriptToRegularChar = RegularToSuperScriptChar.ToDictionary(kp => kp.Value, kp => kp.Key);
                }
                return _SuperScriptToRegularChar;
            }

        } 

        private static bool TryStringConvert(string numStr, Dictionary<char, char> charMap, out string result)
        {
            StringBuilder builder = new StringBuilder(7);
            foreach (char c in numStr)
            {
                char uniChar;

                if (charMap.ContainsKey(c))
                {
                    uniChar = charMap[c];
                }
                else
                {
                    result = null;
                    return false;
                }

                builder.Append(uniChar);
            }
            result = builder.ToString();
            return true;
        }


    }
}
