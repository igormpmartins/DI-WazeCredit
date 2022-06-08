using System;

namespace WazeCredit.Data.Repository.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        ICreditApplicationRepository CreditApplication { get; }
        void Save();
    }
}
