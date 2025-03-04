using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using RevitAPI3.Extensions;
using RevitAPI3.GeometryUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitAPI3
{
    [Transaction(TransactionMode.Manual)]
    public class Geometry : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDocument = commandData.Application.ActiveUIDocument;
            Document document = uiDocument.Document;

            Element element = uiDocument.GetSelectedElements()[0];

            Options options = new Options();
            options.ComputeReferences = true;
            options.DetailLevel = ViewDetailLevel.Fine;
            options.IncludeNonVisibleObjects = true;

            List<Solid> solids = element.GetSolids(document, options);


            Solid mergedSolid = GeometryUtils.SolidUtils.UnionSolids(solids);

            document.Run(() =>
            {

                mergedSolid.Visualize(document);

            });

            return Result.Succeeded;
        }
    }
}
