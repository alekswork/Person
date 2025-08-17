using System;
using System.Collections.Generic;
using System.Linq;

namespace Person.Infrastructure.Persistence.Repositories
{
    public class PersonRepository
    {
        private readonly string _connectionString;

        public PersonRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Add(Domain.Entities.Person person)
        {
            using (var db = new PersonContext(_connectionString))
            {
                db.Persons.Add(person);
                db.SaveChanges();
            }
        }

        public void Edit(Domain.Entities.Person updatedPerson)
        {
            using (var db = new PersonContext(_connectionString))
            {
                var person = db.Persons.FirstOrDefault(p => p.PersonalId == updatedPerson.PersonalId);
                if (person == null)
                    throw new ArgumentException("Пользователь не найден");

                person.LastName = updatedPerson.LastName;
                person.FirstName = updatedPerson.FirstName;
                person.Patronymic = updatedPerson.Patronymic;
                person.BirthDate = updatedPerson.BirthDate;
                person.Email = updatedPerson.Email;
                person.Phone = updatedPerson.Phone;

                db.SaveChanges();
            }
        }

        public void Delete(Guid personalId)
        {
            using (var db = new PersonContext(_connectionString))
            {
                var person = db.Persons.FirstOrDefault(p => p.PersonalId == personalId);
                if (person != null)
                {
                    db.Persons.Remove(person);
                    db.SaveChanges();
                }
                else
                    throw new ArgumentException("Пользователь не найден");
            }
        }

        public Domain.Entities.Person Get(Guid personalId)
        {
            using (var db = new PersonContext(_connectionString))
            {
                return db.Persons.FirstOrDefault(p => p.PersonalId == personalId);
            }
        }

        public List<Domain.Entities.Person> GetAll()
        {
            using (var db = new PersonContext(_connectionString))
            {
                return db.Persons.ToList();
            }
        }
    }
}

