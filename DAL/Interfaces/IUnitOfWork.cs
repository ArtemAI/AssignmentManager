using System;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IAssignmentRepository Assignments { get; }
        IProjectRepository Projects { get; }
        IUserRepository UserProfiles { get; }

        Task<int> SaveAsync();
    }
}
