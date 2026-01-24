using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Task8.Abstractions;

namespace Task8.Services
{
    public class SelectionService : iSelectionnService
    {
        private readonly ExternalCommandData _commandData;
        public SelectionService(ExternalCommandData commandData)
        {
            _commandData = commandData;
        }
        public Wall pickWall()
        {
            try
            {
                Reference reference = _commandData.Application.ActiveUIDocument.Selection.PickObject(ObjectType.Element, new WallFilter());
                Wall wall = _commandData.Application.ActiveUIDocument.Document.GetElement(reference) as Wall;
                return wall;
            }
            catch
            {
                return null;
            }

        }
    }
}
