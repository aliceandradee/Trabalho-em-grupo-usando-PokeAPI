using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace PokeAPI.Models
{
    public class PokemonDTO
    {

       public int id {  get; set; }
       public string name { get; set; }

       public double Altura { get; set; }

       public double Peso {  get; set; }

        public List<string> Tipos { get; set; } = new List<string>();

        public int HP { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public int SpAttack { get; set; }
        public int SpDefense { get; set; }
        public string SpriteUrl { get; set; }  
        public int Speed { get; set; }
        public int BaseStatTotal { get; set; }
        public string CompetitiveRole { get; set; }

        public DateTime DataColeta { get; set; }
        public bool EnviadoParaNuvem { get; set; }




}
}


