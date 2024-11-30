using LAB_3.BLL.Interfaces;
using LAB_3.BLL.Services;
using LAB_3.Client;
using LAB_3.Client.ViewModels;
using LAB_3.DAL;
using LAB_3.DAL.Interfaces;
using LAB_3.DAL.Repositories;
using LAB_3.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Windows;

namespace LAB_3
{
    public partial class App : Application
    {
        private IHost _host;

        public App()
        {
            _host = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration((context, config) =>
                {
                    // Читаем appsettings.json
                    string projectDir = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\"));
                    config.SetBasePath(projectDir);
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                })
                .ConfigureServices((context, services) =>
                {
                    // Получаем конфигурацию
                    var configuration = context.Configuration;

                    // Читаем настройки из appsettings.json
                    string repositoryType = configuration["RepositoryType"] ?? "Database";
                    if (repositoryType == "Database")
                    {
                        string provider = configuration["Database:Provider"];
                        string connectionString = configuration["Database:ConnectionStrings:DefaultConnection"];

                        if (provider == "SQLite")
                        {
                            services.AddDbContext<ApplicationDbContext>(options =>
                                options.UseSqlite(connectionString));

                            services.AddScoped<IStoreRepository, SqlStoreRepository>();
                            services.AddScoped<IProductRepository, SqlProductRepository>();
                        }
                        else
                        {
                            throw new InvalidOperationException("Поддерживается только SQLite.");
                        }
                    }
                    else if (repositoryType == "Csv")
                    {
                        string storesFilePath = configuration["Csv:StoresFilePath"];
                        string productsFilePath = configuration["Csv:ProductsFilePath"];

                        // Абсолютные пути к файлам CSV в папке Data
                        string storesAbsolutePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\", storesFilePath);
                        string productsAbsolutePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\", productsFilePath);

                        if (!File.Exists(storesAbsolutePath))
                        {
                            File.Create(storesFilePath).Close();
                        }

                        if (!File.Exists(productsAbsolutePath))
                        {
                            File.Create(productsFilePath).Close();
                        }

                        services.AddScoped<IStoreRepository>(_ => new FileStoreRepository(storesFilePath));
                        services.AddScoped<IProductRepository>(_ => new FileProductRepository(productsFilePath));
                    }
                    else
                    {
                        throw new InvalidOperationException("Неверный тип репозитория в настройках.");
                    }

                    // Регистрируем бизнес-логику
                    services.AddScoped<IStoreService, StoreService>();
                    services.AddScoped<IProductService, ProductService>();
                    services.AddScoped<IBatchService, BatchService>();

                    // Настройка главного окна
                    services.AddTransient<MainWindow>();
                    services.AddTransient<MainViewModel>();
                })
                .Build();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await _host.StartAsync();

            using (var scope = _host.Services.CreateScope())
            {
                var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
                var repositoryType = configuration["RepositoryType"] ?? "Database";
                var projectDir = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\"));
                Environment.CurrentDirectory = projectDir;
                if (repositoryType == "Database")
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    dbContext.Database.Migrate();
                }
            }

            var mainWindow = _host.Services.GetRequiredService<MainWindow>();
            mainWindow.Show();
            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await _host.StopAsync();
            _host.Dispose();
            base.OnExit(e);
        }
    }
}
