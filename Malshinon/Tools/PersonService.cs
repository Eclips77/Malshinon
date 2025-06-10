using Malshinon.Dals;
using Malshinon.entityes;
using System;

namespace Malshinon.Services
{
    public class PersonService
    {
        private readonly ValidateDal validator = new ValidateDal();
        private readonly Dal dal = new Dal();

        public bool Exists(string firstName)
        {
            return validator.ExistsInDatabase(firstName);
        }

        public int GetIdB(string firstName)
        {
            return validator.GetIdByName(firstName);
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
