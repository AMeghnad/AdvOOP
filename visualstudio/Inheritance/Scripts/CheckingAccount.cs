using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inheritance
{
    class CheckingAccount : BankAccount
    {
        public float transtactionFee = 0.005f;
        public void Fee()
        {
            money -= money * transtactionFee;
        }
        public string GetFee()
        {
            return "Fee: " + (transtactionFee * 100).ToString() + "%";
        }

        public override float Withdraw(float amount)
        {
            amount = base.Withdraw(amount);
            Fee();
            return amount;
        }

        public override string GetStatement()
        {
            return base.GetStatement() + "\n" + "\t" + GetFee() + "\n";
        }
    }
}
