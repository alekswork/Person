using Person.Infrastructure.Persistence.Repositories;
using System;
using System.Configuration;
using System.Windows.Forms;

namespace Person
{
    internal static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm(InitPersonRepository()));
        }

        static PersonRepository InitPersonRepository()
        {
            var connStr = ConfigurationManager.ConnectionStrings["PersonDB"].ConnectionString;
            return new PersonRepository(connStr);
        }
    }
}
