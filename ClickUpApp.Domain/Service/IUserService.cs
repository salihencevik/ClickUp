using System.Linq.Expressions; 
using ClickUpApp.Domain.Entities;

namespace ClickUpApp.Domain.Service
{
    public interface IUserService
    { 
        Task<bool> InsertUser(User user);
        Task<User> FindByEmailAsync(string email);
        Task<User> CheckPasswordAsync(string email, string password);
        Task<User> FindUserByPradiceAsync(Expression<Func<User, bool>> predicate);
    }
}