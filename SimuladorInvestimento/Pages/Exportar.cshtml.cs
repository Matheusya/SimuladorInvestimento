using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OfficeOpenXml;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using SimuladorInvestimento.Data;
using SimuladorInvestimento.Models;

namespace SimuladorInvestimento.Pages
{
    [Authorize]
    public class ExportarModel : PageModel
    {
        private readonly SimuladorContext _context;
        private readonly UserManager<Usuario> _userManager;

        public ExportarModel(SimuladorContext context, UserManager<Usuario> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGetExcelAsync()
        {
            try
            {
                var usuario = await _userManager.GetUserAsync(User);
                if (usuario == null)
                    return Unauthorized();

                var simulacoes = _context.SimulacoesHistorico
                    .Where(s => s.UsuarioId == usuario.Id)
                    .OrderByDescending(s => s.DataSimulacao)
                    .ToList();

                ExcelPackage.License.SetNonCommercialPersonal("Simulador de Investimento");

                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("Simulações");

                    // Cabeçalho
                    worksheet.Cells[1, 1].Value = "Data";
                    worksheet.Cells[1, 2].Value = "Valor Inicial";
                    worksheet.Cells[1, 3].Value = "Taxa de Juros (%)";
                    worksheet.Cells[1, 4].Value = "Tempo (Meses)";
                    worksheet.Cells[1, 5].Value = "Montante Final";
                    worksheet.Cells[1, 6].Value = "Total de Juros";
                    worksheet.Cells[1, 7].Value = "Rentabilidade (%)";
                    worksheet.Cells[1, 8].Value = "Descrição";

                    // Formatação do cabeçalho
                    for (int col = 1; col <= 8; col++)
                    {
                        worksheet.Cells[1, col].Style.Font.Bold = true;
                        worksheet.Cells[1, col].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        worksheet.Cells[1, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                    }

                    // Dados
                    int row = 2;
                    foreach (var sim in simulacoes)
                    {
                        worksheet.Cells[row, 1].Value = sim.DataSimulacao;
                        worksheet.Cells[row, 1].Style.Numberformat.Format = "dd/mm/yyyy hh:mm";
                        worksheet.Cells[row, 2].Value = sim.ValorInicial;
                        worksheet.Cells[row, 2].Style.Numberformat.Format = "#,##0.00";
                        worksheet.Cells[row, 3].Value = sim.TaxaJuros;
                        worksheet.Cells[row, 3].Style.Numberformat.Format = "0.00";
                        worksheet.Cells[row, 4].Value = sim.TempoMeses;
                        worksheet.Cells[row, 5].Value = sim.MontanteFinal;
                        worksheet.Cells[row, 5].Style.Numberformat.Format = "#,##0.00";
                        worksheet.Cells[row, 6].Value = sim.TotalJuros;
                        worksheet.Cells[row, 6].Style.Numberformat.Format = "#,##0.00";
                        worksheet.Cells[row, 7].Value = sim.RentabilidadePercentual;
                        worksheet.Cells[row, 7].Style.Numberformat.Format = "0.00";
                        worksheet.Cells[row, 8].Value = sim.Descricao;
                        row++;
                    }

                    // Auto-ajuste de colunas
                    for (int col = 1; col <= 8; col++)
                    {
                        worksheet.Column(col).AutoFit();
                    }

                    var fileBytes = package.GetAsByteArray();
                    return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Simulacoes_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao exportar: {ex.Message}");
            }
        }

        public async Task<IActionResult> OnGetPdfAsync()
        {
            try
            {
                var usuario = await _userManager.GetUserAsync(User);
                if (usuario == null)
                    return Unauthorized();

                var simulacoes = _context.SimulacoesHistorico
                    .Where(s => s.UsuarioId == usuario.Id)
                    .OrderByDescending(s => s.DataSimulacao)
                    .ToList();

                var document = Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Size(PageSizes.A4);
                        page.Margin(20);

                        page.Header().Text($"Relatório de Simulações - {DateTime.Now:dd/MM/yyyy HH:mm}")
                            .FontSize(20)
                            .Bold();

                        page.Content()
                            .PaddingVertical(10)
                            .Column(column =>
                            {
                                column.Spacing(10);

                                if (simulacoes.Any())
                                {
                                    column.Item().Table(table =>
                                    {
                                        table.ColumnsDefinition(columns =>
                                        {
                                            columns.RelativeColumn(1.2f);
                                            columns.RelativeColumn(1f);
                                            columns.RelativeColumn(1f);
                                            columns.RelativeColumn(1f);
                                            columns.RelativeColumn(1f);
                                            columns.RelativeColumn(1f);
                                        });

                                        // Cabeçalho
                                        table.Header(header =>
                                        {
                                            header.Cell().Background("#1F2937").Text("Data").FontColor("FFFFFF").Bold();
                                            header.Cell().Background("#1F2937").Text("Valor Inicial").FontColor("FFFFFF").Bold();
                                            header.Cell().Background("#1F2937").Text("Taxa (%)").FontColor("FFFFFF").Bold();
                                            header.Cell().Background("#1F2937").Text("Meses").FontColor("FFFFFF").Bold();
                                            header.Cell().Background("#1F2937").Text("Montante Final").FontColor("FFFFFF").Bold();
                                            header.Cell().Background("#1F2937").Text("Rentabilidade (%)").FontColor("FFFFFF").Bold();
                                        });

                                        // Dados
                                        foreach (var sim in simulacoes)
                                        {
                                            table.Cell().Text(sim.DataSimulacao.ToString("dd/MM/yyyy"));
                                            table.Cell().Text($"R$ {sim.ValorInicial:F2}");
                                            table.Cell().Text($"{sim.TaxaJuros:F2}%");
                                            table.Cell().Text(sim.TempoMeses.ToString());
                                            table.Cell().Text($"R$ {sim.MontanteFinal:F2}");
                                            table.Cell().Text($"{sim.RentabilidadePercentual:F2}%");
                                        }
                                    });
                                }
                                else
                                {
                                    column.Item().Text("Nenhuma simulação registrada");
                                }
                            });

                        page.Footer().AlignCenter().Text($"Relatório gerado em {DateTime.Now:dd/MM/yyyy HH:mm}");
                    });
                });

                var pdfBytes = document.GeneratePdf();
                return File(pdfBytes, "application/pdf", $"Simulacoes_{DateTime.Now:yyyyMMdd_HHmmss}.pdf");
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao exportar PDF: {ex.Message}");
            }
        }

        public async Task<IActionResult> OnGetPdfSimulacaoAsync(int id)
        {
            var usuario = await _userManager.GetUserAsync(User);
            if (usuario == null) return Unauthorized();

            var simulacao = _context.SimulacoesHistorico.FirstOrDefault(s => s.UsuarioId == usuario.Id && s.Id == id);
            if (simulacao == null) return NotFound();

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(20);
                    page.Header().Text($"Simulação Individual - {DateTime.Now:dd/MM/yyyy HH:mm}").FontSize(20).Bold();

                    page.Content().PaddingVertical(10).Column(column =>
                    {
                        column.Spacing(10);
                        column.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(); columns.RelativeColumn(); columns.RelativeColumn();
                                columns.RelativeColumn(); columns.RelativeColumn(); columns.RelativeColumn();
                            });

                            table.Header(header =>
                            {
                                header.Cell().Background("#1F2937").Text("Data").FontColor("#FFFFFF").Bold();
                                header.Cell().Background("#1F2937").Text("Inicial").FontColor("#FFFFFF").Bold();
                                header.Cell().Background("#1F2937").Text("Taxa (%)").FontColor("#FFFFFF").Bold();
                                header.Cell().Background("#1F2937").Text("Meses").FontColor("#FFFFFF").Bold();
                                header.Cell().Background("#1F2937").Text("Montante").FontColor("#FFFFFF").Bold();
                                header.Cell().Background("#1F2937").Text("Rentab.").FontColor("#FFFFFF").Bold();
                            });

                            table.Cell().Text(simulacao.DataSimulacao.ToString("dd/MM/yyyy HH:mm"));
                            table.Cell().Text(simulacao.ValorInicial.ToString("C"));
                            table.Cell().Text(simulacao.TaxaJuros.ToString("F2"));
                            table.Cell().Text(simulacao.TempoMeses.ToString());
                            table.Cell().Text(simulacao.MontanteFinal.ToString("C"));
                            table.Cell().Text(simulacao.RentabilidadePercentual.ToString("F2") + "%");
                        });
                    });
                });
            });

            return File(document.GeneratePdf(), "application/pdf", $"Simulacao_{id}.pdf");
        }

        public async Task<IActionResult> OnGetExcelSimulacaoAsync(int id)
        {
            var usuario = await _userManager.GetUserAsync(User);
            if (usuario == null) return Unauthorized();

            var simulacao = _context.SimulacoesHistorico.FirstOrDefault(s => s.UsuarioId == usuario.Id && s.Id == id);
            if (simulacao == null) return NotFound();
            ExcelPackage.License.SetNonCommercialPersonal("Simulador de Investimento");
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Simulação");
                worksheet.Cells[1, 1].Value = "Data";
                worksheet.Cells[1, 2].Value = "Valor Inicial";
                worksheet.Cells[1, 3].Value = "Taxa de Juros (%)";
                worksheet.Cells[1, 4].Value = "Tempo (Meses)";
                worksheet.Cells[1, 5].Value = "Montante Final";
                worksheet.Cells[1, 6].Value = "Rentabilidade (%)";

                // Estilo do cabeçalho
                using (var range = worksheet.Cells[1, 1, 1, 6])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                }

                worksheet.Cells[2, 1].Value = simulacao.DataSimulacao.ToString("dd/MM/yyyy HH:mm");
                worksheet.Cells[2, 2].Value = simulacao.ValorInicial;
                worksheet.Cells[2, 2].Style.Numberformat.Format = "#,##0.00";
                worksheet.Cells[2, 3].Value = simulacao.TaxaJuros;
                worksheet.Cells[2, 4].Value = simulacao.TempoMeses;
                worksheet.Cells[2, 5].Value = simulacao.MontanteFinal;
                worksheet.Cells[2, 5].Style.Numberformat.Format = "#,##0.00";
                worksheet.Cells[2, 6].Value = simulacao.RentabilidadePercentual;

                worksheet.Cells.AutoFitColumns();
                return File(package.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Simulacao_{id}.xlsx");
            }
        }
        public async Task<IActionResult> OnGetPdfSonhoAsync(int id)
        {
            var usuario = await _userManager.GetUserAsync(User);
            if (usuario == null) return Unauthorized();

            var sonho = _context.Sonhos.FirstOrDefault(s => s.UsuarioId == usuario.Id && s.Id == id);
            if (sonho == null) return NotFound();

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(20);
                    page.Header().Text($"Relatório do Sonho - {DateTime.Now:dd/MM/yyyy HH:mm}").FontSize(20).Bold();

                    page.Content().PaddingVertical(10).Column(column =>
                    {
                        column.Spacing(10);
                        column.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(); columns.RelativeColumn();
                                columns.RelativeColumn(); columns.RelativeColumn(); columns.RelativeColumn();
                            });

                            table.Header(header =>
                            {
                                header.Cell().Background("#1F2937").Text("Objetivo").FontColor("#FFFFFF").Bold();
                                header.Cell().Background("#1F2937").Text("Valor Sonho").FontColor("#FFFFFF").Bold();
                                header.Cell().Background("#1F2937").Text("Valor Inicial").FontColor("#FFFFFF").Bold();
                                header.Cell().Background("#1F2937").Text("Aporte").FontColor("#FFFFFF").Bold();
                                header.Cell().Background("#1F2937").Text("Taxa (%)").FontColor("#FFFFFF").Bold();
                            });

                            table.Cell().Text(sonho.NomeObjetivo);
                            table.Cell().Text(sonho.ValorObjetivo.ToString("C"));
                            table.Cell().Text(sonho.ValorInicial.ToString("C"));
                            table.Cell().Text(sonho.AporteMensal.ToString("C"));
                            table.Cell().Text(sonho.TaxaJurosMensal.ToString("F2") + "%");
                        });
                    });
                });
            });

            return File(document.GeneratePdf(), "application/pdf", $"Sonho_{id}.pdf");
        }

        public async Task<IActionResult> OnGetExcelSonhoAsync(int id)
        {
            var usuario = await _userManager.GetUserAsync(User);
            if (usuario == null) return Unauthorized();

            var sonho = _context.Sonhos.FirstOrDefault(s => s.UsuarioId == usuario.Id && s.Id == id);
            if (sonho == null) return NotFound();
            ExcelPackage.License.SetNonCommercialPersonal("Simulador de Investimento");
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Sonho");
                worksheet.Cells[1, 1].Value = "Objetivo";
                worksheet.Cells[1, 2].Value = "Valor do Sonho";
                worksheet.Cells[1, 3].Value = "Valor Inicial";
                worksheet.Cells[1, 4].Value = "Aporte Mensal";
                worksheet.Cells[1, 5].Value = "Taxa Mensal (%)";

                using (var range = worksheet.Cells[1, 1, 1, 5])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                }

                worksheet.Cells[2, 1].Value = sonho.NomeObjetivo;
                worksheet.Cells[2, 2].Value = sonho.ValorObjetivo;
                worksheet.Cells[2, 2].Style.Numberformat.Format = "#,##0.00";
                worksheet.Cells[2, 3].Value = sonho.ValorInicial;
                worksheet.Cells[2, 3].Style.Numberformat.Format = "#,##0.00";
                worksheet.Cells[2, 4].Value = sonho.AporteMensal;
                worksheet.Cells[2, 4].Style.Numberformat.Format = "#,##0.00";
                worksheet.Cells[2, 5].Value = sonho.TaxaJurosMensal;

                worksheet.Cells.AutoFitColumns();
                return File(package.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Sonho_{id}.xlsx");
            }
        }

    }
}
