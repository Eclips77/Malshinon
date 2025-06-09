using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Malshinon.Dals;
using Malshinon.menus;

namespace Malshinon
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Dal dal = new Dal();
            bool x = dal.SearchExist("fgll");
            Console.WriteLine(x);
        }
    }
}
