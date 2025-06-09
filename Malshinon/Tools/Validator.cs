using Malshinon.Dals;
using Malshinon.factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Malshinon.Tools
{
    internal class Validator
    {
        private readonly ValidateDal validator;
        private readonly Factory factory;

        public Validator()
        {
            this.validator = new ValidateDal();
            this.factory = new Factory();
        }
        public void newPersoncheck()
        {
            Console.WriteLine("enter your first name with capital first latter");
            string firstName = Console.ReadLine();
            Console.WriteLine("enter your first name with capital first latter");
            string lastName = Console.ReadLine();
            if (!validator.SearchExist(firstName))
            {
                factory.CreateNewAgent(firstName, lastName);
            }
        }
        public (string, string)? ValidateCapitalLetter(string txt)
        {
            string[] words = txt.Split(' ');
            for (int i = 0; i < words.Length - 1; i++)
            {
                if (char.IsUpper(words[i][0]) && char.IsUpper(words[i + 1][0]))
                {
                    string firstName = words[i];
                    string lastName = words[i + 1];
                    return (firstName, lastName);
                }
            }
            Console.WriteLine("Could not find valid first and last names with capital letters.");
            return null;
        }

    }
}
