using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using RevitAPI3.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitAPI3.GeometryUtils
{
    public static class SolidUtils
    {
        public static void Visualize(this Solid solid, Document doc)
        {
            doc.CreateDirectShape(solid);
        }
        public static Solid UnionSolids(List<Solid> solids)
        {
            if (solids == null || solids.Count == 0)
                return null;

            List<Solid> validSolids = solids.Where(s => s != null && s.Volume > 0).ToList();
            if (validSolids.Count == 0)
                return null;

            Solid resultSolid = validSolids.First();

            foreach (Solid solid in validSolids.Skip(1))
            {
                try
                {
                    resultSolid = BooleanOperationsUtils.ExecuteBooleanOperation(resultSolid, solid, BooleanOperationsType.Union);
                }
                catch (Exception ex)
                {
                    TaskDialog.Show("Error", $"Cannot join solid: {ex.Message}");
                }
            }
            return resultSolid;
        }
    }
}
