using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;

[Transaction(TransactionMode.Manual)]
public class MyCommand : IExternalCommand
{
    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
        UIDocument uiDoc = commandData.Application.ActiveUIDocument;
        Document doc = uiDoc.Document;

        //Получаем все стены
        var walls = new FilteredElementCollector(doc)
        .OfClass(typeof(Wall))
        .OfType<Wall>()
        .ToList();

        //Обработать случай, когда стен в проекте нет
        if (walls.Count == 0)
        {
            TaskDialog.Show("Ошибка", "В модели нет стен");
            return Result.Failed;
        }

        //Статистика по стенам
        List<double> wallsLengthList = new List<double>();
        double lengthSum = 0;

        foreach (Wall wall in walls)
        {
            // Получаем длину
            Parameter lengthParam = wall.get_Parameter(BuiltInParameter.CURVE_ELEM_LENGTH);
            wallsLengthList.Add(lengthParam.AsDouble());
            lengthSum += lengthParam.AsDouble();
        }

        wallsLengthList.Sort();

        double shortWall = wallsLengthList[0];
        double longWall = wallsLengthList[wallsLengthList.Count - 1];
        //Средняя длина = (Сумма длин всех стен) / (Количество стен)
        double averageLengthWalls = lengthSum / walls.Count;

        // Конвертируем длину в миллиметры
        double shortWallmm = UnitUtils.ConvertFromInternalUnits(shortWall, DisplayUnitType.DUT_MILLIMETERS);
        double longWallmm = UnitUtils.ConvertFromInternalUnits(longWall, DisplayUnitType.DUT_MILLIMETERS);
        double averageLengthWallsmm = UnitUtils.ConvertFromInternalUnits(averageLengthWalls, DisplayUnitType.DUT_MILLIMETERS);

        TaskDialog.Show("Статистика по стенам",
        $"Общее количество стен: {walls.Count} шт.\n" +
        $"Самая короткая стена: {shortWallmm:F2} мм.\n" +
        $"Самая длинная стена: {longWallmm:F2} мм.\n" +
        $"Средняя длина: {averageLengthWallsmm:F2} мм.");

        //Для самой длинной стены устанавливает комментарий "Самая длинная стена"
        //Для самой короткой стены устанавливает комментарий "Самая короткая стена"

        using (Transaction t = new Transaction(doc, "Длина в комментарий"))
        {
            t.Start();

            foreach (Wall wall in walls)
            {
                // Получаем длину
                Parameter lengthParam = wall.get_Parameter(BuiltInParameter.CURVE_ELEM_LENGTH);
                if (lengthParam == null || !lengthParam.HasValue) continue;

                // Получаем комментарий
                Parameter commentParam = wall.LookupParameter("Комментарии");
                if (commentParam == null || commentParam.IsReadOnly) continue;

                if (Math.Abs(lengthParam.AsDouble() - shortWall) < 0.001)
                {
                    commentParam.Set("Самая короткая стена");
                }
                else if (Math.Abs(lengthParam.AsDouble() - longWall) < 0.001)
                {
                    commentParam.Set("Самая длинная стена");
                }
            }

            t.Commit();

            return Result.Succeeded;
        }
    }
}