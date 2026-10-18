using System.Threading.Tasks;
using Password.Models;

namespace Password.Interfaces
{
    internal interface IJsonService
    {
        Task SaveAsync(PasswordEvent passwordEvent);
    }
}