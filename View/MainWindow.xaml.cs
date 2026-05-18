using System;
using System.Windows;
using PokeAPI.Services;

namespace PokeAPI.View
{
    public partial class MainWindow : Window
    {
        private readonly PokemonApiService _apiService;

        public MainWindow()
        {
            InitializeComponent();
            _apiService = new PokemonApiService();
        }

        private async void BtnCarregar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Desabilita o botão temporariamente para evitar cliques duplos
                if (sender is System.Windows.Controls.Button btn) btn.IsEnabled = false;

                // 1. Busca os dados reais lá no servidor que o Erick te mandou
                var listaPokemons = await _apiService.GetPokemonsAsync();

                // 2. VINCULA A LISTA AO SEU NOVO LISTBOX DA TELA!
                ListaPokemons.ItemsSource = listaPokemons;

                MessageBox.Show($"Sucesso! {listaPokemons.Count} Pokémons foram recebidos e processados no relatório.", "PokeAPI");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar dados do relatório: {ex.Message}", "Erro");
            }
            finally
            {
                // Reabilita o botão após a conclusão ou falha da requisição
                if (sender is System.Windows.Controls.Button btn) btn.IsEnabled = true;
            }
        }
    }
}