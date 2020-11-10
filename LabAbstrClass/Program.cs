using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.DirectoryServices.AccountManagement;

namespace LabAbstrClass
{
    public abstract class Validator
    {
        public abstract bool IsPasswordValid(string password);
        public abstract string HashPassword(string password);
        public abstract bool IsUserExists(string login, string password);
        public abstract bool IsEmailValid(string email);
        public abstract bool IsPhoneValid(string phone);
        public abstract bool IsWebPageAvailable(string url);
        public abstract bool IsDatabaseAccessible(string connectionString);
        public abstract bool IsDateValid(string date);
        public abstract bool IsUserRoot();
        public abstract void Log();
    }

    class MyValidator : Validator
    {
        public override string HashPassword(string password)
        {
            uint hash = 0;
            foreach (var c in password)
                hash = ((hash << 5) + hash) + c;

            return hash.ToString();
        }

        public override bool IsDatabaseAccessible(string connectionString)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
                return true;
            }
            catch(SqlException)
            {
                return false;
            }
        }

        public override bool IsDateValid(string date)
        {
            throw new NotImplementedException();
        }

        public override bool IsEmailValid(string email)
        {
            throw new NotImplementedException();
        }

        public override bool IsPasswordValid(string password)
        {
            throw new NotImplementedException();
        }

        public override bool IsPhoneValid(string phone)
        {
            throw new NotImplementedException();
        }

        /* Что должен делать метод? */
        public override bool IsUserExists(string login, string password)
        {
            using(PrincipalContext principalContext = new PrincipalContext(ContextType.Machine))
            {
                return principalContext.ValidateCredentials(login, password);
            }
        }

        public override bool IsUserRoot()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        public override bool IsWebPageAvailable(string url)
        {
            WebClient webClient = new WebClient();
            try
            {
                webClient.OpenRead(url);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /* Что должен делать метод? */
        public override void Log()
        {
            throw new NotImplementedException();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            MyValidator validator = new MyValidator();
            Console.WriteLine(validator.IsUserExists("naught", ""));
            Console.ReadKey();
        }
    }
}
