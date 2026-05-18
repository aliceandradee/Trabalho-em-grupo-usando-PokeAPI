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
            // Quando o Erick entregar a API, coloque o link dele aqui
            string url = "COLOQUE_O_LINK_DA_API_AQUI";

            for (int i = 0; i < 3; i++)
            {
                try
                {
                    // Consome os dados que o Vinícius salvou e o Erick disponibilizou
                    var dadosRecebidos = await client.GetFromJsonAsync<List<PokemonDTO>>(url);

                    return LimparDados(dadosRecebidos);
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
                // Tratamento básico de nulos
                if (string.IsNullOrEmpty(p.name)) p.name = "Desconhecido";
                if (p.Tipos == null || p.Tipos.Count == 0) p.Tipos = new List<string> { "Normal" };

                // geração dos relatórios
                p.BaseStatTotal = p.HP + p.Attack + p.Defense + p.SpAttack + p.SpDefense + p.Speed;

                if (p.Attack > p.Defense)
                {
                    p.CompetitiveRole = "Physical Sweeper";
                }
                else
                {
                    p.CompetitiveRole = "Wall / Tank";
                }

                // Configura metadados para auditoria do relatório
                p.DataColeta = DateTime.Now;
                p.EnviadoParaNuvem = false;
            }

            return pokemons;
        }
    }
}