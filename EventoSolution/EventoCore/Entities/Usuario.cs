using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventoCore.Entities
{
    [Table("Usuarios")]
    public class Usuario : IdentityUser<Guid>
    {
        public string? Nome { get; set; }
        public bool Excluido { get; set; } = false;
        public DateTime Inclusao { get; set; } = DateTime.Now;
        public DateTime Alteracao { get; set; } = DateTime.Now;
    }
}
