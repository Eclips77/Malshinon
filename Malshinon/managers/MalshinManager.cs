using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Malshinon.Dals;

namespace Malshinon.managers
{
    internal class MalshinManager
    {
        Dal peopleDal = new Dal();
        public void ValidPeopleExists(string Fullname)
        {
            string[] Names = Fullname.Split(' ');
            bool exist = peopleDal.SearchExist(Names[0]);
            if (exist)
            {
                Console.WriteLine("you are in the db");
            }
            else
            {
                
            }
        }
        public void CreatePerson()
        {

        }

    }
}
