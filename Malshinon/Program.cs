using Bogus;
using Malshinon.Dals;
using Malshinon.entityes;
using Malshinon.factory;
using Malshinon.menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Malshinon
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Factory f = new Factory();
            ValidateDal dod = new ValidateDal();
            //People p = f.CreateNewAgent();
            //dal.setPersonToDb(p);
            Console.WriteLine(dod.SearchExist("Leone"));
        }
    }
}
