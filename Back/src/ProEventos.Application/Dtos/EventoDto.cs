using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProEventos.Application.Dtos
{
    public class EventoDto
    {
        public int Id { get; set; }
        public string Local { get; set; }
        public string DataEvento { get; set; }
        
        [Required(ErrorMessage ="O campo {0} é obrigatório."),
        StringLength(50, MinimumLength = 3,
            ErrorMessage = "Intervalo permitido de 3 a 50 caracteres"
        ),
        ]
        public string Tema { get; set; }

        
        [Display(Name = "Qtd Pessoas")]
        [Required(ErrorMessage ="O campo {0} é obrigatório.")]
        [Range(1, 120000, ErrorMessage ="{0} não pode ser menor que 1 e maior que 120.000")]        
        public int QtdPessoas { get; set; }

        [RegularExpression(@".*\.(gif|jpe?g|bmp|png)$",
            ErrorMessage ="Não é uma imagem válida. (gif, jpg, jpge, bmp ou png)")]
        public string ImageURL { get; set; }
        

        [Required(ErrorMessage ="O campo {0} é obrigatório."),
        Phone(ErrorMessage ="O campo {0} informado é inválido ")]
        public string Telefone { get; set; }

        [Display(Name = "e-mail")]
        [EmailAddress(ErrorMessage ="O campo {0} precisa ser um email Válido.")]
        public string Email { get; set; }

        public IEnumerable<LoteDto> Lotes { get; set; }
        public IEnumerable<RedeSocialDto> RedesSociais { get; set; }
        public IEnumerable<PalestranteDto> Palestrantes { get; set; }
    }
}