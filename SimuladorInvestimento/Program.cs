var builder = WebApplication.CreateBuilder(args);

// BACK-END: ativa o Razor Pages (páginas com front + lógica)
builder.Services.AddRazorPages();

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

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
