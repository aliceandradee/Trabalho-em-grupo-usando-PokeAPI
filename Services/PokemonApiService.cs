using PokeAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace PokeAPI.Services
{
    public class PokemonApiService
    {

    private readonly HttpClient _httpClient;
        public PokemonApiService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<List<PokemonDTO>> GetPokemonsAsync()
        {
            string url = ""; // colocar o link da API

            // Tenta buscar os dados 3 vezes antes de desistir
            for (int i = 0; i < 3; i++)
            {
                try
                {
                    var response = await _httpClient.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        var data = await response.Content.ReadFromJsonAsync<List<PokemonDTO>>();
                        return LimparDados(data);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Tentativa {i + 1} falhou: {ex.Message}");
                }

                // Espera 2 segundos antes de tentar novamente, caso falhe
                await Task.Delay(2000);
            }

            return new List<PokemonDTO>(); // Retorna lista vazia se não conseguir após 3 tentativas
        }

        private List<PokemonDTO> LimparDados(List<PokemonDTO> pokemons)
        {
            if (pokemons == null) return new List<PokemonDTO>();

            foreach (var p in pokemons)
            {
                // Garante que não teremos erros se algum dado vier nulo da API
                if (string.IsNullOrEmpty(p.name)) p.name = "Desconhecido";
                if (p.Tipos == null) p.Tipos = new List<string> { "Normal" };
            }

            return pokemons;
        }


    }
}



