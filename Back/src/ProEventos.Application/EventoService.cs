using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ProEventos.Application.Contratos;
using ProEventos.Application.Dtos;
using ProEventos.Domain;
using ProEventos.Persistence.Contratos;
using ProEventos.Persistence.Models;

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


        public async Task<EventoDto> AddEventos(int userId, EventoDto model)
        {
            
            try
            {
                var evento = _mapper.Map<Evento>(model);
                evento.UserId = userId;

                _geralPersist.Add<Evento>(evento);
                if (await _geralPersist.SaveChangesAsync()){
                
                    var resultado = await _eventoPersist.GetAllEventoByIdAsync(userId, evento.Id,false);
                    return _mapper.Map<EventoDto>(resultado);
                }
                
                 return null;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<EventoDto> UpdateEventos(int userId, int eventoId, EventoDto model)
        {
             try
             {

                var evento = await _eventoPersist.GetAllEventoByIdAsync(userId, eventoId,false);

                if (evento == null ) return null;
                
                model.Id = evento.Id;
                model.UserId = userId;
                
               
                _mapper.Map(model,evento);   
                _geralPersist.Update<Evento>(evento);
                  
                 if (await _geralPersist.SaveChangesAsync())
                  {
                    var resultado = await _eventoPersist.GetAllEventoByIdAsync(userId, model.Id, false);
                    return  _mapper.Map<EventoDto>(resultado);
                  }
               
                  return null;
             }
             catch (Exception ex)
             {
                 throw new Exception (ex.Message);
             }
        }
    

        public async Task<bool> DeleteEvento(int userId, int eventoId)
        {
            try
            {
                 var evento = await _eventoPersist.GetAllEventoByIdAsync(userId, eventoId,false);

                 if (evento == null ) throw new Exception("Evento não encontrado para delete.");
                    
                 _geralPersist.Delete<Evento>(evento);

                 return await _geralPersist.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception (ex.Message);
            }
      
        }

        public async Task<EventoDto> GetAllEventoByIdAsync(int userId, int eventoId, bool includePalestrantes)
        {
            try
            {
                var evento = await _eventoPersist.GetAllEventoByIdAsync(userId, eventoId,includePalestrantes);
                if (evento == null) return null;
                
                var resultado = _mapper.Map<EventoDto>(evento);

                
                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
 
        public async Task<PageList<EventoDto>> GetAllEventosAsync(int userId, PageParams pageParams, bool includePalestrantes)
        {
            try
            {
                var eventos = await _eventoPersist.GetAllEventosAsync(userId, pageParams ,includePalestrantes);
                if (eventos == null) return null;

                var resultado = _mapper.Map<PageList<EventoDto>>(eventos);

                resultado.CurrentPage = eventos.CurrentPage;
                resultado.TotalCount = eventos.TotalCount; 
                resultado.TotalPages = eventos.TotalPages; 
                resultado.PageSize = eventos.PageSize; 
                
                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception (ex.Message);
            }
        }

       

       
    }
}