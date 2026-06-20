using System.ComponentModel.DataAnnotations;

namespace SimuladorInvestimento.Models
{
    public class Sonho
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UsuarioId { get; set; } 

        [Required]
        [StringLength(100)]
        public string NomeObjetivo { get; set; }  

        [Required]
        public decimal ValorObjetivo { get; set; }

        [Required]
        public decimal ValorInicial { get; set; } 

        [Required]
        public decimal AporteMensal { get; set; }

        [Required]
        public decimal TaxaJurosMensal { get; set; } 
    }
}