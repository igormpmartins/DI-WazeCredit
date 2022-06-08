using WazeCredit.Data.Repository.IRepository;
using WazeCredit.Models;

namespace WazeCredit.Data.Repository
{
    public class CreditApplicationRepository: Repository<CreditApplication>, 
        ICreditApplicationRepository
    {
        private readonly ApplicationDbContext db;

        public CreditApplicationRepository(ApplicationDbContext db): base(db)
        {
            this.db = db;
        }

        public void Update(CreditApplication obj)
        {
            db.CreditApplications.Update(obj);
        }
    }
}
