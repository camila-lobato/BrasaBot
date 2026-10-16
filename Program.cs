using BrasaBot.Data;
using BrasaBot.Repositories.Interfaces;
using BrasaBot.Repositories;
using BrasaBot.Services.Interfaces;
using BrasaBot.Services;
using BrasaBot.Models;
using Microsoft.EntityFrameworkCore;

namespace BrasaBot
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseMySql(
                    builder.Configuration.GetConnectionString("DefaultConnection"),
                    ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
                ));

            // === REPOSITÓRIOS ===
            builder.Services.AddScoped<IFuncionarioRepository, FuncionarioRepository>();
            builder.Services.AddScoped<IClienteRepository, ClienteRepository>();

            builder.Services.AddScoped<IFornecedorRepository, FornecedorRepository>();

            builder.Services.AddScoped<IInsumoRepository, InsumoRepository>();
            builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();
            builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();


            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));  

            // === SERVICES ===
            builder.Services.AddScoped<IFuncionarioService, FuncionarioService>();
            builder.Services.AddScoped<ICategoriaService, CategoriaService>();
            builder.Services.AddScoped<IServicoService, ServicoService>();

            builder.Services.AddScoped<IClienteService, ClienteService>();

            builder.Services.AddScoped<IFornecedorService, FornecedorService>();

            builder.Services.AddScoped<IInsumoService, InsumoService>();

            builder.Services.AddScoped<IProdutoService, ProdutoService>();
            builder.Services.AddScoped<IUsuarioService, UsuarioService>();

            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}