using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using Microsoft.Win32;
using PokeAPI.Models;
using PokeAPI.Services;

namespace PokeAPI.View
{
    public partial class MainWindow : Window
    {
        private readonly PokemonApiService _apiService;
        private List<PokemonDTO> _dadosAtuais;

        public MainWindow()
        {
            InitializeComponent();
            _apiService = new PokemonApiService();
            _dadosAtuais = new List<PokemonDTO>();
        }

        private async void BtnCarregar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BtnCarregar.IsEnabled = false;
                ListaPokemons.ItemsSource = null;

                _dadosAtuais = await _apiService.GetPokemonsAsync();

                if (_dadosAtuais == null || _dadosAtuais.Count == 0)
                {
                    MessageBox.Show("Nenhum Pokémon válido encontrado no servidor do Erick.", "Aviso");
                    return;
                }

                ListaPokemons.ItemsSource = _dadosAtuais;
                MessageBox.Show($"Sucesso! {_dadosAtuais.Count} Pokémon(s) carregados da nuvem.\n\n💡 Clique em cima dos Pokémons desejados para selecioná-los e exportar apenas eles!", "PokeAPI");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao conectar ou ler os dados:\n\n" + ex.Message, "Erro de Conexão");
            }
            finally
            {
                BtnCarregar.IsEnabled = true;
            }
        }

        private void BtnExportar_Click(object sender, RoutedEventArgs e)
        {
            // 🎯 CAPTURA RIGOROSA: Lê exatamente o que está marcado como selecionado na tela
            var pokemonsSelecionados = new List<PokemonDTO>();

            if (ListaPokemons.SelectedItems != null)
            {
                foreach (var item in ListaPokemons.SelectedItems)
                {
                    if (item is PokemonDTO pokemon)
                    {
                        pokemonsSelecionados.Add(pokemon);
                    }
                }
            }

            // Se o usuário clicar em exportar sem ter marcado nenhum card azul na tela, nós barramos aqui:
            if (pokemonsSelecionados.Count == 0)
            {
                MessageBox.Show("Por favor, clique em cima de pelo menos um Pokémon na lista para selecioná-lo antes de salvar o relatório.", "Nenhum Pokémon Selecionado");
                return;
            }

            try
            {
                SaveFileDialog salvarArquivo = new SaveFileDialog
                {
                    Filter = "Arquivo de Texto (*.txt)|*.txt",
                    FileName = "Relatorio_Pokemon_Personalizado"
                };

                if (salvarArquivo.ShowDialog() == true)
                {
                    using (StreamWriter sw = new StreamWriter(salvarArquivo.FileName))
                    {
                        sw.WriteLine("=========================================");
                        sw.WriteLine("   RELATÓRIO ESTRATÉGICO PERSONALIZADO   ");
                        sw.WriteLine("=========================================");
                        sw.WriteLine($"Gerado em: {DateTime.Now}");
                        sw.WriteLine($"Total de monstros salvos: {pokemonsSelecionados.Count}\n");

                        // Escreve APENAS os selecionados salvos na nossa lista temporária
                        foreach (var p in pokemonsSelecionados)
                        {
                            sw.WriteLine($"Nome: {p.NomeExibicao}");
                            sw.WriteLine($"Tipos: {string.Join(", ", p.Tipos)}");
                            sw.WriteLine($"Função Competitiva: {p.CompetitiveRole}");
                            sw.WriteLine($"Status base -> HP: {p.HP} | ATK: {p.Attack} | DEF: {p.Defense}");
                            sw.WriteLine($"Poder Total (BST): {p.BaseStatTotal}");
                            sw.WriteLine("-----------------------------------------");
                        }
                    }

                    MessageBox.Show($"Relatório personalizado com {pokemonsSelecionados.Count} monstro(s) exportado com sucesso!", "Sucesso");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao salvar arquivo: " + ex.Message, "Erro");
            }
        }
    }
}