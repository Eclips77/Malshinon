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

        public int GetId(string firstName, string lastName)
        {
            return dal.GetPersonId(firstName, lastName);
        }

        public int CreateReporter(string firstName, string lastName)
        {
            People newPerson = factory.CreateNewReporter(firstName, lastName);
            return dal.InsertPersonAndGetId(newPerson);
        }

        public People GetPersonById(int id)
        {
            return dal.GetPersonById(id);
        }
    }
}
