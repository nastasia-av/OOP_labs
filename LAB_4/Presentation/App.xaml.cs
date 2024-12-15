using LAB_4.DAL;
using LAB_4.DAL.Repositories;
using LAB_4.DAL.Interfaces;
using LAB_4.BLL.Interfaces;
using LAB_4.BLL.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Windows;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace LAB_4.Presentation
{
    public partial class App : Application
    {
        private IHost _host;

        public App()
        {
            _host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    string connectionString = "Data Source=../DAL/Data/familytree.db";

                    services.AddDbContext<ApplicationDbContext>(options =>
                        options.UseSqlite(connectionString));

                    services.AddScoped<IPersonRepository, SqlPersonRepository>();
                    services.AddScoped<ITreeRepository, SqlTreeRepository>();
                    services.AddScoped<IRelationRepository, SqlRelationRepository>();

                    services.AddTransient<MainWindow>();
                    services.AddScoped<IPersonService, PersonService>();
                    services.AddScoped<ITreeService, TreeService>();
                    services.AddScoped<IRelationService, RelationService>();

                })
                .Build();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await _host.StartAsync();

            using (var scope = _host.Services.CreateScope())
            {
                var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
                var projectDir = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\"));
                Environment.CurrentDirectory = projectDir;
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                dbContext.Database.Migrate();
            }
            Resources.Add("RelationTypeToStringConverter", new LAB_4.Presentation.Utils.RelationTypeToStringConverter());
            Resources.Add("GenderToStringConverter", new LAB_4.Presentation.Utils.GenderToStringConverter());
            Resources.Add("PersonToDisplayConverter", new LAB_4.Presentation.Utils.PersonToDisplayConverter());
            var mainWindow = ActivatorUtilities.CreateInstance<MainWindow>(_host.Services);
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
