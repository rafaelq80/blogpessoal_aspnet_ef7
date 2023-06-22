using blogpessoal.Models;

namespace blogpessoal.Services
{
    public interface IAppAuthService
    {
        Task<UserLogin?> Autenticar(UserLogin userLogin);
    }
}
