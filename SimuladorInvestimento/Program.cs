using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using SimuladorInvestimento.Data;
using SimuladorInvestimento.Models;
using QuestPDF.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Configuração do Banco de Dados
builder.Services.AddDbContext<SimuladorContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConexaoPadrao")));

// Configuração do Identity
builder.Services.AddIdentity<Usuario, IdentityRole>(options =>
{
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireDigit = false;
})
.AddEntityFrameworkStores<SimuladorContext>()
.AddDefaultTokenProviders();

// BACK-END: ativa o Razor Pages
builder.Services.AddRazorPages();

// QuestPDF License
QuestPDF.Settings.License = LicenseType.Community;

var app = builder.Build();

// Pipeline de execução da aplicação
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // Segurança em produção
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
