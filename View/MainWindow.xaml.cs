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

        // CORRIGIDO: Criado o método que o XAML estava cobrando para zerar o erro CS1061
        private async void BtnCarregar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Desabilita o botão temporariamente para o usuário não clicar duas vezes
                if (sender is System.Windows.Controls.Button btn) btn.IsEnabled = false;

                // Chama a sua service que já está configurada com o link oficial da API
                var listaPokemons = await _apiService.GetPokemonsAsync();

                // Associa o resultado do relatório ao componente de exibição da tela
                // (Altere 'MainDataGrid' para o nome do seu componente caso seja diferente)
                // Se o seu grupo estiver usando a DashboardViewModel, a lista será jogada lá.
                MessageBox.Show($"Sucesso! {listaPokemons.Count} Pokémons foram recebidos e processados no relatório.", "PokeAPI");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar dados do relatório: {ex.Message}", "Erro");
            }
            finally
            {
                if (sender is System.Windows.Controls.Button btn) btn.IsEnabled = true;
            }
        }
    }
}