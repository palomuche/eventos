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
    public class ConviteRepository : IConviteRepository
    {
        protected readonly ApplicationContext _dbContext;
        static readonly string apiUrl = "https://localhost:7252";

        public ConviteRepository(ApplicationContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Convite> GetByUsuario(string token, Guid usuarioId)
        {
            try
            {
                var url = $"{apiUrl}/api/convite/meus-convites/{usuarioId}";

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

                    var retorno = JsonConvert.DeserializeObject<IEnumerable<Convite>>(responseBody);

                    return retorno;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Convite GetById(string token, Guid id)
        {
            try
            {
                var url = $"{apiUrl}/api/convite/{id}";

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

                    var retorno = JsonConvert.DeserializeObject<Convite>(responseBody);

                    return retorno;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public RetornoViewModel ConfirmarConvite(string token, Guid id, bool confirmado)
        {
            try
            {
                var url = $"{apiUrl}/api/convite/confirmar/{id}";

                // Cria uma instância do HttpClient
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    var convite = new Convite()
                    {
                        Id = id,
                        Status = confirmado ? Convite.StatusConvite.Aceito : Convite.StatusConvite.Recusado
                    };

                    // Serializa o objeto para JSON
                    var json = JsonConvert.SerializeObject(convite);

                    // Cria um StringContent que contém o JSON
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    // Envia a requisição PUT
                    HttpResponseMessage response = client.PutAsync(url, content).Result;

                    string responseBody = response.Content.ReadAsStringAsync().Result;

                    // Verifica se deu erro
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception();
                    }


                    return new RetornoViewModel()
                    {
                        Sucesso = true,
                        Mensagem = $"{(confirmado ? "Confirmado" : "Recusado")} com sucesso!"
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
