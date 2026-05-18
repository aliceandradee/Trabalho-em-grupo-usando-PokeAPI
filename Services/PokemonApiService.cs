using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using PokeAPI.Models;

namespace PokeAPI.Services
{
    public class PokemonApiService
    {
        private readonly HttpClient client;
        private const string Url = "https://apimonsterdeconexao-366354054678.southamerica-east1.run.app/api/PokemonData/relatorios";

        public PokemonApiService()
        {
            client = new HttpClient();
            // Avisa o servidor que queremos os dados brutos (JSON) e não a página do site
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<List<PokemonDTO>> GetPokemonsAsync()
        {
            try
            {
                string jsonPuro = await client.GetStringAsync(Url);

                // Se voltar vazio ou o site do swagger, ignora para não quebrar o app - correção do erro que estava dando e essa foi a solução encontrada:
                if (string.IsNullOrEmpty(jsonPuro) || jsonPuro.Trim() == "[]" || jsonPuro.Contains("<!DOCTYPE html>"))
                {
                    return new List<PokemonDTO>();
                }

                var listaPokemons = new List<PokemonDTO>();
                var nodoRaiz = JsonNode.Parse(jsonPuro);

                if (nodoRaiz is JsonArray arrayJson)
                {
                    foreach (var item in arrayJson)
                    {
                        if (item == null) continue;

                        int hpVerificacao = item["hp"]?.GetValue<int>() ?? item["HP"]?.GetValue<int>() ?? 0;
                        int atkVerificacao = item["attack"]?.GetValue<int>() ?? item["Attack"]?.GetValue<int>() ?? 0;
                        int defVerificacao = item["defense"]?.GetValue<int>() ?? item["Defense"]?.GetValue<int>() ?? 0;
                        int spAtkVerificacao = item["spAttack"]?.GetValue<int>() ?? item["SpAttack"]?.GetValue<int>() ?? 0;
                        int spDefVerificacao = item["spDefense"]?.GetValue<int>() ?? item["SpDefense"]?.GetValue<int>() ?? 0;
                        int speedVerificacao = item["speed"]?.GetValue<int>() ?? item["Speed"]?.GetValue<int>() ?? 0;

                        string idString = item["id"]?.ToString() ?? item["Id"]?.ToString() ?? "";

                        if (idString == "string" || (hpVerificacao == 0 && atkVerificacao == 0 && defVerificacao == 0))
                        {
                            continue;
                        }

                        var p = new PokemonDTO();

                        p.Id = !string.IsNullOrEmpty(idString) ? idString : "Desconhecido";
                        p.NomeExibicao = "Monster " + p.Id;

                        p.SpriteUrl = item["spriteUrl"]?.ToString() ?? item["SpriteUrl"]?.ToString();
                        if (string.IsNullOrEmpty(p.SpriteUrl) || p.SpriteUrl == "string")
                        {
                            p.SpriteUrl = "https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/25.png";
                        }

                        p.HP = hpVerificacao;
                        p.Attack = atkVerificacao;
                        p.Defense = defVerificacao;
                        p.SpAttack = spAtkVerificacao;
                        p.SpAttack = spAtkVerificacao;
                        p.SpDefense = spDefVerificacao;
                        p.Speed = speedVerificacao;

                        p.Tipos = new List<string>();
                        var nodesTipos = (item["tipos"] ?? item["Tipos"]) as JsonArray;
                        if (nodesTipos != null)
                        {
                            foreach (var t in nodesTipos)
                            {
                                if (t != null && t.ToString() != "string")
                                    p.Tipos.Add(t.ToString());
                            }
                        }

                        if (p.Tipos.Count == 0) p.Tipos.Add("Unknown");

                        int bstDefeito = item["baseStatTotal"]?.GetValue<int>() ?? item["BaseStatTotal"]?.GetValue<int>() ?? 0;
                        p.BaseStatTotal = bstDefeito > 0 ? bstDefeito : (p.HP + p.Attack + p.Defense + p.SpAttack + p.SpDefense + p.Speed);

                        string roleDaApi = item["competitiveRole"]?.ToString() ?? item["CompetitiveRole"]?.ToString() ?? "";
                        p.CompetitiveRole = (!string.IsNullOrEmpty(roleDaApi) && roleDaApi != "string")
                                            ? roleDaApi
                                            : (p.Attack > p.Defense ? "Physical Sweeper" : "Wall / Tank");

                        p.DataColeta = DateTime.Now;
                        p.EnviadoParaNuvem = true;

                        listaPokemons.Add(p);
                    }
                }

                return listaPokemons;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Erro ao processar os dados da API: " + ex.Message);
                return new List<PokemonDTO>();
            }
        }
    }
}