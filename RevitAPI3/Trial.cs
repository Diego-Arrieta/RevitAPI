using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using RevitAPI3.Extensions;
using RevitAPI3.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitAPI3
{
    [Transaction(TransactionMode.Manual)]
    public class Trial : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            // Code below
            //doc.CreatePoint(1, 5, 2);

            Element elem = uidoc.PickElement();
            FamilyInstance column = elem as FamilyInstance;
            //ColumnWrapper colWrp = new ColumnWrapper(column);
            ColumnWrapper colWrp = uidoc.PickWrapper<ColumnWrapper>();

            var walls = doc.GetElementByTypes<Wall>(w => w.Name.Contains("300"));
            TaskDialog.Show("Walls", walls.Count.ToString());

            doc.Run(() => colWrp.CutInstance());

            return Result.Succeeded;
        }
    }
}
