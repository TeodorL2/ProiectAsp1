using Drive.Data;
using Drive.Repositories.BaseDirectoryRepository;
using Drive.Repositories.UserRepository;
using Drive.Services.FileSystemService;

namespace Drive.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private DriveContext _driveContext;

        private IUserRepository _userRepository;
        private IBaseDirectoryRepository _baseDirectoryRepository;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IFileSystemService _fileSystemService;

        public UnitOfWork(DriveContext driveContext, IWebHostEnvironment hostEnvironment, IFileSystemService fileSystemService)
        {
            _driveContext = driveContext;
            _hostEnvironment = hostEnvironment;
            _fileSystemService = fileSystemService;
        }

        public IUserRepository UserRepository
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

        public IBaseDirectoryRepository BaseDirectoryRepository
        {
            get
            {
                if (_baseDirectoryRepository == null)
                {
                    _baseDirectoryRepository = new BaseDirectoryRepository(_driveContext, _fileSystemService);
                    return _baseDirectoryRepository;
                }
                else { return _baseDirectoryRepository; }
            }
        }

        public bool Save()
        {
            return _driveContext.SaveChanges() > 0;
        }
        public async Task<bool> SaveAsync()
        {
            return await _driveContext.SaveChangesAsync() > 0;
            //Console.WriteLine("before");
            //var aux = await _driveContext.SaveChangesAsync();
            //Console.WriteLine(aux);
            //return aux > 0;
        }

        public void Dispose()
        {
            _driveContext.Dispose();
        }
    }
}
