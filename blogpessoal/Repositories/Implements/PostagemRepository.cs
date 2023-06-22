using blogpessoal.Models;
using blogpessoal.Data;
using Microsoft.EntityFrameworkCore;

namespace blogpessoal.Repositories.Implements
{
    public class PostagemRepository : IPostagemRepository
    {
        public readonly AppDbContext _context;

        public PostagemRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Postagem>> GetAll()
        {
            return await _context.Postagens
                .Include(p => p.Tema)
                .Include(u => u.User)
                .ToListAsync();
        }

        public async Task<Postagem?> GetById(long id)
        {
            try
            {
                var Postagem = await _context.Postagens
                    .Include(p => p.Tema)
                    .Include(u => u.User)
                    .FirstAsync(i => i.Id == id);

                return Postagem;
            }
            catch
            {
                return null;
            }

        }

        public async Task<IEnumerable<Postagem>> GetByTitulo(string titulo)
        {
            var PostagemReturn = await _context.Postagens
                .Include(p => p.Tema)
                .Include(u => u.User)
                .Where(p => p.Titulo.ToLower().Contains(titulo.ToLower()))
                .ToListAsync();

            return PostagemReturn;
        }

        public async Task<Postagem?> Create(Postagem postagem)
        {

            if (postagem.Tema is not null)
            {
                var BuscaTema = await _context.Temas.FindAsync(postagem.Tema.Id);

                if (BuscaTema is null)
                    return null;
            }

            postagem.Tema = postagem.Tema is not null ? _context.Temas.FirstOrDefault(t => t.Id == postagem.Tema.Id) : null;
            postagem.User = postagem.User is not null ? _context.Users.FirstOrDefault(t => t.Id == postagem.User.Id) : null;

            await _context.Postagens.AddAsync(postagem);
            await _context.SaveChangesAsync();
            return postagem;

        }

        public async Task<Postagem?> Update(Postagem postagem)
        {

            var PostagemUpdate = await _context.Postagens.FindAsync(postagem.Id);

            if (PostagemUpdate is null)
                return null;

            if (postagem.Tema is not null)
            {
                var BuscaTema = await _context.Temas.FindAsync(postagem.Tema.Id);

                if (BuscaTema is null)
                    return null;
            }

            postagem.Tema = postagem.Tema is not null ? _context.Temas.FirstOrDefault(t => t.Id == postagem.Tema.Id) : null;
            postagem.User = postagem.User is not null ? _context.Users.FirstOrDefault(t => t.Id == postagem.User.Id) : null;

            _context.Entry(PostagemUpdate).State = EntityState.Detached;
            _context.Entry(postagem).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return postagem;

        }

        public async Task Delete(Postagem postagem)
        {

            _context.Postagens.Remove(postagem);
            await _context.SaveChangesAsync();

        }

    }
}
