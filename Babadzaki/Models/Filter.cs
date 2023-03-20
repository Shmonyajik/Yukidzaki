using System.ComponentModel.DataAnnotations;

namespace Babadzaki.Models
{
    public class Filter
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsChecked { get; set; } = false;
        public virtual ICollection<Attribute> Attributes { get; set; }// навигационное свойство

        public Filter()
        {
            Attributes = new List<Attribute>();

        }
    }
}
