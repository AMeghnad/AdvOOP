using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inheritance
{
    class Program
    {
        static void Main(string[] args)
        {
            List<BankAccount> accounts = new List<BankAccount>();
            accounts.Add(new SavingsAccount());

            accounts[0].Deposit(100);

            foreach (var acc in accounts)
            {
                Console.WriteLine(acc.GetStatement());
            }
            Console.ReadLine();
        }
    }
}
