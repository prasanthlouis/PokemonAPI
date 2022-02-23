using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonAPI.Models
{
    public class PokemonDetails
    {

        public string Name { get; set; }
        public string Description { get; set; }
#nullable enable
        public Fruit? Fruit { get; set; }
#nullable disable
    }
}
