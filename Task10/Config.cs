using Task10.Abstractions;
using Task10.Services;
using Task10.ViewModels;
using Task10.Views;
using Microsoft.Extensions.DependencyInjection;
using RxBim.Di;

namespace Task10
{
    internal class Config : ICommandConfiguration
    {
        public void Configure(IServiceCollection services)
        {
            services.AddSingleton<IPlacementService, PlacementService>();
            services.AddSingleton<MainWindowViewModel, MainWindowViewModel>();
            services.AddSingleton<MainWindow, MainWindow>();
        }
    }
}
