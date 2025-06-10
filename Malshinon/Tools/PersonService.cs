using Malshinon.Dals;
using Malshinon.entityes;
using Malshinon.factory;
using System;

namespace Malshinon.Services
{
    public class PersonService
    {
        private readonly ValidateDal validator = new ValidateDal();
        private readonly Dal dal = new Dal();
        private readonly Factory factory = new Factory();

        public bool Exists(string firstName)
        {
            return validator.SearchExist(firstName);
        }

        public int GetId(string firstName)
        {
            return validator.GetIdByName(firstName);
        }

        public void CreateReporter(string firstName, string lastName)
        {
            People newPerson = factory.CreateNewReporter(firstName, lastName);
             dal.setPersonToDb(newPerson);
        }
        public void CreateTarget(string firstName, string lastName)
        {
            People newPerson = factory.CreateNewTarget(firstName, lastName);
            dal.setPersonToDb(newPerson);
        }

        //public People GetPersonBy(string name)
        //{
        //    return validator.GetPersonByName(name);
        //}
    }
}
