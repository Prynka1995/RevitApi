using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Task9.Abstractions;

namespace Task9.Services
{
    internal class SectionService : ISectionService
    {
        private readonly ExternalCommandData _commandData;

        public SectionService(
            ExternalCommandData commandData)
        {
            _commandData = commandData;
        }

        public bool CreateSection(
            FamilyInstance instance,
            double widthOffsetMm,
            double depthOffsetMm,
            double heightOffsetMm,
            ObservableCollection<string> sectionNames
            )
        {
            var doc = _commandData.Application.ActiveUIDocument.Document;

            var bbox = instance.get_BoundingBox(null);

            string sectionNameX = sectionNames[0];
            string sectionNameY = sectionNames[1];
            string sectionNameZ = sectionNames[2];

            if (bbox == null)
            {
                return false;
            }

            var center = (bbox.Min + bbox.Max) / 2;
            var size = bbox.Max - bbox.Min;


            var widthOffset = UnitUtils.ConvertToInternalUnits(widthOffsetMm, DisplayUnitType.DUT_MILLIMETERS);
            var depthOffset = UnitUtils.ConvertToInternalUnits(depthOffsetMm, DisplayUnitType.DUT_MILLIMETERS);
            var heightOffset = UnitUtils.ConvertToInternalUnits(heightOffsetMm, DisplayUnitType.DUT_MILLIMETERS);

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
            sectionBoxX.Min = new XYZ(-size.X / 2 - widthOffset, -size.Z / 2 - depthOffset, -size.Y / 2 - heightOffset);
            sectionBoxX.Max = new XYZ(size.X / 2 + widthOffset, size.Z / 2 + depthOffset, size.Y / 2 + heightOffset);

            var sectionBoxY = new BoundingBoxXYZ();
            sectionBoxY.Transform = transformY;
            sectionBoxY.Min = new XYZ(-size.X / 2 - widthOffset, -size.Z / 2 - depthOffset, -size.Y / 2 - heightOffset);
            sectionBoxY.Max = new XYZ(size.X / 2 + widthOffset, size.Z / 2 + depthOffset, size.Y / 2 + heightOffset);

            var sectionBoxZ = new BoundingBoxXYZ();
            sectionBoxZ.Transform = transformZ;
            sectionBoxZ.Min = new XYZ(-size.X / 2 - widthOffset, -size.Z / 2 - depthOffset, -size.Y / 2 - heightOffset);
            sectionBoxZ.Max = new XYZ(size.X / 2 + widthOffset, size.Z / 2 + depthOffset, size.Y / 2 + heightOffset);

            var viewType = new FilteredElementCollector(doc)
            .OfClass(typeof(ViewFamilyType))
            .OfType<ViewFamilyType>()
            .First(x => x.ViewFamily == ViewFamily.Section);

            //var center = (bbox.Min + bbox.Max) / 2;
            //var size = bbox.Max - bbox.Min;

            //Transform transform = Transform.CreateTranslation(XYZ.Zero);
            //transform.Origin = center;
            //transform.BasisX = (XYZ.BasisZ.CrossProduct(XYZ.BasisY)).Normalize();
            //transform.BasisY = XYZ.BasisZ;
            //transform.BasisZ = XYZ.BasisY;


            //var sectionBox = new BoundingBoxXYZ();
            //sectionBox.Transform = transform;
            //sectionBox.Min = new XYZ(-size.X / 2 - widthOffset, -size.Z / 2 - depthOffset, -size.Y / 2 - heightOffset);
            //sectionBox.Max = new XYZ(size.X / 2 + widthOffset, size.Z / 2 + depthOffset, size.Y / 2 + heightOffset);

            //var viewType = new FilteredElementCollector(doc)
            //    .OfClass(typeof(ViewFamilyType))
            //    .OfType<ViewFamilyType>()
            //    .FirstOrDefault(x => x.ViewFamily == ViewFamily.Section);

            if (viewType == null)
            {
                return false;
            }

            try
            {
                using (var transaction = new Transaction(doc, "CreateSections"))
                {
                    transaction.Start();
                    //var viewSection = ViewSection.CreateSection(doc, viewType.Id, sectionBox);
                    //viewSection.Name = sectionName;
                    var viewSectionX = ViewSection.CreateSection(doc, viewType.Id, sectionBoxX);
                    viewSectionX.Name = sectionNameX;
                    var viewSectionY = ViewSection.CreateSection(doc, viewType.Id, sectionBoxY);
                    viewSectionY.Name = sectionNameY;
                    var viewSectionZ = ViewSection.CreateSection(doc, viewType.Id, sectionBoxZ);
                    viewSectionZ.Name = sectionNameZ;
                    transaction.Commit();
                }
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Ошибка", "SectionService");
            }
            return true;
        }
    }
}
