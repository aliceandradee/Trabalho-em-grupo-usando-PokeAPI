using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using PokeAPI.Models;

namespace PokeAPI.Services
{
    public class PokemonApiService
    {
        private readonly HttpClient client;
        private const string Url = "https://apimonsterdeconexao-366354054678.southamerica-east1.run.app";

        public PokemonApiService()
        {
            client = new HttpClient();
        }

        public async Task<List<PokemonDTO>> GetPokemonsAsync()
        {
            for (int i = 0; i < 3; i++)
            {
                try
                {
                    var dados = await client.GetFromJsonAsync<List<PokemonDTO>>(Url);

                    if (dados == null || dados.Count == 0)
                    {
                        return LimparDados(GerarDadosMock());
                    }

                    return LimparDados(dados);
                }
                catch
                {
                    await Task.Delay(2000);
                }
            }

            return LimparDados(GerarDadosMock());
        }

        private List<PokemonDTO> LimparDados(List<PokemonDTO> pokemons)
        {
            foreach (var p in pokemons)
            {
                if (string.IsNullOrEmpty(p.name)) p.name = "Desconhecido";
                if (p.Tipos == null) p.Tipos = new List<string> { "Normal" };

                p.BaseStatTotal = p.HP + p.Attack + p.Defense + p.SpAttack + p.SpDefense + p.Speed;

                if (p.Attack > p.Defense)
                {
                    p.CompetitiveRole = "Physical Sweeper";
                }
                else
                {
                    p.CompetitiveRole = "Wall / Tank";
                }

                if (string.IsNullOrEmpty(p.SpriteUrl) || p.SpriteUrl == "string")
                {
                    int id = p.HP > 0 && p.HP < 900 ? p.HP : 25;
                    p.SpriteUrl = $"https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/{id}.png";
                }

                p.DataColeta = DateTime.Now;
                p.EnviadoParaNuvem = false;
            }

            return pokemons;
        }

        private List<PokemonDTO> GerarDadosMock()
        {
            return new List<PokemonDTO>
            {
                new PokemonDTO { name = "Pikachu", HP = 25, Attack = 55, Defense = 40, SpAttack = 50, SpDefense = 50, Speed = 90 },
                new PokemonDTO { name = "Charizard", HP = 6, Attack = 84, Defense = 78, SpAttack = 109, SpDefense = 85, Speed = 100 },
                new PokemonDTO { name = "Blastoise", HP = 9, Attack = 83, Defense = 100, SpAttack = 85, SpDefense = 105, Speed = 78 }
            };
        }
    }
}