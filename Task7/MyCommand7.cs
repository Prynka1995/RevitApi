using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System.Linq;
namespace Task7
{
    [Transaction(TransactionMode.Manual)]
    public class MyCommand7 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message,
                      ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;

            var el = doc.GetElement(uiDoc.Selection.PickObject(ObjectType.Element, "Выберите элемент"));
            Options options = new Options();

            var solids = el.get_Geometry(options)
            .Where(g => g is Solid)
            .OfType<Solid>()
            .ToList();

            double vol = 0; //суммарный объем
            double area = 0; //суммарный объем
            int faceCount = 0;  //количество граней
            int edgeCount = 0;  //количество ребер
            double edgeLength = 0;  //суммарная длина ребер

            foreach (var solid in solids)
            {
                FaceArray faces = solid.Faces;
                EdgeArray edges = solid.Edges;
                vol += solid.Volume;
                area += solid.SurfaceArea;
                faceCount += faces.Size;
                edgeCount += edges.Size;
                foreach (Edge edge in edges)
                {
                    Curve curve = edge.AsCurve();
                    edgeLength += curve.Length;
                }
            }

            double volM3 = UnitUtils.ConvertFromInternalUnits(vol, DisplayUnitType.DUT_CUBIC_METERS);
            double areaM2 = UnitUtils.ConvertFromInternalUnits(area, DisplayUnitType.DUT_SQUARE_METERS);
            double edgeLengthM = UnitUtils.ConvertFromInternalUnits(edgeLength, DisplayUnitType.DUT_METERS);

            TaskDialog.Show("Статистика",
            $"Суммарный объем: {volM3:F1} м3,\n\n"  +
            $"Суммарная площадь поверхностей всех Solid-ов.: {areaM2:F1} м2,\n\n" +
            $"Количество граней: {faceCount},\n\n" +
            $"Количество ребер: {edgeCount},\n\n" +
            $"Суммарная длина ребер: {edgeLengthM:F1} м"
            );

            return Result.Succeeded;
        }
    }
}
