using WazeCredit.Models;

namespace WazeCredit.Service
{
    public class CreditApprovedHigh : ICreditApproved
    {
        public double GetCreditApproved(CreditApplication creditApplication) =>
            creditApplication.Salary * 0.3;
    }
}
