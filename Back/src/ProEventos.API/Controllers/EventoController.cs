using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProEventos.API.Models;

namespace ProEventos.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventoController : ControllerBase
    {
        public EventoController()
        {
            
        }

        public IEnumerable<Evento> _eventos = new Evento[] {
            new Evento() {
            EventoId = 1,
            Tema = "Angular e Dot net ",
            Local = "Belo Horizonte",
            Lote = "1° Lote",
            QtdPessoas = 250,
            DataEvento = DateTime.Now.AddDays(2).ToString(),
            ImageURL = "foto.png"
            },
            new Evento() {
            EventoId = 2,
            Tema = "Angular e suas Novidades ",
            Local = "Sao Paulo",
            Lote = "2° Lote",
            QtdPessoas = 350,
            DataEvento = DateTime.Now.AddDays(3).ToString(),
            ImageURL = "foto1.png"
            }
        };

        [HttpGet]
        public IEnumerable<Evento> Get()
        {
            return _eventos;
        }

        [HttpGet("{id}")]
        public IEnumerable<Evento> GetById(int id)
        {
            return _eventos.Where(x => x.EventoId == id);
        }

        [HttpPost]
        public string Post()
        {
            return  "Exemplo de Post";
        }

        [HttpPut("{id}")]
        public string Put(int id)
        {
            return  $"Exemplo de Put com id = {id}";
        }

        [HttpDelete("{id}")]
        public string Delete(int id)
        {
            return  $"Exemplo de Delete com id = {id}";
        }
    }
}
