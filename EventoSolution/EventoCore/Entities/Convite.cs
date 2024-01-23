using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventoCore.Entities
{
    [Table("Convites")]
    public class Convite : EntidadeBase
    {
        public StatusConvite Status { get; set; }

        [ForeignKey("ConvidadoId")]
        public Guid ConvidadoId { get; set; }
        public virtual Usuario? Convidado { get; set; }

        [ForeignKey("EventoId")]
        public Guid EventoId { get; set; }
        public virtual Evento? Evento { get; set; }

        public enum StatusConvite
        {
            Pendente = 0,
            Aceito = 1,
            Recusado = 2,
        }
    }
}
