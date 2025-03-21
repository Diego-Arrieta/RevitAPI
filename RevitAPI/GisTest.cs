using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using RevitAPI.Extensions;
using RevitAPI.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RevitAPI
{
    [Transaction(TransactionMode.Manual)]
    public class GisTest : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Autodesk.Revit.UI.UIDocument uiDocument = commandData.Application.ActiveUIDocument;
            Autodesk.Revit.DB.Document document = uiDocument.Document;

            Autodesk.Revit.DB.Element element = uiDocument.PickElement();
            LocationPoint location = element.Location as LocationPoint;

            Transform toInternal = document.ActiveProjectLocation.GetTotalTransform();
            Transform toShared = toInternal.Inverse;

            XYZ sharedPoint = toShared.OfPoint(location.Point);

            double xPoint = UnitUtils.ConvertFromInternalUnits(sharedPoint.X, UnitTypeId.Meters);
            double yPoint = UnitUtils.ConvertFromInternalUnits(sharedPoint.Y, UnitTypeId.Meters);
            double zPoint = UnitUtils.ConvertFromInternalUnits(sharedPoint.Z, UnitTypeId.Meters);

            TaskDialog.Show("Point", $"{xPoint} - {yPoint}");

            return Result.Succeeded;
        }
    }
}
