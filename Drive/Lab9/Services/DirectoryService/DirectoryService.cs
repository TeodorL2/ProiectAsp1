using Drive.Helpers.JwtUtil;
using Drive.UnitOfWork;
using Drive2.StorageManagement;

namespace Drive2.Services.DirectoryService
{
    public class DirectoryService: IDirectoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DirectoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public List<EntryStruct> GetEntries(string path)
        {
            Console.WriteLine("DirectoryService: " + path);
            return _unitOfWork.BaseDirectoryRepository.GetEntries(path);
        }
    }
}
