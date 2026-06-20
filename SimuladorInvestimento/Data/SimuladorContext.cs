using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SimuladorInvestimento.Models;

namespace SimuladorInvestimento.Data
{
    public class SimuladorContext : IdentityDbContext<Usuario>
    {
        public SimuladorContext(DbContextOptions<SimuladorContext> options) : base(options)
        {
        }

        public DbSet<SimulacaoHistorico> SimulacoesHistorico { get; set; }
        public DbSet<Sonho> Sonhos { get; set; }
    }
}