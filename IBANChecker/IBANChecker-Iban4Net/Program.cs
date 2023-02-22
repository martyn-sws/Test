using SinKien.IBAN4Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBANChecker_Iban4Net
{
    public class Program
    {
        static void Main(string[] args)
        {
            var exit = false;
            while (!exit)
            {
                var IBAN = "";
                var BIC = "";
                Console.Write("Please enter a IBAN:");
                IBAN = Console.ReadLine();
                Console.Write("Please enter a BIC:");
                BIC = Console.ReadLine();
                Console.WriteLine("Processing...");
                CheckIBAN(IBAN, BIC);
            }
        }
        private static void CheckIBAN(string iBAN, string bIC)
        {
            var isValidIban = CheckValidIban(iBAN);
            var isValidBic = CheckValidBic(bIC);
            if (isValidIban && isValidBic)
            {
                Console.WriteLine("Valid IBAN and BIC");
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine("Invalid");
                Console.WriteLine();
            }
            GetSortCodeFromIban(iBAN);
        }

        private static bool CheckValidBic(string bIC)
        {
            BicFormatViolation validationResult = BicFormatViolation.NO_VIOLATION;
            return BicUtils.IsValid(bIC, out validationResult);
        }

        private static bool CheckValidIban(string iBAN)
        {
            IbanFormatViolation validationResult = IbanFormatViolation.NO_VIOLATION;
            return IbanUtils.IsValid(iBAN, out validationResult);
        }
    }
}
