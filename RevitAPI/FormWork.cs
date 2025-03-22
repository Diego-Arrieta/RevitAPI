using Autodesk.Revit.Attributes;
using RevitDB = Autodesk.Revit.DB;
using RevitUI = Autodesk.Revit.UI;
using RevitAPI.Extensions;
using RevitAPI.GeometryUtils;
using System.Collections.Generic;
using System.Linq;

namespace RevitAPI
{
    [Transaction(TransactionMode.Manual)]
    public class FormWork : RevitUI.IExternalCommand
    {
        public RevitUI.Result Execute(RevitUI.ExternalCommandData commandData, ref string message, RevitDB.ElementSet elements)
        {
            RevitUI.UIDocument uiDocument = commandData.Application.ActiveUIDocument;
            RevitDB.Document document = uiDocument.Document;

            RevitDB.Reference faceReference = uiDocument.Selection.PickObject(RevitUI.Selection.ObjectType.Element, "Select a column");
            RevitDB.Element element = document.GetElement(faceReference);
            RevitDB.GeometryObject geometryObject = element.GetGeometryObjectFromReference(faceReference);



            return RevitUI.Result.Succeeded;
        }
    }
}
