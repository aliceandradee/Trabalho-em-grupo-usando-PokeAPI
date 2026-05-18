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

                _dadosAtuais = await _apiService.GetPokemonsAsync();
                ListaPokemons.ItemsSource = _dadosAtuais;

                MessageBox.Show("Dados carregados com sucesso!", "PokeAPI");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar: " + ex.Message, "Erro");
            }
            finally
            {
                BtnCarregar.IsEnabled = true;
            }
        }

        // Nova lógica de exportação focada em persistência de arquivos (.txt)
        private void BtnExportar_Click(object sender, RoutedEventArgs e)
        {
            if (_dadosAtuais == null || _dadosAtuais.Count == 0)
            {
                MessageBox.Show("Carregue os dados antes de exportar.", "Aviso");
                return;
            }

            try
            {
                SaveFileDialog salvarArquivo = new SaveFileDialog
                {
                    Filter = "Arquivo de Texto (*.txt)|*.txt",
                    FileName = "Relatorio_Pokemon"
                };

                if (salvarArquivo.ShowDialog() == true)
                {
                    using (StreamWriter sw = new StreamWriter(salvarArquivo.FileName))
                    {
                        sw.WriteLine("=========================================");
                        sw.WriteLine("       RELATÓRIO ESTRATÉGICO POKÉMON     ");
                        sw.WriteLine("=========================================");
                        sw.WriteLine($"Gerado em: {DateTime.Now}\n");

                        foreach (var p in _dadosAtuais)
                        {
                            sw.WriteLine($"Nome: {p.name}");
                            sw.WriteLine($"Função Competitiva: {p.CompetitiveRole}");
                            sw.WriteLine($"Status base -> HP: {p.HP} | ATK: {p.Attack} | DEF: {p.Defense}");
                            sw.WriteLine($"Poder Total (BST): {p.BaseStatTotal}");
                            sw.WriteLine("-----------------------------------------");
                        }
                    }

                    MessageBox.Show("Relatório em texto exportado com sucesso!", "Sucesso");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao salvar arquivo: " + ex.Message, "Erro");
            }
        }
    }
}