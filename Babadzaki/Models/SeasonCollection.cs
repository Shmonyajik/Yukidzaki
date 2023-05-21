
using AutoMapper;
using AutoMapper.Configuration.Annotations;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Babadzaki.Models
{
    public class SeasonCollection
    {
        [Key]
        [DisplayName("Season Collection")]
        public int Id { get; set; }
        [SourceMember("season_collection")]
        public string Name { get; set; }

        public virtual ICollection<Token> Tokens { get; set; }// навигационное свойство

        public SeasonCollection()
        {
            Tokens = new List<Token>();
        }
    }
}
