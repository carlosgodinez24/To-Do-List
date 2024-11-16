using ToDoList.API.Models;

namespace ToDoList.API.Services
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}
