using IbanNet;
using System;
using System.Collections.Generic;
using System.Text;

namespace IBANChecker.Utilities
{
    public class IbanValidator
    {
        public bool IsValidFormat(string Iban)
        {
            var parsedIban = ParseIban(Iban);
            if (parsedIban != null)
            {
                OutputIbanInFormats(parsedIban);
                var IsValid = ValidateIban(Iban);
                return IsValid;
            }
            return false;
        }

        private void OutputIbanInFormats(Iban parsedIban)
        {
            Console.Write("Electronic: ");
            Console.WriteLine(String.Format("{0:E}", parsedIban));
            Console.Write("Print: ");
            Console.WriteLine(String.Format("{0:P}", parsedIban));
            Console.Write("Obfuscated: ");
            Console.WriteLine(String.Format("{0:O}", parsedIban));
            Console.Write("Branch Identifier: ");
            Console.WriteLine(parsedIban.BankIdentifier.ToString());
            Console.Write("Country: ");
            Console.WriteLine(parsedIban.Country.ToString());
            Console.Write("Bban: ");
            Console.WriteLine(parsedIban.Bban.ToString());
            Console.Write("Branch Identifier: ");
            Console.WriteLine(parsedIban.BranchIdentifier.ToString());
        }

        private bool ValidateIban(string iban)
        {
            IIbanValidator validator = new IbanNet.IbanValidator();
            ValidationResult result = validator.Validate(iban);
            if (result.IsValid)
            {
                return true;
            }
            return false;
        }

        private Iban ParseIban(string iban)
        {
            try
            {
                IIbanParser parser = new IbanParser(new IbanNet.IbanValidator());
                Iban thisIban = parser.Parse(iban);
                return thisIban;
            }
            catch(IbanFormatException ex)
            {
                Console.WriteLine("Error parsing IBAN");
            }
            return null;
        }
    }
}
