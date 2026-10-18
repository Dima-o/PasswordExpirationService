using Password.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Password.Interfaces
{
    internal interface IPasswordService
    {
        Task ProcessUsersAsync(
            List<AdUser> users);
    }
}
