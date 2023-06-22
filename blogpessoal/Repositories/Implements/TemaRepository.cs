using blogpessoal.Models;
using blogpessoal.Data;
using Microsoft.EntityFrameworkCore;

namespace blogpessoal.Repositories.Implements
{
    public class TemaRepository : ITemaRepository
    {
        public readonly AppDbContext _context;
        
        public TemaRepository(AppDbContext context)
        {
            _context = context;

        }

        public async Task<IEnumerable<Tema>> GetAll()
        {
            return await _context.Temas
                .Include(t => t.Postagem)
                .ToListAsync();
        }

        public async Task<Tema?> GetById(long id)
        {
            try
            {
                var Tema = await _context.Temas
                    .Include(t => t.Postagem)
                    .FirstAsync(i => i.Id == id);

                return Tema;
            }
            catch
            {
                return null;
            }

        }

        public async Task<IEnumerable<Tema>> GetByDescricao(string descricao)
        {
            var TemaReturn = await _context.Temas
                .Include(t => t.Postagem)
                .Where(t => t.Descricao.ToLower().Contains(descricao.ToLower()))
                .ToListAsync();

            return TemaReturn;
        }

        public async Task<Tema> Create(Tema tema)
        {
            _context.Temas.Add(tema);
            await _context.SaveChangesAsync();

            return tema;
        }

        public async Task<Tema?> Update(Tema tema)
        {

            var TemaUpdate = await _context.Temas.FindAsync(tema.Id);

            if (TemaUpdate == null)
                return null;
                

            _context.Entry(TemaUpdate).State = EntityState.Detached;
            _context.Entry(tema).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return tema;
        }

        public async Task Delete(Tema tema)
        {

            _context.Temas.Remove(tema);
            await _context.SaveChangesAsync();

        }

    }
}
