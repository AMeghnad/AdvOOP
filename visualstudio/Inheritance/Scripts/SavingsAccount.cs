using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inheritance
{
    class SavingsAccount : BankAccount
    {
        public float interest = 0.04f;
        public void Credit()
        {
            money += money * interest;
        }
        public string GetInterest()
        {
            return "Interest:" + (interest * 100).ToString() + "%";
        }

        public override void Deposit(float amount)
        {
            base.Deposit(amount);
            // Credit every time you deposit! YAY!
            Credit();
        }
        public override string GetStatement()
        {
            return base.GetStatement() + "\n" + "\t" + GetInterest() + "\n";
        }
    }
}
