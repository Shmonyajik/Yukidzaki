
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Babadzaki.Models
{
    public class SeasonCollection
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public System.Int16? TotalTokensNum { get; set; }

        public ICollection<Token> Tokens { get; set; }

        public SeasonCollection()
        {
            Tokens = new List<Token>();
        }
    }
}
