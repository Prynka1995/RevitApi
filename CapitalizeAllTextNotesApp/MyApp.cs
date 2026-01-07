using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapitalizeAllTextNotesApp
{
    [Transaction(TransactionMode.Manual)]
    public class MyApp : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication application)
        {
            // Инициализация при запуске Revit
            application.CreateRibbonTab("ПИК-Привет");
            var panel = application.CreateRibbonPanel("ПИК-Привет", "Общее");
            var button = new PushButtonData(
            "CapitalizeAllTextNotes",
            "ЗаглавныеБуквы",
            "C:\\Users\\savinsv\\AppData\\Roaming\\Autodesk\\Revit\\Addins\\2019\\bin\\Debug\\CapitalizeAllTextNotes.dll",
            "Revit.SDK.Samples.CapitalizeAllTextNotes.CS.Command"
            );
            panel.AddItem(button);
            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication app)
        {
            // Очистка при закрытии Revit
            return Result.Succeeded;
        }
    }
}
