using Microsoft.AspNetCore.Identity;

namespace SimuladorInvestimento.Models
{
    public class Usuario : IdentityUser
    {
        public DateTime DataCriacao { get; set; } = DateTime.Now;
        public ICollection<SimulacaoHistorico> Simulacoes { get; set; } = new List<SimulacaoHistorico>();
    }
}
