using Autodesk.Revit.DB;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Task9.Abstractions
{
    public interface ISectionService
    {
        bool CreateSection(FamilyInstance familyInstance, double widthOffsetMm, double depthOffsetMm, double heightOffsetMm, ObservableCollection<string> sectionNames);
    }
}
