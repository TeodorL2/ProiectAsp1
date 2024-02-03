using Drive.Data;

using Drive.Data.Models;
using Drive.Repositories.GenericRepository;
using Microsoft.EntityFrameworkCore;

namespace Drive.Repositories.DirectoryDescRepository
{
    public class DirectoryDescRepository : GenericRepository<DirectoryDesc>, IDirectoryDescRepository
    {
        public DirectoryDescRepository(DriveContext driveContext) : base(driveContext) { }

        public void UpdateDescription(DirectoryDesc directoryDesc, string desc)
        {
            directoryDesc.description = desc;
            Update(directoryDesc);
        }
    }
}
