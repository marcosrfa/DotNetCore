using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProAgil.Domain;

namespace ProAgil.Repository
{
    public class ProAgilRepositoty : iProAgilRepository
    {
        public readonly ProAgilContext _context;
        public ProAgilRepositoty(ProAgilContext contexto)
        {
            _context = contexto;
        }


        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }
        
        public void Updade<T>(T entity) where T : class
        {
            _context.Update(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }
        
        public async Task<bool> SaveChangesAsync()
        {
            return await(_context.SaveChangesAsync()) > 0;
        }


        public async Task<Evento[]> GetAllEventosAsync(bool includePalestrante = false)
        {
            IQueryable<Evento> query = _context.Eventos
            .Include(l => l.Lotes)
            .Include(r => r.RedesSociais);

            if(includePalestrante)
                query = query
                .Include(pe => pe.PalestrantesEventos)
                .ThenInclude(p => p.Palestrante);
            
            query = query.OrderByDescending(e => e.DataEvento);

            return await query.ToArrayAsync();
        }

        public async Task<Evento[]> GetAllEventosAsyncByTema(string tema, bool includePalestrante)
        {
            IQueryable<Evento> query = _context.Eventos
            .Include(l => l.Lotes)
            .Include(r => r.RedesSociais);

            if(includePalestrante)
                query = query
                .Include(pe => pe.PalestrantesEventos)
                .ThenInclude(p => p.Palestrante);
            
            query = query.OrderByDescending(e => e.DataEvento)
                    .Where(e => e.Tema.ToLower().Contains(tema.ToLower()));

            return await query.ToArrayAsync();
        }
        public async Task<Evento> GetEventoAsyncById(int eventoId, bool includePalestrante)
        {
            IQueryable<Evento> query = _context.Eventos
            .Include(l => l.Lotes)
            .Include(r => r.RedesSociais);

            if(includePalestrante)
                query = query
                .Include(pe => pe.PalestrantesEventos)
                .ThenInclude(p => p.Palestrante);
            
            query = query.OrderByDescending(e => e.DataEvento)
                    .Where(e => e.Id == eventoId);

            return await query.FirstOrDefaultAsync();
        }
        
        public async Task<Palestrante> GetPalestranteAsyncById(int palestranteId, bool includeEventos = false)
        {
            IQueryable<Palestrante> query = _context.Palestrante
            .Include(r => r.RedesSociais);

            if(includeEventos)
                query = query
                .Include(pe => pe.PalestrantesEventos)
                .ThenInclude(e => e.Evento);
            
            query = query.OrderBy(p => p.Nome)
                    .Where(p => p.Id == palestranteId);

            return await query.FirstOrDefaultAsync();
        }
        public async Task<Palestrante[]> GetAllPalestrantesAsyncByNome(string nome, bool includeEventos = false)
        {
           IQueryable<Palestrante> query = _context.Palestrante
            .Include(r => r.RedesSociais);

            if(includeEventos)
                query = query
                .Include(pe => pe.PalestrantesEventos)
                .ThenInclude(e => e.Evento);
            
            query = query.OrderBy(p => p.Nome)
                    .Where(p => p.Nome.ToLower().Contains(nome.ToLower()));

            return await query.ToArrayAsync();
        }

    }
}