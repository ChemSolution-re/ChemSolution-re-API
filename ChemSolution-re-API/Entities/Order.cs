using System.ComponentModel.DataAnnotations;

namespace ChemSolution_re_API.Entities
{
    public class Order
    {
        [Key]
        public string OrderId { set; get; } = string.Empty;
        [Range(0, int.MaxValue)]
        public int CoinsAmount { set; get; }
        public User? User { set; get; }
        public string Data { set; get; } = string.Empty;
        public string Signature { set; get; } = string.Empty;
        public DateTime DateTime { set; get; } = DateTime.UtcNow;
    }
}
