using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimuladorInvestimento.Data;
using SimuladorInvestimento.Models;

namespace SimuladorInvestimento.Pages
{
    [Authorize] // Garante que só utilizadores com login acedem
    public class SimuladorSonhoModel : PageModel
    {
        private readonly SimuladorContext _context;
        private readonly UserManager<Usuario> _userManager;

        public SimuladorSonhoModel(SimuladorContext context, UserManager<Usuario> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Esta propriedade vai capturar os dados que o utilizador digitar na tela
        [BindProperty]
        public Sonho NovoSonho { get; set; }

        // Variáveis para exibir os resultados na tela depois de calcular
        public bool MostrarResultado { get; set; } = false;
        public int MesesNecessarios { get; set; }
        public decimal TotalInvestido { get; set; }
        public decimal TotalJuros { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id.HasValue) // Se clicou no botão Editar, carrega os dados
            {
                var usuario = await _userManager.GetUserAsync(User);
                if (usuario == null) return Unauthorized();

                NovoSonho = _context.Sonhos.FirstOrDefault(s => s.Id == id.Value && s.UsuarioId == usuario.Id);

                if (NovoSonho == null) return NotFound();
            }
            else
            {
                NovoSonho = new Sonho(); // Se entrou pelo menu normal, a tela fica vazia
            }
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            // Pega o utilizador logado no sistema
            var usuario = await _userManager.GetUserAsync(User);
            if (usuario == null) return Unauthorized();

            NovoSonho.UsuarioId = usuario.Id;

            // --- A LÓGICA DE SIMULAÇÃO (O "Motor") ---
            decimal saldoAtual = NovoSonho.ValorInicial;
            int mesesDecorridos = 0;
            decimal dinheiroTiradoDoBolso = NovoSonho.ValorInicial;

            // O laço roda mês a mês até o saldo alcançar o valor do objetivo
            while (saldoAtual < NovoSonho.ValorObjetivo)
            {
                // 1. Rende os juros sobre o saldo atual
                saldoAtual += saldoAtual * (NovoSonho.TaxaJurosMensal / 100);

                // 2. Adiciona o dinheiro do mês
                saldoAtual += NovoSonho.AporteMensal;
                dinheiroTiradoDoBolso += NovoSonho.AporteMensal;

                // 3. Passa 1 mês
                mesesDecorridos++;

                // Trava de segurança para não causar um loop infinito (ex: se o aporte for zero)
                if (mesesDecorridos > 1200) break; // Limite de 100 anos
            }

            // Guarda os resultados calculados nas variáveis para o HTML mostrar
            MesesNecessarios = mesesDecorridos;
            TotalInvestido = dinheiroTiradoDoBolso;
            TotalJuros = saldoAtual - dinheiroTiradoDoBolso;
            MostrarResultado = true; // Libera a exibição dos cartões coloridos

            if (NovoSonho.Id == 0)
            {
                _context.Sonhos.Add(NovoSonho); // É novo, adiciona
            }
            else
            {
                _context.Sonhos.Update(NovoSonho); // Já existe, apenas atualiza
            }

            await _context.SaveChangesAsync();

            return Page();
        }
    }
}