using System;
using System.Text.RegularExpressions;

namespace Malshinon.Tools
{
    public class TextParser
    {
        public (string, string)? ParseNames(string text)
        {
            var match = Regex.Match(text, @"\b([A-Zא-ת][a-zא-ת]+)\s+([A-Zא-ת][a-zא-ת]+)\b");

            if (match.Success)
            {
                string firstName = match.Groups[1].Value;
                string lastName = match.Groups[2].Value;
                return (firstName, lastName);
            }

            return null;
        }
    }
}
