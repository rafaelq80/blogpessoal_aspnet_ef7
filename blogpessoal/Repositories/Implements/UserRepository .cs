using blogpessoal.Models;
using blogpessoal.Data;
using Microsoft.EntityFrameworkCore;

namespace blogpessoal.Repositories.Implements
{
    public class UserRepository : IUserRepository
    {
        public readonly AppDbContext _context;
        
        public UserRepository(AppDbContext context)
        {
            _context = context;

        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await _context.Users
                .Include(u => u.Postagem)
                .ToListAsync();
        }

        public async Task<User?> GetById(long id)
        {
            try
            {
                var User = await _context.Users
                    .Include(u => u.Postagem)
                    .FirstAsync(i => i.Id == id);

                User.Senha = "";

                return User;
            }
            catch
            {
                return null;
            }

        }

        public async Task<User?> GetByUsuario(string usuario)
        {
            try
            {
                var Usuario = await _context.Users
                    .Where(u => u.Usuario == usuario)
                    .FirstOrDefaultAsync();

                return Usuario;
            }
            catch
            {
                return null;
            }
        }

        public async Task<User?> Create(User usuario)
        {
            var BuscarUsuario = await GetByUsuario(usuario.Usuario);

            if (BuscarUsuario is not null)
                return null;

            if (usuario.Foto is null || usuario.Foto == "")
                usuario.Foto = "https://i.imgur.com/I8MfmC8.png";

            usuario.Senha = BCrypt.Net.BCrypt.HashPassword(usuario.Senha, workFactor: 10);

            _context.Users.Add(usuario);
            await _context.SaveChangesAsync();

            return usuario;
        }

        public async Task<User?> Update(User usuario)
        {

            var UsuarioUpdate = await _context.Users.FindAsync(usuario.Id);

            if (UsuarioUpdate is null)
                return null;

            if (usuario.Foto is null || usuario.Foto == "")
                usuario.Foto = "https://i.imgur.com/I8MfmC8.png";

            usuario.Senha = BCrypt.Net.BCrypt.HashPassword(usuario.Senha, workFactor: 10);

            _context.Entry(UsuarioUpdate).State = EntityState.Detached;
            _context.Entry(usuario).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return usuario;
        }

    }
}
