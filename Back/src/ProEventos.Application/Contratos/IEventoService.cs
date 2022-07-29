using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProEventos.Domain;

namespace ProEventos.Application.Contratos
{
    public interface IEventoService
    {
        Task<Evento>  addEventos (Evento model);
        Task<Evento> UpdateEventos (int eventoId, Evento model);
        Task<bool> DeleteEvento (int eventoId);

        Task<Evento[]> GetAllEventosAsync( bool includePalestrantes);
        Task<Evento[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes);
        Task<Evento> GetAllEventoByIdAsync(int eventoId, bool includePalestrantes);
    }
}