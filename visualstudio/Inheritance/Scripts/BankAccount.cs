using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inheritance
{
    class BankAccount
    {
        public int accountNumber;
        protected float money;

        public BankAccount()
        {
            Random rand = new Random(DateTime.Now.Millisecond);
            accountNumber = rand.Next(100000000, 999999999);
        }

        public virtual float Withdraw(float amount)
        {
            // Error check            
            // If money > amount
            if (money > amount)
            {
                // reduce money
                money -= amount;
            }
            // else
            else
            {
                // print "You don't have the funds!"
                Console.WriteLine("You don't have the funds!");
                // return nothing
                return 0f;
            }



            return amount;
        }
        public virtual void Deposit(float amount)
        {
            money += amount;
        }

        // Forms a statement and return a string
        // containing said statement
        public virtual string GetStatement()
        {
            return GetAccountNo() + "\n" + "\t" + GetMoney() + "\n";
        }

        private string GetAccountNo()
        {
            return "Account No.:" + accountNumber.ToString();
        }
        private string GetMoney()
        {
            return "Money: $" + money.ToString();
        }
    }
}
