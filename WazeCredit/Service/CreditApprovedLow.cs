using WazeCredit.Models;

namespace WazeCredit.Service
{
    public class CreditApprovedLow : ICreditApproved
    {
        public double GetCreditApproved(CreditApplication creditApplication) =>
            creditApplication.Salary * 0.5;
    }
}
