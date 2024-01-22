using EventoCore.Context;
using EventoCore.Entities;
using EventoCore.Interfaces;
using EventoCore.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public IEnumerable<Evento> GetByUsuario(string token, Guid usuarioId)
        {
            try
            {
                var url = $"{apiUrl}/api/evento/meus-eventos/{usuarioId}";

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

        public Evento GetById(string token, Guid id)
        {
            try
            {
                var url = $"{apiUrl}/api/evento/{id}";

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

                    var retorno = JsonConvert.DeserializeObject<Evento>(responseBody);

                    return retorno;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }       

        public RetornoViewModel CadastrarEvento(CadastroEventoViewModel model, string token)
        {
            try
            {
                var url = $"{apiUrl}/api/evento";

                // Cria uma instância do HttpClient
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    var evento = new Evento()
                    {
                        Titulo = model.Titulo,
                        Descricao = model.Descricao,
                        Inicio = model.Inicio,
                        Fim = model.Fim,
                        UsuarioInclusaoId = model.UsuarioId, 
                    };

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
                        // Convertendo a string JSON em JObject
                        JObject jsonResponse = JObject.Parse(responseBody);

                        // Acessando a propriedade 'title'
                        string title = jsonResponse["title"]?.ToString();
                        throw new Exception(title);
                    }

                    return new RetornoViewModel()
                    {
                        Mensagem = "Sucesso",
                        Sucesso = true
                    };
                }
            }
            catch (Exception)
            {
                throw;
            }
        }



        public RetornoViewModel EditarEvento(CadastroEventoViewModel model, string token)
        {
            try
            {
                var url = $"{apiUrl}/api/evento/{model.Id}";

                // Cria uma instância do HttpClient
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    // Serializa o objeto para JSON
                    var json = JsonConvert.SerializeObject(model);

                    // Cria um StringContent que contém o JSON
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    // Envia a requisição POST
                    HttpResponseMessage response = client.PutAsync(url, content).Result;

                    string responseBody = response.Content.ReadAsStringAsync().Result;

                    // Verifica se deu erro
                    if (!response.IsSuccessStatusCode)
                    {
                        // Convertendo a string JSON em JObject
                        JObject jsonResponse = JObject.Parse(responseBody);

                        // Acessando a propriedade 'title'
                        string title = jsonResponse["title"]?.ToString();
                        throw new Exception(title);
                    }

                    var retorno = JsonConvert.DeserializeObject<ActionResult<Evento>>(responseBody);

                    return new RetornoViewModel()
                    {
                        Mensagem = "Sucesso",
                        Sucesso = true
                    };
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        public RetornoViewModel ExcluirEvento(string token, Guid id)
        {
            try
            {
                var url = $"{apiUrl}/api/evento/{id}";

                // Cria uma instância do HttpClient
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    // Envia a requisição POST
                    HttpResponseMessage response = client.DeleteAsync(url).Result;

                    string responseBody = response.Content.ReadAsStringAsync().Result;

                    // Verifica se deu erro
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception();
                    }


                    return new RetornoViewModel()
                    {
                        Sucesso = true,
                        Mensagem = "Exluído com sucesso!"
                    };
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
