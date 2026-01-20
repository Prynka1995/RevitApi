using Autodesk.Revit.DB;

namespace Task9.Abstractions
{
    public interface ISelectionService
    {
        FamilyInstance PickFamilyInstance();
    }
}
