using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ProEventos.Application.Contratos;
using ProEventos.Application.Dtos;
using ProEventos.Domain;
using ProEventos.Persistence.Contratos;

namespace ProEventos.Application
{
    public class EventoService : IEventoService
    {
        private readonly IEventoPersist _eventoPersist;
        private readonly IGeralPersist _geralPersist;
        private readonly IMapper _mapper;

        public EventoService(
            IEventoPersist eventoPersist, 
            IGeralPersist geralPersist,
            IMapper mapper)
        {
            _geralPersist = geralPersist;
            _mapper = mapper;
            _eventoPersist = eventoPersist;
            
        }


        public async Task<EventoDto> addEventos(EventoDto model)
        {
            
            try
            {
                var evento = _mapper.Map<Evento>(model);

                _geralPersist.Add<Evento>(evento);
                if (await _geralPersist.SaveChangesAsync()){
                
                    var resultado = await _eventoPersist.GetAllEventoByIdAsync(evento.Id,false);
                    return _mapper.Map<EventoDto>(resultado);
                }
                
                 return null;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<EventoDto> UpdateEventos(int eventoId, EventoDto model)
        {
             try
             {

                  var evento = await _eventoPersist.GetAllEventoByIdAsync(eventoId,false);

                 if (evento == null ) return null;
                model.Id = evento.Id;
               
                _mapper.Map(model,evento);   
                _geralPersist.Update<Evento>(evento);
                  
                 if (await _geralPersist.SaveChangesAsync())
                  {
                    var resultado = await _eventoPersist.GetAllEventoByIdAsync(model.Id, false);
                    return  _mapper.Map<EventoDto>(resultado);
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

        public async Task<EventoDto> GetAllEventoByIdAsync(int eventoId, bool includePalestrantes)
        {
            try
            {
                var evento = await _eventoPersist.GetAllEventoByIdAsync(eventoId,includePalestrantes);
                if (evento == null) return null;
                
                var resultado = _mapper.Map<EventoDto>(evento);

                
                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
 
        public async Task<EventoDto[]> GetAllEventosAsync(bool includePalestrantes)
        {
            try
            {
                var eventos = await _eventoPersist.GetAllEventosAsync(includePalestrantes);
                if (eventos == null) return null;

                var resultado = _mapper.Map<EventoDto[]>(eventos);

                
                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception (ex.Message);
            }
        }

        public async Task<EventoDto[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes)
        {
            try
            {
                var eventos = await _eventoPersist.GetAllEventosByTemaAsync(tema, includePalestrantes);
                if (eventos == null) return null;

                var resultado = _mapper.Map<EventoDto[]>(eventos);

                
                return resultado;  
            }
            catch (Exception ex)
            {
                throw new Exception (ex.Message);
            }
        }

       
    }
}