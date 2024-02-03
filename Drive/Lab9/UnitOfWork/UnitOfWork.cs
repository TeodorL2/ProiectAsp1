using Drive.Data;
using Drive.Data.Models;
using Drive.Repositories.BaseDirectoryRepository;
using Drive.Repositories.DirectoryDescRepository;
using Drive.Repositories.UserRepository;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Drive.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private DriveContext _driveContext;

        private UserRepository _userRepository;
        private BaseDirectoryRepository _baseDirectoryRepository;
        private DirectoryDescRepository _directoryDescRepository;

        public UnitOfWork(DriveContext driveContext)
        {
            _driveContext = driveContext;
        }

        public UserRepository UserRepository
        {
            get
            {
                if (_userRepository == null)
                {
                    _userRepository = new UserRepository(_driveContext);
                    return _userRepository;
                }
                else { return _userRepository; }
            }
        }

        public BaseDirectoryRepository BaseDirectoryRepository
        {
            get
            {
                if (_baseDirectoryRepository == null)
                {
                    _baseDirectoryRepository = new BaseDirectoryRepository(_driveContext);
                    return _baseDirectoryRepository;
                }
                else { return _baseDirectoryRepository; }
            }
        }

        public DirectoryDescRepository DirectoryDescRepository
        {
            get
            {
                if (_directoryDescRepository == null)
                {
                    _directoryDescRepository = new DirectoryDescRepository(_driveContext);
                    return _directoryDescRepository;
                }
                else { return _directoryDescRepository; }
            }
        }

        public bool Save()
        {
            return _driveContext.SaveChanges() > 0;
        }
        public async Task<bool> SaveAsync()
        {
            //return await _driveContext.SaveChangesAsync() > 0;
            Console.WriteLine("before");
            var aux = await _driveContext.SaveChangesAsync();
            Console.WriteLine(aux);
            return aux > 0;
        }

        public void Dispose()
        {
            _driveContext.Dispose();
        }
    }
}
