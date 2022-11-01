using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IbanCheckerFramework
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var exit = false;
            while (!exit)
            {
                var IBAN = "";
                Console.Write("Please enter a IBAN:");
                IBAN = Console.ReadLine();
                Console.WriteLine("Processing...");
                CheckIBAN(IBAN);
            }
        }
        private static void CheckIBAN(string iBAN)
        {
            IbanCheckerFramework.IbanValidator validator = new IbanCheckerFramework.IbanValidator();
            var isValid = validator.IsValidFormat(iBAN);
            if (isValid)
            {
                Console.WriteLine("Valid IBAN");
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine("Invalid IBAN");
                Console.WriteLine();
            }
        }
    }
}
