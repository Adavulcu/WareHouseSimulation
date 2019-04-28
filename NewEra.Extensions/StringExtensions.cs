using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace System
{
    public static class StringExtensions
    {
        public static Guid ToGuid(this string value)
        {          
            return new Guid(value);
        }
        public static bool CheckRegex(this string value,string pattern)
        {
            Regex regex = new Regex(pattern);
            return regex.Match(value).Success;
        }
        public static string ReplaceTurkishCharacters(this string value)
        {
            var oldValues = new char[] { 'Ç', 'ç', 'Ğ', 'ğ', 'İ', 'ı', 'Ö', 'ö', 'Ü', 'ü', 'Ş', 'ş' };
            var newValues = new char[] { 'C', 'c', 'G', 'g', 'I', 'i', 'O', 'o', 'U', 'u', 'S', 's' };
            for (int i = 0; i < oldValues.Length; i++)
            {
                value = value.Replace(oldValues[i], newValues[i]);
            }
            return value;
        }
        public static string TrimWhiteSpaceX(this string value)
        {
            return value.Replace(" ", "");
        }
        public static bool IsNullOrEmpty(this string value)
        {
            return String.IsNullOrEmpty(value);
        }
        public static bool IsNotNullOrEmpty(this string value)
        {
            return !String.IsNullOrEmpty(value);
        }
        public static string ToUpperCase(this string value)
        {

            char[] metin = value.ToLower().ToCharArray();
            metin[0] = Char.ToUpper(metin[0]);
            for (int i = 0; i < metin.Length - 1; i++)
            {
                if (Char.IsWhiteSpace(metin[i]))
                {
                    metin[i + 1] = Char.ToUpper(metin[i + 1]);
                }
            }
            string returnValue = new string(metin);
            return returnValue;
        }
        public static string ToUpperCaseX(this string value)
        {
            string returnValue = "";
            var values = value.ToSingleWhitespace().Split(' ');
            for (int i = 0; i < values.Length; i++)
            {
                char[] c = values[i].ToLower().ToCharArray();
                c[0] = Char.ToUpper(c[0]);
                returnValue += (returnValue.IsNotNullOrEmpty() ? " " : "") + new string(c);
            }
            return returnValue;
        }
        public static string ToSingleWhitespace(this string value)
        {
            var values = value.Split(' ');
            var returnValues = "";
            for (int i = 0; i < values.Length; i++)
            {
                if (values[i].IsNotNullOrEmpty())
                {
                    if (returnValues.IsNotNullOrEmpty())
                    {
                        returnValues += " ";
                    }
                    returnValues += values[i];
                }
            }
            return returnValues;
        }
        public static T EnumParse<T>(this string value)
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("Parameter is Not Enum");
            }
            return (T)Enum.Parse(typeof(T), value);
        }

        public static string RemoveWhitespaces(this string value)
        {
            var returnValue = "";
            for (int i = 0; i < value.Length; i++)
            {
                if (!Char.IsWhiteSpace(value[i]))
                {
                    returnValue += value[i];
                }
            }
            return returnValue;
        }

        public static string RemoveDigits(this string value)
        {
            var returnValue = "";
            for (int i = 0; i < value.Length; i++)
            {
                if (!Char.IsDigit(value[i]))
                {
                    returnValue += value[i];
                }
            }
            return returnValue;
        }

        public static string RemoveLetters(this string value)
        {
            var returnValue = "";
            for (int i = 0; i < value.Length; i++)
            {
                if (!Char.IsLetter(value[i]))
                {
                    returnValue += value[i];
                }
            }
            return returnValue;
        }

        public static string RemoveSpecialCharacters(this string value)
        {
            var returnValue = "";
            for (int i = 0; i < value.Length; i++)
            {
                if (Char.IsLetterOrDigit(value[i]))
                {
                    returnValue += value[i];
                }
            }
            return returnValue;
        }

        public static string Display(this string value, params object[] values)
        {
            return String.Format(value, values);
        }
        public static string DisplayCurrency(this string value)
        {
            return String.Format("{0:C2}", value.To<double>());
        }
        public static string DisplayCurrency(this string value, IFormatProvider provider)
        {
            return value.To<double>().ToString("C2", provider);
        }

        /// <summary>
        /// Labe Name özellğine atanan strin degeri ayıklayarak o labelin indis değerlerini string dizisi içerisinde döndürür
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string[] SplitLabelName(this string str)
        {
            char[] c = { 'l', 'b', '_' };
            string[] s = str.Split(c);
            string str1 = "";
            for (int i = 0; i < s.Length; i++)
            {
                str1 += s[i];

            }

            return s;
        }
    }
}

