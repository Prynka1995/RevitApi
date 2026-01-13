using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Task8.Abstractions;
using Task8.Models;

namespace Task8.Services
{
    public class GeometryService : IGeometryService
    {
        public GeometryService()
        {
        }
        public OpeningInfo GetWallInfo(Wall wall, double WidthLimit)
        {
            Parameter lengthParam = wall.get_Parameter(BuiltInParameter.CURVE_ELEM_LENGTH);
            Parameter heightParam = wall.get_Parameter(BuiltInParameter.WALL_USER_HEIGHT_PARAM);
            Parameter volParam = wall.get_Parameter(BuiltInParameter.HOST_VOLUME_COMPUTED);
            Parameter areaParam = wall.get_Parameter(BuiltInParameter.HOST_AREA_COMPUTED);
            string FamilyName = wall.WallType.FamilyName;
            string FamilyType = wall.WallType.Name;



            double lengthMM = UnitUtils.ConvertFromInternalUnits(
                         lengthParam.AsDouble(), DisplayUnitType.DUT_MILLIMETERS);
            double heightMM = UnitUtils.ConvertFromInternalUnits(
                         heightParam.AsDouble(), DisplayUnitType.DUT_MILLIMETERS);
            double widthMM = UnitUtils.ConvertFromInternalUnits(
                         wall.Width, DisplayUnitType.DUT_MILLIMETERS);
            double volumeM3 = UnitUtils.ConvertFromInternalUnits(
                         volParam.AsDouble(), DisplayUnitType.DUT_CUBIC_METERS);
            double areaM2 = UnitUtils.ConvertFromInternalUnits(
                         volParam.AsDouble(), DisplayUnitType.DUT_SQUARE_METERS);
            return new OpeningInfo()
            {
                Length = lengthMM,
                Height = heightMM,
                Width = widthMM,
                Volume = volumeM3,
                Area = areaM2,
                FamilyName = FamilyName,
                FamilyType = FamilyType,
                IsCorrect = WidthLimit > widthMM?false:true
            };
        }
    }
}
