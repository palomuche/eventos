using Azure;
using EventoCore.Context;
using EventoCore.Entities;
using EventoCore.Interfaces;
using EventoCore.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventoCore.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        protected readonly ApplicationContext _dbContext;
        static readonly string apiUrl = "https://localhost:7252";

        public UsuarioRepository(ApplicationContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Usuario> GetAll()
        {
            return _dbContext.Usuarios.Where(w => !w.Excluido).AsEnumerable();
        }

        public Usuario GetById(Guid id)
        {
            return _dbContext.Usuarios.SingleOrDefault(t => t.Id == id);
        }

        public UsuarioLogadoViewModel RealizaLogin(AutenticacaoViewModel model)
        {
            try
            {
                var url = $"{apiUrl}/api/conta/login";

                // Cria uma instância do HttpClient
                using (var client = new HttpClient())
                {
                    // Serializa o objeto para JSON
                    var json = JsonConvert.SerializeObject(model);

                    // Cria um StringContent que contém o JSON
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    // Envia a requisição POST
                    HttpResponseMessage response = client.PostAsync(url, content).Result;

                    string responseBody = response.Content.ReadAsStringAsync().Result;

                    // Verifica se deu erro
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception();
                    }

                    UsuarioLogadoViewModel retorno = JsonConvert.DeserializeObject<UsuarioLogadoViewModel>(responseBody);

                    return retorno;
                }

            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
