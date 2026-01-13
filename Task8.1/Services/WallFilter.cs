using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;

namespace Task8.Services
{
    class WallFilter : ISelectionFilter
    {
        public bool AllowElement(Element elem) => elem is Wall;
        public bool AllowReference(Reference reference, XYZ position) =>  false;
    }
}
