using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task5
{
    [Transaction(TransactionMode.Manual)]
    public class MyCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;
            Document doc = uiDoc.Document; //база данных

            try
            {
                IList<Reference> pickedRefs = uiDoc.Selection.PickObjects(ObjectType.Element, new FamilyInstanceFilter(), "Выберите элементы");

                //Словарь для подсчета по категориям <название категории, количество элементов>
                Dictionary<string, int> dict = new Dictionary<string, int>();
                foreach (var item in pickedRefs)
                {
                    Element elem = doc.GetElement(item);//нашли элемент в документе
                    string elemCategory = elem.Category.Name;
                    if (dict.ContainsKey(elemCategory))
                        dict[elemCategory] += 1;
                    else dict.Add(elemCategory, 1);
                }

                string str = $"Общее количество элементов: {pickedRefs.Count} шт.\n";
                foreach (KeyValuePair<string,int> item in dict)
                {
                    //TaskDialog.Show("Info", $"Категория: {item.Key}, количество: {item.Value} шт.\n");
                    str += $"{item.Key}: {item.Value} шт.\n";
                }
                TaskDialog.Show("info", str);
            }
            catch
            {
                TaskDialog.Show("Info", "Элементы не выбраны");
            }
            return Result.Succeeded;
        }
    }
}
