using System;
using System.Text.RegularExpressions;

namespace Malshinon.Tools
{
    public static class TextParser
    {
        public static (string fname, string lname) ParseNames(string text)
        {
            var match = Regex.Match(text, @"\b([A-Zא-ת][a-zא-ת]+)\s+([A-Zא-ת][a-zא-ת]+)\b");
            string firstName = "";
            string lastName = "";
            if (match.Success)
            {
                firstName = match.Groups[1].Value;
                lastName = match.Groups[2].Value;
            }
            return (firstName, lastName);
        }
    }
}
