using System;
using System.Collections.Generic;

namespace PokeAPI.Models
{
    public class PokemonDTO
    {
        public string Id { get; set; } = string.Empty;
        public string NomeExibicao { get; set; } = "Pokémon";
        public DateTime DataColeta { get; set; }
        public bool EnviadoParaNuvem { get; set; }
        public double Altura { get; set; }
        public double Peso { get; set; }
        public List<string> Tipos { get; set; } = new List<string>();
        public string SpriteUrl { get; set; } = string.Empty;
        public int HP { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public int SpAttack { get; set; }
        public int SpDefense { get; set; }
        public int Speed { get; set; }
        public int BaseStatTotal { get; set; }
        public string CompetitiveRole { get; set; } = "Nenhuma";

        // --- 🔄 ATALHOS PARA CORRIGIR OS ERROS DA IMAGE_6E03B7.PNG ---
        public string name { get => NomeExibicao; set => NomeExibicao = value; }
        public int hp { get => HP; set => HP = value; }
        public int attack { get => Attack; set => Attack = value; }
        public int defense { get => Defense; set => Defense = value; }
        public int spAttack { get => SpAttack; set => SpAttack = value; }
        public int spDefense { get => SpDefense; set => SpDefense = value; }
        public int speed { get => Speed; set => Speed = value; }
    }
}