using EventoCore.Context;
using EventoCore.Entities;
using EventoCore.Interfaces;
using EventoCore.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;

namespace EventoCore.Repositories
{
    public class EventoRepository : IEventoRepository
    {
        protected readonly ApplicationContext _dbContext;
        static readonly string apiUrl = "https://localhost:7252";

        public EventoRepository(ApplicationContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Evento> GetAll()
        {
            return _dbContext.Eventos.Where(w => !w.Excluido).AsEnumerable();
        }

        public IEnumerable<Evento> GetAll(string token)
        {
            try
            {
                var url = $"{apiUrl}/api/evento";

                // Cria uma instância do HttpClient
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    // Envia a requisição POST
                    HttpResponseMessage response = client.GetAsync(url).Result;

                    string responseBody = response.Content.ReadAsStringAsync().Result;

                    // Verifica se deu erro
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception();
                    }

                    var retorno = JsonConvert.DeserializeObject<IEnumerable<Evento>>(responseBody);

                    return retorno;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Evento GetById(Guid id)
        {
            return _dbContext.Eventos.SingleOrDefault(t => t.Id == id);
        }       

        public Evento CadastrarEvento(AutenticacaoViewModel model, string token)
        {
            try
            {
                var url = $"{apiUrl}/api/evento";

                // Cria uma instância do HttpClient
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    var evento = new Evento();

                    // Serializa o objeto para JSON
                    var json = JsonConvert.SerializeObject(evento);

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

                    return new Evento();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
