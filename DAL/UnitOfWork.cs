using DAL.Interfaces;
using DAL.Repositories;
using System;
using System.Threading.Tasks;

namespace DAL
{
    /// <summary>
    /// Implementation of Unit of Work pattern that allows to manage database operations on entities as one transaction.
    /// </summary>
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly AssignmentManagerContext _context;
        private ProjectRepository _projects;
        private AssignmentRepository _assignments;
        private UserRepository _userProfiles;

        public UnitOfWork()
        {
            _context = new AssignmentManagerContext();
        }

        public IProjectRepository Projects
        {
            get
            {
                return _projects = _projects ??= new ProjectRepository(_context);
            }
        }

        public IAssignmentRepository Assignments
        {
            get
            {
                return _assignments = _assignments ??= new AssignmentRepository(_context);
            }
        }

        public IUserRepository UserProfiles
        {
            get
            {
                return _userProfiles = _userProfiles ??= new UserRepository(_context);
            }
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        #region IDisposable Support
        private bool _disposedValue = false; 

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                _disposedValue = true;
            }
        }
        
        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
