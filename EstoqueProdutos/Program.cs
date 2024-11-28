using EstoqueProdutos.Data;
using EstoqueProdutos.Helper;
using EstoqueProdutos.Repositorio;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace EstoqueProdutos
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<BancoContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DataBase"));
            });

            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            builder.Services.AddScoped<IVendaRepositorio, VendaRepositorio>();
            builder.Services.AddScoped<IClienteRepositorio, ClienteRepositorio>();
            builder.Services.AddScoped<IProdutoRepositorio, ProdutoRepositorio>();
            builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();
            builder.Services.AddScoped<IConfiguracaoRepositorio, ConfiguracaoRepositorio>();
            builder.Services.AddScoped<ISessao, Sessao>();
            builder.Services.AddScoped<IEmail, Email>();

            builder.Services.AddSession(o =>
            {
                o.Cookie.HttpOnly = true;
                o.Cookie.IsEssential = true;
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseSession();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Login}/{action=Index}/{id?}");

            app.Run();

            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("pt-BR");
            CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("pt-BR");
        }
    }
}
