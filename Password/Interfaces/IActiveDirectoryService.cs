using Password.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Password.Interfaces
{
    internal interface IActiveDirectoryService
    {
        Task<List<AdUser>> GetAllUsersAsync();

        Task ChangePasswordAsync(
            string distinguishedName,
            string newPassword);
    }
}
