using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimuladorInvestimento.Data;
using SimuladorInvestimento.Models;

namespace SimuladorInvestimento.Pages
{
    [Authorize]
    public class HistoricoModel : PageModel
    {
        private readonly SimuladorContext _context;
        private readonly UserManager<Usuario> _userManager;

        public HistoricoModel(SimuladorContext context, UserManager<Usuario> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public List<SimulacaoHistorico> Simulacoes { get; set; } = new();
        public List<Sonho> HistoricoSonhos { get; set; } = new();

        public void OnGet()
        {
            CarregarSimulacoes();
        }

        private void CarregarSimulacoes()
        {
            var usuario = _userManager.GetUserAsync(User).Result;
            if (usuario != null)
            {
                Simulacoes = _context.SimulacoesHistorico
                    .Where(s => s.UsuarioId == usuario.Id)
                    .OrderByDescending(s => s.DataSimulacao)
                    .ToList(); 

                HistoricoSonhos = _context.Sonhos
                    .Where(s => s.UsuarioId == usuario.Id)
                    .OrderByDescending(s => s.Id)
                    .ToList();
            }
        }
        public async Task<IActionResult> OnPostDeletarAsync(int id)
        {
            // Pega o utilizador logado para garantir segurança
            var usuario = await _userManager.GetUserAsync(User);
            if (usuario == null) return Unauthorized();

            // Procura o sonho exato na base de dados
            var sonho = _context.Sonhos.FirstOrDefault(s => s.Id == id && s.UsuarioId == usuario.Id);

            if (sonho != null)
            {
                _context.Sonhos.Remove(sonho); // Dá a ordem de exclusão
                await _context.SaveChangesAsync(); // Grava a exclusão no banco
            }

            // Recarrega a página para atualizar a tabela
            return RedirectToPage();    
        }
        public async Task<IActionResult> OnPostDeletarSimulacaoAsync(int id)
        {
            // Garante que o utilizador está logado
            var usuario = await _userManager.GetUserAsync(User);
            if (usuario == null) return Unauthorized();

            // Procura a simulação exata na tabela do histórico
            var simulacao = _context.SimulacoesHistorico.FirstOrDefault(s => s.Id == id && s.UsuarioId == usuario.Id);

            if (simulacao != null)
            {
                _context.SimulacoesHistorico.Remove(simulacao); // Remove a simulação
                await _context.SaveChangesAsync(); // Grava no banco de dados
            }

            // Atualiza a página atual
            return RedirectToPage();
        }
    }
}
