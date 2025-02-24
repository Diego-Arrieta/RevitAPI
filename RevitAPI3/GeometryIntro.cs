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
    public class GeometryIntro : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            var wall = uidoc.GetSelectedElements()[0];

            var solids = wall.get_Geometry(new Options())
                .Cast<Solid>();


            doc.Run(() =>
            {
                foreach (var solid in solids)
                {
                    solid.Visualize(doc);
                }
            });

            return Result.Succeeded;
        }
    }
}
