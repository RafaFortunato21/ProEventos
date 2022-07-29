using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProEventos.Application.Contratos;
using ProEventos.Domain;
using ProEventos.Persistence.Contratos;

namespace ProEventos.Application
{
    public class EventoService : IEventoService
    {
        private readonly IEventoPersist _eventoPersist;
        private readonly IGeralPersist _geralPersist;
        
        public EventoService(IEventoPersist eventoPersist, IGeralPersist geralPersist)
        {
            _geralPersist = geralPersist;
            _eventoPersist = eventoPersist;
            
        }


        public async Task<Evento> addEventos(Evento model)
        {
            try
            {
                _geralPersist.Add<Evento>(model);
                if (await _geralPersist.SaveChangesAsync())
                    return await _eventoPersist.GetAllEventoByIdAsync(model.Id,false);
                
                 return null;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Evento> UpdateEventos(int eventoId, Evento model)
        {
            try
            {
                 var evento = await _eventoPersist.GetAllEventoByIdAsync(eventoId,false);

                 if (evento == null ) return null;

                model.Id = evento.Id;
                 
                 _geralPersist.Update<Evento>(model);


                 if (await _geralPersist.SaveChangesAsync())
                 {
                     return await _eventoPersist.GetAllEventoByIdAsync(model.Id, false);
                 }
                 
                 return null;
            }
            catch (Exception ex)
            {
                
                throw new Exception (ex.Message);
            }
        }
    

        public async Task<bool> DeleteEvento(int eventoId)
        {
            try
            {
                 var evento = await _eventoPersist.GetAllEventoByIdAsync(eventoId,false);

                 if (evento == null ) throw new Exception("Evento n�o encontrado para delete.");
                    
                 _geralPersist.Delete<Evento>(evento);

                 return await _geralPersist.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception (ex.Message);
            }
      
        }

        public async Task<Evento> GetAllEventoByIdAsync(int eventoId, bool includePalestrantes)
        {
            try
            {
                  var evento = _eventoPersist.GetAllEventoByIdAsync(eventoId,includePalestrantes);
                  if (evento == null) return null;

                  return await evento;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
 
        public async Task<Evento[]> GetAllEventosAsync(bool includePalestrantes)
        {
            try
            {
                var enventos = await _eventoPersist.GetAllEventosAsync(includePalestrantes);
                if (enventos == null) return null;

                return enventos;     
            }
            catch (Exception ex)
            {
                throw new Exception (ex.Message);
            }
        }

        public async Task<Evento[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes)
        {
            try
            {
                var enventos = await _eventoPersist.GetAllEventosByTemaAsync(tema, includePalestrantes);
                if (enventos == null) return null;

                return enventos;     
            }
            catch (Exception ex)
            {
                throw new Exception (ex.Message);
            }
        }

       
    }
}