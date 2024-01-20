using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventoCore.Entities
{
    [Table("Eventos")]
    public class Evento : EntidadeBase
    {
        public string? Titulo { get; set; }

        public string? Descricao { get; set; }

        [Required(ErrorMessage = "A data de início é obrigatória")]
        public DateTime Inicio { get; set; }

        [Required(ErrorMessage = "A data de fim é obrigatória")]
        public DateTime Fim { get; set; }
    }
}
