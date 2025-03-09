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

namespace RevitAPI
{
    [Transaction(TransactionMode.Manual)]
    public class Trial : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Autodesk.Revit.UI.UIDocument uiDocument = commandData.Application.ActiveUIDocument;
            Autodesk.Revit.DB.Document document = uiDocument.Document;

            // Code below
            //doc.CreatePoint(1, 5, 2);

            Autodesk.Revit.DB.Element element = uiDocument.PickElement();
            FamilyInstance column = element as FamilyInstance;
            //ColumnWrapper colWrp = new ColumnWrapper(column);
            ColumnWrapper colWrp = uiDocument.PickWrapper<ColumnWrapper>();

            var walls = document.GetElementByTypes<Wall>(w => w.Name.Contains("300"));
            TaskDialog.Show("Walls", walls.Count.ToString());

            document.Run(() => colWrp.CutInstance());

            return Result.Succeeded;
        }
    }
}
