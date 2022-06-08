using WazeCredit.Models;

namespace WazeCredit.Data.Repository.IRepository
{
    public interface ICreditApplicationRepository: IRepository<CreditApplication>
    {
        void Update(CreditApplication obj);
    }
}
