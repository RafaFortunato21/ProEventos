using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProEventos.Domain;

namespace ProEventos.Persistence.Contratos
{
    public interface ILotePersist
    {
        
        /// <summary>
        /// Método get que retornará uma lista de lotes po eventoId.
        /// </summary>
        /// <param name="eventoId"></param>
        /// <returns>Array de Lotes</returns>
        Task<Lote[]> GetAllLotesByEventoIdAsync(int eventoId);
        
        /// <summary>
        /// Método get que retornará apenas 1 lote.
        /// </summary>
        /// <param name="EventoId">Codigo chave da tabela Evento</param>
        /// <param name="loteId">Codigo chave da tabela Lote</param>
        /// <returns>Apenas 1 lote</returns>
        Task<Lote> GetLoteByIdsAsync(int eventoId, int loteId);
    }
}