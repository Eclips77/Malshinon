using System;

namespace Malshinon.Tools
{
    internal class Validator
    {
        public bool ValidateCapitalLetter(string txt)
        {
            string[] words = txt.Split(' ');
            for (int i = 0; i < words.Length - 1; i++)
            {
                if (char.IsUpper(words[i][0]) && char.IsUpper(words[i + 1][0]))
                {
                    return true;
                }
            }
            Console.WriteLine("Could not find valid first and last names with capital letters.");
            return false;
        }

        public bool ValidateReportText(string txt)
        {
            return !string.IsNullOrWhiteSpace(txt) && txt.Length >= 10;
        }
    }
}
