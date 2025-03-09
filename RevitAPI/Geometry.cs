using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using RevitAPI.Extensions;
using RevitAPI.GeometryUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitAPI
{
    [Transaction(TransactionMode.Manual)]
    public class Geometry : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Autodesk.Revit.UI.UIDocument uiDocument = commandData.Application.ActiveUIDocument;
            Document document = uiDocument.Document;

            Autodesk.Revit.DB.Element element = uiDocument.GetSelectedElements()[0];

            Autodesk.Revit.DB.Options options = new Autodesk.Revit.DB.Options();
            options.ComputeReferences = true;
            options.DetailLevel = ViewDetailLevel.Fine;
            options.IncludeNonVisibleObjects = true;

            List<Solid> solids = element.GetSolids(options);


            Solid mergedSolid = GeometryUtils.SolidUtils.UnionSolids(solids);

            document.Run(() =>
            {
                mergedSolid.Visualize(document);                
            });

            return Result.Succeeded;
        }
    }
}
