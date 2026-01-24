using Autodesk.Revit.DB;
using CSharpFunctionalExtensions;
using Task10.Abstractions;
using Task10.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Task10.Services
{
    public class PlacementService : IPlacementService
    {
        private readonly Document _document;

        public PlacementService(Document document)
        {
            _document = document;
        }

        public Result Place(TreeType TreeType, int count)
        {
            return Validate(count)
                .Bind(() => FindFamily(TreeType))
                .Bind(s => PlaceInstances(s, count));
        }

        private Result Validate(int count)
        {
            if (count < 0)
                return Result.Failure("Количество должно быть больше 0");
            return Result.Success();
        }

        private Result<FamilySymbol> FindFamily(TreeType TreeType)
        {
            string treeName = string.Empty;
            switch (TreeType)
            {
                case TreeType.FirTree:
                    treeName = "Ель";
                    break;
                case TreeType.OrangeTree:
                    treeName = "Апельсиновое дерево";
                    break;
                case TreeType.Willowtree:
                    treeName = "Ива";
                    break;
            }

            FamilySymbol familySymbol = new FilteredElementCollector(_document)
                .OfCategory(BuiltInCategory.OST_Planting)
                .OfClass(typeof(FamilySymbol))
                .OfType<FamilySymbol>()
                .Where(x => x.FamilyName.Contains(treeName))
                .FirstOrDefault();

            if (familySymbol == null)
                return Result.Failure<FamilySymbol>("Не найден типоразмер для размещения");

            return familySymbol;
        }

        private Result PlaceInstances(FamilySymbol familySymbol, int count)
        {
            try
            {
                double step = UnitUtils.ConvertToInternalUnits(12, DisplayUnitType.DUT_METERS);
                var points = new List<XYZ>();
                double n = Math.Ceiling(Math.Sqrt(count));
                int increment = 0;
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        if (increment == count) break;
                    points.Add(new XYZ(i * step, j*(-step), 0));
                    increment++;
                    }
                }

                var level = new FilteredElementCollector(_document)
                    .OfClass(typeof(Level))
                    .OfType<Level>()
                    .OrderBy(l => l.Elevation)
                    .FirstOrDefault();

                if (level == null)
                    return Result.Failure("Не удалось определить уровень для размещения");

                using (Transaction transaction = new Transaction(_document, "Размещение деревьев"))
                {
                    transaction.Start();

                    if (!familySymbol.IsActive)
                    {
                        familySymbol.Activate();
                    }

                    foreach (var point in points)
                    {
                        _document.Create.NewFamilyInstance(
                            point,
                            familySymbol,
                            level,
                            Autodesk.Revit.DB.Structure.StructuralType.NonStructural);
                    }
                    transaction.Commit();
                }
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(ex.Message);
            }
        }
    }
}
