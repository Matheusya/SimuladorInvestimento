using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimuladorInvestimento.Models
{
    public class SimulacaoHistorico
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UsuarioId { get; set; }

        [ForeignKey(nameof(UsuarioId))]
        public virtual Usuario Usuario { get; set; }

        [Required]
        public decimal ValorInicial { get; set; }

        [Required]
        public decimal TaxaJuros { get; set; }

        [Required]
        public int TempoMeses { get; set; }

        public decimal MontanteFinal { get; set; }

        public decimal TotalJuros { get; set; }

        public decimal RentabilidadePercentual { get; set; }

        [Required]
        public DateTime DataSimulacao { get; set; } = DateTime.Now;

        public string Descricao { get; set; }
    }
}
