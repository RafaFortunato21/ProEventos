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
    public class PalestranteService : IPalestranteService
    {
        private readonly IPalestrantePersist _palestrantePersist;
        private readonly IMapper _mapper;

        public PalestranteService(
            IPalestrantePersist palestrantePersist, 
            IMapper mapper)
        {
            _mapper = mapper;
            _palestrantePersist = palestrantePersist;
            
        }


        public async Task<PalestranteDto> AddPalestrantes(int userId, PalestranteAddDto model)
        {
            
            try
            {
                var palestrante = _mapper.Map<Palestrante>(model);
                palestrante.UserId = userId;

                _palestrantePersist.Add<Palestrante>(palestrante);
                if (await _palestrantePersist.SaveChangesAsync()){
                
                    var resultado = await _palestrantePersist.GetPalestranteByUserIdAsync(userId, false);
                    return _mapper.Map<PalestranteDto>(resultado);
                }
                
                 return null;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PalestranteDto> UpdatePalestrantes(int userId, PalestranteUpdateDto model)
        {
             try
             {

                var palestrante = await _palestrantePersist.GetPalestranteByUserIdAsync(userId, false);

                if (palestrante == null ) return null;
                
                model.Id = palestrante.Id;
                model.UserId = userId;
                
               
                _mapper.Map(model,palestrante);   
                _palestrantePersist.Update<Palestrante>(palestrante);
                  
                 if (await _palestrantePersist.SaveChangesAsync())
                  {
                    var resultado = await _palestrantePersist.GetPalestranteByUserIdAsync(userId, false);
                    return  _mapper.Map<PalestranteDto>(resultado);
                  }
               
                  return null;
             }
             catch (Exception ex)
             {
                 throw new Exception (ex.Message);
             }
        }
    

        public async Task<PalestranteDto> GetPalestranteByUserIdAsync(int userId,  bool includePalestrantes = false)
        {
            try
            {
                var palestrante = await _palestrantePersist.GetPalestranteByUserIdAsync(userId, includePalestrantes);
                if (palestrante == null) return null;
                
                var resultado = _mapper.Map<PalestranteDto>(palestrante);

                
                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
 
        public async Task<PageList<PalestranteDto>> GetAllPalestrantesAsync(PageParams pageParams, bool includePalestrantes = false)
        {
            try
            {
                var palestrantes = await _palestrantePersist.GetAllPalestranteAsync(pageParams, includePalestrantes);
                if (palestrantes == null) return null;

                var resultado = _mapper.Map<PageList<PalestranteDto>>(palestrantes);

                
                resultado.CurrentPage = palestrantes.CurrentPage;
                resultado.TotalCount = palestrantes.TotalCount; 
                resultado.TotalPages = palestrantes.TotalPages; 
                resultado.PageSize = palestrantes.PageSize; 
            
                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception (ex.Message);
            }
        }

       

       
    }
}