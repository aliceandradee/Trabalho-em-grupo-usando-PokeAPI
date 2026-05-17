using System.Collections.ObjectModel;
using System.Threading.Tasks;
using PokeAPI.Models;
using PokeAPI.Services;

namespace PokeAPI.ViewModels
{
    public class DashboardViewModel
    {
        private readonly PokemonApiService apiService;

        // Esta lista armazena os Pokémons já processados com a inteligência do relatório
        public ObservableCollection<PokemonDTO> Pokemons { get; set; }

        public DashboardViewModel()
        {
            apiService = new PokemonApiService();
            Pokemons = new ObservableCollection<PokemonDTO>();
        }

        public async Task CarregarDados()
        {
            var lista = await apiService.GetPokemonsAsync();

            Pokemons.Clear();
            foreach (var p in lista)
            {
                Pokemons.Add(p);
            }
        }
    }
}