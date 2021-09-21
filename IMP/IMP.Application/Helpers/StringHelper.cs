using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IMP.Application.Helpers
{
    public static class StringHelper
    {
        public static string ConvertToUnSign(this string s)
        {
            Regex regex = new("\\p{IsCombiningDiacriticalMarks}+");
            string temp = s.Normalize(NormalizationForm.FormD);
            return regex.Replace(temp, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
        }

        public static string GetSnakeCaseName(this string field)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(char.ToLower(field[0]));

            for (int i = 1; i < field.Length; i++)
            {
                if (char.IsUpper(field[i]))
                {
                    builder.Append('_');
                }
                builder.Append(char.ToLower(field[i]));
            }

            return builder.ToString();
        }

        /// <summary>
        /// Generate a random string with given size
        /// </summary>
        /// <param name="size"></param>
        /// <param name="lowerCase"></param>
        /// <returns></returns>
        public static string RandomString(int size, bool lowerCase = false)
        {
            var builder = new StringBuilder(size);

            // Unicode/ASCII Letters are divided into two blocks
            // (Letters 65–90 / 97–122):   
            // The first group containing the uppercase letters and
            // the second group containing the lowercase.  

            // char is a single Unicode character  
            char offset = lowerCase ? 'a' : 'A';
            const int lettersOffset = 26; // A...Z or a..z: length = 26  
            var random = new Random();
            for (var i = 0; i < size; i++)
            {
                var @char = (char)random.Next(offset, offset + lettersOffset);
                builder.Append(@char);
            }

            return lowerCase ? builder.ToString().ToLower() : builder.ToString();
        }
    }
}
