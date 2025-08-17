using System.Data.Entity;

namespace Person.Infrastructure.Persistence
{
    public class PersonContext : DbContext
    {
        public PersonContext() : base("name=PersonDB")
        {
        }

        public PersonContext(string connectionString) : base(connectionString)
        {
        }

        public DbSet<Domain.Entities.Person> Persons { get; set; }
    }
}

