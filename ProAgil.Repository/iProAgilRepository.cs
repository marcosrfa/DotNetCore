using System.Threading.Tasks;
using ProAgil.Domain;

namespace ProAgil.Repository
{
    public interface iProAgilRepository
    {
         void Add<T> (T entity) where T : class;
         void Updade<T> (T entity) where T : class;
         void Delete<T> (T entity) where T : class;
         Task<bool> SaveChangesAsync();

         //Eventos
         Task<Evento[]> GetAllEventosAsyncByTema(string tema, bool includePalestrante);
         Task<Evento[]> GetAllEventosAsync(bool includePalestrante);
         Task<Evento> GetEventoAsyncById(int EventoId, bool includePalestrante);

         //Participantes
         Task<Palestrante[]> GetAllPalestrantesAsyncByNome(string nome, bool includeEventos);
         Task<Palestrante> GetPalestranteAsyncById(int PalestranteId, bool includeEventos);
    }
}