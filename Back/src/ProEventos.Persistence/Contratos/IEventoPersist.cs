using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProEventos.Domain;

namespace ProEventos.Persistence.Contratos
{
    public interface IEventoPersist
    {
        //Eventos
        Task<Evento[]> GetAllEventosAsync(int userId, bool includePalestrantes);
        Task<Evento[]> GetAllEventosByTemaAsync(int userId, string tema, bool includePalestrantes);
        Task<Evento> GetAllEventoByIdAsync(int userId, int EventoId, bool includePalestrantes);
    }
}