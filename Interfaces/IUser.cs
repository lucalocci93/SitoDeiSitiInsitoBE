using DAL.Enums;
using Identity.DTOs;
using Identity.Models;

namespace Identity.Interfaces
{
    public interface IUser
    {
        public Task<Response<JWT>> GenerateToken(string username, string password);
        public Task<Response<List<User>>> GetAllUser();
        public Task<Response<User>> GetUser(Guid RowGuid);
        public Task<Response<User>> CreateUser(User user);
        public Task<Response<User>> UpdateUser(UserDbOperationEnum operation, User user);
    }
}
