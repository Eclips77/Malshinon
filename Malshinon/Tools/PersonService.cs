using Malshinon.Dals;
using Malshinon.entityes;
using System;

namespace Malshinon.Services
{
    public class PersonService
    {
        //private readonly ValidateDal validator = new ValidateDal();
        private readonly PersonDal PersonDal = new PersonDal();

        public int GetIdB(string firstName)
        {
            return PersonDal.GetIdByName(firstName);
        }

        //public void CreateReporter(string firstName, string lastName)
        //{

        //     dal.setPersonToDb(firstName, lastName,);
        //}
        //public void CreateTarget(string firstName, string lastName)
        //{
        //    People newPerson = factory.CreateNewTarget(firstName, lastName);
        //    dal.setPersonToDb(newPerson);
        //}

        //public People GetPersonBy(string name)
        //{
        //    return validator.GetPersonByName(name);
        //}
    }
}
