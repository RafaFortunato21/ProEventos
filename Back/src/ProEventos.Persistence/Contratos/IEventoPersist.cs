using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProEventos.Domain;
using ProEventos.Persistence.Models;

namespace ProEventos.Persistence.Contratos
{
    public interface IEventoPersist
    {
        //Eventos
        Task<PageList<Evento>> GetAllEventosAsync(int userId, PageParams pageParams, bool includePalestrantes);
        Task<Evento> GetAllEventoByIdAsync(int userId, int EventoId, bool includePalestrantes);
    }
}