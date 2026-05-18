using PokeAPI.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace PokeAPI.Services
{
    public class PokemonApiService
    {
        private readonly HttpClient client;

        public PokemonApiService()
        {
            client = new HttpClient();
        }

        public async Task<List<PokemonDTO>> GetPokemonsAsync()
        {
            // URL oficial da API que foi fornecida para o projeto
            string url = "https://apimonsterdeconexao-366354054678.southamerica-east1.run.app";

            for (int i = 0; i < 3; i++)
            {
                try
                {
                    // Consome os dados reais do JSON direto do servidor em nuvem
                    var dadosRecebidos = await client.GetFromJsonAsync<List<PokemonDTO>>(url);

                    // Garante que se a API responder algo vazio, o sistema não quebra
                    return LimparDados(dadosRecebidos ?? new List<PokemonDTO>());
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Tentativa {i + 1} falhou ao conectar na API: {ex.Message}");
                }

                await Task.Delay(2000);
            }

            return new List<PokemonDTO>();
        }

        private List<PokemonDTO> LimparDados(List<PokemonDTO> pokemons)
        {
            if (pokemons == null) return new List<PokemonDTO>();

            foreach (var p in pokemons)
            {
                // Tratamento básico de nulos para segurança do relatório
                if (string.IsNullOrEmpty(p.name)) p.name = "Desconhecido";
                if (p.Tipos == null || p.Tipos.Count == 0) p.Tipos = new List<string> { "Normal" };

                // Regra do Relatório: Gera o Total de Status automaticamente
                p.BaseStatTotal = p.HP + p.Attack + p.Defense + p.SpAttack + p.SpDefense + p.Speed;

                // Define a classificação competitiva baseada nos atributos recebidos
                if (p.Attack > p.Defense)
                {
                    p.CompetitiveRole = "Physical Sweeper";
                }
                else
                {
                    p.CompetitiveRole = "Wall / Tank";
                }

                // Metadados locais da aplicação
                p.DataColeta = DateTime.Now;
                p.EnviadoParaNuvem = false;
            }

            return pokemons;
        }
    }
}