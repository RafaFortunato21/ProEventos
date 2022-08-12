using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProEventos.Application.Contratos;
using ProEventos.Application.Dtos;

namespace ProEventos.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventosController : ControllerBase
    {
        
        
        private readonly IEventoService _eventoService;
        private readonly IWebHostEnvironment _hostEnvironment;

        public EventosController(IEventoService eventoService, IWebHostEnvironment hostEnvironment)
        {
            _eventoService = eventoService;
            _hostEnvironment = hostEnvironment;
        }


        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                 var eventos = await _eventoService.GetAllEventosAsync(true);
                 if (eventos == null) return NoContent();
                
                 return Ok(eventos);
                 
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar recuperar eventos. Erro: {ex.Message} ");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
           try
            {
                 var evento = await _eventoService.GetAllEventoByIdAsync(id, true);

                 if (evento == null) return NoContent();;

                 return Ok(evento);
                 
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar recuperar eventos. Erro: {ex.Message} ");
            }
        }

        [HttpGet("{tema}/tema")]
        public async Task<IActionResult> GetByTema(string tema)
        {
           try
            {
                 var evento = await _eventoService.GetAllEventosByTemaAsync(tema, true);

                 if (evento == null) return NoContent();;

                 return Ok(evento);
                 
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar recuperar eventos. Erro: {ex.Message} ");
            }
        }

        [HttpPost("upload-image/{eventoid}")]
        public async Task<IActionResult> UploadImage(int eventoId)
        {
            try
            {
                
                var evento = await _eventoService.GetAllEventoByIdAsync(eventoId, true);
                 if (evento == null) return NoContent();;;
                 
                var file = Request.Form.Files[0];

                if (file.Length > 0)
                {
                    DeleteImage(evento.ImageURL);
                    //evento.ImageURL = SaveImage(file);
                }
                var EventoRetorno = await _eventoService.UpdateEventos(eventoId, evento);

                 return Ok(EventoRetorno);
                 
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar gravar Evento, erro: {ex.Message} ");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(EventoDto model)
        {
            try
            {
                 var evento = await _eventoService.addEventos(model);

                 if (evento == null) return NoContent();;;
                 

                 return Ok(evento);
                 
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar gravar Evento, erro: {ex.Message} ");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, EventoDto model)
        {
            try
            {
                 var evento = await _eventoService.UpdateEventos(id, model);

                 if (evento == null) return NoContent();;;
                 

                 return Ok(evento);
                 
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar atualizar um Evento, erro: {ex.Message} ");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var evento = await _eventoService.GetAllEventoByIdAsync(id,true);

                if (evento == null) return NoContent();
                

                return await _eventoService.DeleteEvento(id) 
                    ? Ok( new { message = "Deletado"}) 
                    : throw new Exception("Ocorreu um erro não especifico ao deletar o Evento.");
                 
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar deletar um Evento, erro: {ex.Message} ");
            }
        }

        [NonAction]
        public void DeleteImage(string imageName) {
            
            var imagePath =  Path.Combine(_hostEnvironment.ContentRootPath, @"Resources/images", imageName);

            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }
        }
    }
}
