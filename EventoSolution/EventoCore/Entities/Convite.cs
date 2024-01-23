using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.NetworkInformation;
using System.Reflection;

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
            [Description("Pendente")]
            Pendente = 0,
            [Description("Aceito")]
            Aceito = 1,
            [Description("Recusado")]
            Recusado = 2,
        }

        [NotMapped]
        public string StatusDescricao
        {
            get
            {
                var field = typeof(StatusConvite).GetField(Status.ToString());
                var attribute = (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));

                return attribute == null ? Status.ToString() : attribute.Description;
            }
        }
    }
}
