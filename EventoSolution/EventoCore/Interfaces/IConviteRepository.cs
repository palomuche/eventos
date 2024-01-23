using EventoCore.Entities;
using EventoCore.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventoCore.Interfaces
{
    public interface IConviteRepository
    {
        IEnumerable<Convite> GetByUsuario(string token, Guid usuarioId);
        Convite GetById(string token, Guid id);
        RetornoViewModel ConfirmarConvite(string token, Guid id, bool confirmado);
        RetornoViewModel EnviarConvite(string token, Guid eventoId, Guid convidadoId);
    }
}
