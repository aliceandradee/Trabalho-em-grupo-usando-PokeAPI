using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using PokeAPI.Services;
using PokeAPI.Models;

namespace PokeAPI.View
{
    public partial class MainWindow : Window
    {
        private readonly PokemonApiService _apiService;
        private List<PokemonDTO>? _dadosAtuais;

        public MainWindow()
        {
            InitializeComponent();
            _apiService = new PokemonApiService();
        }

        private async void BtnCarregar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (sender is Button btn) btn.IsEnabled = false;

                // 1. Busca os dados na API do Erick
                _dadosAtuais = await _apiService.GetPokemonsAsync();

                // 2. Vincula a lista com todas as propriedades ao ListBox
                ListaPokemons.ItemsSource = _dadosAtuais;

                MessageBox.Show("Relatório atualizado com sucesso!", "PokeAPI");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar dados: " + ex.Message, "Erro");
            }
            finally
            {
                if (sender is Button btn) btn.IsEnabled = true;
            }
        }

        // NOVA LÓGICA: Exportar para PDF de forma nativa (Sem bibliotecas estranhas)
        private void BtnExportar_Click(object sender, RoutedEventArgs e)
        {
            if (_dadosAtuais == null || _dadosAtuais.Count == 0)
            {
                MessageBox.Show("Não há dados carregados para salvar. Clique em ATUALIZAR DADOS primeiro.", "Aviso");
                return;
            }

            try
            {
                // Abre a caixinha nativa de impressão do Windows
                PrintDialog caixaImpressao = new PrintDialog();

                if (caixaImpressao.ShowDialog() == true)
                {
                    // Altera temporariamente a dica visual do botão para o usuário saber que está salvando
                    if (sender is Button btn) btn.Content = "SALVANDO...";

                    // Manda o Windows imprimir o componente "ListaPokemons" direto como documento/PDF
                    caixaImpressao.PrintVisual(ListaPokemons, "Relatório Estratégico Pokémon");

                    MessageBox.Show("Relatório enviado para salvar/imprimir com sucesso!", "Sucesso");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao tentar salvar o documento: " + ex.Message, "Erro");
            }
            finally
            {
                if (sender is Button btn) btn.Content = "SALVAR PDF";
            }
        }
    }
}