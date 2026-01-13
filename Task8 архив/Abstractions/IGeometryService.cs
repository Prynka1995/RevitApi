using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task8.Models;

namespace Task8.Abstractions
{
    public interface IGeometryService
    {
        OpeningInfo GetWallInfo(Wall wall, double WidthLimit);
    }
}
