using System;

namespace Malshinon.Tools
{
    public static class Validator
    {
        public static bool ValidateCapitalLetter(string txt)
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

        public static bool ValidateReportText(string txt)
        {
            return !string.IsNullOrWhiteSpace(txt) && txt.Length >= 10;
        }
        public static string Prompt(string message)
        {
            Console.WriteLine(message);
            return Console.ReadLine()?.Trim() ?? string.Empty;
        }
        public  static string PromptName(string message)
        {
            string input;
            do
            {
                input = Prompt(message);
                if (string.IsNullOrWhiteSpace(input))
                    Console.WriteLine("name cannot be empty try again");
            } while (string.IsNullOrWhiteSpace(input));

            return CapitalizeFirstLetter(input);
        }
        public static string CapitalizeFirstLetter(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            input = input.ToLower();
            return char.ToUpper(input[0]) + input.Substring(1);
        }
    }
}
