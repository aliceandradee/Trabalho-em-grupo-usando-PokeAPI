using System;
using System.Collections.Generic;
using System.Linq;
using PokeAPI.Models;

namespace PokeAPI.View
{
    public class RankingView
    {
        // Esta função vai receber os dados que chegarem da API do Erick
        // e ordená-los automaticamente pelo total de status (BST) para o relatório
        public List<PokemonDTO> GerarRankingPorForca(List<PokemonDTO> listaProcessada)
        {
            if (listaProcessada == null)
            {
                return new List<PokemonDTO>();
            }

            // Organiza do maior status para o menor
            return listaProcessada.OrderByDescending(p => p.BaseStatTotal).ToList();
        }
    }
}