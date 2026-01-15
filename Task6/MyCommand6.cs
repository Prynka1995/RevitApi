using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Net;

namespace Task6
{
    [Transaction(TransactionMode.Manual)]
    public class MyCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;

            var selectedIds = uiDoc.Selection.GetElementIds(); //выбранные элементы

            if (selectedIds.Count != 2)
            {
                TaskDialog.Show("Ошибка", "Выберите 2 стены");
                return Result.Failed;
            }

            Wall wall1 = uiDoc.Document.GetElement(selectedIds.First()) as Wall;
            Wall wall2 = uiDoc.Document.GetElement(selectedIds.Last()) as Wall;

            if (wall1 == null || wall2 == null)
            {
                TaskDialog.Show("Ошибка", "Выберите именно стены");
                return Result.Failed;
            }

            //-----------ПРОВЕРЯЕМ ПАРАЛЛЕЛЬНОСТЬ СТЕН-----------
            XYZ direction1 = GetWallDirection(wall1);
            XYZ direction2 = GetWallDirection(wall2);

            double dotProduct = direction1.DotProduct(direction2);
            double tolerance = 0.001;
            if (Math.Abs(Math.Abs(dotProduct) - 1) < tolerance)
            {
                TaskDialog.Show("Результат", "Стены ПАРАЛЛЕЛЬНЫ");
            }
            else
            {
                TaskDialog.Show("Ошибка", "Стены не ПАРАЛЛЕЛЬНЫ");
                return Result.Failed;
            }
            //-----------НАХОДИМ СЕРЕДИНУ СТЕН-----------

            XYZ midPoint1 = MidPoint(doc, wall1);
            XYZ midPoint2 = MidPoint(doc, wall2);

            //-----------Вычислить вектор между этими точками-----------
            XYZ vectorBetween = midPoint2 - midPoint1;

            //-----Спроецировать этот вектор на нормаль стены чтобы получить перпендикулярное расстояние--------
            LocationCurve location1 = wall1.Location as LocationCurve;
            XYZ normal = direction1.CrossProduct(XYZ.BasisZ);//нормаль к стене1
            
            using (var transaction = new Transaction(doc, "CrossProduct vectors"))
            {
                transaction.Start();
                VisualizeAsVector(doc, location1.Curve.GetEndPoint(0), location1.Curve.GetEndPoint(1));
                VisualizeAsVector(doc, normal, normal*10);
                VisualizeAsVector(doc, normal, -normal*10);
                VisualizeAsVector(doc, midPoint1, midPoint2);
                transaction.Commit();
            }
            double result = vectorBetween.DotProduct(normal);
            double resultMM = UnitUtils.ConvertFromInternalUnits(result, DisplayUnitType.DUT_MILLIMETERS);
            TaskDialog.Show("info", $"{resultMM :F1}");

            return Result.Succeeded;
        }
        private XYZ GetWallDirection(Element wall)
        {
            LocationCurve location = wall.Location as LocationCurve;
            if (location == null) return null;

            Curve curve = location.Curve;
            XYZ start = curve.GetEndPoint(0);
            XYZ end = curve.GetEndPoint(1);

            return (end - start).Normalize();
        }
        public void VisualizeAsPoint(Document doc, XYZ point)
        {
            var directShape = DirectShape.CreateElement(doc, new ElementId(BuiltInCategory.OST_GenericModel));
            directShape.SetShape(new List<GeometryObject>() { Point.Create(point) });
        }
        public void VisualizeAsVector(Document doc, XYZ startPoint, XYZ endPoint)
        {
                var directShape = DirectShape.CreateElement(doc, new ElementId(BuiltInCategory.OST_GenericModel));
                directShape.SetShape(new List<GeometryObject>() { Line.CreateBound(startPoint, endPoint) });
        }
        private XYZ MidPoint(Document doc, Element wall)
        {
            LocationCurve location = wall.Location as LocationCurve;
            Curve curve = location.Curve;
            XYZ start = curve.GetEndPoint(0);
            XYZ end = curve.GetEndPoint(1);
            XYZ midPoint = (start + end) / 2;
            using (var transaction = new Transaction(doc, "Отобразить среднюю линию стены"))
            {
                transaction.Start();
                //VisualizeAsPoint(doc, start);
                ////VisualizeAsPoint(doc, end);
                VisualizeAsPoint(doc, midPoint);
                transaction.Commit();
            }
            return midPoint;
        }
    }
}