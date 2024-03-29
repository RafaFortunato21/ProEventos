﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProEventos.Application.Contratos;
using ProEventos.Application.Dtos;
using ProEventos.API.Extensions;

namespace ProEventos.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RedesSociaisController : ControllerBase
    {
        private readonly IRedeSocialService _redeSocialService;
        private readonly IEventoService _eventoService;
        private readonly IPalestranteService _palestranteService;

        public RedesSociaisController(IRedeSocialService redeSocialService, 
                                      IEventoService eventoService,
                                      IPalestranteService palestranteService)
        {
            _redeSocialService = redeSocialService;
            _eventoService = eventoService;  
            _palestranteService = palestranteService;
        }


        [HttpGet("evento/{eventoId}")]
        public async Task<IActionResult> GetByEvento(int eventoId)
        {
            try
            {
                if (! (await AutorEvento(eventoId)) )
                    return Unauthorized();

                 var redeSocial = await _redeSocialService.GetAllByEventoIdAsync(eventoId);
                 if (redeSocial == null) return NoContent();
                
                 return Ok(redeSocial);
                 
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar recuperar Rede Social por Evento. Erro: {ex.Message} ");
            }
        }

        [HttpGet("palestrante")]
        public async Task<IActionResult> GetByPalestrante()
        {
            try
            {

                 var palestrante = await _palestranteService.GetPalestranteByUserIdAsync(User.GetUserId());
                 if (palestrante == null) return Unauthorized();

                 var redeSocial = await _redeSocialService.GetAllByPalestranteIdAsync(palestrante.Id);
                 if (redeSocial == null) return NoContent();
                
                 return Ok(palestrante);
                 
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar recuperar Rede Social por Palestrante. Erro: {ex.Message} ");
            }
        }


        [HttpPut("evento/{eventoId}")]
        public async Task<IActionResult> SaveByEvento(int eventoId, RedeSocialDto[] models)
        {
            try
            {
                if (! (await AutorEvento(eventoId)) )
                    return Unauthorized();


                 var redeSocial = await _redeSocialService.SaveByEvento(eventoId, models);
                 if (redeSocial == null) return NoContent();

                 return Ok(redeSocial);
                 
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar salvar Rede Social por Evento, erro: {ex.Message} ");
            }
        }

         [HttpPut("palestrante")]
        public async Task<IActionResult> SaveByPalestrante( RedeSocialDto[] models)
        {
            try
            {
                var palestrante = await _palestranteService.GetPalestranteByUserIdAsync(User.GetUserId(), false);
                if (palestrante == null) return Unauthorized();

                
                 var redeSocial = await _redeSocialService.SaveByPalestrante(palestrante.Id, models);
                 if (redeSocial == null) return NoContent();

                 return Ok(redeSocial);
                 
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar salvar Rede Social por Palestrante, erro: {ex.Message} ");
            }
        }



        [HttpDelete("{eventoId}/{redeSocialId}")]
        public async Task<IActionResult> DeleteByEvento(int eventoId, int redeSocialId)
        {
            try
            {
                
                if (! (await AutorEvento(eventoId)) )
                    return Unauthorized();
                    

                var redeSocial = await _redeSocialService.GetRedeSocialEventoByIdsAsync(eventoId,redeSocialId);
                if (redeSocial == null ) return NoContent();


                return await _redeSocialService.DeleteByEvento(eventoId,redeSocial.Id) 
                    ? Ok( new { message = "Rede Social Deletada."})
                    : throw new Exception("Falha ao deletar uma rede social por evento.");
                 
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar deletar Lotes, erro: {ex.Message} ");
            }
        }


        [HttpDelete("palestrante/{redeSocialId}")]
        public async Task<IActionResult> DeleteByPalestrante(int redeSocialId)
        {
            try
            {
                var palestrante = await _palestranteService.GetPalestranteByUserIdAsync(User.GetUserId());
                if (palestrante == null ) return Unauthorized();


                var redeSocial = await _redeSocialService.GetRedeSocialPalestranteByIdsAsync(palestrante.Id,redeSocialId);
                if (redeSocial == null ) return NoContent();


                return await _redeSocialService.DeleteByPalestrante(palestrante.Id, redeSocial.Id) 
                    ? Ok( new { message = "Rede Social de palestrante Deletada."})
                    : throw new Exception("Falha ao deletar uma rede social por palestrante.");
                 
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar deletar Palestr, erro: {ex.Message} ");
            }
        }

        [NonAction]
        private async Task<bool> AutorEvento(int eventoId){
            var evento = await _eventoService.GetAllEventoByIdAsync(User.GetUserId(), eventoId, false);

            if (evento == null) return false;

            return true;


        }
    }
}
