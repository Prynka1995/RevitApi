using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Task9.Abstractions;
using Task9.Services;
using Task9.ViewModels;
using Task9.Views;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;

namespace Task9
{
    [Transaction(TransactionMode.Manual)]
    public class Cmd : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message,
                      ElementSet elements)
        {
            var uidoc = commandData.Application.ActiveUIDocument;
            var doc = uidoc.Document;

            ServiceCollection services = new ServiceCollection();
            services.AddSingleton<ExternalCommandData>(commandData);
            services.AddSingleton<RevitTask>(new RevitTask());
            services.AddSingleton<ISelectionService, SelectionService>();
            services.AddSingleton<ISectionService, SectionService>();
            services.AddSingleton<MainWindowViewModel, MainWindowViewModel>();
            services.AddSingleton<MainWindow, MainWindow>();
            var provider = services.BuildServiceProvider();

            var mainWindow = provider.GetRequiredService<MainWindow>();
            mainWindow.Show();



            var reference = uidoc.Selection.PickObject(ObjectType.Element, "Выберите элементы");
            var instance = doc.GetElement(reference);

            var bbox = instance.get_BoundingBox(null);
            var center = (bbox.Min + bbox.Max) / 2;
            var size = bbox.Max - bbox.Min;

            //Transform transform = Transform.Identity;

            Transform transformX = Transform.CreateTranslation(XYZ.Zero);
            transformX.Origin = center;
            transformX.BasisX = (XYZ.BasisZ.CrossProduct(XYZ.BasisX)).Normalize();
            transformX.BasisY = XYZ.BasisZ;
            transformX.BasisZ = XYZ.BasisX;

            Transform transformY = Transform.CreateTranslation(XYZ.Zero);
            transformY.Origin = center;
            transformY.BasisX = (XYZ.BasisZ.CrossProduct(XYZ.BasisY)).Normalize();
            transformY.BasisY = XYZ.BasisZ;
            transformY.BasisZ = XYZ.BasisY;

            Transform transformZ = Transform.CreateTranslation(XYZ.Zero);
            transformZ.Origin = center;
            transformZ.BasisX = XYZ.BasisX;
            transformZ.BasisY = XYZ.BasisY;
            transformZ.BasisZ = XYZ.BasisZ;

            var sectionBoxX = new BoundingBoxXYZ();
            sectionBoxX.Transform = transformX;
            sectionBoxX.Min = new XYZ(-size.X / 2, -size.Z / 2, -size.Y / 2);
            sectionBoxX.Max = new XYZ(size.X / 2, size.Z / 2, size.Y / 2);

            var sectionBoxY = new BoundingBoxXYZ();
            sectionBoxY.Transform = transformY;
            sectionBoxY.Min = new XYZ(-size.X / 2, -size.Z / 2, -size.Y / 2);
            sectionBoxY.Max = new XYZ(size.X / 2, size.Z / 2, size.Y / 2);

            var sectionBoxZ = new BoundingBoxXYZ();
            sectionBoxZ.Transform = transformZ;
            sectionBoxZ.Min = new XYZ(-size.X / 2, -size.Z / 2, -size.Y / 2);
            sectionBoxZ.Max = new XYZ(size.X / 2, size.Z / 2, size.Y / 2);

            var viewType = new FilteredElementCollector(doc)
            .OfClass(typeof(ViewFamilyType))
            .OfType<ViewFamilyType>()
            .First(x => x.ViewFamily == ViewFamily.Section);

            using (Transaction t = new Transaction(doc, "CreateSection"))
            {
                t.Start();
                VisualizeTransform(transformX, doc);
                VisualizeTransform(transformY, doc);
                VisualizeTransform(transformZ, doc);
                try
                {
                    var viewSectionX = ViewSection.CreateSection(doc, viewType.Id, sectionBoxX);
                    viewSectionX.Name = "Разрез X";
                    var viewSectionY = ViewSection.CreateSection(doc, viewType.Id, sectionBoxY);
                    viewSectionY.Name = "Разрез Y";
                    var viewSectionZ = ViewSection.CreateSection(doc, viewType.Id, sectionBoxZ);
                    viewSectionZ.Name = "Разрез Z";
                    t.Commit();
                }
                catch (System.Exception)
                {
                    TaskDialog.Show("Ошибка", "Нужно удалить сущ разрезы в модели");
                }
            }

            return Result.Succeeded;
        }

        public void VisualizeTransform(Transform transform, Document document, double scale = 3)
        {
            var colors = new List<Color>()
    {
        new Color(255, 0, 0),   // X - красный
        new Color(0, 255, 0),   // Y - зеленый
        new Color(0, 0, 255)    // Z - синий
    };

            var colorToLines = Enumerable.Range(0, 3)
                .Select(transform.get_Basis)
                .Select(x => Line.CreateBound(
                    transform.Origin,
                    transform.Origin + x * scale))
                .Zip(colors, (line, color) => (Line: line, Color: color))
                .ToList();

            foreach (var (line, color) in colorToLines)
            {
                var directShape = DirectShape.CreateElement(document, new ElementId(BuiltInCategory.OST_GenericModel));
                directShape.SetShape(new List<GeometryObject>() { line });

                var overrideGraphics = new OverrideGraphicSettings();
                overrideGraphics.SetProjectionLineColor(color);
                overrideGraphics.SetProjectionLineWeight(4);
                document.ActiveView.SetElementOverrides(directShape.Id, overrideGraphics);
            }
        }
        public Solid CreateSolidFromBoundingBox(BoundingBoxXYZ bbox)
        {
            var min = bbox.Min;
            var max = bbox.Max;
            var transform = bbox.Transform;

            // Получаем все 8 вершин параллелепипеда в локальной системе координат
            var vertices = new List<XYZ>
            {
                new XYZ(min.X, min.Y, min.Z), // 0: min-min-min
                new XYZ(max.X, min.Y, min.Z), // 1: max-min-min
                new XYZ(max.X, max.Y, min.Z), // 2: max-max-min
                new XYZ(min.X, max.Y, min.Z), // 3: min-max-min
                new XYZ(min.X, min.Y, max.Z), // 4: min-min-max
                new XYZ(max.X, min.Y, max.Z), // 5: max-min-max
                new XYZ(max.X, max.Y, max.Z), // 6: max-max-max
                new XYZ(min.X, max.Y, max.Z)  // 7: min-max-max
            };

            // Преобразуем вершины в глобальную систему координат
            var globalVertices = vertices.Select(v => transform.OfPoint(v)).ToList();

            // Создаем Solid через GeometryCreationUtilities
            // Используем метод создания через выдавливание (extrusion)
            var profileCurves = new List<Curve>();

            // Создаем профиль из нижней грани (Z = min.Z в локальной системе)
            var bottomFace = new List<XYZ>
            {
                globalVertices[0], // min-min-min
                globalVertices[1], // max-min-min
                globalVertices[2], // max-max-min
                globalVertices[3]  // min-max-min
            };

            // Создаем замкнутый контур нижней грани
            for (int i = 0; i < bottomFace.Count; i++)
            {
                var start = bottomFace[i];
                var end = bottomFace[(i + 1) % bottomFace.Count];
                profileCurves.Add(Line.CreateBound(start, end));
            }

            // Обертываем кривые в CurveLoop
            var profile = new CurveLoop();
            foreach (var curve in profileCurves)
            {
                profile.Append(curve);
            }

            // Направление выдавливания = высота параллелепипеда в глобальной системе
            var height = globalVertices[4] - globalVertices[0]; // от нижней к верхней вершине

            // Создаем Solid через выдавливание
            var solid = GeometryCreationUtilities.CreateExtrusionGeometry(
                new List<CurveLoop> { profile },
                height.Normalize(),
                height.GetLength());

            return solid;
        }
        public void VisualizeAsSolid(Document doc, Solid solid)
        {
            var directShape = DirectShape.CreateElement(doc, new ElementId(BuiltInCategory.OST_GenericModel));
            directShape.SetShape(new List<GeometryObject>() { solid });
        }
    }

}
