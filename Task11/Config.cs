using FamalyPlacment.Abstractions;
using FamalyPlacment.Services;
using FamalyPlacment.ViewModels;
using FamalyPlacment.Views;
using Microsoft.Extensions.DependencyInjection;
using RxBim.Di;

namespace FamalyPlacment
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
