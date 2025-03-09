using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using RevitAPI.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitAPI
{
    [Transaction(TransactionMode.Manual)]
    public class Encofrado : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Autodesk.Revit.UI.UIDocument uiDocument = commandData.Application.ActiveUIDocument;
            Document document = uiDocument.Document;

            // Code
            Reference faceReference = uiDocument.Selection.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Face, "Select a face");
            Autodesk.Revit.DB.Element element = document.GetElement(faceReference);
            GeometryObject geometryObject = element.GetGeometryObjectFromReference(faceReference);

            if (geometryObject is Face face)
            {
                List<CurveLoop> curveLoops = face.GetEdgesAsCurveLoops().ToList();

                document.Run(() =>
                {
                    foreach (CurveLoop curveLoop in curveLoops)
                    {
                        foreach (Curve curve in curveLoop)
                        {
                            if (curve is Line line)
                            {
                                document.CreateDirectShape(line);
                            }
                        }
                    }
                });
            }

            return Result.Succeeded;
        }
    }
}
