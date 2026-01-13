using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.UI;
using System.Linq;
using Task8.Views;
using Task8.ViewModels;
using Task8.Services;
using Microsoft.Extensions.DependencyInjection;
using Task8.Abstractions;

namespace Task8
{
    [Transaction(TransactionMode.Manual)]
    public class Cmd : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message,
                      ElementSet elements)
        {
            ServiceCollection services = new ServiceCollection();
            services.AddSingleton<ExternalCommandData>(commandData);
            services.AddSingleton<iSelectionnService, SelectionService>();
            services.AddSingleton<IGeometryService, GeometryService>();
            services.AddSingleton<MainWindowViewModel, MainWindowViewModel>();
            services.AddSingleton<MainWindow, MainWindow>();
            var provider = services.BuildServiceProvider();

            var mainWindow = provider.GetRequiredService<MainWindow>();
            mainWindow.Show();

            return Result.Succeeded;
        }
    }
}
