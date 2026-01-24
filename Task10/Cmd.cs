using Autodesk.Revit.Attributes;
using Task10.Views;
using Microsoft.Extensions.DependencyInjection;
using RxBim.Command.Revit;
using RxBim.Shared;
using System;

namespace Task10
{
    [Transaction(TransactionMode.Manual)]
    public class Cmd : RxBimCommand
    {
        public PluginResult ExecuteCommand(IServiceProvider provider)
        {
            var mainWindow = provider.GetRequiredService<MainWindow>();
            mainWindow.ShowDialog();
            return PluginResult.Succeeded;
        }
    }
}
