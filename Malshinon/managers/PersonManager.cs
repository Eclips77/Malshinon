using Malshinon.Dals;
using Malshinon.Generator;
using Malshinon.Services;
using Malshinon.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Malshinon.managers
{
    internal class PersonManager
    {
        private readonly PersonDal _personDal;
  

        public PersonManager(PersonDal personDal)
        {
            this._personDal = personDal;
        }

        public void HandleAddReporter()
        {
            Console.Write("enter your id to sign in:");

            PersonService.AddNewReporterToDb();
        }




    }
}
