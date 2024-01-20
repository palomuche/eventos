using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventoCore.Entities
{
    public class EntidadeBase
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Codigo { get; set; }

        public bool Excluido { get; set; } = false;
        public DateTime Inclusao { get; set; } = DateTime.Now;
        public DateTime Alteracao { get; set; } = DateTime.Now;

        [ForeignKey("UsuarioInclusao")]
        public Guid? UsuarioInclusaoId { get; set; }
        public virtual Usuario? UsuarioInclusao { get; set; }

        [ForeignKey("UsuarioAlteracao")]
        public Guid? UsuarioAlteracaoId { get; set; }
        public virtual Usuario? UsuarioAlteracao { get; set; }
    }
}
