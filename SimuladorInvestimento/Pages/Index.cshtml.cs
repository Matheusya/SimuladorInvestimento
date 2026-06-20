using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimuladorInvestimento.Data;
using SimuladorInvestimento.Models;
using System.ComponentModel.DataAnnotations;

namespace SimuladorInvestimento.Pages
{

    public class ExtratoMes
    {
        public int Mes { get; set; }
        public decimal JurosMensais { get; set; }
        public decimal SaldoAcumulado { get; set; }
    }


    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly SimuladorContext _context;
        private readonly UserManager<Usuario> _userManager;

        public IndexModel(SimuladorContext context, UserManager<Usuario> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        [Range(0.01, double.MaxValue, ErrorMessage = "Informe um valor inicial maior que zero.")]
        public decimal ValorInicial { get; set; }

        [BindProperty]
        [Range(0, 100, ErrorMessage = "Informe uma taxa de juros mensal entre 0 e 100.")]
        public decimal TaxaJuros { get; set; }

        [BindProperty]
        [Range(1, 1200, ErrorMessage = "Informe o tempo em meses (mínimo 1).")]
        public int TempoMeses { get; set; }

        [BindProperty]
        public string Descricao { get; set; }

        public decimal MontanteFinal { get; set; }

        public decimal TotalJuros { get; set; }

        public decimal RentabilidadePercentual { get; set; }

        public List<decimal> HistoricoMeses { get; set; } = new();

        public List<ExtratoMes> ExtratoMeses { get; set; } = new();

        public List<SimulacaoHistorico> MinhasSimulacoes { get; set; } = new();

        public void OnGet()
        {
            HistoricoMeses = new List<decimal>();
            ExtratoMeses = new List<ExtratoMes>();
            CarregarSimulacoesUsuario();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                HistoricoMeses = new List<decimal>();
                ExtratoMeses = new List<ExtratoMes>();
                CarregarSimulacoesUsuario();
                return Page();
            }

            var taxaMensal = TaxaJuros / 100m;
            HistoricoMeses = new List<decimal>();
            ExtratoMeses = new List<ExtratoMes>();
            var saldoAtual = ValorInicial;

            // Calcula mês a mês os juros compostos
            for (var mes = 1; mes <= TempoMeses; mes++)
            {
                var jurosMes = saldoAtual * taxaMensal;
                saldoAtual += jurosMes;

                var saldoArredondado = decimal.Round(saldoAtual, 2);
                var jurosArredondado = decimal.Round(jurosMes, 2);

                HistoricoMeses.Add(saldoArredondado);
                ExtratoMeses.Add(new ExtratoMes
                {
                    Mes = mes,
                    JurosMensais = jurosArredondado,
                    SaldoAcumulado = saldoArredondado
                });
            }

            MontanteFinal = decimal.Round(saldoAtual, 2);
            TotalJuros = decimal.Round(MontanteFinal - ValorInicial, 2);
            RentabilidadePercentual = ValorInicial > 0
                ? decimal.Round((TotalJuros / ValorInicial) * 100m, 2)
                : 0m;

            // Salvar simulação no banco
            var usuario = await _userManager.GetUserAsync(User);
            if (usuario != null)
            {
                var simulacao = new SimulacaoHistorico
                {
                    UsuarioId = usuario.Id,
                    ValorInicial = ValorInicial,
                    TaxaJuros = TaxaJuros,
                    TempoMeses = TempoMeses,
                    MontanteFinal = MontanteFinal,
                    TotalJuros = TotalJuros,
                    RentabilidadePercentual = RentabilidadePercentual,
                    Descricao = Descricao,
                    DataSimulacao = DateTime.Now
                };

                _context.SimulacoesHistorico.Add(simulacao);
                await _context.SaveChangesAsync();
            }

            CarregarSimulacoesUsuario();
            return Page();
        }

        private void CarregarSimulacoesUsuario()
        {
            var usuario = _userManager.GetUserAsync(User).Result;
            if (usuario != null)
            {
                MinhasSimulacoes = _context.SimulacoesHistorico
                    .Where(s => s.UsuarioId == usuario.Id)
                    .OrderByDescending(s => s.DataSimulacao)
                    .ToList();
            }
        }
    }
}
