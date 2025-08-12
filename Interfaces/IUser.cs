using DAL.Enums;
using SitoDeiSiti.DAL.Models;
using SitoDeiSiti.DTOs;
using SitoDeiSiti.Models;

namespace SitoDeiSiti.Backend.Interfaces
{
    public interface IUser
    {
        public Task<Response<JWT>> GenerateToken(string username, string password);
        public Task<Response<List<User>>> GetAllUser();
        public Task<Response<User>> GetUser(Guid RowGuid);
        public Task<Response<User>> CreateUser(User user);
        public Task<Response<User>> UpdateUser(UserDbOperationEnum operation, User user);
        public Task<Response<List<Belts>>> GetCinture();
        public Task<Response<List<Organization>>> GetOrganizzazioni();
        public Task<Response<List<User>>> GetAtletiOrganizzazione(Guid Org);

    }
}
