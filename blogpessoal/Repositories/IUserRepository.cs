using blogpessoal.Models;

namespace blogpessoal.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAll();

        Task<User?> GetById(long id);

        Task<User?> GetByUsuario(string usuario);

        Task<User?> Create(User user);

        Task<User?> Update(User user);

    }
}
