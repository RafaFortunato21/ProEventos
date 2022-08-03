using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProEventos.Application.Dtos;

namespace ProEventos.Application.Contratos
{
    public interface IEventoService
    {
        Task<EventoDto>  addEventos (EventoDto model);
        Task<EventoDto> UpdateEventos (int eventoId, EventoDto model);
        Task<bool> DeleteEvento (int eventoId);

        Task<EventoDto[]> GetAllEventosAsync( bool includePalestrantes);
        Task<EventoDto[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes);
        Task<EventoDto> GetAllEventoByIdAsync(int eventoId, bool includePalestrantes);
    }
}