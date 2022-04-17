using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChemSolution_re_API.Entities
{
    public class Valence
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        public int ValenceId { get; set; }

        public List<Element> Elements { get; set; } = new List<Element>();
    }
}
